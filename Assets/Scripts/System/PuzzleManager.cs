﻿using System.Collections;
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

	static List<ObjectInfo> puzzleList = new List<ObjectInfo>();
	[SerializeField]
	int instantiateQty = 1;
	static int InstantiateQty;
	[SerializeField]
	float checkDistance = 2.0f;
	static float CheckDistance;

	[SerializeField]
	GameObject spherePrefab;
	static GameObject SpherePrefab;

	int frame = 0;

	void Start(){
		instance = this;
		InstantiateQty = instantiateQty;
		CheckDistance = checkDistance;
		SpherePrefab = spherePrefab;
	}

	void Update(){
		if(frame++ % 120 == 1){
			instantiateSphere(ColorName.White, new Vector3(Random.Range(-8.0f, 8.0f), 30, 14));
		}
	}

	GameObject instantiateSphere(ColorName colorName, Vector3 pos, float size = 4){
		GameObject obj = Instantiate(spherePrefab, pos, Quaternion.identity) as GameObject;
	 	puzzleList.Add(new ObjectInfo(obj, obj.GetComponent<PuzzleSphere>()));
		obj.transform.localScale = new Vector3(size, size, size);
		return obj;
	}

	public void ShowListContentsInTheDebugLog<T>(List<T> list)
	{
    string log = "";

    foreach(var content in list.Select((val, idx) => new {val, idx}))
    {
        if (content.idx == list.Count - 1)
            log += content.val.ToString();
        else
            log += content.val.ToString() + ", ";
    }

Debug.Log(log);
}
	
	public static void SplitSperer(GameObject obj, ColorName colorName){
		for (int i = 0; i < InstantiateQty; ++i)
		{
			var size = obj.transform.localScale.x * 0.5f;
			Vector3 pos = new Vector3((instantiatePotision[i].x * size / 2) + obj.transform.position.x, (instantiatePotision[i].y * size / 2) + obj.transform.position.y, obj.transform.position.z);
			
			var o = instance.instantiateSphere(colorName, obj.transform.position, size);
			o.transform.parent = obj.transform.root.gameObject.transform;
		}
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