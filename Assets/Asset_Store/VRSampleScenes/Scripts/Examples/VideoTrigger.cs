using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.Video;

namespace VRStandardAssets.Examples
{
    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class VideoTrigger : MonoBehaviour
    {
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] VideoPlayer _videoPlayer;
        [SerializeField] MovieScreen _movieScreenScript;

        [SerializeField] PanoramaHandler _panoramaHandlerScript;
        [SerializeField] PanoramaType _panoramaType;

        [SerializeField] Material _hoverInMaterial;
        [SerializeField] Material _hoverOutMaterial;
        [SerializeField] Renderer _Renderer;

        [SerializeField] float _duration = 3f;
        bool _isCounting = false;
        float _count;


        private void Awake ()
        {
        
        }

        private void Update()
        {
            if (_isCounting)
            {
                _count += Time.deltaTime;
                if(_count > _duration){
                    if (_panoramaType != PanoramaType.Null){
                        _panoramaHandlerScript.ChangePanorama(_panoramaType);
                        _isCounting = false;
                    }
                }
            }
        }

        void ChangePanorama()
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
            if (!_isCounting)
            {
                _videoPlayer.Play();
                _isCounting = true;
                _count = 0.0f;
                _Renderer.material = _hoverInMaterial;

            }
        }


        //Handle the Out event
        private void HandleOut()
        {
            Debug.Log("Show out state");
            _isCounting = false;
            _videoPlayer.Stop();
            _movieScreenScript.InitializeVideo();
            _Renderer.material = _hoverOutMaterial;

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