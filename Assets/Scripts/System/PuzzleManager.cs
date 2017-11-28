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

	// スタートインジケーター関連
	const int kStartFrame = 60 * 60;
	static int startFrame = kStartFrame;
	static Image indicatorImage;
	public static bool IsStarted = false;
	static Rect startIndicatorColliderRect;

	public static int CurrentStage = 0;
	public const float kMapDepth = 0.5f;
	public const int kMapWidth = 15;
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
		indicatorImage = GameObject.Find("PlayIndicator/Indicator").GetComponent<Image>();

		firstCameraPosition = CameraObject.transform.position;

		startIndicatorColliderRect.xMin = 0;
		startIndicatorColliderRect.xMax = 180;
		startIndicatorColliderRect.yMin = Screen.height - 180;
		startIndicatorColliderRect.yMax = Screen.height;

		initialize();
	}

	void Update(){
		// スタートしてるか処理
		if(++startFrame < kStartFrame){
			var target = (float)startFrame / (float)kStartFrame;
			if(target < indicatorImage.fillAmount){
				indicatorImage.fillAmount -= 0.02f;
			}else{
				indicatorImage.fillAmount = target; 
			}
		}else if(startFrame == kStartFrame){
			IsStarted = true;
			indicatorImage.fillAmount = 1f;
		}
		if(startIndicatorColliderRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0)){
			IsStarted = true;
			startFrame = kStartFrame;

		}

		if(IsStarted && indicatorImage.fillAmount < 1f){
			indicatorImage.fillAmount += 0.02f;
		}

		if(Input.GetKeyDown(KeyCode.Q)){
			initialize();
		}
		if(Input.GetKeyDown(KeyCode.R)){
			"Stage Reset".Log();
			Destroy(GameObject.Find("Player"));
			ResetIndicatorAnimation();
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

		IsStarted = true;
		indicatorImage.fillAmount = 1f;

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
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(4,3,0);
		StageData[StageData.Count - 1].AddCoin(5,1);

		map = new int[,]{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,21, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 1, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0, 4, 0, 0, 0, 0, 0, 4, 0, 0, 0},
			{ 0, 0, 0,21, 3, 3, 3, 3, 3,20, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(9, 5, 2);
		StageData[StageData.Count - 1].AddCoin(3,4);

		map = new int[,]{
			{ 0,22, 3, 3, 3, 3, 3, 3,23, 0, 0, 0, 0},
			{ 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0},
			{ 0, 1, 0, 0,22, 3, 3, 3,20, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 2, 3,23, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 4, 0, 0, 4, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 3, 3,20, 0, 0, 0, 0, 0},
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(1, 2, 0);
		
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
			{ 0, 4, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0},
			{ 0,21, 3, 3,20, 0, 0, 0, 0, 0, 0, 0, 0}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(4, 7, 2);

		
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
					var obj = Instantiate(generateBlocks[item.Map[z,x]],pos, generateBlocks[item.Map[z,x]].transform.rotation) as GameObject;
					obj.transform.parent = mapObj.transform;
					obj.name = "[" + x + "," + z + "] " + obj.name;
					obj.layer = LayerMask.NameToLayer("DefaultBlock");
					var mesh = obj.GetComponentInChildren<MeshRenderer>();
					mesh.material.color = new Color(0,0,0,1);
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
				// obj.GetComponent<CoinParticle>().microCoin = objItem.microCoin;
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
			ResetIndicatorAnimation();
			
		}else{
			"AllClear!".Log();
		}
	}

	public static void ResetIndicatorAnimation(){
		PuzzleManager.startFrame = 0;
		IsStarted = false;
	}
}