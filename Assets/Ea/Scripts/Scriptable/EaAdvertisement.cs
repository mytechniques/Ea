﻿using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using System;
using GoogleMobileAds.Api;
using Ea;
using System.Linq;
using Sirenix.OdinInspector;

namespace Ea{
	public enum AdType{
		BANNER_TOP,
		BANNER_BOTTOM,
		INTERSTITIAL,
	}
public class EaAdvertisement : ScriptableObject {
		#region IENUMERATOR
		public enum EaAdSize
		{
			BANNER,
			SMART_BANNER,
			IAB_BANNER,
			LEADERBOARD,
			MEDIUM_RECTANGLE
		
		}
		public enum Banner{
			SINGLE_ID,
			MULTIPLER_ID,
		}
	
		#endregion
		#region SERIALIZABLE VARIABLE

		[HideInInspector]
		public bool isSingleBanner = true;

		[TabGroup("EaAdvertisement","Debug"),PropertyOrder(2)][Range(50,200)]
		public int testBannerHeight = 100;

		[TabGroup("EaAdvertisement","Debug"),PropertyOrder(1)]
		public bool googleDebug,eaDebug;

		[TabGroup("EaAdvertisement","Banner"),ShowIf("isSingleBanner")]
		public string banner;

		[TabGroup("EaAdvertisement","Banner"),HideIf("isSingleBanner")]
		public string bannerTop,bannerBottom;

		[TabGroup("EaAdvertisement","Banner")]
		public  EaAdSize adSize= EaAdSize.SMART_BANNER;

		[TabGroup("EaAdvertisement","Banner")]
		public  Banner bannerType  {
			get {return _bannerType;}
			set{
				_bannerType = value;
				isSingleBanner = _bannerType == Banner.SINGLE_ID ? true : false;
			}
		}

		private Banner _bannerType = Banner.SINGLE_ID;


		[TabGroup("EaAdvertisement","Interstitial")]
		public string  interstitial;

		#endregion
		#region MEMBER VARIABLE
		public const string reawardPlacementId = "rewardedVideo";
		public const string videoPlacementId = "video";
		static bool initalized;
		public static bool isRewardVideoAvaiable{get{return Advertisement.IsReady (reawardPlacementId);}}
		public static bool isVideoAvaiable{get{return Advertisement.IsReady (videoPlacementId);}}
		static BannerView bannerTopAd,bannerBottomAd;
		static InterstitialAd interstitialAd;
		private static EaAdvertisement _EaAd;
		public static EaAdvertisement EaAd{ get {return _EaAd ?? (_EaAd = Resources.Load<EaAdvertisement> (typeof(EaAdvertisement).Name)); }set{_EaAd = value;} }
		public static EaBanner dummyBannerTop, dummyBannerBottom;
		#endregion
		#region PUBLIC METHOD
		public static void ShowVideo(){
			if (Advertisement.IsReady ("video"))
				Advertisement.Show ();
		}
		public static void ShowVideo(Action onFinished,Action onFailed = null){
			if (Advertisement.IsReady (reawardPlacementId)) {
				ShowOptions options = new ShowOptions ();
				options.resultCallback = result => {
					switch(result){
					case ShowResult.Finished:
						onFinished .Call();
						break;
					case ShowResult.Failed:
						onFailed.Call();
						break;
					
					}
				};

				Advertisement.Show (reawardPlacementId,options);
			}	else if (EaAd.eaDebug)
				  Debug.Log ("VIDEO is not avaiable!".color("00FFFF"));
				
		}
		public static void ShowVideo(Action<ShowResult> resultCallback){

			if (Advertisement.IsReady (reawardPlacementId)) {
				ShowOptions options = new ShowOptions ();
				options.resultCallback = resultCallback;
				Advertisement.Show (reawardPlacementId, options);
			} else if (EaAd.eaDebug)
				Debug.Log ("VIDEO is not avaiable".color("00FFFF"));
		}
		public static void ShowVideo(UnityEngine.Events.UnityAction<ShowResult> e){
			ShowOptions op = new ShowOptions ();
			op.resultCallback = new Action<ShowResult> (e);
			
		}
		public static void Show(params AdType  [] types){
			Initialize ();
			types.ToList ().ForEach (type => {
				switch (type) {
				case AdType.BANNER_TOP:
					ShowBanner (ref bannerTopAd);

					break;
				case AdType.BANNER_BOTTOM:
					ShowBanner (ref bannerBottomAd);
					break;
				case AdType.INTERSTITIAL:
					if (interstitialAd != null && interstitialAd.IsLoaded ()){
						interstitialAd.Show ();
						if(EaAd.eaDebug){
							Debug.Log("SHOW INTERSTITIAL".color("FF0000"));
							ShowEditorAd(AdType.INTERSTITIAL);
						}
					}
					else
						if(EaAd.eaDebug)Debug.Log ("INTERSTITIAL wasn't loaded.");
					break;
				}
			});
		}
		public static void Create(params AdType [] types){
			types.ToList ().ForEach (type => {
				switch (type) {
				case AdType.BANNER_TOP:
					BANNER_TOP_OnAdClosed(null,null);
					break;
				case AdType.BANNER_BOTTOM:
					BANNER_BOTTOM_OnAdClosed(null,null);
					break;
				case AdType.INTERSTITIAL:
					INTERSTITIAL_OnAdClosed(null,null);
					break;
				}
			});
		}

