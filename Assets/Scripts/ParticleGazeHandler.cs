using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class ParticleGazeHandler : MonoBehaviour
{

    Animator anim;
    string GazeValue = "LookAtMe";
    bool _playedAnim = false;

    [SerializeField] VRInteractiveItem m_InteractiveItem;

    [SerializeField] ParticleSystem _particle;

    public AudioSource _sound;

    public float _duration = 1f;
    public bool _isLooking = false;
    Timer _gazeTimer;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        _gazeTimer = new Timer(_duration);
        _particle.Stop();
    }

    void Update()
    {
        if (_gazeTimer.IsOffCooldown && _isLooking && !_playedAnim)
        {
            anim.SetBool(GazeValue, true);
            _playedAnim = true;
            _isLooking = false;
            _particle.Stop();
        }
    }

    public void OnEndAnimation()
    {
        if (_isLooking)
        {
            _gazeTimer.Reset();
            _particle.Play();
        }
        anim.SetBool(GazeValue, false);
        _playedAnim = false;
    }

    void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }


    void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }


    //Handle the Over event
    void HandleOver()
    {
        _particle.gameObject.SetActive(true);
        _gazeTimer.Reset();
        _isLooking = true;
        if (!_playedAnim)
        {
            _particle.Play();
        }
    }


    //Handle the Out event
    void HandleOut()
    {
        _isLooking = false;
        _particle.Stop();
    }


    public void PlaySound()
    {
        _sound.Play();
    }
}