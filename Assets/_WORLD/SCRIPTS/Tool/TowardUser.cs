using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardUser : MBehavior {

	protected override void MUpdate ()
	{
		base.MUpdate ();
		Vector3 toward = Global.UserPosition - transform.position;
		transform.forward = toward;
	}
}
