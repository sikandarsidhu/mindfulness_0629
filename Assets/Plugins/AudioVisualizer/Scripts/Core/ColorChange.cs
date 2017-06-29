using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


namespace AudioVisualizer
{
    /// <summary>
    /// This class changes an object's material color based on the audio.
    /// </summary>
    public class ColorChange : MonoBehaviour
    {

		public int audioIndex = 0; // index into audioSampler audioSources or audioFIles list. Determines which audio source we want to sample
		public FrequencyRange frequencyRange = FrequencyRange.Decibal; // what frequency will we listen to? 
		public float sensitivity = 2; // how sensitive is this script to the audio. This value is multiplied by the audio sample data.
		public Color lowColor = Color.white; // when music decibal level is low, material is this color.
		public Color highColor = Color.white; // when music decibal level is high, material is this color.
		public float lerpSpeed = 10; // lerp between current color, and desired color
        public bool useAudioFile = false; // flag saying if we should use a pre-recorded audio file

        private Gradient gradient; // color gradient from lowColor to highColor
		private List<Material> materials; // this material
		private float alpha; // material opacity

                
       
		// Use this for initialization
		void Awake () {
			gradient = PanelWaveform.GetColorGradient(lowColor,highColor);
            materials = new List<Material>();

            foreach (Material mat in this.GetComponent<MeshRenderer>().materials)
            {
                materials.Add(mat);
            }
			alpha = materials[0].color.a;
		}
		
		// Update is called once per frame
		void Update () 
		{
			ColorPanel ();
		}

		public void Reset()
		{
			gradient = PanelWaveform.GetColorGradient(lowColor,highColor);
		}

		//color the panel, using the audio average decibal level to fade between colors.
		void ColorPanel()
		{
			float value;
			if(frequencyRange == FrequencyRange.Decibal)
			{
				value = AudioSampler.instance.GetRMS (audioIndex,useAudioFile)*sensitivity;//get the noise level 0-1 of the audio right now
			}
			else
			{
				value = AudioSampler.instance.GetFrequencyVol(audioIndex,frequencyRange,useAudioFile)*sensitivity;
			}

            //Debug.Log("value: " + value);
			value = Mathf.Clamp (value, 0, 1);
            

			Color desiredColor = gradient.Evaluate (value); //evaluate the gradient, grab a color based on the rms value 0-1
			Color newColor = Color.Lerp (materials[0].color, desiredColor, lerpSpeed * Time.deltaTime); // lerp from current color to desired color
			float desiredAlpha = lowColor.a + (highColor.a-lowColor.a)*value; //desired alpha based on rms
			alpha = Mathf.Lerp(alpha,desiredAlpha, lerpSpeed * Time.deltaTime); // lerp alpha
			newColor.a = alpha; // assign alpha to our gradient color


            foreach (Material mat in materials)
            {
                mat.color = newColor; // assign the color to our material
            }
			
		}
	}
}
