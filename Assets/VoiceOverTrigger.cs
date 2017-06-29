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
        [SerializeField] MGazeSwiitchScene _mGazeSwiitchSceneScript;

        AudioSource _audioSource;

        bool _hasBeenPlayedOnce = false;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {

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
            if (!_hasBeenPlayedOnce && !_panoVoiceOverAudioSource.isPlaying)
            {
                _audioSource.Play();
                _hasBeenPlayedOnce = true;
                _mGazeSwiitchSceneScript.SetIsReadyToTrue();
            }
        }


        //Handle the Out event
        private void HandleOut()
        {
            Debug.Log("Show out state");
    

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

}