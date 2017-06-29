using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class MGazeSwiitchScene : MonoBehaviour {

	[SerializeField] private VRInteractiveItem m_InteractiveItem;

	[SerializeField] private string toScene;
	[SerializeField] private float duration = 2f;
	[SerializeField] float HoverDuration = 3f;

	bool _isCounting = false;
	float _count;
	private void Update()
	{
		if (_isCounting)
		{
//			Debug.Log ("Count " + _count);
			float temCount = _count;
			_count += Time.deltaTime;
			if( ( temCount - HoverDuration ) * ( _count - HoverDuration) < 0 ){
				SwitchScene ();
			}
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

	void HandleOver()
	{
		if (!_isCounting) {
			_count = 0;
			_isCounting = true;
		}
	}

	void HandleOut()
	{
		_isCounting = false;

	}


	void SwitchScene()
	{
		MSceneManager.Instance.SwitchScene (toScene, duration);
	}
}
