using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Linq;
//using System.Reflection;
using Ea;
public class EaListener : MonoBehaviour {
	public enum Option
	{
			SelfDestruction,
			SelfDisable,
			IgnoreTimeScale,

	}
	public void ctor(){}
	[Serializable]
	public struct Detail{
		public string reference;
		public string callback;
		public Detail(string reference,string description){
			this.reference = reference;
			this.callback = description;
		}
		public override bool Equals (object obj)
		{
			return(obj.GetHashCode () == this.GetHashCode ());
				
		}
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
		public static bool operator ==(Detail lhs,Detail rhs){
			return(lhs.Equals (rhs) || (lhs.callback == rhs.callback && lhs.reference == rhs.reference || (lhs == default(Detail) && rhs == default(Detail))));
		}
		public static bool operator != (Detail lhs , Detail rhs){
			return!(lhs == rhs);
		}
	}


	[TabGroup("EaListener","Detail")][ReadOnly]
	public Detail detail{get;set;}

	[TabGroup("EaListener","Properties")]
	public bool ignoreTimeScale{ get; private set; } 
	[TabGroup("EaListener","Properties")]
	public bool selfDisable{ get; private set;}

	[TabGroup("EaListener","Properties"),EnableIf("selfDisable")]
	public bool selfDestruction{ get; private set;}



	[TabGroup("EaListener","Properties")]
	public bool isPausing{ get; private set;}
	[TabGroup("EaListener","Properties")]
	public float  listenFrequency{ get; private set;}
	[ReadOnly][TabGroup("EaListener","Readonly")]
	public float listenCounter{ get; private set;}
	[SerializeField][TabGroup("EaListener","Callback")]
	private UnityEvent UnityListener;
	[SerializeField][TabGroup("EaListener","Callback")][ReadOnly]
	private Action OnListenComplete;

	public void Reset(){Stop ();Resume ();}
	public void Pause (){isPausing = true;}
	public void Resume(){isPausing = false;}
	public void Stop(){isPausing = true;listenCounter = 0;}
	public void Destroy(){Destroy (this);}
	public static EaListener AddListener<T>(GameObject obj,float listenFrequency, Action  OnListenComplete){
			EaListener listener =obj.AddComponent<EaListener> ();
			listener.OnListenComplete =  (OnListenComplete);
			listener.listenFrequency = listenFrequency;
			listener.detail =   new Detail(typeof(T).Name,OnListenComplete.Method.Name);
			return listener;
	}
	public static EaListener AddListener<T>(GameObject obj,float listenFrequency, Action  OnListenComplete,params Option [] options){
		EaListener listener = AddListener<T> (obj, listenFrequency, OnListenComplete);
		return  AddOptions( listener,options);
	}	
	public static EaListener AddOptions( EaListener listener,Option [] options){
		int optionLength = options.Length;
		for (int i = 0; i < optionLength; i++) {
			switch (options [i]) {
			case Option.IgnoreTimeScale:
				listener.ignoreTimeScale = true;
				break;
			case Option.SelfDestruction:
				listener.selfDestruction = true;
				break;
			case Option.SelfDisable:
				listener.selfDisable = true;
				break;
			}

		}
		return listener;
	}

	public static EaListener AddListener(GameObject obj,float listenFrequency, Action  OnListenComplete,Detail detail = default(Detail)){
		EaListener listener =obj.AddComponent<EaListener> ();
		listener.OnListenComplete =  (OnListenComplete);
		listener.listenFrequency = listenFrequency;
		listener.detail = detail == default(Detail) ?  new Detail(obj.name, OnListenComplete.Method.Name) : detail;
		return listener;
	}
	public static EaListener AddListener(GameObject obj,float listenFrequency, Action  OnListenComplete,Detail detail, params Option [] options){
	 	EaListener listener = 	EaListener.AddListener (obj, listenFrequency, OnListenComplete, detail);
		return AddOptions (listener, options);
	}
	public static EaListener AddListener(GameObject obj,float listenFrequency, Action  OnListenComplete, params Option [] options){
		EaListener listener = 	EaListener.AddListener (obj, listenFrequency, OnListenComplete, default(Detail));
		return AddOptions (listener, options);
	}

	public static void StopAllListener(GameObject behaviour){
		behaviour.GetComponentsInChildren<EaListener> ().ToList ().ForEach (b => b.Stop ());
	}
	public static void ResumeAllListener(GameObject behaviour){
		behaviour.GetComponentsInChildren<EaListener> ().ToList ().ForEach (b => b.Resume ());
	}
	public static void PauseAllListener(GameObject behaviour){
		behaviour.GetComponentsInChildren<EaListener> ().ToList ().ForEach (b => b.Pause ());
	}
	public static void DestroyAllListener(GameObject behaviour){
		behaviour.GetComponentsInChildren<EaListener> ().ToList ().ForEach (b => b.Destroy ());
	}
	public static void ResetAllListener(GameObject behaviour){
		behaviour.GetComponentsInChildren<EaListener> ().ToList ().ForEach (b => b.Reset ());
	}
	void Update(){
		if (!isPausing) {
			if (listenCounter < listenFrequency)
				listenCounter += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			else {
				listenCounter = 0;
				if(UnityListener != null)
					UnityListener.Invoke ();
				
				OnListenComplete.Call ();


				if (selfDisable)
					gameObject.SetActive (false);
				
				if (selfDestruction)
					Destroy (this);
			

			}
		}
	}

}
