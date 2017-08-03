using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour {
	[SerializeField] Vector3[] _options;
	int _currentIndex = 0;
	int _goalIndex = 0;

	[SerializeField] AnimationCurve _butteryflyCurve;

	Timer _butterflyTimer;
	[SerializeField] float _flyDuration = 7.0f;

    Quaternion _lookRotation;

	void Awake(){
		_butterflyTimer = new Timer (_flyDuration);
		_currentIndex = _goalIndex;
		_goalIndex = RandomChoice (_currentIndex);
		_butterflyTimer.Reset ();
	}
		
	void Update(){
		GoToLocation (_currentIndex, _goalIndex);
		if (_butterflyTimer.IsOffCooldown) {
			_butterflyTimer.Reset ();
			_currentIndex = _goalIndex;
			_goalIndex = RandomChoice (_currentIndex);
		}

	}

	int RandomChoice(int current){
		int tempGoal = Random.Range (0, _options.Length);
		if (tempGoal == current) {
			return RandomChoice (current);
		} else {
			return tempGoal;
		}
	}

    // It goes to the location I think
	void GoToLocation(int current, int goal){
		Vector3 tempPos = Vector3.Lerp (_options[current], _options[goal], _butteryflyCurve.Evaluate(_butterflyTimer.PercentTimePassed));
		transform.localPosition = tempPos;
		Vector3 direction = _options [goal] - tempPos;
        Vector3 normalizedDirection = direction.normalized;

        _lookRotation = Quaternion.LookRotation(direction);
        Vector3 tempEulerRotation = _lookRotation.eulerAngles;
        tempEulerRotation.y += 180.0f;
        _lookRotation = Quaternion.Euler(tempEulerRotation);
        transform.rotation = _lookRotation;
	}
}
