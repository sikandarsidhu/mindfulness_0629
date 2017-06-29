using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackMask : MBehavior {

	[SerializeField] MeshRenderer render;
	[SerializeField] float delay = 1f;
	[SerializeField] float fadeTime = 0.2f;
	[SerializeField] Ease easeType;

	protected override void MAwake ()
	{
		base.MAwake ();

		if (render == null)
			render = GetComponent<MeshRenderer> ();
	}

	protected override void MOnEnable ()
	{
		base.MOnEnable ();
		M_Event.RegisterEvent (LogicEvents.PointAtGenereNodeBegin, OnNodeToFront);
		M_Event.RegisterEvent (LogicEvents.PointAtGenereNodeExit, OnNodeBack);
	}

	protected override void MOnDisable ()
	{
		base.MOnDisable ();
		M_Event.UnregisterEvent (LogicEvents.PointAtGenereNodeBegin, OnNodeToFront);
		M_Event.UnregisterEvent (LogicEvents.PointAtGenereNodeExit, OnNodeBack);
	}


	public void OnNodeToFront ( LogicArg arg )
	{
		Debug.Log ("On Node To Front");
		render.material.DOKill ();
		render.material.DOFade (0.7f, "_Color", fadeTime).SetEase(easeType).SetDelay(delay);
	}

	public void OnNodeBack ( LogicArg arg )
	{
		render.material.DOKill ();
		render.material.DOFade (0, "_Color", fadeTime ).SetEase(easeType);
	}
}
