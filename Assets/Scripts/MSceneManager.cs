using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MSceneManager : MonoBehaviour {


	static MSceneManager m_Instance;
	static public MSceneManager Instance{
		get {
			if (m_Instance == null)
				m_Instance = FindObjectOfType<MSceneManager> ();
			return m_Instance;
		}
	}

	[SerializeField] UnityStandardAssets.ImageEffects.ScreenOverlay overlayEffect;
	[SerializeField] bool isFadeInOnAwake = true;

	public void Awake()
	{
		if (isFadeInOnAwake) {
			overlayEffect.intensity = 1f;
			DOTween.To (() => overlayEffect.intensity, (x) => overlayEffect.intensity = x, 0, 2f);
		}
	}

	public void SwitchScene( string toScene , float duration ) {

		DOTween.To (() => overlayEffect.intensity, (x) => overlayEffect.intensity = x , 1f, duration).OnComplete (delegate {
			UnityEngine.SceneManagement.SceneManager.LoadScene( toScene );	
		});

	}
}
