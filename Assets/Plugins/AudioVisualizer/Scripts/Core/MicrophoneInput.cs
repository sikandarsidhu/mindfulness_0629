﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace AudioVisualizer
{
    /// <summary>
    /// Streams microphone data through an AudioSource component.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class MicrophoneInput : MonoBehaviour
    {

        public AudioMixerGroup mixer; // and AudioMixerGroup, with the db volume set as low as possible. used to mute the audiosource without affecting waveforms
        public int deviceNum = 0; // which microhpone device are we using?
        public bool debug = false; // show the current microphone

        string currentAudioInput = "none";
        float delay = 0.030f; // audio play back delay
        const float freq = 24000f;
        string[] inputDevices; // list of attached devices
        AudioSource aSource;

        void Start()
        {
            aSource = GetComponent<AudioSource>();
            //get the list of microphones on this device
            inputDevices = new string[Microphone.devices.Length];
            for (int i = 0; i < Microphone.devices.Length; i++)
            {
                inputDevices[i] = Microphone.devices[i].ToString();
                Debug.Log("Device: " + i + ": " + inputDevices[i]);
            }
            //record the name of the device we're using
            if (Microphone.devices.Length > 0)
            {
                currentAudioInput = Microphone.devices[deviceNum].ToString();
            }
            //start streaming the mic through an audio clip
            aSource.clip = Microphone.Start(currentAudioInput, true, 5, (int)freq);

            //hookup the AudioMixerGroup to the AudioSource
            aSource.outputAudioMixerGroup = mixer;
            
            //start playing back the audio streamed in from the mic
            //we won't hear the mic played back, because it's muted through the AudioMixerGroup
            aSource.Play();
        }

        
        
        private void Update()
        {
            //check for the mic recording to stop
            if (aSource.isPlaying)
                return;

            //start it up again if it does stop
            int microphoneSamples = Microphone.GetPosition(currentAudioInput);
            if (microphoneSamples / freq > delay)
            {
                //play the audio source
                aSource.timeSamples = (int)(microphoneSamples - (delay * freq));
                aSource.Play();
            }
        }

        //display debug info
        public void OnGUI()
        {
            if (debug)
            {
                GUI.Label(new Rect(10, 10, 400, 400), currentAudioInput);
            }
        }
    }
}