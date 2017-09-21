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
namespace Ea{
//	[Serializable]
	public interface IEaSerializable {
		string path{ get; set;}
		bool cryption{get;set;}
	}
	[Serializable]
	public class EaSerializable : IEaSerializable {
		public  string path{get;set;}
		public bool cryption { get; set;}
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
				Debug.Log (typeof(T).Name + " saved!");	

			});
			fileSave.Start ();
			fileSave.Join ();
		}
		private static void delete(this string path){
			File.Delete (path);
		}
		public static T Open<T>(bool decryption = false)  where T : IEaSerializable,new(){
			return Open<T> ("",decryption);
		}
		public static T Open<T>  (string fileName,bool decryption = false) where T : IEaSerializable, new()
		{
			MonoSingleton<EaMobile>.Initialize ();

			string path = EaDevice.filePath (typeof(T).Name.brackets() + fileName + EaDevice.fileType);
			T @out = new T ();
			ThreadStart fileResult = new ThreadStart (() => {
				BinaryFormatter bf = new BinaryFormatter ();
				if (path.exist ()) {
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
				@out.cryption = decryption;
				@out.path = path;
				if(!EaMobile.openedFiles.Contains(path)){
					EaMobile.openedFiles.Add(path);
					EaMobile.onQuit  += delegate {
							Debug.Log(typeof(T).Name + " saving...");
							@out.Save();

				};
	
						EaMobile.onPause += status => {
						if(status){
							Debug.Log(typeof(T).Name + " saving...");
							@out.Save();
						}
					};
				}
			});
			Thread fileThread = new Thread(fileResult);
			fileThread.Start ();
			fileThread.Join ();
		
			return @out;
		}
		public static void Save<T> (this T file) where T: IEaSerializable{
			file.replace ();

		}


	}
	public class EaDevice{
		public const string fileDirectory = "Files";
		public const string fileType  = ".ea";
		public static string filePath<T>()where T : IEaSerializable{
			return (Path.Combine(path(fileDirectory), typeof(T).Name.brackets() + fileType));
		}
		public static string filePath(string fileName){
			return (Path.Combine (path(fileDirectory), fileName));
		}

	
				public static string path(string directory){
		
				#if !UNITY_EDITOR
				return Application.persistentDataPath;
				#else	
					if(!Directory.Exists(Application.dataPath + "/Ea/" + directory))
						Directory.CreateDirectory(Application.dataPath  + directory);

			return Application.dataPath  + fileDirectory;
				#endif
			
		}


	}
	public  static class Cryptography  {
		static readonly string passwordHash = "@er&u@d&ej#a$d!e";
		static readonly string saltKey = "@erudejade@aBcDeF";
		static readonly string viKey =  "@er$u#de^ja&d*e)";
		static private byte  [] cipherTextBytes, keyBytes,plainTextBytes;

		static private RijndaelManaged symmetricKey;
		static private ICryptoTransform decryptor,encryptor;
		static private MemoryStream memoryStream;
		static private CryptoStream cryptoStream;
//		static private BinaryFormatter bf = new BinaryFormatter();
		public static string decrypt(this string encryptedText)
		{
			cipherTextBytes = Convert.FromBase64String(encryptedText);
			keyBytes = new Rfc2898DeriveBytes(passwordHash, Encoding.ASCII.GetBytes(saltKey)).GetBytes(256 / 8);
			symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

			decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(viKey));
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
			keyBytes = new Rfc2898DeriveBytes (passwordHash,Encoding.ASCII.GetBytes(saltKey)).GetBytes(256/8);
			symmetricKey = new RijndaelManaged(){Mode = CipherMode.CBC,Padding =PaddingMode.Zeros};
			encryptor = symmetricKey.CreateEncryptor(keyBytes,Encoding.ASCII.GetBytes(viKey));

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
		
	
