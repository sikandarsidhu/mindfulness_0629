using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class GazeTemplate : MonoBehaviour {

    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public float _duration = 1f;
    public bool _isCounting = false;
    public float _count;

    private void Update()
    {
        if (_isCounting)
        {
            _count += Time.deltaTime;
            if (_count > _duration)
            {
                //do somethingafter gazing for the duration
            }
        }
    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_InteractiveItem.OnClick += HandleClick;
        m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnClick -= HandleClick;
        m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
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


    //Handle the Click event
    private void HandleClick()
    {
        //Debug.Log("Show click state");

    }


    //Handle the DoubleClick event
    private void HandleDoubleClick()
    {
        //Debug.Log("Show double click");

    }
}
