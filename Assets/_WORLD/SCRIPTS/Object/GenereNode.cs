using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(SphereCollider))]
public class GenereNode : SMWPointable {

	float originalScale;
	Vector3 originalPosition{
		get {
			return origianlDirection * originalDistance;
		}
		set {
			origianlDirection = value.normalized;
		}
	}
	Vector3 origianlDirection;
	float originalDistance;
	[SerializeField] Ease easeType;
	[SerializeField] float fadeTime = 3f;
	SphereCollider m_sphereCollider;
	float originalColliderRadius;
	[SerializeField] GameObject glow;
	/// <summary>
	/// The rotating speed every second in degreed
	/// </summary>
	[SerializeField][ReadOnlyAttribute] float m_rotateSpeed = 1f;
	[SerializeField] Transform rotateCenter;
	[SerializeField][ReadOnlyAttribute] bool shouldRotate = true;
	[SerializeField][ReadOnlyAttribute] float m_progress;
	[SerializeField] ChildNode[] childrenNodes;
	[SerializeField] TMP_Text title;
	[SerializeField] TMP_Text enterTips;
	[SerializeField] float titleRadius = 80f;
	[SerializeField] AudioManager.GenreTitle genreTitle;

	public bool IsFront
	{
		get {
			return m_stateMachine.State == State.Front || m_stateMachine.State == State.FrontHover;
		}
	}

	public bool IsOrigin
	{
		get {
			return m_stateMachine.State == State.Original || m_stateMachine.State == State.Hover || m_stateMachine.State == State.Back;
		}
	}

	[System.Serializable]
	public class AnimationSetting
	{
		[Tooltip("The distance progress mutiplyer ")]
		public float distanceK = 0.66f;
		[Tooltip("The range of rotating speed in degreed")]
		public MinMax rotateSpeed = new MinMax (2f, 3f);
		[Tooltip("The scale mutiplyer when the planet move toward user")]
		public float forwardScaleK = 2f;
	};
	[SerializeField] AnimationSetting m_animationSetting;

	public enum State
	{
		None,
		Original,
		Front,
		Hover,
		Back,
		FrontHover,
	};

	AStateMachine<State,LogicEvents> m_stateMachine;
	[SerializeField][ReadOnlyAttribute] State m_state;

	[ReadOnlyAttribute][SerializeField] float currentDistance;

	float MoveProgress{
		get { return m_progress; }
		set { 
			m_progress = Mathf.Clamp (value, -1f, 1f);
			UpdateProgress (m_progress);
		}
	}
	[SerializeField] float progressSpeed = 0.01f;

//	public enum SelectType
//	{
//		Hover,
//		Point,
//	}
//	[SerializeField] SelectType selectType;

	public enum FocusType
	{
		Point,
		Grab
	}
	[SerializeField] FocusType focusType;


	#region BEHAVIOR
	protected override void MAwake ()
	{
		base.MAwake ();

		// record the initial data
		originalScale = transform.localScale.x;
		originalPosition = transform.position;
		originalDistance = transform.position.magnitude;
		m_sphereCollider = GetComponent<SphereCollider> ();
		originalColliderRadius = m_sphereCollider.radius;
		m_rotateSpeed = m_animationSetting.rotateSpeed.rand;

		// initialize the feature
		HideGlow ();
		HideEnter ();

		// init artist node
		if ( childrenNodes != null )
			foreach (ChildNode node in childrenNodes) {
				if ( node != null )
					node.Init (this);	
			}

		InitStateMachine ();
	}

	protected override void MUpdate ()
	{
		base.MUpdate ();
//		RotateWithCenter ();
		m_state = m_stateMachine.State;
		currentDistance = transform.position.magnitude;
		m_stateMachine.Update ();
	}

	protected override void MOnEnable ()
	{
		base.MOnEnable ();
		M_Event.RegisterEvent (LogicEvents.PointAtGenereNodeBegin, PointAtGenreNodeBegin);
		M_Event.RegisterEvent (LogicEvents.PointAtGenereNodeExit, PointAtGenreNodeExit);
		M_Event.RegisterEvent (LogicEvents.GrabGenereNodeUpdate, GrabGenreNodeUpdate);
		M_Event.RegisterEvent (LogicEvents.WorldRotate, OnWorldRotate);
	}

