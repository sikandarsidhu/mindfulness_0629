using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGazebo : MonoBehaviour {

    AudioSource _meditationAudioSource;
    [SerializeField] Transform _gazeboTransform;

    [SerializeField] Vector3 _goalPosition;
    [SerializeField] Vector3 _startPosition;
    [SerializeField] float _duration = 5.0f;
    Timer _moveTimer;

    bool _isEnabled = false;

	// Use this for initialization
	void Awake () {
        _meditationAudioSource = GetComponent<AudioSource>();
        _moveTimer = new Timer(_duration);
        _gazeboTransform.localPosition = _startPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if ((!_meditationAudioSource.isPlaying || Input.GetKeyDown(KeyCode.Q)) && !_isEnabled)
        {
            _isEnabled = true;
            _moveTimer.Reset();
        }

        if (_isEnabled)
        {
            _gazeboTransform.localPosition = Vector3.Lerp(_startPosition, _goalPosition, _moveTimer.PercentTimePassed);
        }
	}
}
