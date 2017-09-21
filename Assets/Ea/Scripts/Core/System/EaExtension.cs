using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ea{
#region STRUCTURE
[Serializable]
public struct EaMatchCollection{
	public readonly string key,value;
	public EaMatchCollection(string key ,string value)
	{
		this.key = key;
		this.value = value;
	}

}
	[Flags]
public enum Shift{
		None = 0x00,
		Left = 0x01,
		Right = 0x02,
	}
public struct EaPredicate{
	public readonly bool condition;
	public readonly Action action;
	public EaPredicate(bool condition,Action action){
		this.condition = condition;
		this.action = action;
	}

}
#endregion
#region INTERFACE

#endregion
#region ENUMERATOR
	/// <summary>
	/// shorthand write for Short Number Unit
	/// </summary>
public enum SNUnit {
	None,
	Kilo,
	Million,
	Billion,
	Trillion
}
	/// <summary>
	/// shorthand write for Short Number Method
	/// </summary>
public enum  SNMethod{
	None,
	Round,
	Floor
}
#endregion
public  static class EaExtension {




	#region A
		public static Dictionary<T,V> AddIf<T,V>(this Dictionary<T,V> @collection, T key,V value , Func<Dictionary<T,V>,bool> predicate ) {
			if (predicate (@collection))
				@collection.Add (key,value);
			
			return @collection;
	}

	public static Vector2 abs(this Vector2 value){
		return new Vector2 (value.x < 0 ? -value.x : value.x, value.y < 0 ? -value.y : value.y);
	}
	public static int abs(this int value){
		return (value > 0 ? value : -value);
	}
	public static float abs(this float value){
		return (value > 0 ? value : -value);
	}

	public static double abs(this double value){
		return (value > 0 ? value : -value);
	}

	public static float arctan(this float value){
		return Mathf.Atan (value);
	}
	public static float arctan2(this float value, float target){
		return Mathf.Atan2(value,target);
	}
	public static float arccos (this float value){
		return Mathf.Acos (value);
	}

	/// <summary>
	/// Convert value to arcsine 
	/// </summary>
	/// <param name="value">Value.</param>
	public static float arcsin(this float value){
		return Mathf.Asin (value);
	}
	#endregion
	#region B
		public static Dictionary<T,V> NullCheck<T,V>(this Dictionary<T,V> collection,T key) where V : new(){
			if (!collection.ContainsKey (key))
				collection.Add (key, new V());

			return collection;
		}



		public static float begin_count(this float start){
			Debug.Log ("Counting...");
			return Time.realtimeSinceStartup;
		}
	public static string brackets(this string inside){
		return "(" + inside + ")";
	}
	#endregion
	#region C
	/// <summary>
	/// Clean the specified list and predicate.
	/// </summary>
	/// <param name="list">List.</param>
	/// <param name="predicate">Predicate.</param>
	/// <typeparam name="T">delete item with match predicate</typeparam>

	public static float cos (this float value){
		return Mathf.Cos (value);
	}
	/// <summary>
	/// Clamp the specified value, min and max.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public static float clamp(this float value,float min,float max){
		return (value < min ? min : value > max ? max : value);
	}
	/// <summary>
	/// Clamp01 the specified value beetween 0 and 1.
	/// </summary>
	/// <param name="value">Value.</param>
	public static float clamp01(this float value){
		return (value < 0 ? 0 : value > 1 ? 1 : value);
	}
	/// <summary>
	/// Clamps the minimum.
	/// </summary>
	/// <returns>The minimum.</returns>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	public static float clamp_min(this float value,float min){
		return (value < min ? min : value);
	}
	/// <summary>
	/// Clamps the minimum.
	/// </summary>
	/// <returns>The minimum.</returns>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	public static double clamp_min(this double value,double min){
		return(value  < min ? min : value);
	}
	/// <summary>
	/// Clamp the specified value, min and max.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public static int clamp(this int value,int min,int max){
		return (value < min ? min : value > max ? max : value);
	}
	/// <summary>
	/// Clamps the minimum.
	/// </summary>
	/// <returns>The minimum.</returns>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	public static int clamp_min(this int value,int min){
		return (value < min ? min : value);
	}
	/// <summary>
	/// Clamps the max.
	/// </summary>
	/// <returns>The max.</returns>
	/// <param name="value">Value.</param>
	/// <param name="max">Max.</param>
	public static int clamp_max(this int value,int max){
		return(value > max ? max : value);
	}
	/// <summary>
	/// Clamp the specified value, min and max.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public static Vector3 clamp(this Vector3 value, Vector3 min, Vector3 max){
		value = value.clamp_min (min);
		value = value.clamp_max (max);
		return value;
		
	}
	/// <summary>
	/// Clamps the max.
	/// </summary>
	/// <returns>The max.</returns>
	/// <param name="value">Value.</param>
	/// <param name="max">Max.</param>
	public static float clamp_max(this float value,float max){
		return (value  > max ? max : value);
	}
	/// <summary>
	/// Clamps the max.
	/// </summary>
	/// <returns>The max.</returns>
	/// <param name="value">Value.</param>
	/// <param name="max">Max.</param>
	public static double clamp_max(this double value,double max){
		return (value  > max ? max : value);
	}
	/// <summary>
	/// Color the specified value and hex.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="hex">Hex.</param>
	public static string color(this string value,string hex){
		return string.Format("<color=#{0}>{1}</color>",hex,value);
	}
	/// <summary>
	/// Color the specified value and hex.
	/// </summary>
	/// <param name="value">Value.</param>
	/// <param name="hex">Hex.</param>
	public static string color(this int value,string hex){
		return value.ToString ().color (hex);
	}
	/// <summary>
	/// Clamps the max.
	/// </summary>
	/// <returns>The max.</returns>
	/// <param name="value">Value.</param>
	/// <param name="max">Max.</param>
	public static Vector3 clamp_max(this Vector3 value,Vector3 max){
		float x = value.x > max.x ? max.x : value.x;
		float y = value.y > max.y ? max.y : value.y;
		float z = value.z > max.z ? max.z : value.z;
		return new Vector3 (x, y,z);
	}
	/// <summary>
	/// Clamps the max.
	/// </summary>
	/// <returns>The max.</returns>
	/// <param name="value">Value.</param>
	/// <param name="max">Max.</param>
	public static Vector2 clamp_max(this Vector2 value,Vector2 max){
		float x = value.x > max.x ? max.x : value.x;
		float y = value.y > max.y ? max.y : value.y;
		return new Vector2 (x, y);
	}

	/// <summary>
	/// Clamps the minimum.
	/// </summary>
	/// <returns>The minimum.</returns>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	public static Vector3 clamp_min(this Vector3 value,Vector3 min){
		float x = value.x < min.x ? min.x : value.x;
		float y = value.y < min.y ? min.y : value.y;
		float z = value.z < min.z ? min.z : value.z;
		return new Vector3 (x, y,z);
	}

	/// <summary>
	/// Clamps the minimum.
	/// </summary>
	/// <returns>The minimum.</returns>
	/// <param name="value">Value.</param>
	/// <param name="min">Minimum.</param>
	public static Vector2 clamp_min(this Vector2 value,Vector2 min){
		float x = value.x < min.x ? min.x : value.x;
		float y = value.y < min.y ? min.y : value.y;
		return new Vector2 (x, y);
	}

		public static void Call(this IEnumerator enumerator,MonoBehaviour behaviour){
			behaviour.StartCoroutine (enumerator);
		}
	/// <summary>
	/// Call the specified event if not null.
	/// </summary>
	/// <param name="evt">Evt.</param>
	public static void Call(this Action evt){
		if (evt != null)
			evt.Invoke ();

	}
	/// <summary>
	/// Call the specified event if not null.
	/// </summary>
	/// <param name="evt">Evt.</param>
	/// <param name="param">Parameter.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Call<T>(this Action<T> evt , T param){
		if (evt != null)
			evt.Invoke (param);

	}
	/// <summary>
	/// Call the specified event if not null
	/// </summary>
	/// <param name="evt">Evt.</param>
	/// <param name="param">Parameter.</param>
	/// <param name="param1">Param1.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	/// <typeparam name="V">The 2nd type parameter.</typeparam>
	public static void Call<T,V>(this Action<T,V> evt , T param,V param1){
		if (evt != null)
			evt.Invoke (param,param1);

	}
	#endregion
	#region D
	/// <summary>
	/// convert radiant to degree
	/// </summary>
	/// <param name="value">Value.</param>
	public static float deg2rad(this float value){
		
		return (value * (Mathf.PI / 180f));
	}
	/// <summary>
	///  convert float to double.
	/// </summary>
	/// <returns>The convert.</returns>
	/// <param name="value">Value.</param>
	public static double double_convert(this float value){
		return (double)value;
	}

	#endregion

	#region E
		/// <summary>
		/// Equal the specified value and value.
		/// </summary>
		/// <param name="value">Value.</param>
		public static bool equals<T>(this Enum @this,T value)  {
				int self = (int)(object)@this;
				int target = (int)(object)value;
				return  (self & target) == target;
		
				
		}
