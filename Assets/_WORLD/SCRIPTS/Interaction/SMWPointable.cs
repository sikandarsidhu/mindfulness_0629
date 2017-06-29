using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMWPointable : SMWInteractable {

	[SerializeField][ReadOnlyAttribute] protected SMWPointer m_pointerParent;

	public void Init( SMWPointer parent )
	{
		m_pointerParent = parent;
	}

	virtual public void PointBegin()
	{
//		Debug.Log (name + "Point Begin");
	}

	virtual public void PointUpdate()
	{
	}

	virtual public void PointExit ()
	{
//		Debug.Log (name + " Point Exit");
	}

	virtual public void HoverBegin()
	{
	}

	virtual public void HoverUpdate()
	{
	}

	virtual public void HoverExit()
	{
	}

	virtual public void GrabBegin()
	{
	}

	virtual public void GrabUpdate()
	{
	}

	virtual public void GrabExit()
	{
	}

	virtual public string GetTips()
	{
		return "Pointable Object";
	}
}
