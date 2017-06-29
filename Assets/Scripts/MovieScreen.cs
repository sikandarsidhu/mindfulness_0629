using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MovieScreen : MonoBehaviour {
    [SerializeField] VideoPlayer _videoPlayer;

	// Use this for initialization
	void Start () {
        InitializeVideo();
	}

    public void InitializeVideo()
    {
        _videoPlayer.Play();
        StartCoroutine(InitialPause());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator InitialPause()
    {
        yield return new WaitForSeconds(0.1f);
        _videoPlayer.Pause();
    }
}
