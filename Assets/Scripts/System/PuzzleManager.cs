using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {
	class StageInfo {
		public int[,] Map{ get; private set; }
		public List<ObjectInfo> Objects{ get; private set; }

		public StageInfo(int[,] map)
		{
			Objects = new List<ObjectInfo>();
			Map = new int[map.GetLength(0), map.GetLength(1)];
			for (int z = 0; z < map.GetLength(1); ++z)
			{
				for (int x = 0; x < map.GetLength(0); ++x)
				{
					Map[x,z] = map[x,z];
				}
			}
		}

		public void AddObject(float x, float z, int objectNumber)
		{
			Objects.Add(new ObjectInfo(x, z, objectNumber));
		}
	}

	class ObjectInfo
	{
		public Vector3 pos;
		public int obj;

		public ObjectInfo(float x, float z, int objectNumber)
		{
			pos = new Vector2(x, z);
			obj = objectNumber;
		}

		public Vector3 GetPos(float height, int length){
			return new Vector3(pos.x, height, length - pos.y);
		}
	}

	int[,] map;
	List<StageInfo> StageData;

	[SerializeField]
	GameObject[] GenerateBlocks;
	[SerializeField]
	GameObject[] GenerateObjects;
	[SerializeField, Space(6)]
	GameObject SphereController;

	[SerializeField]
	Color notTurnColor, turnColor, moveColor;
	public static Color NotTurnColor, TurnColor, MoveColor;

	public static GameObject CameraObject;

	public static int StageNumber = 0;
	public static float MapHeight = 0.5f;
	public static int MapSize = 0;

	// Use this for initialization
	void Start () {
		NotTurnColor = notTurnColor;
		TurnColor = turnColor;
		MoveColor = moveColor;

		// オブジェクトリスト生成 リリース時取り除く
		var str = "\n";
		for (int i = 0; i < GenerateBlocks.Length; ++i)
		{
			str += i + "\t" + GenerateBlocks[i].name + "\n";
		}
		
		str.Log();

		initialize();

		CameraObject = GameObject.Find("Main Camera");
		// cameraObject.transform.position = new Vector3(map.GetLength(2) / 2f - 0.5f, map.GetLength(0) + 0.5f, map.GetLength(1) / 2f - 0.5f);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			initialize();
		}
		if(MapSize != StageData[StageNumber].Map.GetLength(0)){
			MapSize = StageData[StageNumber].Map.GetLength(0);
		}
	}

	void initialize(){
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		StageData = new List<StageInfo>();

		map = new int[10, 10]{
			{ 2, 3, 3,25, 3, 3,25, 3, 3,23},
			{ 4, 0, 0, 4, 0, 0, 4, 0, 0, 4},
			{ 4, 0, 0, 4, 0, 0, 4, 0, 0, 4},
			{25, 3, 3,24, 3, 3,15, 3, 3,27},
			{ 4, 0, 0, 4, 0, 0, 4, 0, 0, 4},
			{ 4, 0, 0, 4, 0, 0, 4, 0, 0, 4},
			{25, 3, 3,15, 3, 3,26, 3, 3,27},
			{ 4, 0, 0, 4, 0, 0, 4, 0, 0, 4},
			{ 4, 0, 0, 4, 0, 0, 4, 0, 0, 4},
			{21, 3, 3,24, 3, 3,24, 3, 3,20}
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(9,8,0);
		/*
		map = new int[5, 5]{
			{0,0,15,2,0},
			{0,0,4,0,0},
			{15,3,10,0,0},
			{4,0,4,0,0},
			{14,3,14,2,0},
		};
		StageData.Add(new StageInfo(map));

		map = new int[5, 5]{
			{15,3,20,3,16},
			{4,0,4,3,4},
			{4,0,14,3,13},
			{8,3,3,3,16},
			{2,3,3,3,13},
		};
		StageData.Add(new StageInfo(map));
		 */
		
		var height = 0;
		foreach (var item in StageData)
		{
			var stageObj = new GameObject("Stage " + height);
			var mapObj = new GameObject("Maps");
			var objObj = new GameObject("Objects");
			stageObj.transform.parent = transform;
			mapObj.transform.parent = stageObj.transform;
			objObj.transform.parent = stageObj.transform;
			for (int z = 0; z < item.Map.GetLength(1); ++z)
			{
				for (int x = 0; x < item.Map.GetLength(0); ++x)
				{
					Vector3 pos = new Vector3(x, -height * MapHeight, (item.Map.GetLength(0) - z) +0.25f);
					if(item.Map[z,x] == 0) continue;
					var obj = Instantiate(GenerateBlocks[item.Map[z,x]],pos, GenerateBlocks[item.Map[z,x]].transform.rotation);
					obj.transform.parent = mapObj.transform;
					obj.name = "[" + x + "," + z + "] " + obj.name;
				}
			}

			foreach (var objItem in item.Objects)
			{
				var obj = Instantiate(GenerateObjects[objItem.obj], objItem.GetPos(-height, item.Map.GetLength(0)), GenerateObjects[objItem.obj].transform.rotation);
				obj.transform.parent = objObj.transform;
				obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
			}

			++height;
		}


	}
}