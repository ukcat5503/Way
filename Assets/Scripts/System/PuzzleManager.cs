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

		public Vector3 GetPos(float height){
			return new Vector3(pos.x, height, pos.y);
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
	Color notTurnColor, rightColor, flipColor, leftColor;
	public static Color NotTurnColor, RightColor, FlipColor, LeftColor;
	

	GameObject cameraObject;

	// Use this for initialization
	void Start () {
		NotTurnColor = notTurnColor;
		RightColor = rightColor;
		FlipColor = flipColor;
		LeftColor = leftColor;

		// オブジェクトリスト生成 リリース時取り除く
		var str = "\n";
		for (int i = 0; i < GenerateBlocks.Length; ++i)
		{
			str += i + "\t" + GenerateBlocks[i].name + "\n";
		}
		
		str.Log();

		initialize();

		cameraObject = GameObject.Find("Main Camera");
		// cameraObject.transform.position = new Vector3(map.GetLength(2) / 2f - 0.5f, map.GetLength(0) + 0.5f, map.GetLength(1) / 2f - 0.5f);
	}

	void initialize(){
		StageData = new List<StageInfo>();

		map = new int[5, 5]{
			{7,3,3,3,8},
			{4,0,0,0,4},
			{4,0,0,0,4},
			{4,0,0,0,4},
			{6,3,3,3,5},
		};
		StageData.Add(new StageInfo(map));
		StageData[StageData.Count - 1].AddObject(4,4,0);

		map = new int[5, 5]{
			{0,0,0,0,0},
			{0,7,3,8,0},
			{0,4,0,4,0},
			{0,6,3,5,0},
			{0,0,0,0,0},
		};
		StageData.Add(new StageInfo(map));

		map = new int[5, 5]{
			{0,0,0,0,0},
			{0,0,0,0,0},
			{0,0,2,0,0},
			{0,0,0,0,0},
			{0,0,0,0,0},
		};
		StageData.Add(new StageInfo(map));

		
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
					Vector3 pos = new Vector3(x, -height * 0.5f, (item.Map.GetLength(0) - z));
					if(item.Map[z,x] == 0) continue;
					var obj = Instantiate(GenerateBlocks[item.Map[z,x]],pos, GenerateBlocks[item.Map[z,x]].transform.rotation);
					obj.transform.parent = mapObj.transform;
					obj.name = "[" + x + "," + z + "] " + obj.name;
				}
			}

			foreach (var objItem in item.Objects)
			{
				var obj = Instantiate(GenerateObjects[objItem.obj], objItem.GetPos(-height), GenerateObjects[objItem.obj].transform.rotation);
				obj.transform.parent = objObj.transform;
				obj.name = "[" + objItem.pos.x + "," + objItem.pos.z + "] " + obj.name;
			}

			++height;
		}


	}
}
