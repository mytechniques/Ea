﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Ea;
using EaEditor;
using System.Reflection;
using ue = UnityEngine;
using Sirenix.OdinInspector;
namespace EaEditor{
[System.Serializable]
public class EaFinder : EditorWindow{
	#region VARIABLE 
	private const int imageId = 0,fontId = 1,textId = 2,spriteRenderId = 3;
	static private EaFinder window;
	static public Font defaultFont;
	static private Camera camera;
	static private 	GUIStyle style;
	static private Editor imagePreview;


	 public List<GameObject> allObjects;
	 public List<Text> texts;
	 public List<Image> images;
	 public List<SpriteRenderer> spriteRenders;
	 public  Dictionary <int,bool> predicates;
	 public Dictionary<Font,List<Text>> fonts;


	static private GUILayoutOption[] icon; 
	static private GUIContent [] toolbar;
	static public Vector2 [] scrollsView;

	static private int toolbarSelection;
	static private bool loaded;
	static private int windowId;

	static public bool isWindowChanged;
	#endregion
	#region WINDOW INITATIALIZE

	static private void InitWindow(GUIContent content){
		window.autoRepaintOnSceneChange = true;
		window.titleContent = content;
		window.Show ();
		loaded = false;
		scrollsView = new Vector2[99];
		if (isWindowChanged)
			window.Load ();
			
	}


	public static void CreateUtilityWindow(){
		if (window != null) {
			window.Close ();
			isWindowChanged = true;
		} else
			isWindowChanged = false;


		windowId = 1;
		window = EditorWindow.GetWindow<EaFinder> (true);
		InitWindow (GUIContent.none);
	}

	[MenuItem("Ea/Finder")]
	public static void CreateWindow(){

		if (window != null) {
			window.Close ();
			isWindowChanged = true;
		} else
			isWindowChanged = false;

			windowId = -1;
			Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");	
			window = EditorWindow.GetWindow<EaFinder> (inspectorType);

			InitWindow (new GUIContent(){text = "Finder", image = Resources.Load<Texture>("megumin")});
		}


	static public void CopyWindow(EaFinder currentWindow ,ref EaFinder newWindow){
		newWindow.allObjects = currentWindow.allObjects;
		newWindow.spriteRenders = currentWindow.spriteRenders;
		newWindow.images = currentWindow.images;
		newWindow.fonts = currentWindow.fonts;
		newWindow.texts = currentWindow.texts;
	}
	void OnEnable(){
		loaded = false;
		string path ="Assets/Ea/Scripts/Editor/Enumerator/";
		string name = "EaSortingLayer";
		var sortinglayers = GetSortingLayerNames();
		EnumGenerator(path,name,sortinglayers);

		

	}
	void LoadVariables(){
		defaultFont =  Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		style = EditorStyles.radioButton;
		images = new List<Image> ();
		allObjects = new List<GameObject> ();
		predicates = new Dictionary<int, bool> ();
		fonts = new Dictionary<Font, List<Text>> ();
		texts = new List<Text> ();
		spriteRenders = new List<SpriteRenderer> ();
		icon = new GUILayoutOption[]{GUILayout.Width (20),GUILayout.MaxWidth(20),GUILayout.MinWidth(20),GUILayout.ExpandWidth(false),GUILayout.Height(20)};
		toolbar = new GUIContent [] {
			new GUIContent{image = EaEditorExtension.GetUnityTexture<Image>()},	
			new GUIContent{image = EaEditorExtension.GetUnityTexture<Font>()},	
			new GUIContent{image = EaEditorExtension.GetUnityTexture<Text>()},
			new GUIContent{image = EaEditorExtension.GetUnityTexture<SpriteRenderer>()},
		

		};
			
	}
	public void Load(){
		LoadVariables ();
		LoadObjects ();

//		Debug.Log ("Total objects loaded: " + allObjects.Count);
//		Debug.Log ("Total images loaded: " + images.Count);
	}
	void LoadObjects(){
		Scene currentHandleScenes = SceneManager.GetActiveScene ();
		currentHandleScenes.GetRootGameObjects (allObjects);
		int length = allObjects.Count;
		for(int i =0;i<length;i++){
			var	childs = allObjects [i].GetComponentsInChildren<Transform> (true).Select (t => t.gameObject).ToList ();
			childs.Remove (allObjects [i]);
			allObjects.AddRange (childs);
		}

		allObjects.ForEach(obj=>{

			Image img = obj.GetComponent<Image>();
			Text  text = obj.GetComponent<Text>();
			SpriteRenderer spriteRender = obj.GetComponent<SpriteRenderer>();

			if(text != null){
				texts.Add(text);
				text.font = text.font == null ? defaultFont : text.font;
				if(!fonts.ContainsKey(text.font))
					fonts.Add(text.font,new List<Text>());

				fonts[text.font].Add(text);
			}
			SafeAdd(ref img,ref images);
			SafeAdd(ref spriteRender,ref spriteRenders);


		});
		loaded = true;
	}
	#endregion
	void OnGUI(){
			
		if (GUILayout.Button (loaded ? "Refresh" : "All")) 
			Load ();
		if (GUILayout.Button (windowId == 1 ? "Dock" : "Window"))
			if (windowId == 1)
					CreateWindow ();
				else
					CreateUtilityWindow ();
						
			

		if (loaded) {
			toolbarSelection = GUILayout.Toolbar (toolbarSelection, toolbar, EditorStyles.miniButton, GUILayout.MaxHeight (25));

			switch (toolbarSelection) {
			case imageId:
				DrawImages ();
				break;
			case fontId:
				DrawFonts ();
				break;
			case textId:
				DrawTexts ();
				break;
			case spriteRenderId:
				DrawSprites ();
				break;
		
			}
		}
	}

