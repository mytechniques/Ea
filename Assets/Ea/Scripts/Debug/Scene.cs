using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EAE{
public class Scene : MonoBehaviour {

	// Use this for initialization
	void Start () {
			Ea.EaSceneManager.Load ("menu");
	}
	
	// Update is called once per frame
	void Update () {
			if (Input.GetKeyDown (KeyCode.Alpha0) || Input.touchCount == 1)
				Ea.EaSceneManager.Load ("menu");
			if (Input.GetKeyDown (KeyCode.Alpha1) || Input.touchCount == 2)
				Ea.EaSceneManager.Load ("game");
			if (Input.GetKeyDown (KeyCode.Alpha2) || Input.touchCount == 3)
				Ea.EaSceneManager.Load ("game1");
			
				
		}
	}
}