/// <summary>
/// Ending string line
/// </summary>
/// <param name="value">Value.</param>
	public static string endline(this string value){
		return value +  "\n";
	}
	/// <summary>
	/// End string with the value
	/// </summary>
	/// <returns>The with.</returns>
	/// <param name="value">Value.</param>
	/// <param name="end_value">End value.</param>
	public static string end_with(this string value,string end_value){
		return value + end_value;
	}
	/// <summary>
	/// call after begin_count to diagnostics how many time the code has been execute
	/// </summary>
	/// <returns>The count.</returns>
	/// <param name="value">Value.</param>
		public static float end_count(this float value,bool logResult = true,int displayLength = 2){
			float result = Time.realtimeSinceStartup - value;
			if(logResult)
				Debug.Log("Counting complete: " + result); 
			return result;

	}


	#endregion
	#region F
	/// <summary>
	/// Floor value to a target value floor(4.5,2) => 4 
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="value">Value.</param>
	/// <param name="target">Target.</param>
	public static float floor_to(this float value,float target){
		return (target * (value / target) - (value % target));
	}
	/// <summary>
	///Floor value to a target value floor(5,2) => 4 
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="value">Value.</param>
	/// <param name="target">Target.</param>
	public static int floor_to(this int value,int target){
		return (target * (value / target) - (value % target));
	}
	/// <summary>
	/// Floor value to a target value floor(5,2) => 4 
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="value">Value.</param>
	/// <param name="target">Target.</param>
	public static double floor_to(this double value,double target){
		return (target * (value / target) - (value % target));
	}
	/// <summary>
	/// Floor the specified value.
	/// </summary>
	/// <param name="value">Value.</param>
	public static int floor(this float value){
		return (int)value;
	}
	/// <summary>
	/// convert string to float 
	/// </summary>
	/// <returns>The convert.</returns>
	/// <param name="this">This.</param>
	public static float float_convert(this string @this){
		return float.Parse(@this);
	}
	/// <summary>
	/// convert double to float with safe range (float.Min,float.Max) and avoidance truncate
	/// </summary>
	/// <returns>The convert.</returns>
	/// <param name="value">Value.</param>
	public static float float_convert(this double value){
		return (value > float.MaxValue ? float.MaxValue : value < float.MinValue ? float.MinValue : (float)value);
	}
	/// <summary>
	/// convert int to float
	/// </summary>
	/// <returns>The convert.</returns>
	/// <param name="value">Value.</param>
	public static float float_convert(this int value){
		return value;
	}
	/// <summary>
	/// Convert double to int
	/// </summary>
	/// <param name="value">Value.</param>
	public static int floor(this double value){
		return (value > int.MaxValue ? int.MaxValue : value < int.MinValue ? int.MinValue : (int)value);
	}
	#endregion
	#region H
	#endregion
	#region I 
	/// <summary>
	/// Is the list not null or empty
	/// </summary>
	/// <param name="this">This.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static bool is_avaiable <T>(this List<T> @this){
		return (@this != null && @this.Count > 0);
	} 
	/// <summary>
	/// Is the array not null or empty
	/// </summary>
	/// <param name="this">This.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static bool is_avaiable <T> (this T [] @this){
		return (@this != null && @this.Length > 0);
	}
	/// <summary>
	/// Is the object not null or empty
	/// </summary>
	/// <param name="this">This.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static bool is_avaiable<T>(this T @this){
		return @this != null;
	}
	public static bool is_active<T>(this T @this) where T : MonoBehaviour{
		return @this != null && @this.gameObject.activeSelf;
	}

	#endregion

	#region M
		/// <summary>
		/// Count how many time value appear in this string
		/// </summary>
		/// <returns>The count.</returns>
		public static int match_count(this string text, string match){
			return	Regex.Matches (Regex.Escape (text), match,RegexOptions.Singleline).Count;
		
		}
	public static string match(this string value,bool caseSensitive,params EaMatchCollection [] matchCollections){
		value = !caseSensitive ? value.ToLower () : value;
		int length = matchCollections.Length;
		for(int i = 0;i<length;i++){
			if((!caseSensitive ? matchCollections[i].key.ToLower(): matchCollections[i].key).Contains(value))
				return matchCollections[i].value;	
			
		}
		return "";
	}
	#endregion
	#region N
	public static float negative(this float value){
		return (value < 0 ? value : -value);
	}
	public static double negative(this double value){
		return (value < 0 ? value : -value);
	}
	#endregion
	#region R
	public static void Repaint(this Camera camera){
			camera.enabled = false;
			camera.enabled = true;
	}
	public static Vector2 replace0(this Vector2 value,Vector2 replacement){
		float x = value.x == 0 ? replacement.x: value.x;
		float y = value.y == 0 ? replacement.y : value.y;
		return new Vector2 (x, y);
	}
	public static string remove(this string @this,string value){
			return @this.Replace (value, "");
	}
		public static string replace(this string @this,string oldValue,string newValue,Shift shiftMethod){
			return @this = @this.Replace (oldValue,((shiftMethod & Shift.Left) == Shift.Left ? newValue : "") + 
				((shiftMethod  & Shift.Left) == Shift.Left || (shiftMethod & Shift.Right) == Shift.Right ? 	oldValue : "") +  
				((shiftMethod & Shift.Right) == Shift.Right ? newValue : ""));
			
	}

		/// <summary>
		/// Replace first match.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="match">Match.</param>
		/// <param name="replacement">Replacement.</param>
		public static string replace1(this string text,string match,string replacement,Shift shift = Shift.None){
			int pos = text.IndexOf (match);
			if (pos < 0)
				return text;
			return  (shift.equals(Shift.Left) ? replacement : "") +  text.Substring (0, pos) + 
				(shift.equals(Shift.None) ? replacement : "") + text.Substring (pos + match.Length) + (shift.equals(Shift.Right) ? replacement : "");	
		}

		//01 | 10 11 &  01 -> 01  -> 10 10 & 11  -> 10
		/// <summary>
		/// Replacement string, using shift operator to shift the text instead of replace it 
		/// </summary>
		/// <param name="this">This.</param>
		/// <param name="oldValues">Old values.</param>
		/// <param name="newValue">New value.</param>
		public static string replace(this string @this,string [] oldValues,string newValue,Shift shiftMethod){
//			Shift shift = newValue.Contains ("<<") ? -1 : newValue.Contains (">>") ? 1 : 0;
//			Debug.Log("SL"  + (shiftMethod & Shift.Left ));
//			Debug.Log (shiftMethod & Shift.Right);

			int length = oldValues.Length;
			for (int i = 0; i < length; i++)
				@this = @this.replace (oldValues [i], newValue, shiftMethod);
			
			return @this;
	}
	public static string remove(this string @this ,params string [] predicates){
		int length = predicates.Length;
		for (int i = 0; i < length; i++)
			@this = @this.Replace (predicates[i], "");
		
		return @this;
	}
	public static List<T> RemoveIf<T>(this List<T> list, Func<T,bool> predicate){
		int length = list.Count;
		for (int i = 0; i < length; i++) {
			if (predicate(list[i])) {
				list.Remove (list [i]);
				i = 0;
				length--;
			}
		}
		return list;
	}	
	public static List<T> Add<T>(this List<T> list,T item){
		list.Add (item);
		return list;

	}	/// <summary>
	/// Rounds the max.
	/// </summary>
	/// <returns>The max.</returns>
	/// <param name="value">Value.</param>
	/// <param name="max">Max.</param>
	/// <param name="mutipler">Mutipler.</param>
		public static int  round_max(this int value,int max, int mutipler){
			if (value > max || max / 2 < value) 
				return max;
			return value.round_max (max / 2, mutipler);
		
		}
	public static int round(this float value){
		return Mathf.RoundToInt(value);
	}
	public static int round(this double value){
		return (int)value;
	}
	public static float rad2deg(this float value){
		return Mathf.Rad2Deg *  (value);
//		return (value * (180 / Mathf.PI));
	}

	public static float round_to(this float value,float target){
		return (target * (value / target) - (value % target) + (value % target  == 0 ? 0 : target));
	}

	public static int round_to(this int value,int target){
		return (target * (value / target) - (value % target) + (value % target  == 0 ? 0 : target));
	}

	public static double round_to(this double value,double target){
		return (target * (value / target) - (value % target) + (value % target  == 0 ? 0 : target));
	}

	#endregion
	#region S
	public static void start_count(out float value){
		value = Time.realtimeSinceStartup;
	}

		public static string short_number(
			this double value,int length  = 0,SNMethod sortingMethod = default(SNMethod),
			int below1000Length = 0, string thousandPredicate = "K",
			string millionPredicate="M" , string billionPredicate = "B",
			string trillionPredicate = "T",	string overloadPredicate =""){

			double pred = value.abs ();
			SNUnit kUnit = pred < 1000 ? SNUnit.None: pred >= 1000 && pred <  1000000 ? SNUnit.Kilo:
				pred >= 1000000 && pred < 1000000000 ? SNUnit.Million:   
				pred >= 1000000000 && pred < 1000000000000 ? SNUnit.Billion :SNUnit.Trillion;


	
		
		string defaultPredicate = "##.", resultPredicate = "";
		string trunc = defaultPredicate;
			if (kUnit == SNUnit.None) {
			for (int i = 0; i < below1000Length; i++)
				trunc += "#";
		} else {
			for (int i = 0; i < length; i++)
				trunc += "#";
		}

		trunc = trunc == defaultPredicate ? "##": trunc;
		if(value > Int32.MaxValue && value < Int32.MinValue) 
				value = sortingMethod == SNMethod.Round ? value.round () : sortingMethod == SNMethod.Floor ? value.floor () : value;

		double tResult = 0;
		switch (kUnit) {
			case SNUnit.None:
			tResult = value;
			resultPredicate = "";
			break;
		
			case SNUnit.Kilo:
			tResult = (value / 1000);
			resultPredicate = thousandPredicate;
			break;

			case SNUnit.Million:
			tResult = (value / 1000000);
			resultPredicate = millionPredicate;
			break;
			case SNUnit.Billion:
			tResult = (value / 1000000000);
			resultPredicate = billionPredicate;
			break;
			case SNUnit.Trillion:
			tResult = (value / 1000000000000);
			resultPredicate = trillionPredicate;
			break;
		}
		if (tResult.abs() < 1)
			return 0.ToString ();
			
		if (tResult.abs () > 1000) {
				return tResult.short_number (length, sortingMethod, below1000Length,
				thousandPredicate, millionPredicate, billionPredicate, trillionPredicate,resultPredicate + overloadPredicate);

		}

		return tResult.ToString (trunc).end_with (resultPredicate + overloadPredicate);



	}

	public static string start_width(this string value,string start_value){
		return start_value + value;
	}
	public static float sqrt(this float value)
	{
		return (Mathf.Sqrt(value));
	}
	public static float sin(this float value){
		return Mathf.Sin (value);
	}
	public static string size(this string value,int size){
	
		return string.Format ("<size={0}>", size) + value + "</size>";
	}
	
	
	#endregion
	#region T
	public static float tan(this float value){
		return Mathf.Tan (value);
	}
	#endregion

		#region P
		public static float pow2(this float value){
			return Mathf.Pow(value,2);
		}
		public static float pow(this float value,float pow){
			return Mathf.Pow(value,pow);
		}
		#endregion
	}


	namespace Editor{
		public static class EaEditorExtension{
			#region EDITOR
			#if UNITY_EDITOR
			public static Texture GetUnityTexture<T>(){
				return UnityEditor.EditorGUIUtility.ObjectContent (null, typeof(T)).image;

			}
			public static T ObjectField<T>(T value,bool allowSeneObject = false) where T : UnityEngine.Object{
				return (T)UnityEditor.EditorGUILayout.ObjectField (value, typeof(T), allowSeneObject);
			}
		
			#endif
			#endregion
		}
	}


}
