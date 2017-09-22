using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
namespace Ea{
[Serializable]
	public class EaDictionary <TKey,TValue> :  EaSerializable{
	#region VARIABLE
	[SerializeField]List<TKey> Keys;
	[SerializeField]List<TValue> Values;
	#endregion

	#region PUBLIC METHOD
	public EaDictionary(){
		Keys = new List<TKey> ();
		Values = new List<TValue> ();

	}


	public bool ContainsKey(TKey key){
		return (Keys.Contains(key));
	}
	public bool ContainsValue(TValue value){
		return (Values.Contains(value));
	}

	public void Add(TKey key,TValue value){
		if (ContainsKey (key))
			throw new DuplicateKeyException (key);

		Keys.Add (key);
		Values.Add (value);
	}
	public void Remove(TKey key){
		int index = Keys.IndexOf (key);

		if (index == -1)
			throw new KeyNotFoundException (key + "not exist,please check again!");
		
		Keys.RemoveAt (index);
		Values.RemoveAt (index);


	
	}
	#endregion
	#region INDEXER
	public TValue this [TKey key]{
		get{
			int index = Keys.IndexOf (key);

			if (index == -1)
				throw new KeyNotFoundException (key + "not exist,please check again!");

			return Values [index];
		}
		set
		{
			Add (key, value);
		}
	}
	#endregion
		

	public class DuplicateKeyException:Exception {
		public string key{get;set;}
			public DuplicateKeyException(object key){
				this.key = key.ToString();
		}
		public override string Message {
			get {
				return  string.Format("{0} has exist,try another key!",key);
			}
		}
	}
	}
}
