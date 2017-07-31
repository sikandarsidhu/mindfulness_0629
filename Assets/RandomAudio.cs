using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour {
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    int randomNumber = 10000;

    [SerializeField] float _minRange = 4.0f;
    [SerializeField] float _maxRange = 8.0f;
    Timer _waitTimer;

    void Awake () {
        _audioSource = GetComponent<AudioSource>();
        _waitTimer = new Timer(1.0f);
        _waitTimer.Reset();
	}
	
	// Update is called once per frame
	void Update () {
        if (!_audioSource.isPlaying && _waitTimer.IsOffCooldown)
        {
            _waitTimer.CooldownTime = Random.Range(_minRange, _maxRange);
            _waitTimer.Reset();
            PlayAudio();
        }
	}

    void PlayAudio()
    {
        randomNumber = ChooseRandomNumber(randomNumber);
        _audioSource.clip = _audioClips[randomNumber];
        _audioSource.Play();
    }

    int ChooseRandomNumber (int prevNumber)
    {
        int tempNumber = Random.Range(0, _audioClips.Length);
        if(tempNumber == prevNumber && _audioClips.Length > 1)
        {
           return ChooseRandomNumber(prevNumber);
        }
        return tempNumber;
    }
}
