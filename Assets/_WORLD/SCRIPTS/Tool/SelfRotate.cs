using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelfRotate : MBehavior {

	[SerializeField]float duration=1f;
	[SerializeField]Vector3 rotateAngle;
	[SerializeField]int reapeatTime = 1;
	[SerializeField]bool onAwake = true;
	[SerializeField]bool isRelative = true;

	protected override void MAwake ()
	{
		base.MAwake ();

		if (onAwake)
			DoSelfRotate ();
	}

	public void DoSelfRotate()
	{
		transform.DOLocalRotate (rotateAngle, duration).SetRelative(isRelative)
			.SetLoops (reapeatTime, LoopType.Incremental).SetEase (Ease.Linear);
	}
}