		public  static void Hide(params AdType [] types){
			types.ToList ().ForEach (type => {
				switch(type){
				case AdType.BANNER_TOP:
					HideBanner (ref bannerTopAd);
					break;
				case AdType.BANNER_BOTTOM:
					HideBanner (ref bannerBottomAd);
					break;
				case AdType.INTERSTITIAL:
					if(EaAd.eaDebug)Debug.LogWarning("Can't hide INTERSTITIAL");
					break;
				}
			
			});

		}
		public static void Initialize(){
			if (initalized)
				return;

			initalized = true;
			if(EaAd.eaDebug)Debug.Log ("EaAdvetisement initialized!".color("0000FF"));
			EaAd = Resources.Load<EaAdvertisement> ("EaAdvertisement");
			CreateBanner (out bannerTopAd,GetBanner(AdPosition.Top), AdPosition.Top);
			CreateBanner (out bannerBottomAd, GetBanner(AdPosition.Bottom), AdPosition.Bottom);
			CreateInterstitial ();
		}
		#endregion
		#region PRIVATE METHOD
		 static void HideBanner(ref BannerView banner){
			if (banner != null) {
				banner.Hide ();
				if (EaAd.eaDebug) {
					Debug.Log (string.Format ("HIDE BANNER_{0}", banner == bannerTopAd ? "TOP" : "BOTTOM").color ("FF0000"));
					HideEditorAd (banner == bannerTopAd ? AdType.BANNER_TOP : AdType.BANNER_BOTTOM);
				}
			}
			else
				if(EaAd.eaDebug)Debug.LogWarning ("BANNER wasn't loaded!\n Hide failed.");
			
			
		}

		 static void ShowBanner(ref BannerView banner){
			if (banner != null) {
				banner.Show ();
				if (EaAd.eaDebug) {
					Debug.Log (string.Format ("SHOW BANNER_{0}", banner == bannerTopAd ? "TOP" : "BOTTOM").color ("FF0000"));
					ShowEditorAd(banner == bannerTopAd ? AdType.BANNER_TOP: AdType.BANNER_BOTTOM);
				}
			}
			
			
			else
				if(EaAd.eaDebug)Debug.LogWarning ("BANNER wasn't loaded!\n Show failed.");
		}
	
		static void CreateInterstitial(){
			AdRequest request = new AdRequest.Builder ().Build ();
			interstitialAd = new InterstitialAd (EaAd.interstitial);
			interstitialAd.OnAdClosed += INTERSTITIAL_OnAdClosed;
			interstitialAd.OnAdLoaded += INTERSTITIAL_OnAdLoaded;
			interstitialAd.LoadAd (request);


		}

		static void CreateBanner(out BannerView banner,string uid,AdPosition position){
			AdRequest request = new AdRequest.Builder ().Build ();
			banner = new BannerView (uid, GetAdSize() , position);
			banner.OnAdClosed += position == AdPosition.Top ? (EventHandler<EventArgs>)BANNER_TOP_OnAdClosed : (EventHandler<EventArgs>)BANNER_BOTTOM_OnAdClosed;
			banner.OnAdLoaded += position == AdPosition.Top ? (EventHandler<EventArgs>)BANNER_TOP_OnAdLoaded : (EventHandler<EventArgs>)BANNER_BOTTOM_OnAdLoaded;
			banner.LoadAd (request);

		}

		#endregion
		static void HideEditorAd(AdType type){
			#if UNITY_EDITOR
			switch(type){
			case AdType.BANNER_BOTTOM:
				if(dummyBannerBottom != null)
					Destroy(dummyBannerBottom.gameObject);


				break;
			case AdType.BANNER_TOP:
				if(dummyBannerTop != null)
					Destroy(dummyBannerTop.gameObject);
				
				break;
			}
			#endif
		}

