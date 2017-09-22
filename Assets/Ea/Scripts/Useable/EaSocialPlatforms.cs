﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
public class EaSocialPlatforms : Singleton<EaSocialPlatforms>  {
	void Start(){
		EaMobile.Initialize (Mobile.Leaderboard);
	}
	public  void PostScore(string leaderboardId, double score){
		EaSocial.ReportScore (leaderboardId,(long)score);
	}
	public  void ShowLeaderboard(){
		EaSocial.Show ();
	}

}