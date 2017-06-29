﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace AudioVisualizer
{
    /// <summary>
    /// Sphere waveform.
    /// Similar to CircleWaveform, but in 3D!
    /// Rotates object around in a sphere, adjusting the radius along with the audio to create waveforms.
    /// </summary>
    public class SphereWaveform : MonoBehaviour
    {

		public int audioIndex = 0; // index into audioSampler audioSources or audioFIles list. Determines which audio source we want to sample
		public FrequencyRange frequencyRange = FrequencyRange.Decibal; // what frequency will we listen to? 
		public float sensitivity = 2; // how sensitive is this script to the audio. This value is multiplied by the audio sample data.
		public List<GameObject> objects; // objects to move around in a sphere
		public float rotationSpeed = 10; // rotate the objects at this rate
		public Vector3 rotationAxis = Vector3.up; // axis of rotation
		public float radius = 10; // radius of the sphere
		public float lerpSpeed = 1; // lerp from current position, to desired position.
		public bool useWaveform = true; //if true: lerp up and down, and around. If false, just around
        public bool useAudioFile = false; // flag saying if we should use a pre-recorded audio file
        public RotationType rotationType = RotationType.Uniform;
		public enum RotationType{Uniform,Rand,Cross};
		/*
		 * Uniform - uniformly rotate around the rotation axis
		 * Rand - rotate around random axes
		 * Cross - rotation axis for each object is Vector3.Cross(centerToObject,rotationAxis)
		 * 
		 * */
		private float rotSpeed;
		private float sign = 1;
		private float startRadius; // radius in the Start() method
		private List<Vector3> axes; //list of rotation axes for each object placed around the sphere
		// Use this for initialization
		void Start () 
		{
			//user warning, must have a rotation axis
			if(rotationAxis == Vector3.zero)
			{
				Debug.LogWarning("WARNING: rotation axis set to 0 on " + this.gameObject.name + " SphereWaveform.cs");
			}

			//place the objects uniformly around a sphere
			ObjectSphere.PlaceObjectsAroundSphere(objects,this.transform.position,radius);
			startRadius = radius;
			InitializeAxes(); // initialize rotaiton axes, needed for Rand, or Cross
			rotSpeed = rotationSpeed;
		}
		
		// Update is called once per frame
		void Update () 
		{
			Rotate(); //rotate each of the objects around it's rotation axis

			if(useWaveform) //if enabled
			{
				Waveform(); //adjust radius using the audio waveform
			}

			PositionObjects(); // lerp the objects position, accourding to changes in the radius.

		}

		//if RotationType = Rand or Cross, set the rotation axis for each object
		void InitializeAxes()
		{
			axes = new List<Vector3>();
			switch(rotationType)
			{
			case RotationType.Rand: 
				foreach(GameObject go in objects)
				{
					axes.Add(Random.onUnitSphere); // add a random axis for each object
				}
				break;
			case RotationType.Cross:
				foreach(GameObject go in objects)
				{
					//for each object, set rotation axis to Cross(center to gameobject, rotationAxis)
					Vector3 toGO = (go.transform.position - this.transform.position).normalized;
					Vector3 cross = Vector3.Cross(toGO,rotationAxis);
					axes.Add(cross);
				}
				break;
			
			default:
				break;
			}
		}

		//rotate each object, around the correct axis
		void Rotate()
		{
			switch(rotationType)
			{
			case RotationType.Uniform: //rotate everything around the rotationAxis
				Vector3 localAxis = this.transform.TransformDirection(rotationAxis); //transform the rotationAxis into local space
				this.transform.RotateAround(this.transform.position,localAxis,rotationSpeed*Time.smoothDeltaTime);
				break;
			case RotationType.Cross:
			case RotationType.Rand: // if cross or random, use the axis set up in "InitializeAxes()" for each object
				float rotSpeed = Time.smoothDeltaTime*rotationSpeed;
				//Debug.Log("rotating at speed: " + rotSpeed);
				for(int i = 0; i < objects.Count; i++)
				{
					objects[i].transform.RotateAround(this.transform.position,axes[i],rotSpeed);
				}
				break;
			default:
				break;
			}
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

		//quickly bump the radius based on the audio
		public void Bump(bool switchSign)
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

			if(switchSign)
			{
				sign = -sign;
			}
			radius = startRadius + value*sign;
			//Debug.Log ("bumping avg: " + avg + " radius: " + radius);

		}

		//adjust the radius every update based the audio
		void Waveform()
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
			radius = startRadius + value;
			//Debug.Log("avg: " + avg);
		}

		//move objects in and out, when a bump comes along, we want to change the radius
		void PositionObjects()
		{
			//position the object accourding to the current radius, which might have been changed in Waveform(), or Bump()
			foreach(GameObject go in objects)
			{
				Vector3 toObj = (go.transform.position - this.transform.position).normalized;
				Vector3 pos = this.transform.position + toObj*radius;
				go.transform.position = Vector3.Lerp(go.transform.position,pos,Time.smoothDeltaTime*lerpSpeed);
			}
		}

	}
}