	protected override void MOnDisable ()
	{
		base.MOnDisable ();
		M_Event.UnregisterEvent (LogicEvents.PointAtGenereNodeBegin, PointAtGenreNodeBegin);
		M_Event.UnregisterEvent (LogicEvents.PointAtGenereNodeExit, PointAtGenreNodeExit);
		M_Event.UnregisterEvent (LogicEvents.GrabGenereNodeUpdate, GrabGenreNodeUpdate);
		M_Event.UnregisterEvent (LogicEvents.WorldRotate, OnWorldRotate);
	}
	#endregion

	#region OVERRIDE
	public override void PointBegin ()
	{
		base.PointBegin ();


		if (focusType == FocusType.Point) {
//			Debug.Log ("Point Begin ");
//			M_Event.FireLogicEvent (LogicEvents.PointAtGenereNodeBegin, new LogicArg (this));
//			MoveForward ();
			if ( m_stateMachine.State == State.Hover)
				m_stateMachine.State = State.Front;
			else if ( m_stateMachine.State == State.Original)
				m_stateMachine.State = State.Front;
		}
	}

	public override void PointExit ()
	{
		base.PointExit ();

		if (focusType == FocusType.Point) {
		}
	}

	public override void HoverBegin ()
	{
		base.HoverBegin ();

		if (m_stateMachine.State == State.Original)
			m_stateMachine.State = State.Hover;
		else if (m_stateMachine.State == State.Front)
			m_stateMachine.State = State.FrontHover;
	}

	public override void HoverExit ()
	{
		base.HoverExit ();

//		if (selectType == SelectType.Hover)
//			HideGlow ();
		if (m_stateMachine.State == State.Hover)
			m_stateMachine.State = State.Original;
		else if (m_stateMachine.State == State.FrontHover)
			m_stateMachine.State = State.Front;
	}

	public override void GrabBegin ()
	{
		base.GrabBegin ();
		if (focusType == FocusType.Grab   && IsOrigin ) {
			shouldRotate = false;
		} else if (IsFront) {
			
		}
	}

	Vector3 LastControllerPosition;
	float deltaProgressSpeed;
	Vector3 lastContactPoint;
	public override void GrabUpdate ()
	{
		base.GrabUpdate ();

		if (focusType == FocusType.Grab && m_pointerParent != null && IsOrigin) {
			Vector3 pos = OVRInput.GetLocalControllerPosition (m_pointerParent.m_controller);
			Vector3 delta = pos - LastControllerPosition;
			Vector3 forward = Global.Center - transform.position; 
			float deltaProgressAcc = (Mathf.Sign (Vector3.Dot (delta, forward.normalized)))
			                         * Mathf.Pow (Mathf.Abs (Vector3.Dot (delta, forward.normalized)), 0.5f)
			                         * progressSpeed;
			deltaProgressSpeed += deltaProgressAcc * Time.deltaTime; 
			deltaProgressSpeed = Mathf.Clamp (deltaProgressSpeed, -0.05f, 0.05f);

			if (Mathf.Abs (MoveProgress) > 0.8f)
				deltaProgressSpeed = Mathf.Lerp (deltaProgressSpeed, 0, 3f * Time.deltaTime);
			
			MoveProgress += deltaProgressSpeed;
			GenereNodeGrabUpdateArg arg = new GenereNodeGrabUpdateArg (this);
			arg.deltaProgress = deltaProgressSpeed;
			M_Event.FireLogicEvent (LogicEvents.GrabGenereNodeUpdate, arg);

			LastControllerPosition = pos;
		} else if (IsFront) {
			Vector3 contactPoint = m_pointerParent.ContactPoint;
			Vector3 delta = contactPoint - lastContactPoint; 


			SelfRotate ( contactPoint , delta);

			lastContactPoint = contactPoint;
		}
	}

	public override void GrabExit ()
	{
		base.GrabExit ();

		if (focusType == FocusType.Grab && IsOrigin) {
			StartCoroutine (GrabEndUpdate ());
		} else if (IsFront) {
		}

	}

