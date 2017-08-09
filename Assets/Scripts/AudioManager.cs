using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	AudioSource _voiceOverSource;
	AudioSource _backgroundSource;

    public AudioClip VO1;


    // Use this for initialization
    void Start () {

        _voiceOverSource = GetComponent<AudioSource>();

        _voiceOverSource.clip = VO1;
        _voiceOverSource.PlayDelayed(10f);
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	public void SetClip( AudioClip clip )
	{
	}
}
