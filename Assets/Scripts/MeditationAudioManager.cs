using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditationAudioManager : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip[] clips;
    public string meditationType;

	// Use this for initialization
	void Start () {

        GameObject mmObj = GameObject.FindWithTag("MeditationManager");
        MeditationManager mm = mmObj.GetComponent<MeditationManager>();
        meditationType = mm.getMeditationType();

        switch (meditationType)
        {
            case "Breathe":
                audioSource.clip = clips[0];
                break;
            case "Relax":
                audioSource.clip = clips[1];
                break;
            case "Restore":
                audioSource.clip = clips[2];
                break;
            case "Unwind":
                audioSource.clip = clips[3];
                break;
        }

        audioSource.Play();
        audioSource.loop = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
