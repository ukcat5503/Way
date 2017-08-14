﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : ModelBase {

	enum ColorName{
		White,
		Red,
		Blue,
		Green,
		length
	}

	static readonly Dictionary<ColorName,Color32> Colors = new Dictionary<ColorName,Color32>() {
		{ColorName.White, new Color32(255,255,255,255)},
		{ColorName.Red, new Color32(175,0,0,255)},
		{ColorName.Blue, new Color32(0,175,0,255)},
		{ColorName.Green, new Color32(0,0,175,255)}
    };

	[SerializeField]
	ColorName myColor;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer>().material.color = Colors[myColor];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
