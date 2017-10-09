using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ea;
using Sirenix.OdinInspector;
public class EaRotation : MonoBehaviour {
	[System.Flags]
	public enum Direction{
		x = 0x01,
		y = x << 1,
		z = y << 1,
	}
	[BoxGroup("Rotation Info")]
	public Direction direction;
	[BoxGroup("Rotation Info")]
	public float speed;

	private Vector3 rotateDirection;


	void LoadConfig(){
		// Time.deltaTime * speed * 
		rotateDirection  =  new Vector3 (direction.equals (Direction.x) ? 1 : 0, direction.equals (Direction.y) ? 1 : 0, direction.equals (Direction.z) ? 1 : 0);;
	}
	void Start(){
		LoadConfig ();
	}
	void Update(){
		transform.Rotate (rotateDirection * speed * Time.deltaTime);
	}

		
}
