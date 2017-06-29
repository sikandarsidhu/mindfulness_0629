﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


namespace AudioVisualizer
{
    /// <summary>
    /// This class places game objects in a circle, rotates them, and adjusts the radius using audiodata.
    /// The gameobjects moved typically have trail renderers attached to them in our examples.
    /// </summary>
    public class CircleWaveform : MonoBehaviour
    {

		public int audioIndex = 0; // index into audioSampler audioSources or audioFIles list. Determines which audio source we want to sample
		public FrequencyRange frequencyRange = FrequencyRange.Decibal; // what frequency will we listen to? 
		public float sensitivity = 2; // how sensitive is this script to the audio. This value is multiplied by the audio sample data.
		public List<GameObject> objects; // the objects we want to move in a circle
		public float rotationSpeed = 10; // rotate objects around at this speed.
		public float radius = 10; // radius of the circle
		public float lerpSpeed = 10; // lerp between current rotation and next rotation.
        public bool useAudioFile = false; // flag saying if we should use a pre-recorded audio file
		public bool useWaveform = true; //if true: lerp up and down, and around. If false, just around
    

		private List<float> startAngles; // starting angle of each object
		private float angle = 0; // current angle
		float rotSpeed; //temp container for rotation speed, used in the "Bump" method
		private bool bumped = false; // state container
		private float startRadius; // initial radius
		private float sign = 1;


		void Awake()
		{
			//get the starting angle for each object
			startAngles = new List<float> ();
			float anglePerObject = 360f / objects.Count;
			for(int i = 0; i < objects.Count; i++)
			{
				startAngles.Add(i*anglePerObject);
			}

			rotSpeed = rotationSpeed; // place rotation speed in a temp container
			startRadius = radius;

		}

		void Start()
		{
			PositionObjects();
		}
		
		// Update is called once per frame
		void Update () {
			PositionObjects();

		}

		//draw a circle around this object in scene view
		void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawSphere (this.transform.position, 1);
		}

		//make everything rotate faster for a short period of time.
		//call from AudioEvent.OnBeat()
		public void Boost(float multiplier)
		{
			Invoke ("ResetSpeed",.1f); //reset the speed in .1 seconds
			rotationSpeed = rotationSpeed*multiplier; // set the new rotation speed		
		}

		void ResetSpeed()
		{
			rotationSpeed = rotSpeed;
		}

		//set the radius to the current sample average 0-1.
		//kind of like 'useWaveform' but doesn't happen every update, only when it's called
		//if switchSign = true, the sign of the audio is switched from 1 to -1 or vise versa every call.
		public void Bump(bool switchSign)
		{
			//float avg = AudioSampler.instance.GetAvg(audioSource,sensitivity,absoluteVal);
			float value;
			if(frequencyRange == FrequencyRange.Decibal)
			{
				value = AudioSampler.instance.GetRMS(audioIndex,useAudioFile)*sensitivity;//get the noise level 0-1 of the audio right now
			}
			else
			{
				value = AudioSampler.instance.GetFrequencyVol(audioIndex,frequencyRange,useAudioFile)*sensitivity;
			}

			if(switchSign)
			{
				sign = -sign;
			}
			radius = startRadius * value * sign;
		}

		//position each object around this gameobject, based on rotation speed.
		void PositionObjects()
		{
			//position the object accourding to the average
			angle += Time.smoothDeltaTime * rotationSpeed; // current rotation over time
			for(int i = 0; i < objects.Count; i++)
			{
				//angle of this object, = angle + startAngle
				float rad = (angle+startAngles[i])*Mathf.Deg2Rad; // radians
				float newRad = radius; // new radius
				if(useWaveform) // if using radius, update the radius based on the current average decibal level
				{
					float value;
					if(frequencyRange == FrequencyRange.Decibal)
					{
						value = AudioSampler.instance.GetRMS(audioIndex,useAudioFile)*sensitivity;//get the noise level 0-1 of the audio right now
					}
					else
					{
						value = AudioSampler.instance.GetFrequencyVol(audioIndex,frequencyRange,useAudioFile)*sensitivity;
					}
					newRad = radius*value;
                   // Debug.Log("Radius: " + newRad);
				}
				float x = Mathf.Cos(rad)*newRad; //update x and y pos
				float y = Mathf.Sin(rad)*newRad;
				//position each object around this one.
				Vector3 newPos = this.transform.position + this.transform.right*x + this.transform.up*y; 
				objects[i].transform.position = Vector3.Lerp(objects[i].transform.position,newPos,lerpSpeed*Time.smoothDeltaTime);
			}
		}
	}
}
