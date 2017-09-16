using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Ea;
using Ea.Editor;
using ue = UnityEngine;
using Sirenix.OdinInspector;
public class EaFinder : EditorWindow{
	#region VARIABLE 
	private const int imageId = 0,fontId = 1,textId = 2,spriteRenderId = 3;
	static private EaFinder window;
	static private Font defaultFont;
	static private Camera camera;
	static private 	GUIStyle style;


	static private List<GameObject> allObjects;
	static private List<Text> texts;
	static private List<Image> images;
	static private List<SpriteRenderer> spriteRenders;


	static private  Dictionary <int,bool> predicates;
	static private Dictionary<Font,List<Text>> fonts;


	static private GUILayoutOption[] icon; 
	static private GUIContent [] toolbar;
	static private Vector2 [] scrollsView;

	static private int toolbarSelection;
	static private bool loaded;
	#endregion
	#region WINDOW INITATIALIZE

	static private void InitWindow(GUIContent content){
		window.autoRepaintOnSceneChange = true;
		window.titleContent = content;
		window.Show ();
		loaded = false;
		scrollsView = new Vector2[99];
	}

	[MenuItem("Ea/Finder/Window")]
	public static void CreateUtilityWindow(){
		if (window != null)
			window.Close ();

		window = EditorWindow.GetWindow<EaFinder> (true);
		InitWindow (GUIContent.none);
	}
	[MenuItem("Ea/Finder/Inspector")]
	public static void CreateWindow(){
		if (window != null) 
			window.Close ();

			Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");	
			window = EditorWindow.GetWindow<EaFinder> (inspectorType);
			InitWindow (new GUIContent(){image = Resources.Load<Texture>("megumin")});

	
	}
	void OnEnable(){
		loaded = false;
		Repaint ();
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
	void Load(){
		LoadVariables ();
		LoadObjects ();

		Debug.Log ("Total objects loaded: " + allObjects.Count);
		Debug.Log ("Total images loaded: " + images.Count);
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
	void DrawSelection<T>(int id,ref List<T> objects,Action<T> 	drawFunc) where T : ue::Object{

		if (objects != null && objects.Count > 0) {
			scrollsView [id] = EditorGUILayout.BeginScrollView (scrollsView [id]);
			Debug.Log (objects.Count);
			objects.ForEach (obj => {

				var clone = obj;
				EditorGUILayout.BeginHorizontal();
				DrawSelectionButton(obj);
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
			spriteRender.flipX =	EditorGUILayout.Foldout(spriteRender.flipX,new GUIContent(){text = "X",tooltip ="FlipX"},style);
			spriteRender.flipY =	EditorGUILayout.Foldout(spriteRender.flipY,new GUIContent(){text = "Y",tooltip ="FlipY"},style);
			spriteRender.name =  EditorGUILayout.TextField(spriteRender.name);
			spriteRender.sprite =  EaEditorExtension.ObjectField<Sprite>(spriteRender.sprite);
			spriteRender.color = EditorGUILayout.ColorField(spriteRender.color);
			spriteRender.material = EaEditorExtension.ObjectField<Material>(spriteRender.sharedMaterial);
			spriteRender.sortingOrder = EditorGUILayout.IntField(spriteRender.sortingOrder);
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
			text.enabled = EditorGUILayout.Foldout(text.enabled,new GUIContent(){text = "Enab",tooltip ="Enabled"},style);
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
			img.enabled = EditorGUILayout.Foldout(img.enabled,new GUIContent(){text = "Enab",tooltip ="Enabled"},style);
			img.raycastTarget =	EditorGUILayout.Foldout(img.raycastTarget,new GUIContent(){text = "Ray",tooltip ="Raycast Target"},style);
			img.preserveAspect =	EditorGUILayout.Foldout(img.preserveAspect,new GUIContent(){text = "Rev",tooltip ="Preserve Aspect"},style);
			img.name =	(string)EditorGUILayout.DelayedTextField(img.name);
			img.sprite =  (Sprite)EditorGUILayout.ObjectField (img.sprite, typeof(Sprite),false);	
			img.color = 	EditorGUILayout.ColorField (img.color);
			img.material =  (Material)EditorGUILayout.ObjectField(img.material,typeof(Material),false);
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
	void DrawSelectionButton(params ue::Object [] obj){
		DrawSelectionButton ("o",null, obj);
	}
	void DrawSelectionButton(string label,GUILayoutOption [] options = null,params ue::Object [] obj){
		if(GUILayout.Button(new GUIContent(){text = label},options ?? (options = icon))){
			if (obj.Count () == 1)
				Selection.activeObject = obj[0];
			else
				Selection.objects = obj;
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
	#endregion
}
