﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour {
	public class StageInfo {
		public int[,] Map { get; private set; }
		public List<ObjectInfo> Objects { get; private set; }
		public List<CoinObjectInfo> Coins { get; private set; }
		public int CoinObjectQty;
		public int CurrentCoinQty;

		public int RequirementBlockQty;
		public int PlaceBlockQty;

		public StageInfo(int[,] map)
		{
			Objects = new List<ObjectInfo>();
			Coins = new List<CoinObjectInfo>();

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

		public void AddCoin(float x, float z)
		{
			++CoinObjectQty;
			Coins.Add(new CoinObjectInfo(x, z));
		}

		public void AddBlockQtyInfo(int requirementBlockQty){
			RequirementBlockQty = requirementBlockQty;
			PlaceBlockQty = requirementBlockQty;
		}

		public bool IsCollectAllCoin(){
			return CurrentCoinQty == CoinObjectQty;
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

		public CoinObjectInfo(float x, float z)
		{
			pos = new Vector2(x, z);
		}

		public Vector3 GetPos(float height, int length) {
			return new Vector3(pos.x, height + 1f, length - pos.y + 0.2f);
		}
	}

	int[,] map;
	public static List<StageInfo> StageData { get; private set; }
	public static List<GameObject> StageObject { get; private set; }

	public static int RequirementBlockQty = 0;
	public static int PlaceBlockQty = 0;
	public static int DeathCount = 0;

	static PuzzleManager instance;

	[SerializeField]
	GameObject[] generateBlocks;
	[SerializeField]
	GameObject[] generateObjects;
	[SerializeField, Space(6)]
	GameObject sphereController;
	[SerializeField]
	GameObject coinPrefabs;
	[SerializeField]
	GameObject resultPrefab;

	public static GameObject SphereController;
	public static GameObject CoinParticleFlyCoinTarget{ get; private set;}

	[SerializeField]
	Color placeColor, notTurnColor, turnColor, turnRightColor, moveColor;
	public static Color PlaceColor, NotTurnColor, TurnColor, TurnRightColor, MoveColor;

	public static GameObject CameraObject;
	public static GameObject HeldBlockSlot;

	Vector3 firstCameraPosition;

	public static int CurrentStage = 0;
	public const float kMapDepth = 0.5f;
	public const int kMapWidth = 15;
	public const int kMapHeight = 10;

	// スコア
	public static int MicroCoin;

	// 画面テキストなど
	[SerializeField]
	GameObject worldSpaceText;
	public static GameObject WorldSpaceText;
	Text currentStageText;
	Text totalStageText;
	Text currentBlockText;
	Text totalBlockText;

	void Awake () {
		instance = this;
		SphereController = sphereController;
		NotTurnColor = notTurnColor;
		TurnColor = turnColor;
		TurnRightColor = turnRightColor;
		MoveColor = moveColor;
		PlaceColor = placeColor;

		WorldSpaceText = worldSpaceText;

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
		currentStageText = GameObject.Find("StageInfo/CurrentStage").GetComponent<Text>();
		totalStageText = GameObject.Find("StageInfo/TotalStage").GetComponent<Text>();
		currentBlockText = GameObject.Find("BlockInfo/CurrentBlock").GetComponent<Text>();
		totalBlockText = GameObject.Find("BlockInfo/TotalBlock").GetComponent<Text>();

		firstCameraPosition = CameraObject.transform.position;


		initialize();
	}

	void Update(){
		currentStageText.text = (CurrentStage + 1).ToString();

		if(Input.GetKeyDown(KeyCode.Q)){
			initialize();
		}
		if(Input.GetKeyDown(KeyCode.R)){
			"Stage Reset".Log();
			Destroy(GameObject.Find("Player"));
		}
		if(Input.GetKeyDown(KeyCode.N)){
			"Stage Skip".Log();
			Destroy(GameObject.Find("Player"));
			NextStage();
		}
		if(Input.GetKey(KeyCode.A)){
			++MicroCoin;
		}
		if(Input.GetKey(KeyCode.S)){
			--MicroCoin;
		}
	}

	void initialize(){
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		CameraObject.transform.position = firstCameraPosition;
		CurrentStage = 0;

		StageData = new List<StageInfo>();
		StageObject = new List<GameObject>();

		/*
		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11, 0},
			{ 0,12,13,14,15,16,17,18,19,20,21,22, 0},
			{ 0,23,24,25,26,27,28,29, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{29,29,29,29,29,29,29,29,29,29,29,29,29}
		};
		StageData.Add(new StageInfo(map));
		*/

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0,29,29, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0,25, 3, 7, 7, 3, 3, 3, 3, 3, 3, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(13,6,3);
		StageData[StageData.Count - 1].AddObject(8,3.5f,12);
		

		map = new int[,]{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0,22, 0, 0, 0, 0, 0, 3,23, 0, 0, 0},
			{ 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,23},
			{ 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 4, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{21, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,20}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(0,0,2);
		StageData[StageData.Count - 1].AddCoin(14,5);
		StageData[StageData.Count - 1].AddCoin(11,5);
		StageData[StageData.Count - 1].AddCoin(7,2);
		StageData[StageData.Count - 1].AddCoin(4,6);
		StageData[StageData.Count - 1].AddBlockQtyInfo(12);

		
		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0,22, 3, 3, 3, 3, 3, 3,23, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 0, 2, 3, 3, 3, 0, 3, 3,20, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(4,3,0);
		StageData[StageData.Count - 1].AddObject(0,0,15);
		StageData[StageData.Count - 1].AddCoin(5,1);
		StageData[StageData.Count - 1].AddCoin(7,1);
		StageData[StageData.Count - 1].AddCoin(9,1);

		map = new int[,]{
			{ 0, 0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,  0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,  0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,  0,21, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,  0, 0, 0, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0,  0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0,  0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0,  0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0,  0, 0, 0,21, 3, 3, 3, 3, 3,20, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(11, 5, 2);
		StageData[StageData.Count - 1].AddCoin(3,4);

		map = new int[,]{
			{ 0,22, 3, 3, 3, 3, 3, 3,23, 0, 0, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0},
			{ 0, 1, 0, 0,22, 3, 3, 3,20, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 2, 3,23, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 3, 3,20, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(1, 2, 0);
		StageData[StageData.Count - 1].AddCoin(6,2);
		StageData[StageData.Count - 1].AddCoin(6,8);

		map = new int[,]{
			{ 0, 0, 0, 0,22, 3, 3, 2, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{22, 3, 3, 3,24, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{21, 3, 1, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(2, 5, 3);

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 1, 3,23, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 2,23, 0, 2, 3, 3, 3, 3,20, 0, 0, 0},
			{ 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0,21, 3, 3, 3, 3,23, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0,21, 3, 3,20, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(7, 0, 1);
		// 上ルート
		StageData[StageData.Count - 1].AddCoin(8,4);
		StageData[StageData.Count - 1].AddCoin(7,4);
		StageData[StageData.Count - 1].AddCoin(6,4);
		// 下ルート
		StageData[StageData.Count - 1].AddCoin(6,6);
		StageData[StageData.Count - 1].AddCoin(5,6);
		StageData[StageData.Count - 1].AddCoin(4,6);
		StageData[StageData.Count - 1].AddCoin(3,6);

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{29, 1, 3, 3, 3, 3, 3, 3,23, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(1, 4, 1);

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0,23, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 2, 3,22, 3, 3, 3,23, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 0, 0},
			{ 0, 0, 0, 0,21, 3, 3, 3,20, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(8, 7, 2);

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{29, 3, 1, 4,23, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(2, 5, 3);


		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,23, 3, 3, 3, 3,22, 0, 0, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,21, 3, 3,20, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(4, 7, 2);

		
		var height = 0;
		foreach (var item in StageData){
			RequirementBlockQty += item.RequirementBlockQty;
			GenerateMap(item, height);
			++height;
		}
		StageObject[0].SetActive(true);
		totalStageText.text = height.ToString();
	}


	public static void GenerateMap(StageInfo item, int stageNumber, bool isActive = false){
		var old = GameObject.Find("Stage " + stageNumber);

		var stageObj = new GameObject("Stage " + stageNumber);
		var mapObj = new GameObject("Maps");
		var objObj = new GameObject("Objects");

		if (old){
			Destroy(old);
			StageObject[stageNumber] = stageObj;
		}
		else
		{
			StageObject.Add(stageObj);
		}

		stageObj.transform.parent = instance.transform;
		mapObj.transform.parent = stageObj.transform;
		objObj.transform.parent = stageObj.transform;
		for (int z = 0; z < item.Map.GetLength(0); ++z)
		{
			for (int x = 0; x < item.Map.GetLength(1); ++x)
			{
				Vector3 pos = new Vector3(x, -stageNumber * kMapDepth, (item.Map.GetLength(0) - z) + 0.25f);
				if (item.Map[z, x] == 0) continue;
				var obj = Instantiate(instance.generateBlocks[item.Map[z, x]], pos, instance.generateBlocks[item.Map[z, x]].transform.rotation) as GameObject;
				obj.transform.parent = mapObj.transform;
				obj.name = "[" + x + "," + z + "] " + obj.name;
				obj.layer = LayerMask.NameToLayer("DefaultBlock");
			}
		}

		foreach (var objItem in item.Objects)
		{
			var obj = Instantiate(instance.generateObjects[objItem.obj], objItem.GetPos(-stageNumber * kMapDepth, item.Map.GetLength(0)), instance.generateObjects[objItem.obj].transform.rotation);
			obj.transform.parent = objObj.transform;
			obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
		}

		foreach (var objItem in item.Coins)
		{
			var obj = Instantiate(instance.coinPrefabs, objItem.GetPos(-stageNumber * kMapDepth, item.Map.GetLength(0)), instance.coinPrefabs.transform.rotation);
			obj.transform.parent = objObj.transform;
			obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
			// obj.GetComponent<CoinParticle>().microCoin = objItem.microCoin;
		}

		stageObj.SetActive(isActive);
		ResetMapState();
	}

	public static void NextStage(GameObject destroyObj = null){
		"Next Stage".Log();
		if(destroyObj == null){
			destroyObj = GameObject.Find("Stage " + CurrentStage);
		}
		Destroy(destroyObj);

		PlaceBlockQty = StageData[CurrentStage].PlaceBlockQty;

		++CurrentStage;
		CameraManager.CameraDown(kMapDepth);
		
		if(StageObject.Count > CurrentStage){
			StageObject[CurrentStage].SetActive(true);
			instance.currentBlockText.text = (StageData[CurrentStage].RequirementBlockQty).ToString();
			instance.totalBlockText.text = (StageData[CurrentStage].RequirementBlockQty).ToString();
			
		}else{
			var obj = Instantiate(instance.resultPrefab) as GameObject;
			obj.transform.SetParent(GameObject.Find("Canvas").transform, true);
			obj.transform.localPosition = new Vector3(0,0,0);
		}
	}

	public static void ResetMapState(){
		PuzzleManager.StageData[PuzzleManager.CurrentStage].CurrentCoinQty = 0;
		StageData[CurrentStage].PlaceBlockQty = StageData[CurrentStage].RequirementBlockQty;

		instance.currentBlockText.text = (StageData[CurrentStage].RequirementBlockQty).ToString();
		instance.totalBlockText.text = (StageData[CurrentStage].RequirementBlockQty).ToString();
	}

	public static void AddTotalBlockText(int add){
		StageData[CurrentStage].PlaceBlockQty += add;
		instance.currentBlockText.text = StageData[CurrentStage].PlaceBlockQty.ToString();

		if(StageData[CurrentStage].PlaceBlockQty < 0){
			instance.currentBlockText.text = "<color=red>" + (-StageData[CurrentStage].PlaceBlockQty).ToString() + "</color>";
		}else{
			instance.currentBlockText.text = StageData[CurrentStage].PlaceBlockQty.ToString();
		}
	}
}