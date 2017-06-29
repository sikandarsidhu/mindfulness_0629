using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gazeebo_Shades_Speed : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Animation>() ["gazebo"].speed = speed;
		
	}
}
