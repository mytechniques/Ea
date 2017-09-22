using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Ea;
using EaEditor;

namespace Ea.Editor{
public delegate void GetTextureSize(TextureImporter textureImporter,ref int width,ref int height);
	public  class EaTextureImporter : AssetPostprocessor {
	GetTextureSize textureSizeDelegate;
	EaTexture _setting;
	EaTexture setting{
		get{
			return _setting ??(_setting = Resources.Load<EaTexture>("EaTextureSetting"));
		}
	}
	void OnPreprocessTexture(){
		if (!setting.enableTextureImporter)
			return;
		
		TextureImporter textureImporter  = (TextureImporter)assetImporter;
		textureImporter.sRGBTexture = true;
		TextureSize size =	GetTextureSize (textureImporter);
		int max = size.width > size.height ? size.width : size.height;
		max = max.round_max (setting.textureMaxSize,0x02);
		textureImporter.maxTextureSize = max;
		textureImporter.textureCompression = TextureImporterCompression.Uncompressed;



	}
//	public  int fibonacy_binary (int value , int max){
//		if (value > max)
//			return max;
//		if (max / 2 < value)
//			return max;
//		
//		
//		return fibonacy_binary (value, max / 2);
//
//
//	}
	TextureSize GetTextureSize(TextureImporter textureImporter){
		var method = typeof(TextureImporter).GetMethod ("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
		textureSizeDelegate = System.Delegate.CreateDelegate (typeof(GetTextureSize), method) as GetTextureSize;
		TextureSize size = new TextureSize ();
		textureSizeDelegate.Invoke (textureImporter, ref size.width, ref size.height);	
		return size;

	}
	struct TextureSize{
		public	int width,height;
		public override string ToString ()
		{
			return string.Format("Width: {0} , Height: {1}",width,height);
		}
		}
	}
  
}