using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TEST : MonoBehaviour {

		void OnGUI(){
		if (GUI.Button (new Rect ((Screen.width / 2) - 100, Screen.height / 60, 200, 50), "Share Text"))
			EaMobile.ShareText ("Hello");
		if (GUI.Button (new Rect ((Screen.width / 2) - 100, (Screen.height / 60) + 55, 200, 50), "Send mail"))
			EaMobile.SendMail ("WELCOME", "CONTENT MAIL", "dunnoprice@gmail.com");
		if (GUI.Button (new Rect ((Screen.width / 2) - 100, (Screen.height / 60) +  55 + 55, 200, 50), "Push notification"))
			EaMobile.PushNotification ("Buble","You have received an gift!",5L);
		if (GUI.Button (new Rect ((Screen.width / 2) - 100, (Screen.height / 60) + 55 + 55 + 55, 200, 50), "Message"))
			EaMobile.ShowMessage ("You pressed the button",0,new MsgLayout(hLayout.Center,vLayout.Top,0,15));
		}

}