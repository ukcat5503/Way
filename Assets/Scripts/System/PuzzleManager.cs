using System.Collections;
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
			PlaceBlockQty = 0;
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

	public static PuzzleManager instance;

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
		AnalyticsManager.LogScreen("Game Start");


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
		
		SoundManager.PlayBGM(SoundManager.BGM.Blue_Ever);


		initialize();
	}

	void Update(){
		currentStageText.text = (CurrentStage + 1).ToString();

		
		if(Input.GetKeyDown(KeyCode.M)){
			IsConnectToGoalBlock(PlayerController.Pos, PlayerController.Direction).Log();
		}
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
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
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

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0,21, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(13,6,3);
		StageData[StageData.Count - 1].AddObject(8,3.5f,12);


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
		StageData[StageData.Count - 1].AddBlockQtyInfo(1);

		StageData[StageData.Count - 1].AddObject(4, 3, 0);
		StageData[StageData.Count - 1].AddObject(1.5f, 8.5f, 16);
		// StageData[StageData.Count - 1].AddObject(7.5f, 8.5f, 17);
		// StageData[StageData.Count - 1].AddObject(13.5f, 8.5f, 18);
		StageData[StageData.Count - 1].AddCoin(5, 1);
		StageData[StageData.Count - 1].AddCoin(7, 1);
		StageData[StageData.Count - 1].AddCoin(9, 1);


		map = new int[,]{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0,22, 0, 3, 3, 3, 0, 3,23, 0, 0, 0},
			{ 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,23},
			{ 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 4, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{21, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,20}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(9);
		StageData[StageData.Count - 1].AddObject(0,0,2);
		StageData[StageData.Count - 1].AddCoin(14,5);
		StageData[StageData.Count - 1].AddCoin(11,5);
		StageData[StageData.Count - 1].AddCoin(7,2);
		StageData[StageData.Count - 1].AddCoin(4,6);
		


		map = new int[,]{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0,22, 0, 0,23, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0,21, 0, 0,15, 0, 0, 0,23, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 3, 3, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 2, 0},
			{21, 3, 3, 3, 3, 3,20, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(12);
		StageData[StageData.Count - 1].AddObject(0, 0, 2);
		StageData[StageData.Count - 1].AddCoin(4, 2);
		StageData[StageData.Count - 1].AddCoin(5, 4);
		StageData[StageData.Count - 1].AddCoin(8, 4);
		StageData[StageData.Count - 1].AddCoin(11, 7);
		


		map = new int[,]{
			{ 1, 3, 3, 3, 3, 9, 9, 9, 9, 9, 3, 3, 3, 3,23},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 0,29,29,29,29,29, 0, 0, 0, 0, 4},
			{ 0, 0, 0, 0, 2, 0, 0,29, 0, 0, 0, 0, 0, 0,20},
			{ 0, 0, 0, 0, 0, 0, 0,29, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0,29, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0,29, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(8);
		StageData[StageData.Count - 1].AddObject(0, 0, 1);
		StageData[StageData.Count - 1].AddCoin(5, 4);
		StageData[StageData.Count - 1].AddCoin(7, 4);
		StageData[StageData.Count - 1].AddCoin(9, 4);


		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 2, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3,23, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 4, 0, 0,29, 3, 3,33, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(12);
		StageData[StageData.Count - 1].AddObject(12, 9, 0);
		StageData[StageData.Count - 1].AddCoin(10, 5);
		StageData[StageData.Count - 1].AddCoin(6, 5);
		StageData[StageData.Count - 1].AddCoin(3, 7);

		map = new int[,]{
			{ 1, 0, 0, 3, 3, 9, 9, 9, 9, 9, 3,23, 0, 0, 0},
			{ 4, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 4,12, 0, 0,29, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 4,12, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4,12, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4,12, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4,21, 3,30, 3, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0,29,29,29,29,29, 0,26, 0, 0,29},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{21, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,20, 0, 0, 0},
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(9);
		StageData[StageData.Count - 1].AddObject(0, 0, 2);
		StageData[StageData.Count - 1].AddCoin(11, 0);
		StageData[StageData.Count - 1].AddCoin(1, 0);
		StageData[StageData.Count - 1].AddCoin(7, 6);

		map = new int[,]{
			{ 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0,14, 0, 0, 0, 4, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0,14, 0, 0, 0, 4, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0,14, 0, 0, 0, 4, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0,14, 3, 3, 3,20, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 2, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0},
			{21, 3, 3, 3, 3, 3, 3,30, 3, 3, 3, 3, 3,20, 0},
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(6);
		StageData[StageData.Count - 1].AddObject(0, 0, 2);
		StageData[StageData.Count - 1].AddCoin(7, 3);
		StageData[StageData.Count - 1].AddCoin(11, 3);
		StageData[StageData.Count - 1].AddCoin(9, 5);

		map = new int[,]{
			{ 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,27},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,23, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0,29, 0, 0,14, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0,29, 0, 0, 0, 0,14, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 4},
			{ 0, 0, 0, 0, 0,29, 0, 0, 0, 0, 0, 0,14, 0, 4},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 4},
			{ 0, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 0,14, 0, 4},
			{ 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,25, 3,24},
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(13);
		StageData[StageData.Count - 1].AddObject(0, 0, 1);
		StageData[StageData.Count - 1].AddCoin(12, 3);
		StageData[StageData.Count - 1].AddCoin(12, 5);
		StageData[StageData.Count - 1].AddCoin(12, 7);

		map = new int[,]{
			{ 1, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,23},
			{26, 3, 3, 3, 3, 3, 3,23, 2, 3, 3, 3, 3,23, 4},
			{ 4,29,29,29,29,29,29, 4,29,29,29,29,29, 4, 4},
			{ 4, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0,29,29,29, 0, 4},
			{ 4, 0, 0,29, 0, 0, 0, 0, 0, 0,29, 0, 0, 0, 4},
			{ 4, 0, 0,29, 0, 0,29, 0,29, 0, 0, 0, 0, 0, 4},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0,29,29,29, 0, 4},
			{ 4, 0, 0,29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
			{25, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,24},
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddBlockQtyInfo(42);
		StageData[StageData.Count - 1].AddObject(0, 0, 1);
		StageData[StageData.Count - 1].AddCoin(7,6);
		StageData[StageData.Count - 1].AddCoin(3,4);
		StageData[StageData.Count - 1].AddCoin(3,7);
		StageData[StageData.Count - 1].AddCoin(11,3);
		StageData[StageData.Count - 1].AddCoin(11,8);

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0,21, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(3, 2, 2); 
		StageData[StageData.Count - 1].AddBlockQtyInfo(0);
		StageData[StageData.Count - 1].AddCoin(5,6);
		StageData[StageData.Count - 1].AddCoin(6,6);
		StageData[StageData.Count - 1].AddCoin(7,6);
		StageData[StageData.Count - 1].AddCoin(8,6);
		StageData[StageData.Count - 1].AddCoin(9,6);
		StageData[StageData.Count - 1].AddCoin(10,6);
		StageData[StageData.Count - 1].AddCoin(11,6);



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

		PlaceBlockQty += StageData[CurrentStage].PlaceBlockQty;

		++CurrentStage;
		CameraManager.CameraDown(kMapDepth);
		
		if(StageObject.Count > CurrentStage){
			StageObject[CurrentStage].SetActive(true);
			instance.currentBlockText.text = 0.ToString();
			instance.totalBlockText.text = (StageData[CurrentStage].RequirementBlockQty).ToString();
			AnalyticsManager.LogScreen("Stage " + CurrentStage);

		}
		else
		{
			--CurrentStage;
			AnalyticsManager.LogScreen("All Clear!");
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

		if(StageData[CurrentStage].PlaceBlockQty > StageData[CurrentStage].RequirementBlockQty){
			instance.currentBlockText.text = "<color=red>" + StageData[CurrentStage].PlaceBlockQty.ToString() + "</color>";
		}else{
			instance.currentBlockText.text = StageData[CurrentStage].PlaceBlockQty.ToString();
		}
	}

	/// <summary>
	/// ゴールにブロックが接続されているかをチェックする
	/// </summary>
	/// <returns>接続されているか</returns>
	public static bool IsConnectToGoalBlock(Vector3 pos, TurnBlockBase.StartPosition direction){
		float blockWidth = 1f;

		// pos.y -= kMapDepth + SphereController.transform.position.y;


		Vector3 newPos = new Vector3(
			(int)(pos.x + 0.5f),
			-PuzzleManager.CurrentStage * PuzzleManager.kMapDepth,
			((int)(pos.z + 0.5f )) + 0.25f
		);

		// 無限ループ防止
		for (int i = 0; i < 100; ++i){
			var objs = Physics.OverlapSphere(newPos, 0.1f, HeldBlockSlotUI.TargetLayer);

			if(objs.Length == 0){
				break;
			}

			if(objs[0].GetComponent<GoalBlock>() != null){
				return true;
			}

			TurnBlockBase block = objs[0].GetComponent<TurnBlockBase>();
			if(block != null){
				switch (direction)
				{
					case TurnBlockBase.StartPosition.North:
						direction = angleToStartPosition(direction, (int)block.TargetFromSouth);	break;
					case TurnBlockBase.StartPosition.South:
						direction = angleToStartPosition(direction, (int)block.TargetFromNorth);	break;
					case TurnBlockBase.StartPosition.East:
						direction = angleToStartPosition(direction, (int)block.TargetFromWest);	break;
					case TurnBlockBase.StartPosition.West:
						direction = angleToStartPosition(direction, (int)block.TargetFromEast);	
						break;
				}

				Vector3 dire;
				switch (direction)
				{
					case TurnBlockBase.StartPosition.North:
						dire = new Vector3(0,0,blockWidth);		break;
					case TurnBlockBase.StartPosition.South:
						dire = new Vector3(0,0,-blockWidth);	break;
					case TurnBlockBase.StartPosition.East:
						dire = new Vector3(blockWidth,0,0);	break;
					case TurnBlockBase.StartPosition.West:
						dire = new Vector3(-blockWidth,0,0);		break;
					default: 
						dire = new Vector3(0,0,0);				break;
				}
				newPos += dire;
			}else{
				break;
			}
		}
		return false;
	}

	static TurnBlockBase.StartPosition angleToStartPosition(TurnBlockBase.StartPosition direction, int rotateAngle){
		int rotateCount = 0;
		switch (rotateAngle)
		{
			case 0:		rotateCount = 0;	break;
			case -90:	rotateCount = 3;	break;
			case 180:	rotateCount = 2;	break;
			case 90:	rotateCount = 1;	break;
		}

		return (TurnBlockBase.StartPosition)(((int)direction + rotateCount) % 4);
	}
}