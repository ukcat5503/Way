using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {
	class ObjectInfo{
		public Vector3 pos;
		public int obj;

		public ObjectInfo(float x, float y, float z, int objectNumber){
			pos = new Vector3(x,y,z);
			obj = objectNumber;
		}
	}

	int[,,] map;
	List<ObjectInfo> objInfo = new List<ObjectInfo>();

	[SerializeField]
	GameObject[] GenerateBlocks;
	[SerializeField]
	GameObject[] GenerateObjects;
	[SerializeField, Space(6)]
	GameObject SphereController;
	

	GameObject cameraObject;

	// Use this for initialization
	void Start () {
		// オブジェクトリスト生成 リリース時取り除く
		var str = "\n";
		for (int i = 0; i < GenerateBlocks.Length; ++i)
		{
			str += i + "\t" + GenerateBlocks[i].name + "\n";
		}
		
		str.Log();

		initialize();

		cameraObject = GameObject.Find("Main Camera");
		cameraObject.transform.position = new Vector3(map.GetLength(2) / 2f - 0.5f, map.GetLength(0) + 0.5f, map.GetLength(1) / 2f - 0.5f);
	}

	void initialize(){
		map = new int[3,5,5]{
			{
				{2,3,3,16,0},
				{0,0,0,4,0},
				{0,0,0,4,0},
				{0,0,0,0,0},
				{0,0,0,0,0},
			},
			{
				{0,0,0,0,0},
				{0,0,0,0,0},
				{4,0,0,0,0},
				{4,0,0,4,0},
				{12,3,3,13,0},
			},
			{
				{18,3,3,3,16},
				{4,0,0,0,4},
				{0,0,0,0,4},
				{0,0,0,0,4},
				{0,0,0,0,1}
			}
		};

		objInfo.Clear();
		objInfo.Add(new ObjectInfo(4,2,0,0));
		objInfo.Add(new ObjectInfo(4,3,1,4));

		for (int y = map.GetLength(0) - 1; y >= 0; --y)
		{
			for (int z = map.GetLength(1) - 1; z >= 0; --z)
			{
				for (int x = map.GetLength(2) - 1; x >= 0; --x)
				{
					Vector3 pos = new Vector3(x, y, (map.GetLength(1) - z - 1));
					if(map[y,z,x] == 0) continue;
					var obj = Instantiate(GenerateBlocks[map[y,z,x]],pos, GenerateBlocks[map[y,z,x]].transform.rotation);
					obj.transform.parent = transform;
					obj.name = "◯[" + x + "," + y +"," + z + "] " + obj.name;
				}
			}
		}

		foreach (var item in objInfo)
		{
			var obj = Instantiate(GenerateObjects[item.obj], item.pos, GenerateObjects[item.obj].transform.rotation);
			obj.transform.parent = transform;
			obj.name = "●[" + item.pos.x + "," + item.pos.y +"," + item.pos.z + "] " + obj.name;
		}
	}
}
