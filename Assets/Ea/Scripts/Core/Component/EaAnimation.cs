using UnityEngine;
using System.Collections;
using Ea;
using System.Collections.Generic;
using Sirenix.OdinInspector;
[System.Serializable]
public class EaAnimation : MonoBehaviour  {
	[TabGroup("SingleAnimation","Configuration")]
	public bool playOnEnable, loop, selfDestruction;
	[TabGroup("SingleAnimation","Configuration")]
	public UnityEngine.Events.UnityEvent callback;



	public bool isPlaying{get;set;}

	[Range(1,60)]
	[TabGroup("SingleAnimation","Animation")]
	public int frameRate = 15;

	[TabGroup("SingleAnimation","Animation")]
	public Sprite[] sprites;
	public SpriteRenderer _rendering{ get; set;}
	public SpriteRenderer rendering{
		get{
			if(_rendering == null)
				_rendering = GetComponent<SpriteRenderer>();

			return _rendering;
		}	

	}

	// Use this for initialization
	private int _index;
	public int index{
		get{ 
			return _index;
		}
		set{ 
			_index = value;
			rendering.sprite = sprites [_index];
		}
	}
	void OnEnable(){
		if (playOnEnable)
			isPlaying = true;
	}
	void Start(){
		rendering.sprite = sprites [index];
		EaListener.AddListener<EaAnimation> (gameObject,1/(float)frameRate,OnCompletePlaying);
	}
	public void OnCompletePlaying(){
		if (!loop && !isPlaying)
			return;
		
		if (index + 1 < sprites.Length)
			index++;
		else {
			index = 0;
			isPlaying = false;
			
			if (callback != null)
				callback.Invoke ();
			
			if (selfDestruction)
				gameObject.SetActive (false);
			

			
			return;
		}


	}



}
