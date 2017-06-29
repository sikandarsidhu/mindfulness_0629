using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMWInputManager : MBehavior {
	private static SMWInputManager _instance;
	public static SMWInputManager instance{
		get{
			if( !_instance ){
				// check if an ObjectPoolManager is already available in the scene graph
				_instance = FindObjectOfType( typeof( SMWInputManager ) ) as SMWInputManager;
			}
			return _instance;
		}
	}

	public OVRInput.Controller controller;
	public OVRInput.Button backButton;

	protected override void MFixedUpdate ()
	{
		base.MFixedUpdate ();

		if (OVRInput.GetDown ( backButton , controller)) {
			M_Event.FireLogicEvent (LogicEvents.PointAtGenereNodeExit, new LogicArg (this));
		}
	}
}
