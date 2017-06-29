using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class MGazeSwiitchScene : MonoBehaviour {
    //Atwood made the typo, blame him
	[SerializeField] private VRInteractiveItem m_InteractiveItem;

	[SerializeField] private string toScene;
	[SerializeField] private float duration = 2f;
	[SerializeField] float HoverDuration = 3f;
    [SerializeField] float _delayBeforeEnable = 17.0f;

	bool _isCounting = false;
    bool _isReadyToStart = false;
	float _count;
    Timer _delayTimer;

    private void Start()
    {
        _delayTimer = new Timer(_delayBeforeEnable);
    }

    private void Update()
	{
        if (_isCounting && _isReadyToStart && _delayTimer.IsOffCooldown)
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

    public void SetIsReadyToTrue()
    {
        _isReadyToStart = true;
        _delayTimer.Reset();
    }
}