	#region BASE DRAWER FUNCTION
	void DrawSelection<T>(int id,ref List<T> objects,Action<T> 	drawFunc) where T : ue::Component{

		if (objects != null && objects.Count > 0) {
			scrollsView [id] = EditorGUILayout.BeginScrollView (scrollsView [id]);
//			Debug.Log (objects.Count);
			objects.ForEach (obj => {

				var clone = obj;
				EditorGUILayout.BeginHorizontal();
				DrawSelectionButton(obj);
				GameObject  gameObject = (obj).GetComponent<Transform>().gameObject;
				bool active = GUILayout.Toggle(gameObject.activeSelf,"®",EditorStyles.objectFieldThumb);

				gameObject.SetActive(active);
				drawFunc(obj);
				EditorGUILayout.EndHorizontal();
				RepaintScene(obj,clone);
			});
			EditorGUILayout.EndScrollView ();
		}
		else
			EditorGUILayout.HelpBox (typeof(T).Name.ToUpper()+ " NOT FOUND",MessageType.Error);
		
	}
	#endregion
	#region TOOLBAR DRAWER
	void DrawSprites(){
		Action<SpriteRenderer> spriteRenderDraw = spriteRender => {

			bool togged = Toggle(spriteRender);
			DrawButton(togged ? "-" : "+",()=>predicates[hashCode(spriteRender)] = !togged);


			spriteRender.name =  EditorGUILayout.TextField(spriteRender.name);

			spriteRender.color = EditorGUILayout.ColorField(spriteRender.color);
			spriteRender.material = EaEditorExtension.ObjectField<Material>(spriteRender.sharedMaterial);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if(togged){
				
				EditorGUILayout.BeginVertical();
	//----------->>>>>>draw sorting layer	
				spriteRender.sprite =  (Sprite)EditorGUILayout.ObjectField ("Source Image",spriteRender.sprite,typeof(Sprite),false,GUILayout.MaxWidth(enumWidth),GUILayout.MaxHeight(15));	
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Flip",GUILayout.MaxWidth(145));
//				EditorGUILayout.BeginHorizontal();
				spriteRender.flipX =	EditorGUILayout.Toggle(spriteRender.flipX,GUILayout.MaxWidth(10));
				EditorGUILayout.LabelField("X",GUILayout.MaxWidth(10));
				spriteRender.flipY =	EditorGUILayout.Toggle(spriteRender.flipY,GUILayout.MaxWidth(10));
				EditorGUILayout.LabelField("Y");

//				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndHorizontal();
				{
				var EaLayer = (EaSortingLayer) Enum.Parse(typeof(EaSortingLayer),spriteRender.sortingLayerName);

						spriteRender.drawMode = (SpriteDrawMode)EditorGUILayout.EnumPopup("Draw Mode",spriteRender.drawMode,GUILayout.MaxWidth(enumWidth));
						spriteRender.sortingLayerName = Enum.GetName(typeof(EaSortingLayer),
						EditorGUILayout.EnumPopup("Sorting Layer",EaLayer,GUILayout.MaxWidth(enumWidth)));
					
				}
			
				spriteRender.sortingOrder = EditorGUILayout.IntField("Order in Layer",spriteRender.sortingOrder,GUILayout.MaxWidth(enumWidth));
				spriteRender.maskInteraction = (SpriteMaskInteraction)EditorGUILayout.EnumPopup("Mask Interaction",spriteRender.maskInteraction,GUILayout.MaxWidth(enumWidth));
				EditorGUILayout.EndVertical();
				spriteRender.sprite =  (Sprite)EditorGUILayout.ObjectField (spriteRender.sprite,typeof(Sprite),false,GUILayout.MaxWidth(enumWidth),GUILayout.MaxHeight(100));
			}
		};
		DrawSelection (spriteRenderId, ref spriteRenders, spriteRenderDraw);
	}

