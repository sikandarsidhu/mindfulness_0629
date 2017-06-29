using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMWPointer : SMWInteractor {
	[Header("-Button-")]
	public OVRInput.Controller m_controller;  //Controller Choice
	public OVRInput.Axis1D m_pointButton;        //Button for Default Movement
	public OVRInput.Axis1D m_grabButton;        //Button for Default Movement

	[SerializeField] float pointBegin = 0.55f;
	[SerializeField] float pointEnd = 0.35f;

	[SerializeField] LayerMask pointMask; 
	[SerializeField] Transform HandAnchor;
	[SerializeField] PointEffect m_pointEffect;
//	[SerializeField] Camera m_Camera;
	[SerializeField][ReadOnlyAttribute] bool m_isPointing;
	public bool IsPointing { get { return m_isPointing; } }
	[SerializeField][ReadOnlyAttribute] float m_prevFlex;
	[SerializeField][ReadOnlyAttribute] bool m_isGrabing;
	public bool IsGrabing { get { return m_isGrabing; } }
	[SerializeField][ReadOnlyAttribute] float m_prevGrabFlex;
	public Vector3 ContactPoint;

	public enum InputType
	{
		VR,
		PC,
	}
	public InputType inputType;

	[SerializeField][ReadOnlyAttribute] SMWPointable m_pointable;
	public SMWPointable TemPointable{
		get {
			return m_pointable;
		}
		set {
			if (m_pointable != value) {
				if (m_pointable != null)
					m_pointable.PointExit ();
				m_pointable = value;
				if (m_pointable != null) {
					m_pointable.PointBegin ();
				}
			}
		}
	}

	[SerializeField][ReadOnlyAttribute] SMWPointable m_hoverPointable;
	public SMWPointable TemHoverPointable{
		get {
			return m_hoverPointable;
		}
		set {
			if (m_hoverPointable != value) {
				if (m_hoverPointable != null)
					m_hoverPointable.HoverExit ();
				m_hoverPointable = value;
				if (m_hoverPointable != null) {
					m_hoverPointable.Init (this);
					m_hoverPointable.HoverBegin ();
				}
			}
		}
	}

	[SerializeField][ReadOnlyAttribute] SMWPointable m_grabPointable;
	public SMWPointable TemGrabPointable{
		get {
			return m_grabPointable;
		}
		set {
			if (m_grabPointable != value) {
				if (m_grabPointable != null)
					m_grabPointable.GrabExit ();
				m_grabPointable = value;
				if (m_grabPointable != null)
					m_grabPointable.GrabBegin ();
			}
		}
	}

	/// <summary>
	/// get if user currently pointing with trigger
	/// </summary>
	protected override void MFixedUpdate ()
	{
		base.MFixedUpdate ();

		if (inputType == InputType.VR) {
			float prevFlex = m_prevFlex;
			m_prevFlex = OVRInput.Get (m_pointButton, m_controller);
			CheckForPointOrRelease (prevFlex);


			float grabPrevFlex = m_prevGrabFlex;
			m_prevGrabFlex = OVRInput.Get (m_grabButton, m_controller);
			CheckForGrabOrRelease (grabPrevFlex);
		}

		if (inputType == InputType.PC) {
			m_isPointing = Input.GetMouseButton (0);
			if (Input.GetMouseButtonDown (0))
				PointBegin ();
			if (Input.GetMouseButtonUp (0))
				PointExit ();

			m_isGrabing = Input.GetMouseButton (1);
			if (Input.GetMouseButtonDown (1))
				GrabBegin ();
			if (Input.GetMouseButtonUp (1))
				GrabExit ();
		}
	}

	/// <summary>
	/// Update the effect
	/// </summary>
	protected override void MUpdate ()
	{
		base.MUpdate ();

		m_pointEffect.UpdateEffect ( GetHandSourcePosition() , GetHandForward() );

		if (m_isPointing) {
			
			if (TemPointable != null)
				TemPointable.PointUpdate ();
		}

		if (m_isGrabing) {
			if (TemGrabPointable != null)
				TemGrabPointable.GrabUpdate ();
		}

		if (TemHoverPointable != null)
			TemHoverPointable.HoverUpdate ();

		checkPointingObject ( m_isPointing , m_isGrabing );
	}

	public Vector3 GetHandSourcePosition()
	{
		if ( inputType == InputType.VR )
			return HandAnchor.position;

		if (inputType == InputType.PC) {
//			return m_Camera.transform.position;
			return Camera.main.ScreenPointToRay (Input.mousePosition).origin ;
		}

		return Vector3.zero;
	}

	public Vector3 GetHandForward()
	{
		if ( inputType == InputType.VR )
			return HandAnchor.forward;
		if (inputType == InputType.PC) {
			Vector3 pos = Input.mousePosition;
			pos.y += Screen.height / 2f;
			return Camera.main.ScreenPointToRay ( pos ).direction;
		}

		return Vector3.forward;
	}

	public void checkPointingObject( bool isPointing , bool isGrabing)
	{
		RaycastHit hitInfo;
		Debug.DrawRay (GetHandSourcePosition (), GetHandForward () * 10f , Color.red);
		if (Physics.Raycast ( GetHandSourcePosition() , GetHandForward() , out hitInfo, 9999f, pointMask)) {
			
			TemHoverPointable = hitInfo.collider.GetComponent<SMWPointable> ();
			if ( isPointing )
				TemPointable = hitInfo.collider.GetComponent<SMWPointable> ();
			if (isGrabing && TemGrabPointable == null )
				TemGrabPointable = hitInfo.collider.GetComponent<SMWPointable> ();
			ContactPoint = hitInfo.point;
		} else {
			ContactPoint = Vector3.zero;
			TemHoverPointable = null;
			if ( isPointing )
				TemPointable = null;
		}
	}


	protected void CheckForPointOrRelease(float prevFlex)
	{
		if ((m_prevFlex >= pointBegin) && (prevFlex < pointBegin))
		{
			PointBegin();
		}
		else if ((m_prevFlex <= pointBegin ) && (prevFlex > pointEnd ))
		{
			PointExit ();
		}
	}

	protected void CheckForGrabOrRelease(float prevFlex)
	{
		if ((m_prevGrabFlex >= pointBegin) && (prevFlex < pointBegin))
		{
			GrabBegin();
		}
		else if ((m_prevGrabFlex <= pointBegin ) && (prevFlex > pointEnd ))
		{
			GrabExit ();
		}
	}

	public void PointBegin()
	{
		m_isPointing = true;
		m_pointEffect.Begin ();
	}

	public void PointExit()
	{
		m_isPointing = false;
		m_pointEffect.End ();
		TemPointable = null;
	}

	public void GrabBegin()
	{
		m_isGrabing = true;
	}

	public void GrabExit()
	{
		m_isGrabing = false;
		TemGrabPointable = null;
	}

	void OnDestory()
	{
		PointExit ();
	}
}
