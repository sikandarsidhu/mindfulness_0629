﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AudioVisualizer
{
    /// <summary>
    /// Records tracks into serialized json files that can later be used for beat anticipation or improved performance.
    /// </summary>
    public class AudioRecorder : MonoBehaviour
    {
        public string path = "Assets\\AudioVisualizer\\Resources\\";
        public string audioFileName; // the file name we'll record data into
        public int audioIndex = 0; // index into audioSampler audioSources or audioFiles list. Determines which audio source we want to sample
        public List<FrequencyRange> ranges; // what frequency will we listen to? only record the ranges you plan on using to keep file size down
        public int sampleBufferSize = 40; // the size of the sample array recorded for each range, every frame
        public bool recordBeats = true; // flag indicating if we should record beats or not. If you're not using beat events set this to false to keep file size down.
        public bool abs = false; //take absolute value of samples every frame
        public bool debug = false; // show debug info or not

        string trackTitle = "";
        int numBeats = 0;
        int numSamples = 0;
        string recordingStatus = "Waiting...";
        //float timer = 0;
        float[] audioSamples;
        bool doneRecording = false;
        bool recording = false;
        AudioSource source;
        AudioData audioData;

        private void OnEnable()
        {
            //subscribe to events
            AudioEventListener.OnBeatRecognized += RecordBeat;
        }
        private void OnDisable()
        {
            //un-subscribe from events
            AudioEventListener.OnBeatRecognized -= RecordBeat;
        }

        // Use this for initialization
        void Start()
        {
            //grab the audio source
            source = AudioSampler.instance.audioSources[audioIndex];
            source.Stop(); //stop it for now


            trackTitle = source.clip.name;
            //create an empty AudioData object
            audioData = new AudioData(this, source.clip.length);
            
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(!recording)
            {
                return;
            }

            //check if the audio source is playing
            if (!source.isPlaying) //if the song stopped playing
            {
                //if we haven't entered this loop before
                if (!doneRecording)
                {
                    //set the status to indicate we're done recording
                    recordingStatus = "Done Recording";
                    doneRecording = true;
                    recording = false;
                    //serialize the audio data into a .txt file
                    SaveAudioInfo();
                }
                return;
            }

            //record samples into the AudioData object
            RecordSamples();


        }

        //display debug info
        void OnGUI()
        {
            if (debug)
            {
                string s = "Track: " + trackTitle + "\n";
                s += "Destination:" + path + audioFileName + ".txt \n";
                s += "Samples: " + numSamples.ToString() + "\n";
                s += "Beats: " + numBeats.ToString() + "\n";
                s += recordingStatus;
                GUI.TextField(new Rect(0, 0, 400, 100), s);
            }
        }

        //start recording the track
        public void StartRecording()
        {
            recordingStatus = "Recording...";
            recording = true;
            AudioSampler.instance.Play();
        }

        //call this method from AudioEventListener to record beats that were detected
        public void RecordBeat(Beat b)
        {
            if (recordBeats)
            {
                numBeats++;
                //Debug.Log("Recording Beat: " + b.time);
                audioData.RecordBeat(b.time, b.volume);
            }
        }

        //records samples at the current point in time
        public void RecordSamples()
        {
            foreach (FrequencyRange range in ranges)
            {
                //record audio samples
                if (range == FrequencyRange.Decibal)
                {
                    audioSamples = AudioSampler.instance.GetAudioSamples(audioIndex, sampleBufferSize, abs, false);
                }
                else
                {
                    audioSamples = AudioSampler.instance.GetFrequencyData(audioIndex, range, sampleBufferSize, abs, false);
                }
                audioData.RecordSamples(range, audioSamples);
            }


            numSamples++;
        }

        //serialize the AudioData object into a .txt file
        public void SaveAudioInfo()
        {
            string fliePath = path + audioFileName + ".txt";
            
            string json = JsonUtility.ToJson(audioData);
            using (FileStream fs = new FileStream(fliePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(json);
                }
            }
            //refresh the asset database so the new file is visible
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
