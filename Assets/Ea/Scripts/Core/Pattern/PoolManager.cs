using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Ea.Pool;
using Ea;
public class PoolManager : MonoBehaviour {
	public  List<GameObject> Pools;
	void Awake(){
		PoolMechanism.Initialize (Pools.Select(pool=>pool.GetComponentInChildren<IPool>()).ToList());

	}
}
public class Poolable<T>:MonoBehaviour,IPool where T : MonoBehaviour{
	public GameObject toInstance{get{return gameObject;}}
	[SerializeField]
	private int _poolAmount;
	public int poolAmount{ get{ return _poolAmount;}}
	public void CreatePool(){
		PoolMechanism.CreatePool <T>(this);			
	}
}