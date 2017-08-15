﻿using System.Collections;
using System.Collections.Generic;
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

	string question = "012012012012012012210210210";

	int height;

	List<BlockInfo[,]> BlockList = new List<BlockInfo[,]>();

	[SerializeField]
	GameObject BlockPrefab;
	GameObject puzzleObjectParent;

	// Use this for initialization
	void Start () {
		puzzleObjectParent = GameObject.Find("Stage/PuzzleObject");
		initializePuzzle(question);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void initializePuzzle(string question){
		if(question.Length % (blockLength * blockLength) != 0){
			("問題が" + (blockLength * blockLength) + "の倍数ではありません。基本的に問題は3x3の空間で展開されます。足りない段は生成されません。").LogWarning();
		}

		BlockInfo[,] info = new BlockInfo[blockLength,blockLength];

		// 段数ループ
		for (int i = 0; i < (int)(question.Length / (blockLength * blockLength)); ++i)
		{
			height = (int)(question.Length / (blockLength * blockLength));
			for (int x = 0; x < blockLength; ++x)
			{
				for (int y = 0; y < blockLength; ++y)
				{
					GameObject obj = Instantiate(BlockPrefab,new Vector3(instantiatePosition[(x * blockLength + y) % (blockLength * blockLength)].x, margin * (i + 1), instantiatePosition[(x * blockLength + y) % (blockLength * blockLength)].z), Quaternion.identity) as GameObject;
					obj.name = "PuzzleBlock (" + x + "," + y + ":" + i + ")";
					obj.transform.parent = puzzleObjectParent.transform;
					info[x,y].obj = obj;
					info[x,y].blockScript = obj.GetComponent<PuzzleBlock>();
					info[x,y].blockScript.SetColorByInt((int)char.GetNumericValue(question[(i * blockLength * blockLength) + x * y]));
					info[x,y].blockScript.Coordinate = new Vector3(x,y,i);
				}
			}
			BlockList.Add(info);
			info = new BlockInfo[blockLength,blockLength];
		}
	}

	public void DestroyTheBlock(Vector3 coordinate){
		var obj = BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].obj;
		BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.BreakWait = true;
		destroyAroundDesignation(coordinate);
		BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript = null;
		BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].obj = null;
		Destroy(obj);
	}

	void destroyAroundDesignation(Vector3 coordinate){
		// 上
		if(coordinate.z != height - 1){
			if(BlockList[(int)coordinate.z + 1][(int)coordinate.x,(int)coordinate.y].blockScript != null){
				if(!BlockList[(int)coordinate.z + 1][(int)coordinate.x,(int)coordinate.y].blockScript.BreakWait){
					if(BlockList[(int)coordinate.z + 1][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor == BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor){
						BlockList[(int)coordinate.z + 1][(int)coordinate.x,(int)coordinate.y].blockScript.BreakBlock();
					}
				}
			}
		}
		// 下
		if(coordinate.z != 0){
			if(BlockList[(int)coordinate.z - 1][(int)coordinate.x,(int)coordinate.y].blockScript != null){
				if(!BlockList[(int)coordinate.z - 1][(int)coordinate.x,(int)coordinate.y].blockScript.BreakWait){
					if(BlockList[(int)coordinate.z - 1][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor == BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor){
						BlockList[(int)coordinate.z - 1][(int)coordinate.x,(int)coordinate.y].blockScript.BreakBlock();
					}
				}
			}
		}
		// 左
		if(coordinate.y != 0){
			if(BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y - 1].blockScript != null){
				if(!BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y - 1].blockScript.BreakWait){
					if(BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y - 1].blockScript.MyColor == BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor){
						BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y - 1].blockScript.BreakBlock();
					}
				}
			}
		}
		// 右
		if(coordinate.y != blockLength - 1){
			if(BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y + 1].blockScript != null){
				if(!BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y + 1].blockScript.BreakWait){
					if(BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y + 1].blockScript.MyColor == BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor){
						BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y + 1].blockScript.BreakBlock();
					}
				}
			}
		}
		// 手前
		if(coordinate.x != blockLength - 1){
			if(BlockList[(int)coordinate.z][(int)coordinate.x + 1,(int)coordinate.y].blockScript != null){
				if(!BlockList[(int)coordinate.z][(int)coordinate.x + 1,(int)coordinate.y].blockScript.BreakWait){
					if(BlockList[(int)coordinate.z][(int)coordinate.x + 1,(int)coordinate.y].blockScript.MyColor == BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor){
						BlockList[(int)coordinate.z][(int)coordinate.x + 1,(int)coordinate.y].blockScript.BreakBlock();
					}
				}
			}
		}
		// 奥
		if(coordinate.x != 0){
			if(BlockList[(int)coordinate.z][(int)coordinate.x - 1,(int)coordinate.y].blockScript != null){
				if(!BlockList[(int)coordinate.z][(int)coordinate.x - 1,(int)coordinate.y].blockScript.BreakWait){
					if(BlockList[(int)coordinate.z][(int)coordinate.x - 1,(int)coordinate.y].blockScript.MyColor == BlockList[(int)coordinate.z][(int)coordinate.x,(int)coordinate.y].blockScript.MyColor){
						BlockList[(int)coordinate.z][(int)coordinate.x - 1,(int)coordinate.y].blockScript.BreakBlock();
					}
				}
			}
		}
		
	}
}
