using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.Video;

namespace VRStandardAssets.Examples
{
    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class VoiceOverTrigger : MonoBehaviour
    {
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] AudioSource _panoVoiceOverAudioSource;

        AudioSource _audioSource;

        bool _hasBeenPlayedOnce = false;
        bool _buttonIsBeingPressed = false;
        [SerializeField] float _buttonPressDuration = 2.0f;

        [SerializeField] float duration = 2f;

        Timer _buttonPressTimer;
        bool _triggeredNextScene = false;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _buttonPressTimer = new Timer(_buttonPressDuration);
        }

        private void Update()
        {
            if (_hasBeenPlayedOnce)
            {
                if (!_audioSource.isPlaying && !_triggeredNextScene)
                {
                   // _mGazeSwiitchSceneScript.SetIsReadyToTrue();
                   // MSceneManager.Instance.SwitchScene(PanoramaHandler._currentPanoramaType.ToString(), duration);

                    Debug.Log("loading scene: ChooseMeditation");

                    GameObject mmObj = GameObject.FindWithTag("MeditationManager");
                    MeditationManager mm = mmObj.GetComponent<MeditationManager>();
                    mm.setLevelName(PanoramaHandler._currentPanoramaType.ToString());

                    _triggeredNextScene = true;

                    MSceneManager.Instance.SwitchScene("ChooseMeditation", duration);
                }
            }

            if (_buttonIsBeingPressed && _buttonPressTimer.IsOffCooldown && !_hasBeenPlayedOnce)
            {
                _audioSource.Play();
                _hasBeenPlayedOnce = true;
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
            if (!_hasBeenPlayedOnce)
            {
                _panoVoiceOverAudioSource.Stop();
                _buttonIsBeingPressed = true;
                _buttonPressTimer.Reset();
            }
        }


        //Handle the Out event
        private void HandleOut()
        {
            //Debug.Log("Show out state");
            _buttonIsBeingPressed = false;

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

}