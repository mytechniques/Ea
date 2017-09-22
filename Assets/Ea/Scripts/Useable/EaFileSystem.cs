using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
public  static class EaFileSystem  {
	public  static T Open<T>(bool security = false) where T: IEaSerializable , new(){	
		return EaSystem.Open<T> (security);
	}
	public static T Open<T>(string fileName,bool security = false) where T :IEaSerializable,new(){
		return EaSystem.Open <T>(fileName,security);
	}
	#region GET
	static T GetValue<T>(EaDictionary<string,T> collection,string key){
		if (collection.ContainsKey (key))
			return collection [key];

		Debug.LogErrorFormat("Key: {0} not found , return default value!",key);
		return default(T);
	}
	public static float GetFloat(string key){
		return	GetValue (EaSystem.dataFloat, key);
	}

	public static int GetInt(string key){
		return GetValue (EaSystem.dataInt, key);

	}
	public static string GetString(string key){
		return GetValue (EaSystem.dataString, key);

	}
	public static bool GetBool(string key){
		return GetValue (EaSystem.dataBool, key);

	}

	#endregion
	#region SET
	public static void SetFloat(string key,float value){
		if (EaSystem.dataFloat.ContainsKey (key)) 
			EaSystem.dataFloat [key] = value;
		else
			EaSystem.dataFloat.Add (key, value);
	
	}
	public static void SetString(string key,string value){
		if (EaSystem.dataString.ContainsKey (key)) 
			EaSystem.dataString [key] = value;
		else
			EaSystem.dataString.Add (key, value);
	}
	public static void SetInt(string key , int value){
		if (EaSystem.dataInt.ContainsKey (key)) 
			EaSystem.dataInt [key] = value;
		else
			EaSystem.dataInt.Add (key, value);
	}
	public static void SetBool(string key , bool value){
		if (EaSystem.dataBool.ContainsKey (key)) 
			EaSystem.dataBool [key] = value;
		else
			EaSystem.dataBool.Add (key, value);
	}

	#endregion
}
