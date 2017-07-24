using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class MouseGaze : MonoBehaviour {

    [SerializeField] private VRInteractiveItem m_InteractiveItem;


    [SerializeField] WanderScript _wanderScript;

    public SpiralParticle spiral;
    public int frame_count = 0;
    public bool played_particles_this_animation = false;

    //public AudioSource sneezeSound;

    public float _duration = 1f;
    public bool _isCounting = false;
    public float _count;

 

    void Start()
    {
        spiral = GetComponentInChildren<SpiralParticle>();
    }

    private void Update()
    {
        if (_isCounting)
        {
            _count += Time.deltaTime;

            if (_count > _duration)
            {
                //do something after gazing for the duration
                played_particles_this_animation = true;

                _wanderScript.StopMouse();
                _isCounting = false;
                //sneezeSound.Play();
            }
            else
            {
                _wanderScript.LetMouseGo();

                if (frame_count >= 10 && !played_particles_this_animation)
                {
                    if (spiral)
                    {
                        spiral.Emit(1);
                    }
                    //Debug.Log("particles: " + spiral.GetParticleCount().ToString());
                    frame_count = 0;
                }
                else
                {
                    frame_count += 1;

                }

            }
        }
        else
        {
            _wanderScript.LetMouseGo();

        }
    }

    public void OnEndAnimation()
    {
        Debug.Log("animation has ended");
        played_particles_this_animation = false;
        _count = 0.0f;
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
