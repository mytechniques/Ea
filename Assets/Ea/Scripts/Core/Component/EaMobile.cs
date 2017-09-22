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
	public class EaMobile  : MonoSingleton<EaMobile>{


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

		public class Leaderboard {
			
		}
	}
}


