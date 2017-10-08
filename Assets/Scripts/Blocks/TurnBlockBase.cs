﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TurnBlockBase : MonoBehaviour {

	public enum StartPosition{
		Left,
		Up,
		Right,
		Down
	}

	float[] targetPoint = new float[4];
	Dictionary<int, StartPosition> SphereList = new Dictionary<int, StartPosition>();

	[SerializeField]
	float targerFromLeft, targerFromUp, targerFromRight, targerFromDown;
	

	// Use this for initialization
	void Start () {
		targetPoint[(int)StartPosition.Left] = targerFromLeft;
		targetPoint[(int)StartPosition.Up] = targerFromUp;
		targetPoint[(int)StartPosition.Right] = targerFromRight;
		targetPoint[(int)StartPosition.Down] = targerFromDown;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	

	void OnCollisionEnter(Collision other)
	{
		if(!SphereList.ContainsKey(other.gameObject.GetInstanceID())){
			if(Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.z - other.transform.position.z)){
				if(transform.position.x - other.transform.position.x > 0){
					SphereList.Add(other.gameObject.GetInstanceID(), StartPosition.Left);
				}else{
					SphereList.Add(other.gameObject.GetInstanceID(), StartPosition.Right);
				}
			}else{
				if(transform.position.z - other.transform.position.z < 0){
					SphereList.Add(other.gameObject.GetInstanceID(), StartPosition.Up);
				}else{
					SphereList.Add(other.gameObject.GetInstanceID(), StartPosition.Down);
				}
			}
		}

		// other.transform.GetInstanceID();
		/*
		other.transform.root.transform.Rotate(0,2,0);
		if(other.transform.root.transform.eulerAngles.y > 90){
			other.transform.root.transform.eulerAngles = new Vector3(0f, 90f, 0f);
		}
		// other.transform.Rotate(0,-1,0);
		 */
	}
}