	IEnumerator GrabEndUpdate(  )
	{
		
		while ( Mathf.Abs(deltaProgressSpeed) > 0.02f) {
			
			if ( Mathf.Abs(MoveProgress) > 0.8f )
				deltaProgressSpeed = Mathf.Lerp (deltaProgressSpeed , 0 , 3f * Time.deltaTime);
			
//			deltaProgressSpeed = Mathf.Lerp (deltaProgressSpeed, 0, 1f * Time.deltaTime);

			MoveProgress += deltaProgressSpeed;

			GenereNodeGrabUpdateArg arg = new GenereNodeGrabUpdateArg (this);
			arg.deltaProgress = deltaProgressSpeed;
			M_Event.FireLogicEvent (LogicEvents.GrabGenereNodeUpdate, arg);

			yield return new WaitForEndOfFrame ();
		}

		deltaProgressSpeed = 0;
	}
	#endregion

	#region Event
	void PointAtGenreNodeBegin( LogicArg arg )
	{
		if (arg.sender != this) {
			m_stateMachine.State = State.Back;
		}
	}
	void PointAtGenreNodeExit( LogicArg arg )
	{
//		Debug.Log ("Exit ");
		if (arg.sender != this) {
			m_stateMachine.State = State.Original;
		}
	}	
	void GrabGenreNodeUpdate( LogicArg arg )
	{
		if (arg.sender != this) {
			GenereNodeGrabUpdateArg gArg = (GenereNodeGrabUpdateArg)arg;
			if ( MoveProgress < 0 || ( MoveProgress > 0 && gArg.deltaProgress > 0) )
				MoveProgress -= gArg.deltaProgress;

			shouldRotate = false;
		}
	}

	void OnWorldRotate( LogicArg arg )
	{
//		WorldRotateArg wArg = (WorldRotateArg)arg;
//		RotateWithCenter (wArg.angle);

	}
	#endregion

	#region FUNCTION
	/////////// Function ////////////

	public void UpdateTitle()
	{
		if (title != null) {
			Vector3 toward = Global.UserPosition - transform.position;
			title.transform.position = transform.position + toward.normalized * titleRadius;
			title.transform.forward = - toward.normalized;
		}
		if (enterTips != null) {
			Vector3 toward = Global.UserPosition - transform.position;
			enterTips.transform.position = transform.position + toward.normalized * titleRadius;
			enterTips.transform.forward = - toward.normalized;
		}
	}

	public void InitStateMachine()
	{
		m_stateMachine = new AStateMachine<State, LogicEvents> ();

		m_stateMachine.AddEnter (State.Front, delegate {
			M_Event.FireLogicEvent (LogicEvents.PointAtGenereNodeBegin, new LogicArg (this));
			MoveForward();
			HideGlow();
			shouldRotate = false;

			foreach(ChildNode node in childrenNodes ) {
				if ( node != null )
					node.Show( fadeTime );
			}

		});

		m_stateMachine.AddExit (State.Front, delegate() {
			shouldRotate = true;
		});

		m_stateMachine.AddEnter (State.Hover, delegate {
			ShowGlow(0.2f);	
			UpdateTitle();
			AudioManager.PlayGenreMusic(genreTitle);
		});

		m_stateMachine.AddExit (State.Hover, delegate {
			HideGlow(0.5f);	
		});

		m_stateMachine.AddEnter (State.Original, delegate {
			MoveToOriginal();
			shouldRotate = true;

			foreach(ChildNode node in childrenNodes ) {
				if ( node != null )
					node.Hide( fadeTime );
			}
		});

		m_stateMachine.AddUpdate (State.Original, delegate {
			RotateWithCenter();	
		});

		m_stateMachine.AddEnter (State.Back, delegate {
			MoveBackward();
			shouldRotate = true;
		});

		m_stateMachine.AddUpdate (State.Back, delegate {
			RotateWithCenter();	
			UpdateTitle();	
		});

		m_stateMachine.AddEnter (State.FrontHover, delegate {
			ShowEnter(0.2f);	
			UpdateTitle();
		});

		m_stateMachine.AddExit (State.FrontHover, delegate {
			HideEnter(0.5f);	
		});

		m_stateMachine.State = State.Original;

	}

