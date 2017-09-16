using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
namespace Ea.Pool{
public interface IPool{		
	GameObject toInstance{ get;  }
	int poolAmount{get;}
	void CreatePool();

}

public static class PoolMechanism{



	public static void Initialize(List<IPool> pools){
		int length = pools.Count;
		pools.ToList ().ForEach (pool => pool.CreatePool ());
	}
	//<-- starting with input prefabs , casting to type , add to pool holder. get by key , get poolholder key instance -->
	public static void CreatePool<T>(IPool data) where T : MonoBehaviour{
		T POOL = default(T);
		List<T> INITALIZED = new List<T> ();
		GameObject INITALIZE;
		for (int i = 0; i < data.poolAmount; i++) {
			INITALIZE = MonoBehaviour.Instantiate<GameObject> (data.toInstance, Vector3.zero, Quaternion.identity);
			INITALIZE.SetActive (false);
			POOL = INITALIZE.GetComponent<T> ();
			INITALIZED.Add (POOL);
		}
			Pools<T>.instance.SetItem (INITALIZED);

		
	}
	}
}