#define DEBUG
#undef DEBUG    //-> ENABLE THIS IF YOU WANT SEE RAW BINARY ON EDITOR
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Linq;
using System;
using System.Threading;
[Serializable]
public class EaSerializable : Ea.IEaSerializable {
	public  string path{get;set;}
	public bool cryption { get; set;}
}

namespace Ea{
	public interface IEaSerializable {
		string path{ get; set;}
		bool cryption{get;set;}
	}
	public static class EaJson{
		public static string json_encode<T>(this T @object){
			 return JsonUtility.ToJson(@object);

		}
		public static T json_decode<T>(this string @string){
			return JsonUtility.FromJson<T>(@string);

		}
	}

	public static class EaSystem{
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
			bool encryption = file.cryption;
			if (file.path.exist ())
					file.path.delete ();
			Thread fileSave = new Thread (() => {
				using (FileStream fs = File.Open (file.path, FileMode.Create)) {
					BinaryFormatter bf = new BinaryFormatter ();

					#if UNITY_EDITOR && DEBUG
					var json = EaJson.json_encode (file);
					bf.Serialize (fs, json);
					#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_IOS
					if(encryption){
					string	cryptor = file.json_encode<T>().encrypt();
					bf.Serialize(fs,cryptor);
				}
				else
					bf.Serialize(fs, file);
					#endif

				}
				Debug.Log (typeof(T).Name.color("0000FF") + " SAVED!".color("00FF00"));	

			});
			fileSave.Start ();
			fileSave.Join ();
		}
		private static void delete(this string path){
			File.Delete (path);
		}



		public static T Open<T>(bool decryption = true)  where T : IEaSerializable,new(){
			return Open<T> ("",decryption);
		}
		public static T Open<T>  (string fileName,bool decryption = true) where T : IEaSerializable, new()
		{
			MonoSingleton<EaMobile>.Initialize ();

			string path = EaDevice.filePath (typeof(T).Name.brackets() + fileName + EaDevice.eaFile.fileType);
			T @out = new T ();
			ThreadStart fileResult = new ThreadStart (() => {
//				Debug.Log("START THREAD");
				BinaryFormatter bf = new BinaryFormatter ();
				if (path.exist ()) {
//					Debug.Log("OPEN FILE:" + path);

					using (FileStream fs = File.Open (path, FileMode.Open)) {
						try {
							#if UNITY_EDITOR && DEBUG
							var json = (string)bf.Deserialize (fs);
							@out = json.json_decode<T> ();
							#elif UNITY_ANDROID || UNITY_IOS || UNITY_IOS
							if(decryption){
								string decryptor = (string)bf.Deserialize(fs);
								@out = decryptor.decrypt().json_decode<T>();
							}
							else
								@out = (T)bf.Deserialize(fs);
							#endif
						} catch (Exception fileFormatException) {
							Debug.LogError ("Can't deserialize, file format not found!");
							throw fileFormatException;
						}
				
					}
				} else
					using (FileStream fs = File.Create (path)) {
//						Debug.Log("CREATE FILE:" + path);
						try {
							#if UNITY_EDITOR && DEBUG
							bf.Serialize (fs, @out.json_encode ());
							#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS
							if(decryption){
								string encryptor =  @out.json_encode<T>().encrypt();
								bf.Serialize(fs,encryptor);
							}else
								bf.Serialize(fs,@out);
							#endif
						} catch (Exception failedSerializeException) {
							Debug.LogError ("Can't serialize object,make sure the object have attribute [System.Serializable]");
							throw failedSerializeException;
						}
					}	
//				Debug.Log("THREAD PROCESS");
				@out.cryption = decryption;
				@out.path = path;
				if(!EaMobile.openedFiles.Contains(path)){
					EaMobile.openedFiles.Add(path);
//					Debug.Log("Subscribed");
					EaMobile.onQuit  += delegate {
						Debug.Log(typeof(T).Name.color("0000FF") + " SAVING...".color("00FF00"));
							@out.Save();

				};
	
						EaMobile.onPause += status => {
						if(status){
							Debug.Log(typeof(T).Name.color("0000FF") + " SAVING...".color("00FF00"));
							@out.Save();
						}
					};
				}
			});
			Thread fileThread = new Thread(fileResult);
			fileThread.Start ();
			fileThread.Join ();
//			Debug.Log ("THEAD CALLED");
			return @out;
		}
		public static void Save<T> (this T file) where T: IEaSerializable{
			file.replace ();

		}
		#region KEY VALUE 
		#region VARIABLE
		static EaDictionary<string,float>_dataFloat;
		static EaDictionary<string,int>_dataInt;
		static EaDictionary<string,bool> _dataBool;
		static EaDictionary<string,string> _dataString;

