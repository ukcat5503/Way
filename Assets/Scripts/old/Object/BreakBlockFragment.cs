﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlockFragment : MonoBehaviour {

	MeshRenderer meshRenderer;

	[HideInInspector]
	public Color CubeColor;

	[SerializeField]
	int deleteCount = 0;
	int count = 0;

	void Start(){
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		meshRenderer.material.color = CubeColor;
	}

	// Update is called once per frame
	void Update () {
		if(++count > deleteCount){
			Destroy(gameObject);
		}

		meshRenderer.material.color = new Color(CubeColor.r, CubeColor.g, CubeColor.b, 1f - ((float)count / (float)deleteCount));
	}
}
