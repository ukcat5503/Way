﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PuzzleManager : MonoBehaviour {

	const int blockLength = 3;

	const float margin = 2f;
	static readonly Vector3[] instantiatePosition = new Vector3[blockLength * blockLength]{
		new Vector3(margin,0,margin),
		new Vector3(0,0,margin),
		new Vector3(-margin,0,margin),
		new Vector3(margin,0,0),
		new Vector3(0,0,0),
		new Vector3(-margin,0,0),
		new Vector3(margin,0,-margin),
		new Vector3(0,0,-margin),
		new Vector3(-margin,0,-margin)
	};

	struct BlockInfo{
		public int id;
		public GameObject obj;
		public PuzzleBlock blockScript;
	}

	string question = "012012012,012012012,210210210,012012012,012012012,210210210,012012012,012012012,210210210";
	// string question = "000000000,000000000,000000000";

	int height;

	List<BlockInfo>[,] BlockList = new List<BlockInfo>[blockLength,blockLength];

	List<int[]> DeleteBlocks = new List<int[]>();

	[SerializeField]
	GameObject blockPrefab;

	[SerializeField]
	GameObject breakBlockPrefab;
	
	GameObject puzzleObjectParent;

	// Use this for initialization
	void Start () {
		// List初期化
		for (int x = 0; x < blockLength; ++x)
		{
			for (int y = 0; y < blockLength; ++y)
			{
				BlockList[x,y] = new List<BlockInfo>();
			}
		}

		puzzleObjectParent = GameObject.Find("Stage/PuzzleObject");
		initializePuzzle(question);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate(){
		deleteBlocksFromList();
	}

	void initializePuzzle(string question){
		string[] questionArr = question.Split(',');

		foreach (var item in questionArr.Select((v, i) => new {Value = v, Index = i }))
		{
			if(item.Value.Length % (blockLength * blockLength) != 0){
				(item.Index + "段の指定がおかしいです。" + blockLength * blockLength + "桁で指定して下さい。").LogWarning();
			}
			for (int x = 0; x < blockLength; ++x)
			{
				for (int y = 0; y < blockLength; ++y)
				{
					BlockInfo blockinfo = new BlockInfo();

					GameObject obj = Instantiate(blockPrefab,new Vector3(instantiatePosition[(x * blockLength + y) % (blockLength * blockLength)].x, margin * (item.Index + 1), instantiatePosition[(x * blockLength + y) % (blockLength * blockLength)].z), Quaternion.identity) as GameObject;
					obj.name = "PuzzleBlock (" + x + "," + y + ":" + item.Index + ")";
					obj.transform.parent = puzzleObjectParent.transform;
					blockinfo.id = obj.GetInstanceID();
					blockinfo.obj = obj;
					blockinfo.blockScript = obj.GetComponent<PuzzleBlock>();
					blockinfo.blockScript.SetColorByInt((int)char.GetNumericValue(item.Value[(x * 3 ) + y]));
					blockinfo.blockScript.SetCoordinates(x, y, item.Index);
					BlockList[x,y].Add(blockinfo);
				}
			}
		++height;
		}
	}

	public void DestroyTheBlock(int x, int y, int z){
		var obj = BlockList[x, y][z];
		BlockList[x, y][z].blockScript.BreakWait = true;
		destroyAroundDesignation(x, y, z);

		BlockInfo b = new BlockInfo();
		b.blockScript = null;
		b.obj = null;
		BlockList[x, y][z] = b;
		
		int[] c = {x, y, z};
		DeleteBlocks.Add(c);
		var breakObj = Instantiate(breakBlockPrefab, obj.obj.transform.position, obj.obj.transform.rotation) as GameObject;
		breakObj.GetComponent<BreakBlock>().CubeColor = obj.blockScript.GetColor();
		Destroy(obj.obj);
	}

	void deleteBlocksFromList(){
		if(DeleteBlocks.Count != 0){
			DeleteBlocks.Sort((a, b) => (int)b[2] - (int)a[2]);
			foreach (var item in DeleteBlocks)
			{
				// 上の要素の座標を変更しておく
				for (int i = (int)item[2] + 1; i < BlockList[(int)item[0],(int)item[1]].Count; ++i)
				{
					BlockList[(int)item[0],(int)item[1]][i].blockScript.SetCoordinates(item[0], item[1], i - 1);
				}

				// (item[0] + "," + item[1] + ":" + item[2] + " 削除").Log();
				BlockList[(int)item[0],(int)item[1]].RemoveAt((int)item[2]);
			}
			DeleteBlocks.Clear();
		}
	}

	void destroyAroundDesignation(int x, int y, int z){
		// 上
		if(z != height - 1){
			if(BlockList[x,y].Count() > z + 1){
				if(BlockList[x,y][z + 1].blockScript != null){
					if(!BlockList[x,y][z + 1].blockScript.BreakWait){
						if(BlockList[x,y][z + 1].blockScript.MyColor == BlockList[x,y][z].blockScript.MyColor){
							BlockList[x,y][z + 1].blockScript.BreakBlock();
						}
					}
				}
			}
		}
		// 下
		if(z != 0){
			if(BlockList[x,y][z - 1].blockScript != null){
				if(!BlockList[x,y][z - 1].blockScript.BreakWait){
					if(BlockList[x,y][z - 1].blockScript.MyColor == BlockList[x,y][z].blockScript.MyColor){
						BlockList[x,y][z - 1].blockScript.BreakBlock();
					}
				}
			}
		}
		// 左
		if(y != 0){
			if(BlockList[x,y - 1].Count() > z && BlockList[x,y - 1].Count() != 0){
				if(BlockList[x,y - 1][z].blockScript != null){
					if(!BlockList[x,y - 1][z].blockScript.BreakWait){
						if(BlockList[x,y - 1][z].blockScript.MyColor == BlockList[x,y][z].blockScript.MyColor){
							BlockList[x,y - 1][z].blockScript.BreakBlock();
						}
					}
				}
			}
		}
		// 右
		if(y != blockLength - 1){
			if(BlockList[x,y + 1].Count() > z && BlockList[x,y + 1].Count() != 0){
				if(BlockList[x,y + 1][z].blockScript != null){
					if(!BlockList[x,y + 1][z].blockScript.BreakWait){
						if(BlockList[x,y + 1][z].blockScript.MyColor == BlockList[x,y][z].blockScript.MyColor){
							BlockList[x,y + 1][z].blockScript.BreakBlock();
						}
					}
				}
			}
		}
		// 手前
		if(x != blockLength - 1){
			if(BlockList[x + 1,y].Count() > z && BlockList[x + 1,y].Count() != 0){
				if(BlockList[x + 1,y][z].blockScript != null){
					if(!BlockList[x + 1,y][z].blockScript.BreakWait){
						if(BlockList[x + 1,y][z].blockScript.MyColor == BlockList[x,y][z].blockScript.MyColor){
							BlockList[x + 1,y][z].blockScript.BreakBlock();
						}
					}
				}
			}
		}
		// 奥
		if(x != 0){
			if(BlockList[x - 1,y].Count() > z && BlockList[x - 1,y].Count() != 0){
				if(BlockList[x - 1,y][z].blockScript != null){
					if(!BlockList[x - 1,y][z].blockScript.BreakWait){
						if(BlockList[x - 1,y][z].blockScript.MyColor == BlockList[x,y][z].blockScript.MyColor){
							BlockList[x - 1,y][z].blockScript.BreakBlock();
						}
					}
				}
			}
		}
	}
}
