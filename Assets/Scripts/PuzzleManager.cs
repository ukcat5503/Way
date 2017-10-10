using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

	int[,] map = new int[5,5]{
		{5,2,2,2,4},
		{3,0,0,0,3},
		{3,0,0,0,3},
		{4,2,2,0,3},
		{0,0,0,0,0}
	};

	[SerializeField]
	GameObject[] GenerateBlocks;
	[SerializeField, Space(6)]
	GameObject SphereController;

	List<Vector3> generateSpherePos = new List<Vector3>();

	// Use this for initialization
	void Start () {
		for (int y = map.GetLength(0) - 1; y >= 0; --y)
		{
			for (int x = map.GetLength(1) - 1; x >= 0; --x)
			{
				Vector3 pos = new Vector3(x, 5, y);
				var obj = Instantiate(GenerateBlocks[map[y,x]],pos, Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
