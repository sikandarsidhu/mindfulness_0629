using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistNode : ChildNode {

	[Header("=== Artist Node ===")]
	[SerializeField] AudioManager.ArtistTitle artistTitle;

	override protected void OnSelected ()
	{
		base.OnSelected ();
		AudioManager.PlayAristMusic (artistTitle);
	}
}
