using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
using Sirenix.OdinInspector;
using System;
using System.Linq;
namespace Ea{
	public enum Transition{
		Scale,
		Translate,
	}
	[Serializable]
	public class SceneEvent{
		[TabGroup("SceneObject")]
		public string scene;
		[TabGroup("SceneObject")]
		public Transition transition;
		[TabGroup("SceneObject")]
		public Vector3 begin,end;


	}
public class EaMenu : EaSceneObject {
		[TabGroup("SceneObject")]
	public SceneEvent loadEvent;
	[TabGroup("SceneObject")]
	public List<SceneEvent>  unloadEvents;

		public override void Load ()
	{
		base.Load ();
		if (!isActiveAndEnabled) {
			gameObject.SetActive (true);
				switch (loadEvent.transition) {
				case Transition.Scale:
					Scale (loadEvent.begin, loadEvent.end,()=>EaSceneManager.isLoading  = false);
				break;
				case Transition.Translate:
					Translate (loadEvent.begin, loadEvent.end,()=>EaSceneManager.isLoading  = false);
				break;
			}
		}
	}
	public  override void Unload (string scene)
	{
		if (isActiveAndEnabled) {
				unloadEvents.ForEach (e => {
					if(e.scene == scene){
						switch (e.transition) {
						case Transition.Scale:
							Scale (e.begin, e.end,()=>{
								gameObject.SetActive(false);
								transform.localScale = e.begin; 
							});
							break;
						case Transition.Translate:
							Translate (e.begin, e.end,()=>{
								gameObject.SetActive(false);
								rect.localPosition = e.begin;
							});
							break;
						}
						return;
					}
				});
			
		}
	}
	IEnumerator MenuEvent(Vector3 begin,Vector3 end,Action<Vector3,Vector3,float> callstack,Action callback){
		float t = 0;
		WaitForEndOfFrame frame = new WaitForEndOfFrame ();
		while (t < 1) {
				t += Time.deltaTime * 2.5f;
				callstack (begin,end,t);
				yield return frame;

		}
		callback .Call();
	}
		void Scale(Vector3 begin,Vector3 end,Action callback){
			MenuEvent (begin, end,(s,t,time)=>transform.localScale = Vector3.Lerp (s,t,time),callback).Call (this);
			
		
	}
		void Translate(Vector3 begin,Vector3 end,Action callback){
			MenuEvent (begin, end,(s,t,time)=>rect.localPosition = Vector3.Lerp (s, t,time),callback).Call (this);

			
	
	}
	}
}