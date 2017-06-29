using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class MoviePlayer : MonoBehaviour
{

    public MovieTexture movie;
    private AudioSource audio;

    void Start()
    {
        if (movie == null)
        {
            movie = Resources.Load<MovieTexture>("Test");
        }

        GetComponent<RawImage>().texture = movie as MovieTexture;

        movie.loop = true;


         audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;

        movie.Play();
        audio.Play();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && movie.isPlaying)
        {
            movie.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.P) && !movie.isPlaying)
        {
            movie.Play();
        }
    }
}
