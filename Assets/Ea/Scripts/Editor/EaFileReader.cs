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
using Object = UnityEngine.Object;
namespace Ea.Editor{
public class EaFileReader  : EditorWindow {
	public static TextAsset file;
	public static IEaSerializable decodeType;
//	public static EaTest.FileTest rawData;	
	public static EditorWindow window;
	public static Rect rect;
	static private 	Vector2 view;
	public static string json;
	[MenuItem("Ea/File Reader")]
	public static void CreateWindow(){
			rect = new Rect (Screen.width / 2, Screen.height / 2,Screen.width ,Screen.height);
			if (window == null) {
				Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
				window = EditorWindow.GetWindow<EaFileReader> (new Type[] { inspectorType });
				window.titleContent = new GUIContent (){ text = "File Reader" };
				window.Show ();

			} else {
				file = Selection.activeObject as TextAsset;
				LoadJson ();
			}
	}
	public void OnGUI(){
			
			file = (TextAsset)EditorGUILayout.ObjectField (file, typeof(TextAsset), false, new GUILayoutOption[]{ });
			if (GUILayout.Button ("Reload")) {
				LoadJson ();
			}
			if (json != "" && json != null) {
				view =	EditorGUILayout.BeginScrollView (view);
				json = EditorGUILayout.TextArea (json);
				EditorGUILayout.EndScrollView ();
			}
			Repaint ();
		}
		public static void LoadJson(){
			if (file.is_avaiable ()) {
				json = file.text;
			}

		}
	}
}
