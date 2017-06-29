using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ChildNode : SMWPointable {
	[SerializeField][ReadOnlyAttribute] float m_speed;
	[SerializeField][ReadOnlyAttribute] SMWPointable m_nodeParent;
	[SerializeField] GameObject glow;
	[SerializeField] bool shouldRotate = true;
	[SerializeField][ReadOnlyAttribute] Transform rotateCenter;
//	[SerializeField][ReadOnlyAttribute] Vector3 axis;
	[SerializeField][ReadOnlyAttribute] float originalScale;
	[SerializeField] float fadeTime = 0.5f;
	[SerializeField] MinMax RotateLength = new MinMax( 100f , 120f );
	[SerializeField] MinMax forwardDistance = new MinMax( 20f , 50f );
	[SerializeField] float initAngle;
	[SerializeField] MinMax Speed = new MinMax(3f , 5f );
	[SerializeField] Transform title;
	[SerializeField] float titleRadius = 50f;
	[SerializeField] ChildNode[] childrenNodes;
	[SerializeField] ChildNode[] neigbours;

	public enum State
	{
		None,
		Hide,
		Back,
		Original,
		Selected,
		SelectedHover,
		Hover,
	}
	AStateMachine<State,LogicEvents> m_stateMachine;
	[SerializeField][ReadOnlyAttribute] State m_state;


	virtual	public void Init( SMWPointable parent )
	{
		m_nodeParent = parent;

		transform.parent = m_nodeParent.transform;
//		transform.localScale = Vector3.one * Random.Range( 0.8f , 0.5f);
		originalScale = transform.localScale.z;
		transform.DOScale (0.0001f, 0);

		m_speed = Speed.rand;

		rotateCenter = m_nodeParent.transform;

		HideGlow ();
		InitStateMachine ();

		if ( childrenNodes != null )
			foreach (ChildNode node in childrenNodes) {
				if ( node != null )
					node.Init (this);	
			}
	}

	virtual public void Show( float duration = 0 )
	{
		if ( m_stateMachine.State == State.Hide)
			m_stateMachine.State = State.Original;
	}

	virtual public void Hide( float duration = 0 )
	{
		m_stateMachine.State = State.Hide;
	}

	public override void PointBegin ()
	{
		Debug.Log ("Point Begin " + m_stateMachine.State );
		base.PointBegin ();
		if (m_stateMachine.State == State.Hover)
			m_stateMachine.State = State.SelectedHover;
		else if (m_stateMachine.State == State.Original)
			m_stateMachine.State = State.Selected;
		else if (m_stateMachine.State == State.Selected)
			m_stateMachine.State = State.Original;
		else if (m_stateMachine.State == State.SelectedHover)
			m_stateMachine.State = State.Hover;
	}

	public override void PointExit ()
	{
		base.PointExit ();
	}

	public override void HoverBegin ()
	{
		base.HoverBegin ();
		if ( m_stateMachine.State == State.Original )
			m_stateMachine.State = State.Hover;
		else if ( m_stateMachine.State == State.Selected )
			m_stateMachine.State = State.SelectedHover;
	}

	public override void HoverExit ()
	{
		base.HoverExit ();
		if ( m_stateMachine.State == State.Hover )
			m_stateMachine.State = State.Original;
		else if (m_stateMachine.State == State.SelectedHover)
			m_stateMachine.State = State.Selected;
	}

	protected override void MUpdate ()
	{
		base.MUpdate ();

		m_stateMachine.Update ();
		m_state = m_stateMachine.State;
	}

	#region FUNCTION

	public void UpdateTitle()
	{
		if (title != null) {
			Vector3 toward = Global.UserPosition - transform.position;
			title.transform.position = transform.position + toward.normalized * titleRadius;
			title.transform.up = Vector3.up;
			title.transform.forward = - toward.normalized;
		}
	}

	public void InitStateMachine()
	{
		m_stateMachine = new AStateMachine<State, LogicEvents> ();


		m_stateMachine.AddEnter (State.Hide, delegate {

			transform.DOScale (0.0001f , fadeTime);
			transform.DOLocalMove(Vector3.zero , fadeTime );
		});

		m_stateMachine.AddExit (State.Hide, delegate {
			transform.DOScale ( originalScale , fadeTime );	

			Vector3 forward = Global.UserPosition - m_nodeParent.transform.position;
			Vector3 xVector = Vector3.Cross( Vector3.up , forward.normalized);
			Vector3 yVector = Vector3.Cross( xVector.normalized , forward.normalized);
			Vector3 pos = RotateLength.rand * ( xVector * Mathf.Cos( initAngle * Mathf.Deg2Rad )
				+ yVector * Mathf.Sin( initAngle * Mathf.Deg2Rad ))
				+ forward.normalized * forwardDistance.rand;

			Vector3 localPosition = m_nodeParent.transform.InverseTransformPoint(pos + m_nodeParent.transform.position);
			transform.DOLocalMove( localPosition , fadeTime );
		});

		m_stateMachine.AddEnter (State.Original, delegate {
			Debug.Log("Enter Original " + name );

			foreach(ChildNode node in childrenNodes )
				node.Hide();
		});

		m_stateMachine.AddUpdate (State.Original, delegate {
			Rotate();
		});

		m_stateMachine.AddEnter (State.Hover, delegate {
			OnEnterHover();
			ShowGlow(0.2f);	
			UpdateTitle();	
//			AudioManager.PlayAristMusic(ArtistTitle);
		});

		m_stateMachine.AddExit (State.Hover, delegate {
			HideGlow(1f);	
		});

		m_stateMachine.AddEnter (State.SelectedHover, delegate {
//			OnEnterHover();
			ShowGlow(0.2f);	
			UpdateTitle();
		});

		m_stateMachine.AddExit (State.SelectedHover, delegate {
			HideGlow(1f);
		});

		m_stateMachine.AddEnter (State.Selected, delegate {
			UpdateTitle();
			Debug.Log("Select " + name );
			foreach(ChildNode node in childrenNodes )
				node.Show();
			OnSelected();
		});

		m_stateMachine.AddExit (State.Selected, delegate {
				
		});

		m_stateMachine.State = State.Hide;
	}

	virtual protected void OnEnterHover()
	{
		
	}

	virtual protected void OnSelected()
	{
		
	}

	public void Rotate()
	{
		if (shouldRotate) {
			Vector3 center = (rotateCenter == null) ? m_nodeParent.transform.position : rotateCenter.transform.position;
			Vector3 forward = Global.UserPosition - m_nodeParent.transform.position;
			transform.RotateAround (center, forward.normalized, Time.deltaTime * m_speed);

			Debug.DrawRay (center, forward);
		}
	}

	public void ShowGlow( float duration = 0 )
	{
		if ( glow != null )
			glow.SetActive (true);
		if (title != null) {
//			title.DOScale (1f, duration );
			title.gameObject.SetActive (true);
		}
	}

	public void HideGlow( float duration = 0 )
	{
		if ( glow != null )
			glow.SetActive (false);
		if (title != null) {
			title.gameObject.SetActive (false);
//			title.DOFade (0f, duration).OnComplete (delegate {
//				title.gameObject.SetActive (false);
//			});
		}
	}

	#endregion
}
