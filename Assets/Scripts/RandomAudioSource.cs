using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour {

	[SerializeField] AudioSource[] _audioSources;

	int randomNumber = 10000;

	[SerializeField] float _minRange = 4.0f;
	[SerializeField] float _maxRange = 8.0f;
	Timer _waitTimer;

	void Awake () {
		_waitTimer = new Timer(1.0f);
		_waitTimer.Reset();
		randomNumber = ChooseRandomNumber (randomNumber);
	}

	// Update is called once per frame
	void Update () {
		if (!_audioSources[randomNumber].isPlaying && _waitTimer.IsOffCooldown)
		{
			_waitTimer.CooldownTime = Random.Range(_minRange, _maxRange);
			_waitTimer.Reset();
			PlayAudio();
		}
	}

	void PlayAudio()
	{
		if (_audioSources != null) {
			randomNumber = ChooseRandomNumber (randomNumber);
//			_audioSources [randomNumber].clip = _audioSources [randomNumber];
			Debug.Log("Play Audio");
			_audioSources [randomNumber].Play ();
		}
	}

	int ChooseRandomNumber (int prevNumber)
	{
		if (_audioSources == null)
			return -1;
		
		int tempNumber = Random.Range(0, _audioSources.Length);
		if(tempNumber == prevNumber && _audioSources.Length > 1)
		{
			return ChooseRandomNumber(prevNumber);
		}
		return tempNumber;
	}
}
