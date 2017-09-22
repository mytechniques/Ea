using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Ea;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Reflection;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;
namespace EaEditor{
public class EaFileReader  : EditorWindow {
//	public static TextAsset file;
	public static IEaSerializable decodeType;
//	public static EaTest.FileTest rawData;	
	public static EditorWindow window;
	public static Rect rect;
	static private 	Vector2 view;
	public static string json;
	public static Object openingFile;
	static int i;
	[MenuItem("Ea/File Reader")]
	public static void CreateWindow(){
			rect = new Rect (Screen.width / 2, Screen.height / 2,Screen.width ,Screen.height);
			if (window == null) {
				Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
				window = EditorWindow.GetWindow<EaFileReader> (new Type[] { inspectorType });
				window.titleContent = new GUIContent (){ text = "File Reader", image = Resources.Load<Texture> ("siro") };
				window.Show ();

			} else {
				LoadJson ();
			}
	}

	
		public void OnGUI(){
			
			if (json.is_avaiable()) {
				view =	EditorGUILayout.BeginScrollView (view);
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.BeginVertical ();
				GUILayoutOption [] option = new GUILayoutOption[] {GUILayout.Height(10)};

				for(int j =0 ; j<i;j++){
					if(GUILayout.Button ("•",option))
						AssetDatabase.OpenAsset (openingFile,j+1);
					
				}
				EditorGUILayout.EndVertical ();
				GUI.enabled = false;
				EditorGUILayout.TextArea (json);
				GUI.enabled = true;
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.EndScrollView ();

				if (GUILayout.Button ("Open")) {
					AssetDatabase.OpenAsset (openingFile);
				}
			}
			else 
				EditorGUILayout.HelpBox("Select text file and click reload!",MessageType.Info);
	
			if (GUILayout.Button ("Reload")) {
				LoadJson ();
			}

	
		}
		public static void LoadJson(){
			i = 0;
			var selection = Selection.activeObject as TextAsset;
			if (selection.is_avaiable ()) {
				openingFile = Selection.activeObject;
				string match = "\n";
				json =  Regex.Replace(selection.text.start_width(++i + " "),match,matches=>{
					return (match + ++i + " ").ToString();
				});
			

			}
		}

		 


	}
}
