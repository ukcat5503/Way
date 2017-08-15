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
		public GameObject obj;
		public PuzzleBlock blockScript;
	}

	string question = "012012012,012012012,210210210,012012012,012012012,210210210,012012012,012012012,210210210";

	int height;

	List<BlockInfo>[,] BlockList = new List<BlockInfo>[blockLength,blockLength];

	[SerializeField]
	GameObject BlockPrefab;
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

					GameObject obj = Instantiate(BlockPrefab,new Vector3(instantiatePosition[(x * blockLength + y) % (blockLength * blockLength)].x, margin * (item.Index + 1), instantiatePosition[(x * blockLength + y) % (blockLength * blockLength)].z), Quaternion.identity) as GameObject;
					obj.name = "PuzzleBlock (" + x + "," + y + ":" + item.Index + ")";
					obj.transform.parent = puzzleObjectParent.transform;
					blockinfo.obj = obj;
					blockinfo.blockScript = obj.GetComponent<PuzzleBlock>();
					blockinfo.blockScript.SetColorByInt((int)char.GetNumericValue(item.Value[(x * 3 ) + y]));
					blockinfo.blockScript.Coordinate = new Vector3(x,y,item.Index);

					(BlockList[x,y]).Add(blockinfo);
				}
			}
		++height;
		}
	}

	public void DestroyTheBlock(Vector3 coordinate){
		var obj = BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].obj;
		BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.BreakWait = true;
		destroyAroundDesignation(coordinate);

		BlockInfo b = new BlockInfo();
		b.blockScript = null;
		b.obj = null;
		BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z] = b;
		
		// BlockList[(int)coordinate.x,(int)coordinate.y].RemoveAt((int)coordinate.z);
		Destroy(obj);
	}

	void destroyAroundDesignation(Vector3 coordinate){
		// 上
		if(coordinate.z != height - 1){
			if(BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z + 1].blockScript != null){
				if(!BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z + 1].blockScript.BreakWait){
					if(BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z + 1].blockScript.MyColor == BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor){
						BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z + 1].blockScript.BreakBlock();
					}
				}
			}
		}
		// 下
		if(coordinate.z != 0){
			if(BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z - 1].blockScript != null){
				if(!BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z - 1].blockScript.BreakWait){
					if(BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z - 1].blockScript.MyColor == BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor){
						BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z - 1].blockScript.BreakBlock();
					}
				}
			}
		}
		// 左
		if(coordinate.y != 0){
			if(BlockList[(int)coordinate.x,(int)coordinate.y - 1][(int)coordinate.z].blockScript != null){
				if(!BlockList[(int)coordinate.x,(int)coordinate.y - 1][(int)coordinate.z].blockScript.BreakWait){
					if(BlockList[(int)coordinate.x,(int)coordinate.y - 1][(int)coordinate.z].blockScript.MyColor == BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor){
						BlockList[(int)coordinate.x,(int)coordinate.y - 1][(int)coordinate.z].blockScript.BreakBlock();
					}
				}
			}
		}
		// 右
		if(coordinate.y != blockLength - 1){
			if(BlockList[(int)coordinate.x,(int)coordinate.y + 1][(int)coordinate.z].blockScript != null){
				if(!BlockList[(int)coordinate.x,(int)coordinate.y + 1][(int)coordinate.z].blockScript.BreakWait){
					if(BlockList[(int)coordinate.x,(int)coordinate.y + 1][(int)coordinate.z].blockScript.MyColor == BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor){
						BlockList[(int)coordinate.x,(int)coordinate.y + 1][(int)coordinate.z].blockScript.BreakBlock();
					}
				}
			}
		}
		// 手前
		if(coordinate.x != blockLength - 1){
			if(BlockList[(int)coordinate.x + 1,(int)coordinate.y][(int)coordinate.z].blockScript != null){
				if(!BlockList[(int)coordinate.x + 1,(int)coordinate.y][(int)coordinate.z].blockScript.BreakWait){
					if(BlockList[(int)coordinate.x + 1,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor == BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor){
						BlockList[(int)coordinate.x + 1,(int)coordinate.y][(int)coordinate.z].blockScript.BreakBlock();
					}
				}
			}
		}
		// 奥
		if(coordinate.x != 0){
			if(BlockList[(int)coordinate.x - 1,(int)coordinate.y][(int)coordinate.z].blockScript != null){
				if(!BlockList[(int)coordinate.x - 1,(int)coordinate.y][(int)coordinate.z].blockScript.BreakWait){
					if(BlockList[(int)coordinate.x - 1,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor == BlockList[(int)coordinate.x,(int)coordinate.y][(int)coordinate.z].blockScript.MyColor){
						BlockList[(int)coordinate.x - 1,(int)coordinate.y][(int)coordinate.z].blockScript.BreakBlock();
					}
				}
			}
		}
	}
}
