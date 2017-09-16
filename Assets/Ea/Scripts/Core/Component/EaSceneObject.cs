using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
namespace Ea{

	public abstract class EaWorldObject  : MonoBehaviour, ISaveTransform{
		public string scene;

		public virtual void Save(){

			position = 	transform.position;
			rotation = transform.rotation;
			localScale = transform.localScale;
			EaSceneManager.Push (scene,this);

		}
		public virtual void Load(){
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = localScale;
		}
		public Vector3 position{ get; set;}
		public Quaternion rotation { get; set;}
		public Vector3 localScale{ get; set;}
		protected virtual void Awake ()
		{
			Save ();

		}
		public abstract void Unload (string scene);

	}

	public abstract class EaSceneObject  : MonoBehaviour, ISaveTransform{
		[Sirenix.OdinInspector.BoxGroup("SceneObject")]
		public string scene;
		public RectTransform rect{ get; set;}
		public virtual void Save(){
			rect = GetComponent<RectTransform> ();
			position = 	rect.localPosition;
			rotation = rect.rotation;
			localScale = rect.localScale;
			EaSceneManager.Push (scene,this);

		}
		public virtual void Load(){
			rect.localPosition = position;
			rect.rotation = rotation;
			rect.localScale = localScale;
		}
		public Vector3 position{ get; set;}
		public Quaternion rotation { get; set;}
		public Vector3 localScale{ get; set;}
		protected virtual void Awake ()
		{
			Save ();

		}
		public abstract void Unload (string scene);

	}
	public abstract class EaSceneObject<T> : EaSceneObject,ISaveData<T>{
		public T saveData{get;set;}
	}
	public abstract class EaWorldObject<T> : EaWorldObject,ISaveData<T>{
		public T saveData{get;set;}
	}


	public interface ISaveable{	
		void Save();
		void Load();
		void Unload(string scene);

	}

	public interface ISavePosition : ISaveable{
		Vector3 position{ get; set;}
	}

	public interface ISaveRotation : ISaveable{
		Quaternion rotation{ get; set;}
	}
	public interface ISaveData <T>: ISaveable {
		T saveData{get;set;}
	}
	public interface ISaveScale : ISaveable{
		Vector3 localScale { get; set;}
	}

	public interface ISaveTransform : ISavePosition,ISaveRotation,ISaveScale{}

}