using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
	public static Vector3 Center = Vector3.zero;
	public static Vector3 UserPosition = Camera.main.transform.position;

	public static Vector3 ClampMagnitudeV3( Vector3 v , MinMax minmax)
	{
		return ClampMagnitudeV3 (v, minmax.min, minmax.max);
	}
	public static Vector3 ClampMagnitudeV3( Vector3 v , float min , float max )
	{
		Vector3 res = v;
		float m = Mathf.Clamp (res.magnitude, min, max);
		return res.normalized * m;
	}
}

[System.Serializable]
public class MinMax
{
	public float min;
	public float max;
	public float rand{
		get { return Random.Range (min, max); }
	}
	public MinMax(float _min , float _max){
		min = _min;
		max = _max;
	}
}