		public static EaDictionary<string,float> dataFloat{get{return _dataFloat ?? (_dataFloat =  LoadData<float>());}}
		public	static EaDictionary<string,int> dataInt{get{ return _dataInt ??(_dataInt =  LoadData<int>());}}
		public	static EaDictionary<string,bool> dataBool{get {return _dataBool ??(_dataBool = LoadData<bool>());}}
		public	static EaDictionary<string,string>dataString{get{ return _dataString ?? (_dataString = LoadData<string>());}}

		#endregion

		#region RETURN VALUE
		static EaDictionary<string,T>  LoadData<T>(){
			EaDictionary<string,T> fileData =Open<EaDictionary<string,T>>(typeof(T).Name);
			return fileData;

		}
		#endregion
		[System.Serializable]
	public	class EaKv<T> : EaSerializable{

			[SerializeField]
			private List<string> Keys;
			[SerializeField]
			private	 List<T> Values;

			public EaKv(){
				Keys = new List<string>();
				Values = new List<T>();
			
			}
			public bool ContainsValue(T value){
				return (Values.FirstOrDefault (v => v.Equals(value)).is_avaiable () ? true : false);
			}
			public void Add(string key,T value){
				if (ContainsKey (key))
					throw new DuplicateKeyException ();
				
				Keys.Add (key);
				Values.Add (value);
				Debug.LogFormat ("Key: {0},Values: {1}", key, value);
			}
			public void Remove(string key){
				int index = Keys.IndexOf (key);
				if(index == -1)
					throw  new KeyNotFoundException();
				
				Keys.RemoveAt(index);
				Values.RemoveAt(index);

				
			}
			public void Clear(){
				if (Keys != null && Values != null) {
					Keys.Clear ();
					Values.Clear ();
				}
			}
			public   T this [string key]{
				get{
					int index = Keys.IndexOf (key);
					if (index == -1)
						throw new KeyNotFoundException ();
					
					return Values [index];
				}
				set{ 
					Add (key, value);

				}
			} 
			public bool ContainsKey(string key){
				return (Keys.FirstOrDefault (k => k == key) != null ? true : false);
			}
		}
		public class DuplicateKeyException : Exception{
			public override string Message {
				get {
					return "Duplicate key";
				}
			}
		}
		#endregion

	}
	public class EaDevice{
		private static EaFile _eaFile;
		public static EaFile eaFile{
			get{
				return _eaFile ?? (_eaFile = Resources.Load<EaFile> (typeof(EaFile).Name));
			}
		}
	
		public static string filePath<T>()where T : IEaSerializable{
			return (Path.Combine(path(eaFile.fileDirectory), typeof(T).Name.brackets() + eaFile.fileType));
		}
		public static string filePath(string fileName){
			return (Path.Combine (path(eaFile.fileDirectory), fileName));
		}

	
		public static string path(string directory){
			string combinedPath = string.Empty;
				#if !UNITY_EDITOR 
				combinedPath = Path.Combine(Application.persistentDataPath,directory);
				#else	
					combinedPath  = Path.Combine(Application.dataPath,directory);
				#endif
			if(!Directory.Exists(combinedPath))
				Directory.CreateDirectory(combinedPath);
			return 	combinedPath;

			
		}


	}
	public  static class Cryptography  {

		static private byte  [] cipherTextBytes, keyBytes,plainTextBytes;

		static private RijndaelManaged symmetricKey;
		static private ICryptoTransform decryptor,encryptor;
		static private MemoryStream memoryStream;
		static private CryptoStream cryptoStream;
//		static private BinaryFormatter bf = new BinaryFormatter();
		public static string decrypt(this string encryptedText)
		{
			cipherTextBytes = Convert.FromBase64String(encryptedText);
			keyBytes = new Rfc2898DeriveBytes(EaDevice.eaFile.passwordHash, Encoding.ASCII.GetBytes(EaDevice.eaFile.saltKey)).GetBytes(256 / 8);
			symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

			decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(EaDevice.eaFile.viKey));
			memoryStream = new MemoryStream(cipherTextBytes);
			cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			plainTextBytes = new byte[cipherTextBytes.Length];

			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
		}



		public static string encrypt(this string input){
			plainTextBytes = Encoding.UTF8.GetBytes (input);
			keyBytes = new Rfc2898DeriveBytes (EaDevice.eaFile.passwordHash,Encoding.ASCII.GetBytes(EaDevice.eaFile.saltKey)).GetBytes(256/8);
			symmetricKey = new RijndaelManaged(){Mode = CipherMode.CBC,Padding =PaddingMode.Zeros};
			encryptor = symmetricKey.CreateEncryptor(keyBytes,Encoding.ASCII.GetBytes(EaDevice.eaFile.viKey));

			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					cipherTextBytes = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}
			return Convert.ToBase64String(cipherTextBytes);
		}
	}
}
		
	
