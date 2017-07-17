using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class GazeboTrigger : MonoBehaviour
{

    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public float duration = 1f;
    public bool isCounting = false;
    public float count;
    public bool alreadyActivated = false;

    public GameObject gazebo;
    public GameObject shade;

    private void Update()
    {
        if (isCounting)
        {
            count += Time.deltaTime;
            if (count > duration && !alreadyActivated)
            {
                gazebo.SetActive(!gazebo.activeSelf);
                shade.SetActive(!shade.activeSelf);
                
                alreadyActivated = true;
            }
        }
        else
        {
            alreadyActivated = false;
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
        if (!isCounting)
        {
            isCounting = true;

        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        //Debug.Log("Show out state");
        isCounting = false;
        count = 0.0f;
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
