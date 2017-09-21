using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Ea;
#if UNITY_ANDROID
using GooglePlayGames;
#endif
namespace Ea{
public class EaSocial : ScriptableObject {
	public static class Instance{
		public static bool isInitialized{ get; private set;}
		public static void Initialize(){
			isInitialized = true;
			Debug.Log ("EaSocial initialized!".color("0000FF"));
			#if UNITY_ANDROID
			PlayGamesPlatform.Activate();
			#endif
			if(!Social.localUser.authenticated)
				Social.localUser.Authenticate (success=>{
					Debug.LogFormat( "Authendicated {0}" , (success ? "success" : "failed"));
					
				});
			
		}
		public static void ReportScore(long score,string board){
			Social.Active.ReportScore (score, board, success => Debug.LogFormat ("Report score status: {0}",  (success ? "success" : "failed")));
		}
		public static void Show(){
			if(Social.localUser.authenticated)
				Social.Active.ShowLeaderboardUI ();
			else
				Social.localUser.Authenticate (success=>Debug.LogFormat ("Authenticate status: {0}",  (success ? "success" : "failed")));
			
			}
				
		}
	}
	
}
