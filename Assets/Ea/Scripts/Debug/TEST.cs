using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ea{
public class TEST : MonoBehaviour {

	// Use this for initialization
	public void Show	 () {
				using (AndroidJavaClass javClass = new AndroidJavaClass ("com.unity3d.player.UnityPlayer"))
				using (AndroidJavaObject javDialog = new AndroidJavaObject ("com.ea.uj.Dialog")) {
					AndroidJavaObject javContext = javClass.GetStatic<AndroidJavaObject> ("currentActivity");
					AndroidJavaObject setDialog = javDialog.CallStatic<AndroidJavaObject> ("instance");
				setDialog.Call ("setContext", javContext);
				javContext.Call("runOnUiThread",new AndroidJavaRunnable(()=>{
					using (AndroidJavaObject javRect  = new AndroidJavaObject("com.ea.uj.Rect",0,1,25,25))
						setDialog.Call("ShowDialog","erudejade",0,javRect);
				}));
			}
	}
		public void Share(){
			using (AndroidJavaClass unityActivity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer")) {
				AndroidJavaObject currentActivity = unityActivity.GetStatic<AndroidJavaObject> ("currentActivity");
				AndroidJavaObject intent = new AndroidJavaObject ("com.ea.uj.Share");
				currentActivity.Call ("startActivity",  intent.Call<AndroidJavaObject> ("Text","share text"));
			}
		}
	// Update is called once per frame
	void Update () {
//			if (Input.GetKeyDown (KeyCode.Alpha0) || Input.touchCount == 1)
//				Ea.EaSceneManager.Load ("menu");
//			if (Input.GetKeyDown (KeyCode.Alpha1) || Input.touchCount == 2)
//				Ea.EaSceneManager.Load ("game");
//			if (Input.GetKeyDown (KeyCode.Alpha2) || Input.touchCount == 3)
//				Ea.EaSceneManager.Load ("game1");
//			if(Input.GetKeyDown(KeyCode.R))
//				UnityEngine.SceneManagement.SceneManager.LoadScene(0x00);
				
		}
	}
}