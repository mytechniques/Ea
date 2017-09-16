using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
using Sirenix.OdinInspector;
public class Pools <V>  where V : MonoBehaviour {
	private static Pools<V> _instance;
	public static Pools<V> instance{
		get{
			if (_instance == null) {
				_instance = new Pools<V> ();
				Debug.LogFormat ("{0}", _instance.GetType ().Name.Replace("`1","<" + typeof(V) + ">") ); 
			}

			return _instance;


		}
	}


	private List <V> pools;
	public V GetItem(){
		int length = pools.Count;
		for (int i = 0; i < length; i++) {
			if (!pools [i].gameObject.activeSelf)
				return pools [i];
		}

		return null;
	}
	public void SetItem(List<V> pools){
		this.pools = pools;
	}

}
