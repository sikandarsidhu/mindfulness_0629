using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MBehavior {

	private static AudioManager _instance;
	public static AudioManager instance{
		get{
			if( !_instance ){
				// check if an ObjectPoolManager is already available in the scene graph
				_instance = FindObjectOfType( typeof( AudioManager ) ) as AudioManager;
			}

			return _instance;
		}
	}

	public AudioClip[] genreList;
	public AudioClip[] artistList;
	public static ArtistTitle[] artistTitleList = {
		ArtistTitle.Sia,
		ArtistTitle.Chainsmokers,
		ArtistTitle.DaftPunk,
		ArtistTitle.Beyonce,
		ArtistTitle.Bowie,
		ArtistTitle.Hendrix,
		ArtistTitle.Harris,
		ArtistTitle.Prince,
		ArtistTitle.Floyd,
		ArtistTitle.MJ,
		ArtistTitle.WalkMoon,
		ArtistTitle.Cole,
	};
		
	public enum ArtistTitle
	{
		Sia,
		Chainsmokers,
		DaftPunk,
		Beyonce,
		Bowie,
		Hendrix,
		Harris,
		Prince,
		Floyd,
		MJ,
		WalkMoon,
		Cole

	}

	public static GenreTitle[] genreTitleList = {
		GenreTitle.Country,
		GenreTitle.EDM,
		GenreTitle.Jazz,
		GenreTitle.Pop,
		GenreTitle.Rap,
		GenreTitle.RB,
		GenreTitle.Rock,
		GenreTitle.World,
		GenreTitle.Empty
	};

	public enum GenreTitle
	{
		Pop,
		EDM,
		Rock,
		Country,
		RB,
		Jazz,
		Rap,
		World,
		Empty

	}



	void Start()
	{
		//Fabric.EventManager.Instance.PostEvent ("PlayRandomSong");
		//Fabric.EventManager.Instance.PostEvent ("PlayRandomSong", Fabric.EventAction.SetSwitch, "Rock", null);

//		PlayGenreMusic (GenreTitle.Rock);
		PlayGenreMusic (GenreTitle.Empty);
	}




	void Update(){

	
		if (Input.GetKeyDown (KeyCode.E))
			PlayGenreMusic (GenreTitle.EDM);
		if (Input.GetKeyDown (KeyCode.R))
			PlayGenreMusic (GenreTitle.Rock);
	}


	public static void PlayGenreMusic( GenreTitle title )
	{
		Debug.Log ("Play  " + title);
		Fabric.EventManager.Instance.PostEvent ("PlayRandomSong", Fabric.EventAction.SetSwitch, Title2StrG(title), null);
	}

	public static void PlayAristMusic( ArtistTitle title )
	{
		Debug.Log ("Play Music " + title);

		foreach( ArtistTitle t in artistTitleList )
			if ( t != title )
				Fabric.EventManager.Instance.SetParameter ("PlaySong", Title2Str(t), 0f, null);
		
		Fabric.EventManager.Instance.SetParameter ("PlaySong", Title2Str(title), 0.5f, null);

//		Fabric.EventManager.Instance.PostEvent ("PlaySong");
	}
		
	public static string Title2Str(ArtistTitle title)
	{
		switch (title) {
		case ArtistTitle.Sia:
			return "Sia";
		case ArtistTitle.Chainsmokers:
			return "Chainsmokers";
		case ArtistTitle.DaftPunk:
			return "DaftPunk";
		case ArtistTitle.Beyonce:
			return "Beyonce";
		case ArtistTitle.Bowie:
			return "Bowie";
		case ArtistTitle.Hendrix:
			return "Hendrix";
		case ArtistTitle.Harris:
			return "Harris";
		case ArtistTitle.Prince:
			return "Prince";
		case ArtistTitle.Floyd:
			return "Floyd";
		case ArtistTitle.WalkMoon:
			return "WalkMoon";
		case ArtistTitle.MJ:
			return "MJ";
		case ArtistTitle.Cole:
			return "Cole";


		default:
			break;
		};
		return "";
	}


	public static string Title2StrG (GenreTitle title)
	{
		switch (title) {
		case GenreTitle.EDM:
			return "EDM";
		case GenreTitle.Rock:
			return "Rock";
		case GenreTitle.Pop:
			return "Pop";
		case GenreTitle.Country:
			return "Country";
		case GenreTitle.RB:
			return "RB";
		case GenreTitle.Rap:
			return "Rap";
		case GenreTitle.World:
			return "World";
		case GenreTitle.Jazz:
			return "Jazz";
		case GenreTitle.Empty:
			return "Empty";
		default:
			break;
		};
		return "";
	}


}

