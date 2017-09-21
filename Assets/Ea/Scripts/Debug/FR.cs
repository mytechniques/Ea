using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
[System.Serializable]
public class TestData : EaSerializable{
	public int somedata;
	public List<int> dList = new List<int> {
		1, 12, 31, 23, 123, 12, 312, 312, 31, 23, 123, 
		12, 31, 23, 123, 12, 321, 3, 123, 123, 12, 31, 
		23, 12, 312, 3, 123, 123, 12, 3, 123, 12, 3, 123, 
		12, 31, 23, 123, 12, 3, 123, 123, 12, 3, 123, 123,
		12, 3, 123, 12, 3, 123, 12, 312, 3, 123, 123,
	};

}
public class FR : MonoBehaviour {
	TestData data;
	// Use this for initialization
	void Start () {
		for(int i = 0;i<2;i++)
			data= EaFile.Open<TestData> (true);

		EaMobile.Initialize (Mobile.Advertisement,Mobile.Leaderboard);
		EaAdvertisement.Create (AdType.BANNER_TOP, AdType.BANNER_BOTTOM, AdType.INTERSTITIAL);
	}
	
	// Update is called once per frame
	public void ShowBanner(){
		EaAdvertisement.Show (AdType.BANNER_TOP, AdType.BANNER_BOTTOM);
	
	}
	public void HideBanner(){
		EaAdvertisement.Hide (AdType.BANNER_TOP, AdType.BANNER_BOTTOM);
	}
	public void ShowInterstitial(){
		EaAdvertisement.Show (AdType.INTERSTITIAL);
	}
	public void ShowVideo(){
		EaAdvertisement.ShowVideo ();
//		EaAdvertisement.ShowVideo (()=>Debug.Log ("Success"));
	}
}
