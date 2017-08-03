using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

    public string levelName;
    AsyncOperation async;

    public void Awake()
	{
		if (isFadeInOnAwake) {
			overlayEffect.intensity = 1f;
			DOTween.To (() => overlayEffect.intensity, (x) => overlayEffect.intensity = x, 0, 2f);
		}
	}

	public void SwitchScene( string toScene , float duration ) {

		DOTween.To (() => overlayEffect.intensity, (x) => overlayEffect.intensity = x , 1f, duration).OnComplete (delegate {
            if (toScene.Equals(levelName))
            {
                ActivateScene();
            }
            else
            {
                SceneManager.LoadScene(toScene);
            }
		});

	}

    public void StartLoading(string toScene)
    {
        levelName = toScene;
        StartCoroutine("load");
    }

    IEnumerator load()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        async = SceneManager.LoadSceneAsync(levelName);
        async.allowSceneActivation = false;
        yield return async;
    }

    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }
}
