using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
namespace Ea{
[Serializable]
	public class EaDictionary <TKey,TValue> :EaSerializable ,IEnumerable{
	#region VARIABLE
		[SerializeField]List<TKey> _Keys;
		[SerializeField]List<TValue> _Values;

		public List<TKey> Keys{ get{return _Keys;}}
		public List<TValue>Values {get{ return _Values;}}

		private Dictionary<TKey,TValue> _Dictionary;
		public Dictionary<TKey,TValue> Dictionary{ 
			get { 
				if (_Dictionary == null)
					_Dictionary = new Dictionary<TKey, TValue> ();
		
				return _Dictionary;
			}
		}

		public int Count{ get { return _Keys.Count; } }
		private int index;
	#endregion
	
	#region PUBLIC METHOD
		public IEnumerator GetEnumerator(){
			return (IEnumerator)this;
		}
	public EaDictionary(){

			_Keys = new List<TKey> ();
			_Values = new List<TValue> ();
			_Dictionary = new Dictionary<TKey, TValue> ();
	}
	public EaDictionary(Dictionary<TKey,TValue>collection){
			_Keys= new List<TKey> (collection.Keys);
			_Values = new List<TValue> (collection.Values);
			_Dictionary = new Dictionary<TKey, TValue> (collection);

		}
	


	public bool ContainsKey(TKey key){
		return (_Keys.Contains(key));
	}
	public bool ContainsValue(TValue value){
		return (_Values.Contains(value));
	}
		public void Clear(){ 
			if (Keys != null && Values != null) {
				_Keys.Clear ();
				_Values.Clear ();
				Dictionary.Clear ();
			}
		}
	public void Add(TKey key,TValue value){
		if (ContainsKey (key))
			throw new DuplicateKeyException (key);

			_Keys.Add (key);
			_Values.Add (value);
			Dictionary.Add (key, value);

	}
	public void Remove(TKey key){
			int index = _Keys.IndexOf (key);

		if (index == -1)
			throw new KeyNotFoundException (key + "not exist,please check again!");
		
			_Keys.RemoveAt (index);
		_Values.RemoveAt (index);
			Dictionary.Remove (key);

	
	
	}

	
	#endregion
	#region INDEXER
	public TValue this [TKey key]{
		get{
				int index = _Keys.IndexOf (key);

			if (index == -1)
				throw new KeyNotFoundException (key + "not exist,please check again!");

			return _Values [index];
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
