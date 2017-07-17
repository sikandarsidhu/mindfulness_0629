using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class MeditationTrigger : MonoBehaviour {

    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public string _meditationName;
    public AudioSource source;

    float _fadeDuration = 2f;

    public float _duration = 3f;
    public bool _isCounting = false;
    public float _count;

    public bool _triggeredNextScene = false;

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
        //Debug.Log("Show over state");
        if (!_isCounting)
        {
            _isCounting = true;
        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        //Debug.Log("Show out state");
        _isCounting = false;
        _count = 0.0f;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (_isCounting)
        {
            _count += Time.deltaTime;
            if (_count > _duration && !_triggeredNextScene)
            {
                GameObject mmObj = GameObject.FindWithTag("MeditationManager");
                MeditationManager mm = mmObj.GetComponent<MeditationManager>();
                mm.setMeditationType(_meditationName);

                MSceneManager.Instance.SwitchScene(mm.getLevelName(), _fadeDuration);

                _triggeredNextScene = true;

                source.Play();
            }
        }

    }
}
