using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class PanoramaData
{
    public PanoramaType whichPanorama;
    public Vector3 panoPosition;
    public Vector3 panoRotation;
    public Vector3 panoScale;
    public VideoClip videoClip;
    public AudioClip audioClip;
	public AudioClip voiceOverClip;
	public float audioSourceVolume = 1.0f;

    public PanoramaData(
        PanoramaType _whichPanorama,
        Vector3 _panoPosition,
        Vector3 _panoRotation,
        Vector3 _panoScale,
        VideoClip _videoClip,
        AudioClip _audioClip,
		AudioClip _voiceOverClip,
		float _audioSourceVolume){
        whichPanorama = _whichPanorama;
        panoPosition = _panoPosition;
        panoRotation = _panoRotation;
        panoScale = _panoScale;
        videoClip = _videoClip;
        audioClip = _audioClip;
		audioSourceVolume = _audioSourceVolume;
		voiceOverClip = _voiceOverClip;
    }
}

public enum PanoramaType {
    Beach = 0,
    Cave = 1,
    Forest = 2,
    Lake = 3,
    Moutain = 4,
    Space = 5,
    Temple = 6,
    Waterfall = 7,
    Null
}

public class PanoramaHandler : MonoBehaviour {
    VideoPlayer _panoramaPlayer;
	[SerializeField] AudioSource _audioPlayer;
	[SerializeField] AudioSource _voiceOverPlayer;
    // volume is between

    //Global Definition of Currently Selected Panorama
    public static PanoramaType _currentPanoramaType = PanoramaType.Null;

    [SerializeField] PanoramaData[] _panoramaData;

    [SerializeField] GameObject startButton;

    public GameObject[] lights;
    private bool dimLights = false;
    public float fade_duration = 1.0F;
    private float t = 0.0F;

    // Use this for initialization
    void Start () {
        _panoramaPlayer = GetComponent<VideoPlayer>();



    }
	
	// Update is called once per frame
	void Update () {

        if(dimLights)
        {
            t += 0.3f * Time.deltaTime;

            foreach (GameObject lightObject in lights)
            {
                Light l = lightObject.GetComponent<Light>();

                if(t < 1 && l.intensity != 0.0f)
                {
                    l.intensity = Mathf.Lerp(1, 0, t);
                }
                else
                {
                    l.intensity = 0.0f;
                }
            }

            if (t >= 1)
            {
                dimLights = false;
                t = 0.0F;
            }
        }

	}


   

    public void ChangePanorama(PanoramaType _panoramaType)
    {
        //Set current panorama to public static
        _currentPanoramaType = _panoramaType;

        _panoramaPlayer.Stop();
        transform.localScale = _panoramaData[(int)_panoramaType].panoScale;
        transform.localRotation = Quaternion.Euler(_panoramaData[(int)_panoramaType].panoRotation);
        transform.localPosition = _panoramaData[(int)_panoramaType].panoPosition;
        _panoramaPlayer.clip = _panoramaData[(int)_panoramaType].videoClip;
        _panoramaPlayer.Play();

        _audioPlayer.clip = _panoramaData[(int)_panoramaType].audioClip;
        _audioPlayer.Play();
		_audioPlayer.loop = true;

		_voiceOverPlayer.clip = _panoramaData [(int)_panoramaType].voiceOverClip;
		_voiceOverPlayer.Play ();
		_audioPlayer.volume = _panoramaData [(int)_panoramaType].audioSourceVolume;

        lights = GameObject.FindGameObjectsWithTag("Lights");

        dimLights = true;


        if (!startButton.activeSelf && (int)_panoramaType == 2)
        {
            startButton.SetActive(true);


        }
    }
}
