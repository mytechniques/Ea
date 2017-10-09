using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Ea;


namespace Ea{
public  class EaTexture : ScriptableObject{
	[BoxGroup("Texture Settings"),GUIColor(1,1,1,1)]
	public bool enableTextureImporter = true;
	[BoxGroup("Texture Settings"),ShowIf("enableTextureImporter"),GUIColor(1,0,0,.5f)]
	public int textureMaxSize {
		get{
			return _textureMaxSize;
		}
		set{
			value = value > maxSize ? maxSize : value < minSize ? minSize : value;
			_textureMaxSize = value.round_max (maxSize,0x02);
		}
	}


	private int _textureMaxSize = 8192;
	private const int maxSize = 8192;
	private const int minSize = 32;

	}
}

