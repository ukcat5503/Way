﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PuzzleManager : MonoBehaviour {

	const float margin = 2;
	static readonly Vector3[] instantiatePosition = new Vector3[9]{
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

	string question = "012012012012012012012012012012012012";

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
		if(question.Length % 9 != 0){
			"問題が9の倍数ではありません。基本的に問題は3x3の空間で展開されます。足りない段は生成されません。".LogWarning();
		}

		BlockInfo[,] info = new BlockInfo[3,3];

		// 段数ループ
		for (int i = 0; i < question.Length / 9; ++i)
		{
			int height = question.Length / 9;
			for (int x = 0; x < 3; ++x)
			{
				for (int y = 0; y < 3; ++y)
				{
					GameObject obj = Instantiate(BlockPrefab,new Vector3(instantiatePosition[(x * 3 + y) % 9].x, 2 * (i + 1), instantiatePosition[(x * 3 + y) % 9].z), Quaternion.identity) as GameObject;
					obj.name = "PuzzleBlock (" + x + "," + y + ":" + height + ")";
					obj.transform.parent = puzzleObjectParent.transform;
					// puzzleObjectParent
					info[x,y].obj = obj;
					info[x,y].blockScript = obj.GetComponent<PuzzleBlock>();
					info[x,y].blockScript.SetColor((int)char.GetNumericValue(question[x * y]));
				}
			}
			BlockList.Add(info);
			info = new BlockInfo[3,3];
		}


/*		foreach (char item in question)
		{
			GameObject obj = Instantiate(BlockPrefab, instantiatePosition[count % 9], Quaternion.identity) as GameObject;

			if(count % 9 == 8){
				BlockList.Add(info);
				info = new BlockInfo[3,3];
			}
			++count;
		}
		BlockList.Add(info);

		 */

	}
}
