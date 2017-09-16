#define DEBUG
//#undef DEBUG    //-> ENABLE THIS IF YOU WANT SEE RAW BINARY ON EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;
using System;
namespace Ea{
//	[Serializable]
	public interface IEaSerializable {
		string path{ get; set;}
	}
	[Serializable]
	public class EaSerializable : IEaSerializable {
		public virtual  string path{get;set;}
	}


	public static class EaJson{
		public static string json_encode<T>(this T @object){
			 return JsonUtility.ToJson(@object);

		}
		public static T json_decode<T>(this string @string){
			return JsonUtility.FromJson<T>(@string);

		}
	}

	public static class EaFile{
		public static bool exist(this string path){
			if (File.Exists (path))
				return true;
			
			return false;
				
		
		}
		public static string to_string(this byte [] bytes){
			return System.Text.Encoding.ASCII.GetString (bytes);
		}
		public static Stream stream(this string @string)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(@string);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
		private static void replace<T>(this T file) where T  : IEaSerializable{
			if (file.path.exist ())
					file.path.delete ();
			
			using (FileStream fs = File.Open (file.path, FileMode.Create)) {
				BinaryFormatter bf = new BinaryFormatter ();
				#if UNITY_EDITOR && DEBUG
				var json = EaJson.json_encode (file);
				bf.Serialize (fs, json);
				#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_IOS
				bf.Serialize(fs,file);
				#endif

			}
		}
		private static void delete(this string path){
			File.Delete (path);
		}
		public static T Open<T>()  where T : IEaSerializable,new(){
			return Open<T> ("");
		}
		public static T Open<T>  (string fileName) where T : IEaSerializable, new()
		{
			string path = EaDevice.dataPath (typeof(T).Name.brackets() + fileName + EaDevice.fileType);
			T @out = new T ();
			BinaryFormatter bf = new BinaryFormatter();

		

			if (path.exist ()) {
				using (FileStream fs = File.Open (path, FileMode.Open)) {
					try {
						#if UNITY_EDITOR && DEBUG
						var json = (string)bf.Deserialize (fs);
						@out = json.json_decode<T>();
						#elif UNITY_ANDROID || UNITY_IOS || UNITY_IOS
						@out = (T)bf.Deserialize(fs);
						#endif
					} catch (Exception fileFormatException) {
						Debug.LogError ("Can't deserialize, file format not found!");
						throw fileFormatException;
					}
				
				}
			} else using (FileStream fs = File.Create(path)){
					try{
						#if UNITY_EDITOR && DEBUG
						bf.Serialize (fs,@out.json_encode());
						#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
						bf.Serialize(fs,@out);
						#endif
					}
					catch(Exception failedSerializeException){
						Debug.LogError ("Can't serialize object,make sure the object have attribute [System.Serializable]");
						throw failedSerializeException;
					}
			}

			@out.path = path;
			return @out;
		}
		public static void Save<T> (this T file) where T: IEaSerializable{
				file.replace ();

		}


	}
	public class EaDevice{
		public const string dataPathDirectory = "/Ea/Files";
		public const string fileType  = ".json";
		public static string dataPath<T>()where T : IEaSerializable{
			return (Path.Combine(path , typeof(T).Name.brackets() + fileType));
		}
		public static string dataPath(string fileName){
			return (Path.Combine (path, fileName));
		}
	
		public static string path{
			get{
				#if !UNITY_EDITOR
				return Application.persistentDataPath;
				#else	
				if(!Directory.Exists(Application.dataPath + dataPathDirectory))
					Directory.CreateDirectory(Application.dataPath  + dataPathDirectory);

				return Application.dataPath  + dataPathDirectory;
				#endif
			}
		}


	}
}
		
	
