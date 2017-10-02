using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GooglePlayGames;
using System;
using System.Linq;
using Ea;
namespace Ea
{
	public delegate void OnQuit();
	public delegate void OnBack();
	public delegate void OnHide();
	public delegate void OnPause(bool status);
	public enum Mobile{
		Advertisement,
		Leaderboard,
	}
}
public enum hLayout{
	Left = -0x01,
	Center = 0x00,
	Right = 0x01,
}
public enum vLayout {
	Top = 0x01,
	Middle = 0x00,
	Bottom = -0x01,
}
public struct MsgLayout{
	public hLayout h;
	public vLayout v;
	public int hOffset,vOffset;
	public MsgLayout(hLayout h,vLayout v,int hOffset,int vOffset){
		this.h = h;
		this.v = v;
		this.hOffset = hOffset;
		this.vOffset = vOffset;
	
	}
}
	public class EaMobile  : MonoSingleton<EaMobile>{
	#if UNITY_ANDROID && !UNITY_EDITOR
		static AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		static	AndroidJavaObject _currentActivity;
		static AndroidJavaObject currentActivity {
			get{ return  _currentActivity ?? (_currentActivity  = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")); }
		}
		static	AndroidJavaObject  _intent;
		static	AndroidJavaObject intent{
			get{
				return _intent ?? (_intent = new AndroidJavaClass ("com.ea.uj.EaIntent").CallStatic<AndroidJavaObject> ("instance", currentActivity));
			}
		}

		#endif

		#region VARIABLE
		public  static event OnPause onPause = delegate{};
		public  static event OnBack onBack = delegate{};
		public  static event OnQuit onQuit = delegate {};

		public static List<string> openedFiles = new List<string> ();

		#endregion
		public static void Initialize(params Mobile [] mobileOptions){
			mobileOptions.ToList ().ForEach (option => {
				switch(option){
				case Mobile.Advertisement:
					EaAdvertisement.Initialize ();
					break;
				case Mobile.Leaderboard:
					EaSocial.Initialize ();
					break;
				}

			});
		}


	
		void OnApplicationQuit(){
			
				onQuit.Invoke ();
		}
		void OnApplicationPause(bool status){
				onPause.Invoke (status);
			
		} 
		void Update(){
			if (Input.GetKeyDown (KeyCode.Escape)) {
				onBack.Invoke ();
			}
		}
		public static void ShowMessage(string msg){
		CallIntent(i=>i.Call("ShowDialog",msg));
		}
		public static void ShowMessage(string msg,int duration){
		CallIntent(i=>i.Call("ShowDialog",msg,duration));
		}
		public static void ShowMessage(string msg,int duration, MsgLayout layout){
		CallIntent(i=>i.Call("ShowDialog",msg,duration,new AndroidJavaObject("com.ea.uj.Rect",(int)layout.h,(int)layout.v,layout.hOffset,layout.hOffset)));

		}
	
		public static void ShareText(string msg,string title = "#SHARE"){
			CallIntent (i => i.Call ("ShareText", title,msg));
		}
		public static void ShareImage (string msg, string imgPath, string title = "#SHARE"){
			CallIntent (i => i.Call ("ShareImage", title, msg, imgPath));
		}
		public static void SendMail(string title,string content,params string [] email){
			CallIntent (i=>i.Call("SendMail", title, content, email));
		}
		public static void PushNotification(string title,string msg,long delay){
		CallIntent (i => i.Call ("PushNotification", title,msg,delay));
		}

		static void CallIntent(Action<AndroidJavaObject> iCallback){
		#if UNITY_ANDROID && !UNITY_EDITOR
			iCallback.Invoke(intent);
			#else
			Debug.Log ("INTENT ONLY WORK ON ANDROID!".color("FF0000"));
			#endif
		}
		public class Leaderboard {
			
		}
	}



