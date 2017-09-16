using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
public class SceneA : EaWorldObject{
	
	public override void Unload (string scene)
	{
		gameObject.SetActive (false);
	}

	public override void Load ()
	{
		base.Load ();
		gameObject.SetActive (true);
	}
}
