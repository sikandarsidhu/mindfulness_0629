using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointEffect : MBehavior {

	[SerializeField] Renderer effect;

	public void UpdateEffect(Vector3 fromPos , Vector3 foward )
	{
//		Debug.Log ("Update " + fromPos + foward);
		transform.position = fromPos;
		transform.forward = foward;

		Debug.DrawLine (fromPos, fromPos + foward * 5f);

	}

	public void Begin()
	{
		effect.enabled = true;
	}

	public void End()
	{
		effect.enabled = false;
	}
}
