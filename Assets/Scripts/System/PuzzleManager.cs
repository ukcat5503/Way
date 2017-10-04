﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO １オブジェクトだけ生成するように

public class PuzzleManager : MonoBehaviour {
	class ObjectInfo{
		public GameObject obj;
		public PuzzleSphere sphere;

		public ObjectInfo(GameObject o, PuzzleSphere s){
			obj = o;
			sphere = s;
		}
	}

	public enum ColorName{
		None,
		White,
		Red,
		Green,
		Blue,
		length
	}

	static List<ObjectInfo> puzzleList = new List<ObjectInfo>();
	[SerializeField]
	float checkDistance = 2.0f;
	static float CheckDistance;

	[SerializeField]
	GameObject spherePrefab;
	

	int frame = 0;

	void Start(){
		CheckDistance = checkDistance;
	}

	void Update(){
		if(++frame % 20 == 0){
			instantiateSphere(ColorName.White, new Vector3(Random.Range(-8.0f, 8.0f), 30, 14));
		}
	}

	void instantiateSphere(ColorName colorName, Vector3 pos){
		GameObject obj = Instantiate(spherePrefab, pos, Quaternion.identity) as GameObject;
	 	puzzleList.Add(new ObjectInfo(obj, obj.GetComponent<PuzzleSphere>()));
	}
	

	public static void ChangeAroundColor(Vector3 pos){
		ColorName colorName = ColorName.Blue;

		foreach (var item in puzzleList)
		{
			var dist = Vector3.Distance(pos, item.obj.transform.position);
			if(dist < CheckDistance){
				item.sphere.ChangeMyColor(colorName);
			}
		}
	}
}