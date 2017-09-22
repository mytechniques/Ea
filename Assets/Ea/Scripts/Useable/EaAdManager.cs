using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
using UnityEngine.Advertisements;

public class EaAdManager : Singleton<EaAdManager> {
	
	void Start () {
		EaMobile.Initialize (Mobile.Advertisement);
		EaAdvertisement.Create (AdType.BANNER_TOP, AdType.BANNER_BOTTOM, AdType.INTERSTITIAL);
//		EaDictionary<string,string> test = new EaDictionary<string, string> (){ { "hello","world" },{"hi","eru"} };
//		EaFileSystem.SetInt("Hello",999);
		Debug.Log (EaFileSystem.GetInt ("Hello"));
	

	}


	public void SHOW_ALL_BANNER(){
		SHOW_BANNER_TOP ();
		SHOW_BANNER_BOTTOM ();
	}
	public void HIDE_ALL_BANNER(){
		HIDE_BANNER_TOP ();
		HIDE_BANNER_BOTTOM ();
	}
	public void SHOW_BANNER_TOP(){
		EaAdvertisement.Show (AdType.BANNER_TOP);
	
	}
	public void SHOW_BANNER_BOTTOM(){
		EaAdvertisement.Show( AdType.BANNER_BOTTOM);
	}
	public void HIDE_BANNER_TOP(){
		EaAdvertisement.Hide( AdType.BANNER_TOP);
	}
	public void HIDE_BANNER_BOTTOM(){
		EaAdvertisement.Hide( AdType.BANNER_BOTTOM);
	}

	public void SHOW_INTERSTITIAL(){
		EaAdvertisement.Show (AdType.INTERSTITIAL);
	}
	public void SHOW_REWARDED_VIDEO(System.Action<ShowResult>resultCallback){
		EaAdvertisement.ShowVideo (resultCallback);
	}
	public void SHOW_REWARDED_VIDEO(System.Action onComplete,System.Action onFailed){
		EaAdvertisement.ShowVideo (onComplete,onFailed);
	}

	public void SHOW_VIDEO(){
		EaAdvertisement.ShowVideo ();

	}

}
