using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{
	public float m_speed = 1.0f;
	public bool y_axis = false; 
	public bool x_axis = false; 
	public bool z_axis = false; 


	void Update () 
	{
		if (x_axis) {
			transform.Rotate (new Vector3 (Time.deltaTime  * m_speed, 0, 0));
		}

		if (y_axis) {
			transform.Rotate (new Vector3 (0, Time.deltaTime  * m_speed, 0));
		}

		if (z_axis) {
			transform.Rotate (new Vector3 (0, 0, Time.deltaTime * m_speed));
		}
	}
}
