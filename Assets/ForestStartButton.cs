using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.Video;

// This script is a simple example of how an interactive item can
// be used to change things on gameobjects by handling events.
public class ForestStartButton : MonoBehaviour
{
    [SerializeField] private VRInteractiveItem m_InteractiveItem;

    public AudioSource _audioSource;

    public Material _hoverInMaterial;
    public Material _hoverOutMaterial;
    public Renderer _Renderer;

    float _duration = 2f;
    public bool _isCounting = false;
    public float _count;

    public bool triggeredNextScene = false;

    private void Update()
    {
        if (_isCounting)
        {
            _count += Time.deltaTime;
            if (_count > _duration && !triggeredNextScene)
            {
                Debug.Log("loading scene: Forest");

                //GameObject mmObj = GameObject.FindWithTag("MeditationManager");
                //MeditationManager mm = mmObj.GetComponent<MeditationManager>();
                //mm.setLevelName(PanoramaHandler._currentPanoramaType.ToString());

                triggeredNextScene = true;

                MSceneManager.Instance.SwitchScene("Forest", 2.0f);
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
            _count = 0.0f;
            _Renderer.material = _hoverInMaterial;
            _audioSource.Play();

        }
    }


    //Handle the Out event
    private void HandleOut()
    {
        //Debug.Log("Show out state");
        _isCounting = false;

        _Renderer.material = _hoverOutMaterial;
        //_audioSource.Stop();

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