		static void ShowEditorAd(AdType type){
			#if UNITY_EDITOR

			int BANNER_POSITION = 0;
			Canvas canvas = FindObjectOfType<Canvas>();
			UnityEngine.EventSystems.EventSystem sys= FindObjectOfType<UnityEngine.EventSystems.EventSystem >();
			if(canvas == null){
				canvas = new GameObject("Canvas").AddComponent<Canvas>();
				canvas.renderMode = RenderMode.ScreenSpaceCamera;
				canvas.worldCamera = FindObjectOfType<Camera>() ?? (new Camera(){orthographic = true,orthographicSize = 5,clearFlags = CameraClearFlags.Color});
				var scalar = canvas.gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();
				scalar.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
				scalar.referenceResolution =new Vector2(480,800);
				scalar.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
				scalar.matchWidthOrHeight  = 1;
				scalar.referencePixelsPerUnit = 100;
				canvas.gameObject.AddComponent<UnityEngine.UI.GraphicRaycaster>().ignoreReversedGraphics = true;

			}
			if(sys == null)
			new GameObject("EventSystem").AddComponent<UnityEngine.EventSystems.EventSystem>().gameObject.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
				

		
			UnityEngine.UI.CanvasScaler scaler = canvas.GetComponent<UnityEngine.UI.CanvasScaler>();

			switch (type) {
			case AdType.BANNER_BOTTOM:
				BANNER_POSITION  = -1;
				goto CREATE_BANNER;

			case AdType.BANNER_TOP:
				BANNER_POSITION = 1;	
				goto CREATE_BANNER;
			case AdType.INTERSTITIAL:
				EaInterstitial i = Resources.Load<EaInterstitial>("EaInterstitial");
				EaInterstitial interstitial =	Instantiate (i,Vector3.zero,Quaternion.identity) as EaInterstitial;

				interstitial.rect.sizeDelta = scaler.referenceResolution;
				interstitial.rect.localScale = Vector3.one;
				interstitial.rect.localPosition = Vector2.zero;
				interstitial.rect.SetParent(canvas.transform,false);
				break;
				CREATE_BANNER:
				if((BANNER_POSITION == 1 && dummyBannerTop != null) || (BANNER_POSITION == -1 && dummyBannerBottom != null))
					return;
				
				EaBanner b = Resources.Load<EaBanner>("EaBanner");
				EaBanner banner =	Instantiate (b,Vector3.zero,Quaternion.identity) as EaBanner;
				banner.transform.SetParent(canvas.transform,false);
				banner.rect.sizeDelta = new Vector2(scaler.referenceResolution.x,EaAd.testBannerHeight);
				banner.rect.localScale = Vector3.one;
				banner.rect.localPosition = new Vector2(0, BANNER_POSITION * ((scaler.referenceResolution.y/2) - (EaAd.testBannerHeight.abs()/ 2)));
				switch(BANNER_POSITION){
				case -1:
					dummyBannerBottom =banner;
					break;
				case 1:
					dummyBannerTop = banner;
					break;
				}
				break;
			}
			#endif
		}

		#region CALLBACK
		static void BANNER_TOP_OnAdLoaded(object sender,EventArgs e){
			if (bannerTopAd != null) {
				bannerTopAd.Hide ();
				if(EaAd.eaDebug) Debug.Log ("BANNER_TOP was loaded");
			}
		}
		static void BANNER_BOTTOM_OnAdLoaded(object sender,EventArgs e){
			if (bannerBottomAd != null) {
				bannerBottomAd.Hide ();
				if(EaAd.eaDebug) Debug.Log ("BANNER_BOTTOM was loaded");

			}
		}

		static void BANNER_TOP_OnAdClosed(object sender,EventArgs e){
			if (bannerTopAd != null) {
				bannerTopAd.OnAdClosed -= BANNER_TOP_OnAdClosed;
				bannerTopAd.OnAdLoaded -= BANNER_TOP_OnAdLoaded;
				bannerTopAd.Destroy ();
			}
			CreateBanner (out bannerTopAd, GetBanner(AdPosition.Top), AdPosition.Top);
		}
		static void BANNER_BOTTOM_OnAdClosed(object sender,EventArgs e){
			if (bannerBottomAd != null) {
				bannerBottomAd.OnAdClosed -= BANNER_BOTTOM_OnAdClosed;
				bannerBottomAd.OnAdLoaded -= BANNER_BOTTOM_OnAdLoaded;
				bannerBottomAd.Destroy ();
			}
			CreateBanner (out bannerBottomAd, GetBanner(AdPosition.Bottom), AdPosition.Bottom);
		}
		static void INTERSTITIAL_OnAdClosed (object sender, EventArgs e)
		{
			if (interstitialAd != null) {
				interstitialAd.OnAdClosed -= INTERSTITIAL_OnAdClosed;
				interstitialAd.OnAdLoaded -= INTERSTITIAL_OnAdLoaded;
				interstitialAd.Destroy ();
			}
			CreateInterstitial ();
		}
		static void INTERSTITIAL_OnAdLoaded(object sender,EventArgs e){
			if (interstitialAd.is_avaiable() && interstitialAd.IsLoaded ()) {
				if (EaAd.eaDebug) 
					Debug.Log ("INTERTSTITIAL was loaded");
				
			}
		}
	

		#endregion

		#region RETURN VALUE 
		static AdSize GetAdSize(){
			switch (EaAd.adSize) {
			case EaAdSize.BANNER:
				return AdSize.Banner;
			case  EaAdSize.IAB_BANNER:
				return AdSize.IABBanner;
			case EaAdSize.LEADERBOARD:
				return AdSize.Leaderboard;
			case EaAdSize.MEDIUM_RECTANGLE:
				return AdSize.MediumRectangle;
			default :
				return AdSize.SmartBanner;
			}
		}
		static string GetBanner(AdPosition position){
			if (EaAd.isSingleBanner)
				return EaAd.banner;

			switch (position) {
			case AdPosition.Top:
				return EaAd.bannerTop;

			default:
				return EaAd.bannerBottom;
			}
		}
		#endregion
	}

}