using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class OtherStartButton : MonoBehaviour
{

    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public float duration = 2f;
    public bool isCounting = false;
    public float count;
    public bool alreadyActivated;

    public AudioSource _audioSource;

    public Material _hoverInMaterial;
    public Material _hoverOutMaterial;
    public Renderer _Renderer;

    public GameObject gazebo;
    public GameObject shade;
    public GameObject buttonPanel;
    public GameObject backButton;
    public GameObject meditationPanel;
	public PanoramaHandler panoramaHandler;

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

                gazebo.SetActive(false);
                shade.SetActive(false);
                buttonPanel.SetActive(false);
                backButton.SetActive(true);
                meditationPanel.SetActive(true);
				Hide ();
//                otherButton.SetActive(false);
				panoramaHandler.StartOtherVideo();
            }
        }
        else
        {
            alreadyActivated = false;
        }
    }

	public void Hide()
	{
		if ( GetComponent<Renderer>() != null )
			GetComponent<Renderer>().enabled = false;
		if ( GetComponent<Collider>() != null )
			GetComponent<Collider>().enabled = false;
		
	}

	public void Show()
	{
		if ( GetComponent<Renderer>() != null )
			GetComponent<Renderer>().enabled = true;
		if ( GetComponent<Collider>() != null )
			GetComponent<Collider>().enabled = true;
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
            count = 0.0f;
            _Renderer.material = _hoverInMaterial;
//            _audioSource.Play();
        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        //Debug.Log("Show out state");
        isCounting = false;

        _Renderer.material = _hoverOutMaterial;
    }


    //Handle the Click event
    private void HandleClick()
    {
        //Debug.Log("Show click state");
		Debug.Log("Click Other Start Button");

		if (!isCounting)
		{
			isCounting = true;
			count = 0.0f;
			_Renderer.material = _hoverInMaterial;
//			_audioSource.Play();
		}
    }


    //Handle the DoubleClick event
    private void HandleDoubleClick()
    {
        //Debug.Log("Show double click");

    }
}