	void DrawFonts(){
		if (fonts != null && fonts.Count > 0) {
			scrollsView [fontId] =	EditorGUILayout.BeginScrollView (scrollsView [fontId]);
			foreach (KeyValuePair<Font,List<Text>> font in fonts) {
				EditorGUILayout.BeginHorizontal ();
				DrawSelectionButton (font.Value.Count.ToString (),
					new GUILayoutOption[]{ GUILayout.MaxWidth (window.position.width / 5), GUILayout.Height (15) },
					font.Value.Select (f => f.gameObject).ToArray ());
				
				Font fontKey = (Font)EditorGUILayout.ObjectField (font.Key, typeof(Font), false);
				EditorGUILayout.EndHorizontal ();
				if (fontKey != font.Key) {
					fontKey = fontKey == null ? defaultFont : fontKey;
					
					font.Value.ForEach (text => text.font = fontKey);

					if (!fonts.ContainsKey (fontKey))
						fonts.Add (fontKey, font.Value);
					else
						fonts [fontKey].AddRange (font.Value);


					fonts.Remove (font.Key);
					RepaintScene (font.Key, fontKey);
					break;
				}
			}
			EditorGUILayout.EndScrollView ();
		} else
			EditorGUILayout.HelpBox ("FONT NOT FOUND",MessageType.Error);
	}
	void DrawTexts(){
		Action<Text> textDrawer = text => {
			bool togged = Toggle(text);
			DrawButton(togged ? "-" : "+",()=>predicates[hashCode(text)] = !togged);
			text.enabled = EditorGUILayout.Foldout(text.enabled,new GUIContent(){text = "Enable",tooltip ="Enabled"},style);
			text.raycastTarget =	EditorGUILayout.Foldout(text.raycastTarget,new GUIContent(){text = "Ray",tooltip ="Raycast Target"},style);
			text.supportRichText =	EditorGUILayout.Foldout(text.supportRichText,new GUIContent(){text = "Rich",tooltip ="Rich text"},style);
			text.resizeTextForBestFit =	EditorGUILayout.Foldout(text.resizeTextForBestFit,new GUIContent(){text = "Best",tooltip ="Best fit"},style);
			text.name = EditorGUILayout.TextField(text.name);
			text.font =(Font) EditorGUILayout.ObjectField(text.font,typeof(Font),false);
			text.fontStyle = (FontStyle)EditorGUILayout.EnumPopup(text.fontStyle);
			text.fontSize = EditorGUILayout.IntField(text.fontSize);
			text.color = EditorGUILayout.ColorField(text.color);
			text.material = (Material)EditorGUILayout.ObjectField(text.material,typeof(Material),false);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			if(togged){
				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField("Text");
				text.text =  EditorGUILayout.TextField(text.text,GUILayout.Height (100));
				EditorGUILayout.EndVertical();
			}
		};
		DrawSelection (textId, ref texts, textDrawer);
		
	}

