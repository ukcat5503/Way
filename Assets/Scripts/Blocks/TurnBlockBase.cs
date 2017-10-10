﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TurnBlockBase : MonoBehaviour {

	class ObjectInfo{
		public StartPosition startPosition;
		public GameObject obj;
		public ObjectInfo(StartPosition p, GameObject o){
			startPosition = p;
			obj = o;
		}
	}

	/// <summary>
	/// Z↑ X→を基準にしたときの方角
	/// </summary>
	public enum StartPosition{
		West,
		North,
		East,
		South
	}

	float[] targetPoint = new float[4];
	Dictionary<int, ObjectInfo> SphereList = new Dictionary<int, ObjectInfo>();
	List<int> waitDelete = new List<int>();

	[SerializeField]
	float targerFromEast, targerFromUp, targerFromWest, targerFromDown;
	

	// Use this for initialization
	void Start () {
		targetPoint[(int)StartPosition.East] = targerFromEast;
		targetPoint[(int)StartPosition.North] = targerFromUp;
		targetPoint[(int)StartPosition.West] = targerFromWest;
		targetPoint[(int)StartPosition.South] = targerFromDown;
		
	}
	
	// Update is called once per frame
	void Update () {

		foreach (var item in SphereList)
		{
			item.Value.obj.transform.root.transform.Rotate(0,2,0);
			if(item.Value.obj.transform.root.transform.eulerAngles.y > 90){
				item.Value.obj.transform.root.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				waitDelete.Add(item.Key);
			}
		}

		foreach (var item in waitDelete)
		{
			SphereList.Remove(item);
		}

			
		
		// other.transform.Rotate(0,-1,0);
	}
	

	void OnCollisionEnter(Collision other)
	{
		if(!SphereList.ContainsKey(other.gameObject.GetInstanceID())){
			StartPosition position;
			if(Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.z - other.transform.position.z)){
				if(transform.position.x - other.transform.position.x > 0){
					position = StartPosition.West;
				}else{
					position = StartPosition.East;
				}
			}else{
				if(transform.position.z - other.transform.position.z < 0){
					position = StartPosition.North;
				}else{
					position = StartPosition.South;
				}
			}
			(position.ToString() + "から来た").Log();
			SphereList.Add(other.gameObject.GetInstanceID(), new ObjectInfo(position, other.gameObject));
		}
	}
}