	public void UpdateProgress( float progress )
	{
		transform.position = Vector3.LerpUnclamped (originalPosition, Global.Center, progress * m_animationSetting.distanceK);

		float scaleK = m_animationSetting.forwardScaleK - 1f;
		transform.localScale = Vector3.one * ((progress > 0) ? (1f + progress * scaleK) : 1f);
		m_sphereCollider.radius = originalColliderRadius * ((progress > 0) ? (1f - progress * 0.66f) : 1f);
	}

	public void MoveForward()
	{
//		Debug.Log ("Move Forward");
//		shouldRotate = false;
//		transform.DOMove (Vector3.Lerp (originalPosition, Global.Center , 0.5f ) , fadeTime ).SetEase(easeType);
//		transform.DOScale (originalScale * 2f, fadeTime).SetEase (easeType);
//		DOTween.To (() => m_sphereCollider.radius, (x) => m_sphereCollider.radius = x, originalColliderRadius / 6f, fadeTime).SetEase (easeType);
		DOTween.To(() => MoveProgress , (x) => MoveProgress = x , 1f , fadeTime ).SetEase(easeType);
	}

	public void MoveToOriginal()
	{
//		transform.DOMove (originalPosition, fadeTime ).SetEase(easeType);
//		transform.DOScale (originalScale , fadeTime).SetEase (easeType);
//		DOTween.To (() => m_sphereCollider.radius, (x) => m_sphereCollider.radius = x, originalColliderRadius, fadeTime).SetEase (easeType);
//		shouldRotate = true;
		DOTween.To(() => MoveProgress , (x) => MoveProgress = x , 0 , fadeTime ).SetEase(easeType);
	}

	public void MoveBackward()
	{
//		shouldRotate = false;
//		transform.DOMove (Vector3.LerpUnclamped (originalPosition, Camera.main.transform.position , -0.5f ) , fadeTime).SetEase(easeType);	
//		transform.DOScale (originalScale , fadeTime).SetEase (easeType);
//		DOTween.To (() => m_sphereCollider.radius, (x) => m_sphereCollider.radius = x, originalColliderRadius , fadeTime).SetEase (easeType);
		DOTween.To(() => MoveProgress , (x) => MoveProgress = x , -1f , fadeTime ).SetEase(easeType);
	}

	public void ShowGlow( float duration = 0 )
	{
		if ( glow != null )
			glow.SetActive (true);
		if (title != null) {
			title.DOKill ();
			title.DOFade (1f, duration );
		}
	}

	public void HideGlow( float duration = 0 )
	{
		if ( glow != null )
			glow.SetActive (false);
		if (title != null) {
			title.DOKill ();
			title.DOFade (0f, duration);
		}
	}

	public void ShowEnter( float duration = 0 )
	{
		if ( glow != null )
			glow.SetActive (true);
		if (enterTips != null) {
			enterTips.DOKill ();
			enterTips.DOFade (1f, duration );
		}
	}

	public void HideEnter ( float duration = 0 )
	{

		if ( glow != null )
			glow.SetActive (false);
		if (enterTips != null) {
			enterTips.DOKill ();
			enterTips.DOFade (0f, duration);
		}
	}
		
	public void RotateWithCenter()
	{
		RotateWithCenter (Time.deltaTime * m_rotateSpeed);
	}


	public void RotateWithCenter( float angle )
	{

		if (shouldRotate) {

			Vector3 center = (rotateCenter == null) ? Vector3.zero : rotateCenter.transform.position;

			Vector3 axis = (rotateCenter == null) ? Vector3.up : rotateCenter.transform.up;

			transform.RotateAround (center, axis, angle );

			// update the direction
			originalPosition = transform.position;

		}
	}

	public void SelfRotate( Vector3 contact , Vector3 delta )
	{
		Vector3 localContact = transform.InverseTransformPoint (contact);
		Vector3 axis = Vector3.Cross (localContact.normalized, delta.normalized);

		float angle = Vector3.Angle (localContact, localContact + delta);

		transform.Rotate (axis, angle, Space.Self);
	}



	#endregion
}
