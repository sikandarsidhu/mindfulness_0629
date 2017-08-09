using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class BringBackGazebo : MonoBehaviour
{

    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public float duration = 1f;
    public bool isCounting = false;
    public float count;
    public bool alreadyActivated;

    public GameObject gazebo;
    public GameObject shade;
    public GameObject buttonPanel;
    public GameObject backButton;
    public GameObject meditationPanel;
//    public GameObject otherButton;
	public OtherStartButton startButton;

    public AudioSource _audioSource;

    public Sprite _hoverInMaterial;
    public Sprite _hoverOutMaterial;
	public SpriteRenderer _Renderer;

    private void Start()
    {
        alreadyActivated = false;
    }

    private void Update()
    {
        if (isCounting)
        {
            count += Time.deltaTime;
            if (count > duration && !alreadyActivated)
            {
                alreadyActivated = true;

                gazebo.SetActive(true);
                shade.SetActive(true);
                buttonPanel.SetActive(true);
                backButton.SetActive(false);
                meditationPanel.SetActive(false);
//                otherButton.SetActive(true);
//				if ( startButton != null )
//					startButton.Show();

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

           // _Renderer.material = _hoverInMaterial;
			_Renderer.sprite = _hoverInMaterial;
            _audioSource.Play();
        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        //Debug.Log("Show out state");
        isCounting = false;
        count = 0.0f;

		_Renderer.sprite = _hoverOutMaterial;
        //_Renderer.material = _hoverOutMaterial;
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
