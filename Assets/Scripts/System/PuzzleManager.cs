﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {
	public class StageInfo {
		public int[,] Map { get; private set; }
		public List<ObjectInfo> Objects { get; private set; }
		public List<CoinObjectInfo> Coins { get; private set; }
		public List<int> HeldBlocks { get; private set; }

		public StageInfo(int[,] map)
		{
			Objects = new List<ObjectInfo>();
			Coins = new List<CoinObjectInfo>();
			HeldBlocks = new List<int>();

			Map = new int[map.GetLength(0), map.GetLength(1)];
			for (int z = 0; z < map.GetLength(1); ++z)
			{
				for (int x = 0; x < map.GetLength(0); ++x)
				{
					Map[x, z] = map[x, z];
				}
			}
		}

		public void AddObject(float x, float z, int objectNumber)
		{
			Objects.Add(new ObjectInfo(x, z, objectNumber));
		}

		public void AddCoin(float x, float z, int microCoin)
		{
			Coins.Add(new CoinObjectInfo(x, z, microCoin));
		}

		public void AddHeldBlocks(int blockId, int blockQty = 1) {
			for (int i = 0; i < blockQty; ++i)
			{
				HeldBlocks.Add(blockId);
			}
		}
	}

	public class ObjectInfo
	{
		public Vector3 pos;
		public int obj;

		public ObjectInfo(float x, float z, int objectNumber)
		{
			pos = new Vector2(x, z);
			obj = objectNumber;
		}

		public Vector3 GetPos(float height, int length) {
			return new Vector3(pos.x, height, length - pos.y + 0.2f);
		}
	}

	public class CoinObjectInfo
	{
		public Vector3 pos;
		public int microCoin;

		public CoinObjectInfo(float x, float z, int microCoin)
		{
			pos = new Vector2(x, z);
			this.microCoin = microCoin;
		}

		public Vector3 GetPos(float height, int length) {
			return new Vector3(pos.x, height + 1f, length - pos.y + 0.2f);
		}
	}

	int[,] map;
	public static List<StageInfo> StageData { get; private set; }
	public static List<GameObject> StageObject { get; private set; }


	[SerializeField]
	GameObject[] generateBlocks;
	[SerializeField]
	GameObject[] generateObjects;
	[SerializeField, Space(6)]
	GameObject sphereController;
	[SerializeField]
	GameObject coinPrefabs;

	public static GameObject SphereController;
	public static GameObject CoinParticleFlyCoinTarget{ get; private set;}

	[SerializeField]
	Color notTurnColor, turnColor, moveColor;
	public static Color NotTurnColor, TurnColor, MoveColor;

	public static GameObject CameraObject;
	public static GameObject HeldBlockSlot;

	public static int CurrentStage = 0;
	public static float MapHeight = 0.5f;
	public static int MapSize = 0;

	// スコア
	public static int MicroCoin;

	void Start () {
		SphereController = sphereController;
		NotTurnColor = notTurnColor;
		TurnColor = turnColor;
		MoveColor = moveColor;

		// オブジェクトリスト生成 リリース時取り除く
		var str = "\n ■ Blocks ■\n";
		for (int i = 0; i < generateBlocks.Length; ++i)
		{
			str += i + "\t" + generateBlocks[i].name + "\n";
		}
		str.Log();

		str = "\n □ Objects □\n";
		for (int i = 0; i < generateObjects.Length; ++i)
		{
			str += i + "\t" + generateObjects[i].name + "\n";
		}
		str.Log();

		CameraObject = GameObject.Find("Main Camera");
		HeldBlockSlot = GameObject.Find("HeldBlockSlot");
		CoinParticleFlyCoinTarget = GameObject.Find("CoinTargetPoint");

		initialize();

		// cameraObject.transform.position = new Vector3(map.GetLength(2) / 2f - 0.5f, map.GetLength(0) + 0.5f, map.GetLength(1) / 2f - 0.5f);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			initialize();
		}
		if(Input.GetKeyDown(KeyCode.N)){
			"Stage Skip".Log();
			Destroy(GameObject.Find("Player(Clone)"));
			NextStage();
		}
		if(Input.GetKey(KeyCode.A)){
			++MicroCoin;
		}
		if(Input.GetKey(KeyCode.S)){
			--MicroCoin;
		}

		if(StageData.Count > CurrentStage){
			if(MapSize != StageData[CurrentStage].Map.GetLength(0)){
				MapSize = StageData[CurrentStage].Map.GetLength(0);
			}
		}
		
	}

	void initialize(){
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		CurrentStage = 0;

		CameraObject.transform.position = new Vector3(CameraObject.transform.position.x, 8f, CameraObject.transform.position.z);

		StageData = new List<StageInfo>();
		StageObject = new List<GameObject>();
		
		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 2, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{21, 3, 3, 3, 3, 3, 3, 3, 3, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(9,6,3);
		StageData[StageData.Count - 1].AddObject(5,3.5f,12);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{22, 3, 3, 3, 3, 3, 3,23, 0, 0},
			{ 1, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 2, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(0,2,0);
		StageData[StageData.Count - 1].AddCoin(4,1,100);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,21,26, 3, 3, 3, 3,23, 0, 0},
			{ 0, 0, 4, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 4, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 4, 0, 0, 0, 0, 1, 0, 0},
			{ 0, 0, 4, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0,21, 3, 3, 3, 3,20, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(7, 7, 2);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,22, 3, 3,23, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 2, 3, 6, 3, 3,23, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0,21, 3, 3,20, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(1, 2, 0);

		map = new int[10, 10]{
			{ 0, 0, 0, 0,22, 3, 3, 2, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
			{22, 3, 3, 3,24, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 4, 0, 0, 0, 0, 0},
			{21, 3, 1, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(2, 5, 3);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 1, 3,23},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 2,23, 0, 2, 3, 3, 3, 3,20},
			{ 0, 0, 4, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,21, 3, 3, 3, 3,23, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0,21, 3, 3,20, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(7, 0, 1);
		// 上ルート
		StageData[StageData.Count - 1].AddCoin(8,4,5);
		StageData[StageData.Count - 1].AddCoin(7,4,5);
		StageData[StageData.Count - 1].AddCoin(6,4,5);
		// 下ルート
		StageData[StageData.Count - 1].AddCoin(6,6,50);
		StageData[StageData.Count - 1].AddCoin(5,6,50);
		StageData[StageData.Count - 1].AddCoin(4,6,50);
		StageData[StageData.Count - 1].AddCoin(3,6,50);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{29, 1, 3, 3, 3, 3, 3, 3,23, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 4, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 4, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 2, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(1, 4, 1);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0,23, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 2, 3,22, 3, 3, 3,23, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 4, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 1, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 4, 0},
			{ 0, 0, 0, 0,21, 3, 3, 3,20, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(8, 7, 2);

		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{29, 3, 1, 4,23, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 2, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(2, 5, 3);


		map = new int[10, 10]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,23, 3, 3, 3, 3,22, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 4, 0, 0, 1, 0, 2, 0, 0, 0},
			{ 0, 4, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0,21, 3, 3,20, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(4, 7, 2);






		HeldBlockManager.GenerateBlocks();
		var height = 0;
		foreach (var item in StageData)
		{
			var stageObj = new GameObject("Stage " + height);
			var mapObj = new GameObject("Maps");
			var objObj = new GameObject("Objects");
			StageObject.Add(stageObj);

			stageObj.transform.parent = transform;
			mapObj.transform.parent = stageObj.transform;
			objObj.transform.parent = stageObj.transform;
			for (int z = 0; z < item.Map.GetLength(1); ++z)
			{
				for (int x = 0; x < item.Map.GetLength(0); ++x)
				{
					Vector3 pos = new Vector3(x, -height * MapHeight, (item.Map.GetLength(0) - z) +0.25f);
					if(item.Map[z,x] == 0) continue;
					var obj = Instantiate(generateBlocks[item.Map[z,x]],pos, generateBlocks[item.Map[z,x]].transform.rotation);
					obj.transform.parent = mapObj.transform;
					obj.name = "[" + x + "," + z + "] " + obj.name;
				}
			}

			foreach (var objItem in item.Objects)
			{
				var obj = Instantiate(generateObjects[objItem.obj], objItem.GetPos(-height * MapHeight, item.Map.GetLength(0)), generateObjects[objItem.obj].transform.rotation);
				obj.transform.parent = objObj.transform;
				obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
			}

			foreach (var objItem in item.Coins)
			{
				var obj = Instantiate(coinPrefabs, objItem.GetPos(-height * MapHeight, item.Map.GetLength(0)), coinPrefabs.transform.rotation);
				obj.transform.parent = objObj.transform;
				obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
				obj.GetComponent<CoinParticle>().microCoin = objItem.microCoin;
			}

			StageObject[StageObject.Count - 1].SetActive(false);
			++height;
		}
		StageObject[0].SetActive(true);
	}

	public static void NextStage(GameObject destroyObj = null){
		"Next Stage".Log();
		if(destroyObj == null){
			destroyObj = GameObject.Find("Stage " + CurrentStage);
		}
		Destroy(destroyObj);
		++CurrentStage;
		CameraManager.CameraDown(MapHeight);
		
		if(StageObject.Count > CurrentStage){
			StageObject[CurrentStage].SetActive(true);
			HeldBlockManager.GenerateBlocks();
		}else{
			"AllClear!".Log();
		}
	}
}