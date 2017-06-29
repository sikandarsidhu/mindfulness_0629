using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMesh : MonoBehaviour 
{
	//private Color m_color;
	[SerializeField][ReadOnlyAttribute] List <GameObject> m_childrenWithMeshes;
	[SerializeField][ReadOnlyAttribute] List <Mesh> m_childMeshes;
	[SerializeField][ReadOnlyAttribute] List <Mesh> m_childOriginalMeshes;

	public bool IsLine{ get { return m_isLine ; } }
	static bool m_isLine  = false ;
	public bool ToLine = false;

	// Use this for initialization
	public void Start () 
	{

		m_childrenWithMeshes = new List<GameObject>();
		m_childMeshes = new List<Mesh> ();
		m_childOriginalMeshes = new List<Mesh> ();

		foreach (Transform child in GetComponentsInChildren<Transform>()) 
		{
			if (child.tag == "Mesh") 
			{
				m_childrenWithMeshes.Add (child.gameObject);
				m_childMeshes.Add (new Mesh ());
				m_childOriginalMeshes.Add (new Mesh ());
			}
		}

		for (int i = 0; i < m_childMeshes.Count; i++) 
		{
			if(m_childrenWithMeshes[i].GetComponent<MeshFilter> () != null) m_childMeshes[i] = m_childrenWithMeshes[i].GetComponent<MeshFilter> ().sharedMesh;
			if ( m_childrenWithMeshes[i].GetComponent< MeshFilter >() != null ) m_childMeshes[i] = m_childrenWithMeshes[i].GetComponent< MeshFilter >().mesh; //added else
			else if ( m_childrenWithMeshes[i].GetComponent< SkinnedMeshRenderer >() != null ) m_childMeshes[i] = m_childrenWithMeshes[i].GetComponent< SkinnedMeshRenderer >().sharedMesh;
		}

		for (int i = 0; i < m_childOriginalMeshes.Count; i++) 
		{
			m_childOriginalMeshes[i].vertices = m_childMeshes [i].vertices;
			m_childOriginalMeshes[i].triangles = m_childMeshes [i].triangles;

			m_childOriginalMeshes[i].uv = new Vector2[m_childMeshes[i].uv.Length];
		}
	}

	bool lastState = false;
	void Update ()
	{
		if (lastState != ToLine) {
			if (ToLine)
				ToLines ();
			else
				ToOriginal ();
		}

		lastState = ToLine;
	}

	void ToLines()
	{
		print (gameObject.name + "in to lines, child count = " + m_childrenWithMeshes.Count + "and child mesh count = " + m_childMeshes.Count);
		for (int i = 0; i < m_childrenWithMeshes.Count; i++) 
		{
			if (m_childMeshes[i].GetTopology (0) != MeshTopology.Lines)
				m_childMeshes[i].SetIndices (m_childMeshes[i].triangles, MeshTopology.Lines, 0);
		}
		m_isLine = true;

		foreach (Transform child in GetComponentsInChildren<Transform>()) 
		{
			if (child.tag == "NonMesh") 
			{
				child.gameObject.GetComponent<Renderer> ().enabled = false;
			}
		}

	}

	void ToOriginal()
	{
		print (gameObject.name + "in to original");
		for (int i = 0; i < m_childMeshes.Count; i++) 
		{
			m_childMeshes[i].vertices = m_childOriginalMeshes[i].vertices;
			m_childMeshes[i].triangles = m_childOriginalMeshes[i].triangles;
		}
		m_isLine = false;

		foreach (Transform child in GetComponentsInChildren<Transform>()) 
		{
			if (child.tag == "NonMesh") 
			{
				child.gameObject.GetComponent<Renderer> ().enabled = true;
			}
		}

	}

//	void OnEnterCharacterRange( LogicArg arg )
//	{
//		MCharacter character = (MCharacter)arg.sender;
//		if ( character != null && character.gameObject == this.gameObject )
//			ToLines ();
//	}
//
//	void OnExitCharacterRange( LogicArg arg )
//	{
//		MCharacter character = (MCharacter)arg.sender;
//		if ( character != null && character.gameObject == this.gameObject )
//			ToOriginal ();
//	}

	void OnEnable()
	{
//		M_Event.logicEvents [(int)LogicEvents.EnterCharacterRange] += OnEnterCharacterRange;
//		M_Event.logicEvents [(int)LogicEvents.ExitCharacterRange] += OnExitCharacterRange;
	}

	void OnDisable()
	{
//		M_Event.logicEvents [(int)LogicEvents.EnterCharacterRange] -= OnEnterCharacterRange;
//		M_Event.logicEvents [(int)LogicEvents.ExitCharacterRange] -= OnExitCharacterRange;
		ToOriginal ();
	}

}