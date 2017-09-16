using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
public class SceneB : EaSceneObject {
	

	public override void Unload ()
	{
		base.Unload ();
		gameObject.SetActive (false);
	}
	public override void Load ()
	{
		base.Load ();
		gameObject.SetActive (true);
	}

		
}
