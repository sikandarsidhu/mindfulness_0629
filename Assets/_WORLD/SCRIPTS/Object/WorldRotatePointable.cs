using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotatePointable : SMWPointable {

	[SerializeField] float sensity = 1f;

	public override void GrabBegin ()
	{
		base.PointBegin ();

		lastFrame = OVRInput.GetLocalControllerRotation (m_pointerParent.m_controller).eulerAngles;

	}

	Vector3 lastFrame;
	public override void GrabUpdate ()
	{
		base.PointUpdate ();

		if (m_pointerParent != null) {
			Vector3 temAngle = OVRInput.GetLocalControllerRotation (m_pointerParent.m_controller).eulerAngles;

			Vector3 eular = temAngle - lastFrame;
			Debug.Log ("Speed in eular" + eular);

			eular.x = 0;
			eular.z = 0;
			if (eular.y > 180f)
				eular.y -= 360f;

			M_Event.FireLogicEvent(LogicEvents.WorldRotate , new WorldRotateArg(this, eular.y * sensity ));
			lastFrame = temAngle;
		}
	}

	public override void GrabExit ()
	{
		base.GrabExit ();
		M_Event.FireLogicEvent (LogicEvents.WorldRotateExit, new LogicArg (this));
	}
}
