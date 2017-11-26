using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {
	public class StageInfo {
		public int[,] Map { get; private set; }
		public List<ObjectInfo> Objects { get; private set; }
		public List<CoinObjectInfo> Coins { get; private set; }
		public int[] HeldBlocks { get; private set; }

		public StageInfo(int[,] map)
		{
			Objects = new List<ObjectInfo>();
			Coins = new List<CoinObjectInfo>();
			HeldBlocks = new int[6];

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

		public void AddHeldBlocks(int block1, int block2, int block3, int block4, int block5, int block6){
			HeldBlocks[0] = block1;
			HeldBlocks[1] = block2;
			HeldBlocks[2] = block3;
			HeldBlocks[3] = block4;
			HeldBlocks[4] = block5;
			HeldBlocks[5] = block6;
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

	Vector3 firstCameraPosition;

	public static int CurrentStage = 0;
	public const float kMapDepth = 0.5f;
	public const int kMapWidth = 13;
	public const int kMapHeight = 10;

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

		firstCameraPosition = CameraObject.transform.position;

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
		
		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{21, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(12,6,3);
		StageData[StageData.Count - 1].AddObject(7,3.5f,12);

		HeldBlockSlotUI.ResetAndAddBlocks(1,2,3,4,5,6);

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
			for (int z = 0; z < item.Map.GetLength(0); ++z)
			{
				for (int x = 0; x < item.Map.GetLength(1); ++x)
				{
					Vector3 pos = new Vector3(x, -height * kMapDepth, (item.Map.GetLength(0) - z) +0.25f);
					if(item.Map[z,x] == 0) continue;
					var obj = Instantiate(generateBlocks[item.Map[z,x]],pos, generateBlocks[item.Map[z,x]].transform.rotation);
					obj.transform.parent = mapObj.transform;
					obj.name = "[" + x + "," + z + "] " + obj.name;
				}
			}

			foreach (var objItem in item.Objects)
			{
				var obj = Instantiate(generateObjects[objItem.obj], objItem.GetPos(-height * kMapDepth, item.Map.GetLength(0)), generateObjects[objItem.obj].transform.rotation);
				obj.transform.parent = objObj.transform;
				obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
			}

			foreach (var objItem in item.Coins)
			{
				var obj = Instantiate(coinPrefabs, objItem.GetPos(-height * kMapDepth, item.Map.GetLength(0)), coinPrefabs.transform.rotation);
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
		CameraManager.CameraDown(kMapDepth);
		
		if(StageObject.Count > CurrentStage){
			StageObject[CurrentStage].SetActive(true);

		}else{
			"AllClear!".Log();
		}
	}
}