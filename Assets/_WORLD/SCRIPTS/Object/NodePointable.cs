using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodePointable : SMWPointable {

	[SerializeField] UnityEvent beginEvent;
	[SerializeField] UnityEvent exitEvent;
	[SerializeField] UnityEvent pointOverEvent;

	public override void PointBegin ()
	{
		base.PointBegin ();
		beginEvent.Invoke ();
	}

	public override void PointExit ()
	{
		base.PointExit ();
		exitEvent.Invoke ();
	}

	public override void PointUpdate ()
	{
		base.PointUpdate ();
		pointOverEvent.Invoke ();
	}
}
