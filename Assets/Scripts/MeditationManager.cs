using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditationManager : MonoBehaviour {

    public string levelName;
    public string meditationType;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setLevelName(string name)
    {
        levelName = name;
    }

    public string getLevelName()
    {
        return levelName;
    }

    public void setMeditationType(string mt)
    {
        meditationType = mt;
    }

    public string getMeditationType()
    {
        return meditationType;
    }
}
