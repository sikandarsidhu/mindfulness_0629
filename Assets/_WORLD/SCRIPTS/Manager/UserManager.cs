using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UserManager : MBehavior {

	[SerializeField] UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration effect;

	public enum State
	{
		None,
		Normal,
		Rotate,
		Front,
	}
	AStateMachine<State,LogicEvents> m_stateMachine;

	protected override void MAwake ()
	{
		base.MAwake ();
		InitStateMachine ();
	}
		
	protected override void MOnEnable ()
	{
		base.MOnEnable ();
		M_Event.RegisterEvent (LogicEvents.WorldRotate, OnWorldRotate);
		M_Event.RegisterEvent (LogicEvents.WorldRotateExit, OnWorldRotateExit);
		M_Event.RegisterEvent (LogicEvents.PointAtGenereNodeBegin, OnPointAtGenereNodeBegin);
		M_Event.RegisterEvent (LogicEvents.PointAtGenereNodeExit, OnPointAtGenereNodeExit);
	}

	protected override void MOnDisable ()
	{
		base.MOnDisable ();
		M_Event.UnregisterEvent (LogicEvents.WorldRotate, OnWorldRotate);
		M_Event.UnregisterEvent (LogicEvents.WorldRotateExit, OnWorldRotateExit);
		M_Event.UnregisterEvent (LogicEvents.PointAtGenereNodeBegin, OnPointAtGenereNodeBegin);
		M_Event.UnregisterEvent (LogicEvents.PointAtGenereNodeExit, OnPointAtGenereNodeExit);
	}


	protected override void MUpdate ()
	{
		base.MUpdate ();
		m_stateMachine.Update (); 
	}

	void OnPointAtGenereNodeBegin( LogicArg arg )
	{
		m_stateMachine.State = State.Front;
	}


	void OnPointAtGenereNodeExit( LogicArg arg )
	{
		m_stateMachine.State = State.Normal;
	}

	void OnWorldRotate( LogicArg arg )
	{
		if (m_stateMachine.State == State.Normal)
			m_stateMachine.State = State.Rotate;

		if (m_stateMachine.State == State.Rotate) {
			WorldRotateArg wArg = (WorldRotateArg)arg;
			RotateWithCenter (-wArg.angle);
		}
	}

	void OnWorldRotateExit( LogicArg arg )
	{
		if (m_stateMachine.State == State.Rotate)
			m_stateMachine.State = State.Normal;
	}


	public void RotateWithCenter( float angle )
	{

			Vector3 center = Vector3.zero ;

			Vector3 axis = Vector3.up ;

			transform.RotateAround (center, axis, angle );
	}

	public void InitStateMachine()
	{

		m_stateMachine = new AStateMachine<State, LogicEvents> ();

		m_stateMachine.AddEnter (State.Rotate, delegate {
			DOTween.To(()=>effect.intensity , (x)=>effect.intensity = x , 80f , 0.5f );
		});

		m_stateMachine.AddExit (State.Rotate, delegate {
			DOTween.To(()=>effect.intensity , (x)=>effect.intensity = x , 0 , 0.5f );
			
		});

		m_stateMachine.State = State.Normal;
	}
}