	void DrawImages(){
		Action<Image> imageDrawer = img => {
			bool togged = Toggle(img);
			DrawButton(togged ? "-" : "+",()=>predicates[hashCode(img)] = !togged);

			img.enabled = EditorGUILayout.Foldout(img.enabled,new GUIContent(){text = "Enable",tooltip ="Enabled"},style);
			img.raycastTarget =	EditorGUILayout.Foldout(img.raycastTarget,new GUIContent(){text ="Ray",tooltip ="Raycast Target"},style);
			img.preserveAspect =	EditorGUILayout.Foldout(img.preserveAspect,new GUIContent(){text = "Res",tooltip ="Preserve Aspect"},style);
			img.name =	(string)EditorGUILayout.DelayedTextField(img.name);

			img.color = 	EditorGUILayout.ColorField (img.color);
			img.material =  (Material)EditorGUILayout.ObjectField(img.material,typeof(Material),false);

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
	

			if(togged){

				EditorGUILayout.BeginVertical();
				img.sprite =  (Sprite)EditorGUILayout.ObjectField ("Source Image",img.sprite,typeof(Sprite),false,GUILayout.MaxWidth(enumWidth),GUILayout.MaxHeight(15));	
				img.type =  (Image.Type)EditorGUILayout.EnumPopup(new GUIContent(){text = "Image Type"}, img.type,GUILayout.MaxWidth(enumWidth));
				bool radial = false;
				if(img.type == Image.Type.Filled){
					img.fillMethod =  (Image.FillMethod)EditorGUILayout.EnumPopup(new GUIContent(){text = "Fill Method"}, img.fillMethod,GUILayout.MaxWidth(enumWidth));

					Enum fillOrigin;
					switch(img.fillMethod){
					case Image.FillMethod.Horizontal:
						fillOrigin = (Image.OriginHorizontal)img.fillOrigin;
						img.fillOrigin = (int)(Image.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin",fillOrigin,GUILayout.MaxWidth(enumWidth));
						break;
					case Image.FillMethod.Vertical:
						fillOrigin = (Image.OriginVertical)img.fillOrigin;
						img.fillOrigin = (int)(Image.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin",fillOrigin,GUILayout.MaxWidth(enumWidth));
						break;
					case Image.FillMethod.Radial90:
						fillOrigin = (Image.Origin90)img.fillOrigin;
						img.fillOrigin = (int)(Image.Origin90)EditorGUILayout.EnumPopup("Fill Origin",fillOrigin,GUILayout.MaxWidth(enumWidth));
						radial = true;
						break;
					case Image.FillMethod.Radial180:
						fillOrigin = (Image.Origin180)img.fillOrigin;
						img.fillOrigin = (int)(Image.Origin180)EditorGUILayout.EnumPopup("Fill Origin",fillOrigin,GUILayout.MaxWidth(enumWidth));
						radial = true;
						break;
					case Image.FillMethod.Radial360:
						fillOrigin = (Image.Origin360)img.fillOrigin;
						img.fillOrigin = (int)(Image.Origin360)EditorGUILayout.EnumPopup("Fill Origin",fillOrigin,GUILayout.MaxWidth(enumWidth));
						radial = true;
						break;
					}


					img.fillAmount = EditorGUILayout.Slider("Fill Amount",img.fillAmount,0,1,GUILayout.MaxWidth(enumWidth));
					if(radial)
						img.fillClockwise = EditorGUILayout.Toggle("Clockwise",img.fillClockwise,GUILayout.MaxWidth(enumWidth));
										

				}
				EditorGUILayout.EndVertical();
				img.sprite =  (Sprite)EditorGUILayout.ObjectField (img.sprite,typeof(Sprite),false,GUILayout.MaxWidth(enumWidth),GUILayout.MaxHeight(100));

			}

		};
		DrawSelection (imageId, ref images, imageDrawer);

	}
	#endregion
	#region UTILITY 
	void RepaintScene(object lhs,object rhs){
		if (lhs.Equals (rhs))
			return;
		
		camera = FindObjectOfType<Camera>();
		if(camera != null)
		{
			camera.Repaint();
		}
	}
static	float enumWidth {
		get { 
			return (window.position.width / 2).clamp_min (250);
		}
	}
	void DrawSelectionButton(params ue::Object [] obj){
		DrawSelectionButton ("o",null, obj);
	}
	void DrawSelectionButton(string label,GUILayoutOption [] options = null,params ue::Object [] obj){
		if(GUILayout.Button(new GUIContent(){text = label},options ?? (options = icon))){
			if (obj.Count () == 1)
				Selection.activeObject = obj[0];
			else
				Selection.objects = obj;

			UnityEditor.SceneView.lastActiveSceneView.FrameSelected();

		}

	}
	void DrawButton(string label,Action callstack,params GUILayoutOption [] options){
		if (GUILayout.Button (label, options))
			callstack ();
			
	}
	#endregion

	#region INIT & PROPERTIES
	 bool Toggle(ue::Object obj){
		int hash = obj.GetHashCode ();
		if(!predicates.ContainsKey(hash))
			predicates.Add (hash,false);

		return predicates [hash];
	}
	int hashCode (ue::Object obj){
		return obj.GetHashCode ();
	}

	void SafeAdd<T>(ref T obj,ref List<T> targetList){
		if (obj != null && !obj .Equals(default(T)))
			targetList.Add (obj);
	}

	public string[] GetSortingLayerNames() {
//		laym
		Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
	// Get the unique sorting layer IDs -- tossed this in for good measure
	public int[] GetSortingLayerUniqueIDs() {
		Type internalEditorUtilityType = typeof(UnityEditorInternal.InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}
	public static void EnumGenerator(string path,string name,params string [] values){
		string filePathAndName = path + name + ".cs"; //The folder Scripts/Enums/ is expected to exist

		using ( System.IO.StreamWriter streamWriter = new System.IO.StreamWriter( filePathAndName) )
		{	
				
			streamWriter.WriteLine( "namespace EaEditor { \n public enum " + name +"{");
			for( int i = 0; i < values.Length; i++ )
			{
				streamWriter.WriteLine( "\t\t" + values[i] + "," );
			}
				streamWriter.WriteLine( "\t} \n }" );
		}
		AssetDatabase.Refresh();
	}



	#endregion
	}
}
	