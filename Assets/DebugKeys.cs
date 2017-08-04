using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeys : MonoBehaviour {

    [SerializeField] AudioSource _voSource;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //reset
            SceneManager.LoadScene("GazeboStart");
        } else if (Input.GetKeyDown(KeyCode.M))
        {
            // mute VO
            if(_voSource.volume == 0.0f)
            {
                _voSource.volume = 1.0f;
            } else
            {
                _voSource.volume = 0.0f;
            }
           
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            //reload this scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}
}
