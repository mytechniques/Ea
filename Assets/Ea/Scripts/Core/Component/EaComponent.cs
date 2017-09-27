using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ea;
namespace Ea{
	public interface EaIComponent{}
}
public class EaComponent<T> : MonoBehaviour  where T : MonoBehaviour,EaIComponent{
		[HideInInspector]
		public  T component;
		protected virtual void Awake(){
			component = gameObject.AddComponent<T> ();
		}
	}
public class EaComponent<T0,T1> :MonoBehaviour 
		where T0: MonoBehaviour ,EaIComponent 
		where T1: MonoBehaviour,EaIComponent{
	[HideInInspector]
	public T0 component;
	[HideInInspector]
	public T1 component1;
		protected virtual void Awake() {
			component = gameObject.AddComponent<T0> ();
			component1 = gameObject.AddComponent<T1> ();
		}
	}
public class EaComponent<T0,T1,T2> :MonoBehaviour
	where T0: MonoBehaviour,EaIComponent 
	where T1: MonoBehaviour,EaIComponent
	where T2: MonoBehaviour,EaIComponent
	{
	[HideInInspector]
	public T0 component;
	[HideInInspector]
	public T1 component1;
	[HideInInspector]
	public T2 component2;


			protected virtual void Awake() {
				component = gameObject.AddComponent<T0> ();
				component1 = gameObject.AddComponent<T1> ();
				component2 = gameObject.AddComponent<T2> ();
			}
	}
	

public class EaComponent<T0,T1,T2,T3> :MonoBehaviour
	where T0: MonoBehaviour,EaIComponent 
	where T1: MonoBehaviour,EaIComponent
	where T2: MonoBehaviour,EaIComponent
	where T3: MonoBehaviour,EaIComponent
{
	[HideInInspector]
	public T0 component;
	[HideInInspector]
	public T1 component1;
	[HideInInspector]
	public T2 component2;
	[HideInInspector]
	public T3 component3;
	[HideInInspector]


	protected virtual void Awake() {
		component = gameObject.AddComponent<T0> ();
		component1 = gameObject.AddComponent<T1> ();
		component2 = gameObject.AddComponent<T2> ();
		component3 = gameObject.AddComponent<T3> ();
	}
}
public class EaComponent<T0,T1,T2,T3,T4> :MonoBehaviour
	where T0: MonoBehaviour,EaIComponent 
	where T1: MonoBehaviour,EaIComponent
	where T2: MonoBehaviour,EaIComponent
	where T3: MonoBehaviour,EaIComponent
	where T4: MonoBehaviour,EaIComponent

{
	[HideInInspector]
	public T0 component;
	[HideInInspector]
	public T1 component1;
	[HideInInspector]
	public T2 component2;
	[HideInInspector]
	public T3 component3;
	[HideInInspector]
	public T4 component4;



	protected virtual void Awake() {
		component = gameObject.AddComponent<T0> ();
		component1 = gameObject.AddComponent<T1> ();
		component2 = gameObject.AddComponent<T2> ();
		component3 = gameObject.AddComponent<T3> ();
		component4 = gameObject.AddComponent<T4> ();

	}
}

public class EaComponent<T0,T1,T2,T3,T4,T5> :MonoBehaviour
	where T0: MonoBehaviour,EaIComponent 
	where T1: MonoBehaviour,EaIComponent
	where T2: MonoBehaviour,EaIComponent
	where T3: MonoBehaviour,EaIComponent
	where T4: MonoBehaviour,EaIComponent
	where T5: MonoBehaviour,EaIComponent

{	
	[HideInInspector]
	public T0 component;
	[HideInInspector]
	public T1 component1;
	[HideInInspector]
	public T2 component2;
	[HideInInspector]
	public T3 component3;
	[HideInInspector]
	public T4 component4;
	[HideInInspector]
	public T5 component5;


	protected virtual void Awake() {
		component = gameObject.AddComponent<T0> ();
		component1 = gameObject.AddComponent<T1> ();
		component2 = gameObject.AddComponent<T2> ();
		component3 = gameObject.AddComponent<T3> ();
		component4 = gameObject.AddComponent<T4> ();
		component5 = gameObject.AddComponent<T5> ();
	}
}



public class EaComponent<T0,T1,T2,T3,T4,T5,T6> :MonoBehaviour
	where T0: MonoBehaviour,EaIComponent 
	where T1: MonoBehaviour,EaIComponent
	where T2: MonoBehaviour,EaIComponent
	where T3: MonoBehaviour,EaIComponent
	where T4: MonoBehaviour,EaIComponent
	where T5: MonoBehaviour,EaIComponent
	where T6: MonoBehaviour,EaIComponent



{	
	[HideInInspector]
	public T0 component;
	[HideInInspector]
	public T1 component1;
	[HideInInspector]
	public T2 component2;
	[HideInInspector]
	public T3 component3;
	[HideInInspector]
	public T4 component4;
	[HideInInspector]
	public T5 component5;
	[HideInInspector]
	public T6 component6;



	protected virtual void Awake() {
		component = gameObject.AddComponent<T0> ();
		component1 = gameObject.AddComponent<T1> ();
		component2 = gameObject.AddComponent<T2> ();
		component3 = gameObject.AddComponent<T3> ();
		component4 = gameObject.AddComponent<T4> ();
		component5= gameObject.AddComponent<T5> ();
		component6 = gameObject.AddComponent<T6> ();

	}
}




#region COMPONENT


	public class EaRigidbody2D : MonoBehaviour, EaIComponent{
		private Rigidbody _rigidbody;
		public new  Rigidbody rigidbody{ get{return _rigidbody ?? (_rigidbody = GetComponent<Rigidbody>()); }}
	}
	public class EaSpriteRenderer : MonoBehaviour,EaIComponent{
		private SpriteRenderer _spriteRenderer;
		public   SpriteRenderer spriteRenderer{ get{return _spriteRenderer ?? (_spriteRenderer = GetComponent<SpriteRenderer>()); }}
	}
	public class EaImage : MonoBehaviour,EaIComponent{
		private Image _image;
		public   Image image{ get{return _image ?? (_image = GetComponent<Image>()); }}
	}
	public class EaButton : MonoBehaviour,EaIComponent{
		private Button _button;
		public   Button button{ get{return _button ?? (_button = GetComponent<Button>()); }}
	}
	public class EaBoxCollider2D : MonoBehaviour,EaIComponent{
		private BoxCollider2D _boxCollider2D;
		public   BoxCollider2D boxCollider2D{ get{return _boxCollider2D ?? (_boxCollider2D = GetComponent<BoxCollider2D>());}}
	}
	public class EaBoxCollider : MonoBehaviour,EaIComponent{
		private BoxCollider _boxCollider;
		public   BoxCollider boxCollider{ get{return _boxCollider ?? (_boxCollider = GetComponent<BoxCollider>());}}
	}
	public class EaRectTransform : MonoBehaviour,EaIComponent{
		private  RectTransform _rectTransform;
		public   RectTransform rectTransform{ get{return _rectTransform ?? (_rectTransform = GetComponent<RectTransform>());}}
	}
#endregion
