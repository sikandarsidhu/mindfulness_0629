using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    AudioSource _audioSource;

    public AudioClip VO1;


    // Use this for initialization
    void Start () {

        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = VO1;
        _audioSource.PlayDelayed(10f);
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}
