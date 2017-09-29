using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TEST : MonoBehaviour {

		void OnGUI(){
			if (GUI.Button (new Rect ((Screen.width / 2)  - 100, Screen.height / 60, 200, 50), "Share Text")) 
				CallIntent((i,u)=>i.Call("ShareText",u,"erudejade","#erudejade"));

			if (GUI.Button (new Rect ((Screen.width / 2)  - 100, (Screen.height / 60) + 55, 200, 50), "Send mail")) 
				CallIntent((i,u)=>i.Call("SendMail",u,"#feedback","Leave us your comment",new String[]{"dunnoprice@gmail.com","tmtudev@gmail.com"}));
		
			if (GUI.Button (new Rect ((Screen.width / 2)  - 100, (Screen.height / 60) + 110, 200, 50), "Notify")) 
				CallIntent((i,u)=>i.Call("PushNotification",u));
		
		}
	IEnumerator	 Start(){
		yield return new WaitForSeconds (10);
		CallIntent((i,u)=>i.Call("PushNotification",u));

	}

	void CallIntent(Action<AndroidJavaObject,AndroidJavaObject> iCallback){
		AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		AndroidJavaObject intent = new AndroidJavaObject ("com.ea.uj.EaIntent");
		iCallback.Invoke(intent,currentActivity);

	}
}