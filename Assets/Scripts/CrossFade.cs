using UnityEngine;
using System.Linq;
using DG.Tweening; // The DOtween, by Demigiant. Get it for free from http://dotween.demigiant.com/


//Credit Igor Aherne. Feel free to use as you wish, but mention me in credits :)
//www.facebook.com/igor.aherne

//audio source which holds a reference to Two audio sources, allowing to transition
//between incoming sound and the previously played one.
[ExecuteInEditMode]
public class DoubleAudioSource : MonoBehaviour {

	AudioSource _source0;
	AudioSource _source1;

	bool _isFirst = true; //is _source0 currently the active AudioSource (plays some sound right now)



	void Update() {

		//constantly check if our game object contains audio sources which we are referencing.

		//if the _source0 or _source1 contain obsolete references (most likely 'null'), then
		//we will re-init them:
		if (_source0 == null || _source1 == null) {

			//re-connect _soruce0 and _source1 to the ones in attachedSources[]
			Component[] attachedSources = gameObject.GetComponents(typeof(AudioSource));
			//For some reason, unity doesn't accept "as AudioSource[]" casting. We would get
			//'null' array instead if we would attempt. Need to re-create a new array:
			AudioSource[] sources = attachedSources.Select(c => c as AudioSource).ToArray();
			InitSources(sources);

			return;
		}

	}


	//re-establishes references to audio sources on this game object:
	void InitSources(AudioSource[] audioSources) {

		if( ReferenceEquals(audioSources, null) || audioSources.Length == 0) {
			_source0 = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			_source1 = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

			_source0.hideFlags = HideFlags.HideInInspector;
			_source1.hideFlags = HideFlags.HideInInspector;
			return;
		}

		switch (audioSources.Length) {
		case 1: {
				_source0 = audioSources[0];
				_source1 = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			}break;
		default: { //2 and more
				_source0 = audioSources[0];
				_source1 = audioSources[1];
			}break;
		}//end switch

		_source0.hideFlags = HideFlags.HideInInspector;
		_source1.hideFlags = HideFlags.HideInInspector;
	}


	//could be called in the editor.
	void Reset() {
		OnDestroy();
	}


	void OnDestroy() {

		#if UNITY_EDITOR
		if (_source0) {
			DestroyImmediate(_source0);
		}
		if (_source1) {
			DestroyImmediate(_source1);
		}
		#else
		if (_source0) {
		Destroy(_source0);
		}
		if (_source1) {
		Destroy(_source1);
		}
		#endif
	}



	//gradually shifts the sound comming from our audio sources to the this clip:
	// maxVolume should be in 0-to-1 range
	//
	//Requires "using DG.Tweening" The DOtween, by Demigiant.
	//Get it for free from http://dotween.demigiant.com/
	public void CrossFade(AudioClip playMe, float maxVolume, float fadingTime) {

		if (_isFirst) { // _source0 is currently playing the most recent AudioClip

			//so launch on source1
			_source1.clip = playMe;
			_source1.Play();

			_source1.DOKill();//remove any previous fading coroutines (if existed).
			_source1.DOFade(maxVolume, fadingTime);//launch a new fade-in coroutine
			_source0.DOKill();
			_source0.DOFade(0, fadingTime);//fade-out the source on which we played previously
			_isFirst = false;


			return;
		}

		//otherwise, _source1 is currently active, so play on _source0
		_source0.clip = playMe;
		_source0.Play();
		_source0.DOKill();
		_source0.DOFade(maxVolume, fadingTime);

		_source1.DOKill();
		_source1.DOFade(0, fadingTime);

		_isFirst = true;
	}//end CrossFade()

}