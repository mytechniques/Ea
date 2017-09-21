using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Ea;
namespace Ea.Editor{

public static class EaScriptable  {

	public static void CreateAsset<T>(string name = "") where T: ScriptableObject{

		string assetPathAndName = Directory +  (name == "" ? typeof(T).Name : name) + ".asset";
			if (!File.Exists (assetPathAndName)) {
				T asset = ScriptableObject.CreateInstance<T> ();
				AssetDatabase.CreateAsset (asset, assetPathAndName);
				AssetDatabase.SaveAssets ();	
				AssetDatabase.Refresh ();
				Selection.activeObject = asset;
			} else {
//				UnityEngine.Debug.Log ("File exists");
				Selection.activeObject = Resources.Load<T> (typeof(T).Name);
			}
			
		EditorUtility.FocusProjectWindow ();

	}
		[MenuItem("Ea/Documents",false, -1)]
		public static void OpenDocument(){
			Application.OpenURL ("https://eaunity.wordpress.com/docs");
		}


	[MenuItem("Ea/Settings/Advertisement")]
	public static void Advetisement(){
		EaScriptable.CreateAsset<EaAdvertisement> ();
	}

		[MenuItem("Ea/Settings/Texture")]
		public static void TextureSettings(){
			EaScriptable.CreateAsset<EaTextureSetting> ();
		}


	public static string Directory{
		get{ 
			return "Assets/Ea/Scripts/Settings/Resources/";
			}	
		}
	}
}
