using UnityEngine;
using System.Collections;


	public class Singleton<T> : MonoBehaviour {
		
		public static T instance;
		protected virtual void Awake(){
			instance = GetComponent<T> ();	
			//Debug.Log ("singleton call");

	}

}
		public class GenericSingleton<T> where T : class ,new(){
		public static T _instance;
		public static T instance{
				get
				{
					return _instance ?? (_instance = new T ());

				}
		}
	} 

	//thean1995	 thean1994
	public class Manager<T> : MonoBehaviour  where T : MonoBehaviour{
	protected  static bool applicationQuit;
	private static object threadSafe = new object();
	protected static T _instance;
		public static T instance{
			get{
					if (applicationQuit)
						return null;


			lock (threadSafe) {		
				if (_instance == null) {
					(_instance = new GameObject ().AddComponent<T> ()).name = typeof(T).ToString ();
					DontDestroyOnLoad (_instance.gameObject);
				}
				return _instance;

			}
		}
	}
			void OnDestroy(){
					applicationQuit = true;
			}

	}

