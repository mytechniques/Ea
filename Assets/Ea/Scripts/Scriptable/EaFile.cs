using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Ea{
public class EaFile : ScriptableObject {
	[TabGroup("EaFile","Cryptography")]
	public string passwordHash = "@er&u@d&ej#a$d!e",  saltKey = "@erudejade@aBcDeF", viKey =  "@er$u#de^ja&d*e)";
	[TabGroup("EaFile","Files"),PropertyOrder(-1)]
	public  string fileDirectory = "Ea/Files",fileType = ".dll";

	}
}
