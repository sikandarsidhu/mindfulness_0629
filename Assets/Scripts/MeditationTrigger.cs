using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class MeditationTrigger : MonoBehaviour {

    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public string _meditationName;
    public AudioSource source;
    public AudioClip clip;

    //float _fadeDuration = 2f;

    public float _duration = 1f;
    public bool _isCounting = false;
    public float _count;

    public bool alreadyActivated = false;

    public bool _triggeredNextScene = false;

    public Material _hoverInMaterial;
    public Material _hoverOutMaterial;
    public Renderer _Renderer;
    public AudioSource _audioSource;
	public PanoramaHandler panoramaHandler;


    // Use this for initialization
    void Start()
    {
        //Debug.Log("initializing: " + clip.ToString());

    }

    // Update is called once per frame
    void Update()
    {

        if (_isCounting)
        {
            _count += Time.deltaTime;

            //Debug.Log("going to play: " + clip.ToString());

            if (_count > _duration && !alreadyActivated)
            {
//                source.clip = clip;
//                source.Play();

				panoramaHandler.PlayVoiceOver (clip);

                alreadyActivated = true;

            }
        }
        else
        {
            alreadyActivated = false;
        }

    }

    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }

    //Handle the Over event
    private void HandleOver()
    {
        //Debug.Log("Show over state: " + _meditationName);

        if (!_isCounting)
        {
            _isCounting = true;

            _Renderer.material = _hoverInMaterial;
//            _audioSource.Play();
        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        //Debug.Log("Show out state");
        _isCounting = false;
        _count = 0.0f;

        _Renderer.material = _hoverOutMaterial;
    }
}
