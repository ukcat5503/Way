﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour {

	[SerializeField]
	GameObject fragment;

	[SerializeField]
	int fragmentSizeWidth = 4;

	// Use this for initialization
	void Start () {
		float cubeSize = 2.0f / (float)fragmentSizeWidth;	//0.5

		for (int x = 0; x < fragmentSizeWidth; ++x)
		{
			for (int y = 0; y < fragmentSizeWidth; ++y)
			{
				for (int z = 0; z < fragmentSizeWidth; ++z)
				{
					Vector3 pos = new Vector3(transform.position.x - 0.75f + ((float)x * cubeSize), (transform.position.y - 0.75f + (float)y * cubeSize), (transform.position.z - 0.75f + (float)z * cubeSize));
					var obj = Instantiate(fragment, pos, Quaternion.identity) as GameObject;
					obj.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
				}
			}
		}
		Destroy(gameObject);
	}
}
