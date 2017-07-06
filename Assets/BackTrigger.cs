using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.Video;

// This script is a simple example of how an interactive item can
// be used to change things on gameobjects by handling events.
public class BackTrigger : MonoBehaviour
{
    [SerializeField] private VRInteractiveItem m_InteractiveItem;
    [SerializeField] AudioSource _panoVoiceOverAudioSource;

    AudioSource _audioSource;

    bool _hasBeenPlayedOnce = false;
    bool _buttonIsBeingPressed = false;
    [SerializeField] float _buttonPressDuration = 3.0f;

    [SerializeField] float duration = 2f;

    Timer _buttonPressTimer;
    bool _triggeredNextScene = false;
    bool createdTimer = false;

    public string mainMenuScene = "Playtest_01";

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        //_buttonPressTimer = new Timer(_buttonPressDuration);
    }

    private void Update()
    {
        if (createdTimer && _buttonPressTimer.IsOffCooldown && !_triggeredNextScene)
        {
            // _mGazeSwiitchSceneScript.SetIsReadyToTrue();
            MSceneManager.Instance.SwitchScene(mainMenuScene, duration);

            Debug.Log("loading scene: " + mainMenuScene);

            _triggeredNextScene = true;
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
        Debug.Log("Show over state");

        if (!createdTimer)
        {
            _buttonPressTimer = new Timer(_buttonPressDuration);
            createdTimer = true;
        }

        _buttonIsBeingPressed = true;
        _buttonPressTimer.Reset();
        
    }


    //Handle the Out event
    private void HandleOut()
    {
        Debug.Log("Show out state");
        _buttonIsBeingPressed = false;

    }


    //Handle the Click event
    private void HandleClick()
    {
        Debug.Log("Show click state");

    }


    //Handle the DoubleClick event
    private void HandleDoubleClick()
    {
        Debug.Log("Show double click");

    }
}