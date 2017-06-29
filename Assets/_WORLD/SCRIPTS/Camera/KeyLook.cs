using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class KeyLook : MBehavior {
	[SerializeField] Transform CameraTrans;
	[SerializeField] float speed;

	protected override void MUpdate ()
	{
		base.MUpdate ();

		float horizontal = CrossPlatformInputManager.GetAxis ("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis ("Vertical");

		transform.Rotate (new Vector3 (0, horizontal * speed * Time.deltaTime, 0));
		CameraTrans.Rotate (new Vector3 ( - vertical * speed * Time.deltaTime , 0 , 0) );
	}
}
