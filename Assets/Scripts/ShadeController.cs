using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShadeController : MonoBehaviour {
	[SerializeField] float _blindFinalYAxis = 4.2f;
	[SerializeField] float _blindRiseDuration = 10.0f;

	float _timer = 0.0f;
	bool _startRaise = false;

	[SerializeField] float _startAmbientIntensity = 0.0f;
	[SerializeField] float _finalAmbientIntensity = 2.0f;

	[SerializeField] float _delay = 5.0f;

	// Use this for initialization
	void Start () {
		transform.DOMoveY (_blindFinalYAxis, _blindRiseDuration).SetEase (Ease.InOutQuart);
		_startRaise = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (_startRaise) {
			_timer += Time.deltaTime;
			RenderSettings.ambientIntensity = Mathf.Lerp (_startAmbientIntensity, _finalAmbientIntensity, ((_timer < _delay)?  0.0f: _timer - _delay) / (_blindRiseDuration-_delay));
			if (_timer > _blindRiseDuration) {
				_startRaise = false;
			}
		}
	}
}
