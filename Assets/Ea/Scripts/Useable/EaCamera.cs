using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaCamera : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<Camera> ().aspect = 480 / 800f;
		Application.targetFrameRate = 120;
		QualitySettings.vSyncCount = 1;
	}
	

}
