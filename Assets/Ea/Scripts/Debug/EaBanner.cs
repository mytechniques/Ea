using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
namespace Ea{
public  class EaBanner : MonoBehaviour  {
	public RectTransform rect{get{ return _rect ?? (_rect = GetComponent<RectTransform> ());}}
	private RectTransform _rect;
		public void CloseAd(){
			Debug.Log (":CKI");
			Destroy (gameObject);
		}
		public void ClickAd(){
			Application.OpenURL ("https://eaunity.wordpress.com/");
		}
	}

}
