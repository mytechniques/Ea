using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Ea
{
	public  static class EaSceneManager  {
		public static bool isLoading {get;set;}
		static private Dictionary<string,List<ISaveable>> _scenes;
		public static Dictionary<string,List<ISaveable>> scenes{
			get
			{	
				if (_scenes == null)
					Initialize ();


				return _scenes ?? (_scenes = new Dictionary<string,List<ISaveable>> ());
			
			}
			set{ _scenes = value;}
		}
		public static void Initialize(){
			Debug.Log ("EaSceneManager Initialized!".color("0000FF"));

			
		}
		/// <summary>
		/// leave the scene id 
		/// </summary>
		/// <param name="sceneId">Scene identifier.</param>
		public static void Load(string scene){
			if (isLoading)
				return;
			
				string reloaded = string.Empty;
				scenes [scene].ForEach (s => {
					s.Load ();
					reloaded += s.ToString () + "\n";
				});
//					Debug.Log ("Load scene:" + scene + "\ntotal object: " + scenes [scene].Count + "\n" + reloaded);
					
				reloaded = string.Empty;
				foreach (KeyValuePair<string,List<ISaveable>> unloadScene in scenes) {
					if (unloadScene.Key != scene) {
						unloadScene.Value.ForEach (s => {
							s.Unload (scene);
							reloaded += s.ToString () + "\n";
		
						});
//					Debug.Log ("Unload scene:" + unloadScene.Key + "\ntotal object: " + unloadScene.Value.Count + "\n" + reloaded);

					}
				}

		}

		public static void Push(string scene,ISaveable data){
			scenes = scenes.NullCheck (scene);
			scenes [scene].Add (data);
			scenes [scene].ForEach (s => s.Unload ("menu\t"));
//			Debug.Log (scene + " : " + scenes [scene].Count);

		}

	}


}