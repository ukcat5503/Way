﻿﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO １オブジェクトだけ生成するように

public class PuzzleManager : MonoBehaviour {

	public static Vector2[] instantiatePotision = new Vector2[4] {
		new Vector2(-1,-1),
		new Vector2(-1,1),
		new Vector2(1,1),
		new Vector2(1,-1)
	};

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

	static PuzzleManager instance;

	static Dictionary<int, ObjectInfo> puzzleList = new Dictionary<int, ObjectInfo>();
	[SerializeField]
	int instantiateQty = 1;
	static int InstantiateQty;
	[SerializeField]
	float checkDistance = 2.0f;
	static float CheckDistance;

	[SerializeField]
	GameObject spherePrefab;
	static GameObject SpherePrefab;

	static GameObject parent;

	int frame = 0;

	void Start(){
		instance = this;
		InstantiateQty = instantiateQty;
		CheckDistance = checkDistance;
		SpherePrefab = spherePrefab;

		parent = GameObject.Find("PuzzleStage/PuzzleObject");
	}

	void Update(){
		if(frame++ % 120 == 1){
			var obj = instantiateSphere(ColorName.White, new Vector3(Random.Range(-8.0f, 8.0f), 30, 14)) as GameObject;
			obj.transform.parent = parent.transform;
		}
	}

	GameObject instantiateSphere(ColorName colorName, Vector3 pos, float size = 4){
		GameObject obj = Instantiate(spherePrefab, pos, Quaternion.identity) as GameObject;
	 	puzzleList.Add(obj.GetInstanceID(),new ObjectInfo(obj, obj.GetComponent<PuzzleSphere>()));
		obj.transform.localScale = new Vector3(size, size, size);
		return obj;
	}

	public static void SplitSperer(GameObject obj, ColorName colorName){
		//obj.Log();
		//obj.transform.position.Log();
		//obj.GetInstanceID().Log();
		obj.transform.localScale.x.Log();
		var size = obj.transform.localScale.x * 0.5f;
		size.Log();

		for (int i = 0; i < InstantiateQty; ++i)
		{
			Vector3 pos = new Vector3((instantiatePotision[i].x * size / 2) + obj.transform.position.x, (instantiatePotision[i].y * size / 2) + obj.transform.position.y, obj.transform.position.z);
			
			var o = instance.instantiateSphere(colorName, pos, size);
			o.transform.parent = parent.transform;
		}
	}


	public static void ChangeAroundColor(Vector3 pos){
		ColorName colorName = ColorName.Blue;

		foreach (var pair in puzzleList)
		{
			var dist = Vector3.Distance(pos, pair.Value.obj.transform.position);
			if(dist < CheckDistance){
				pair.Value.sphere.ChangeMyColor(colorName);
			}
		}
	}

	public static void DeleteFromList(int instanceID){
		if(!puzzleList.Remove(instanceID))
		{
			("指定されたキーは存在しませんでした: " + instanceID).Log();
		}
	}
}