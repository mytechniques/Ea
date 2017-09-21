using UnityEngine;
using System.Collections;
using Ea;

	public class Singleton<T> : MonoBehaviour {
		
		public static T instance;
		protected virtual void Awake(){
			instance = GetComponent<T> ();	
			//Debug.Log ("singleton call");

	}

}
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour{
		public static T _instance;
		public static T instance{
				get
				{
					return _instance ?? (_instance =  new GameObject (typeof(T).Name).AddComponent<T> ());
				}
		}
		public static void Initialize(){
			if(_instance == null)
				Debug.Log (string.Format("{0} Initialized!",instance.GetType().Name).color("0000FF"));
		}
	} 

	//thean1995	 thean1994
	public class Manager<T> : MonoBehaviour  where T : MonoBehaviour{
	protected  static bool applicationQuit;
	private static object threadSafe = new object();
	protected static T _Instance;
		public static T Instance{
			get{
					if (applicationQuit)
						return null;


			lock (threadSafe) {		
				if (_Instance == null) {
					(_Instance = new GameObject ().AddComponent<T> ()).name = typeof(T).Name;
					DontDestroyOnLoad (_Instance.gameObject);


				}
				return _Instance;

			}
		}
	}
	public  static   void Initialize(){
		if (_Instance == null) {
			Debug.Log (string.Format("Manager<{0}> initalizied!" , Manager<T>.Instance.name).color("0000FF")); 
		}

	}
//	public virtual void OnInitialized(){}
			void OnDestroy(){
					applicationQuit = true;
			}

	}

