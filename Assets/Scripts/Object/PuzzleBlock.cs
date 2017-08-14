﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : ModelBase {

	enum ColorName{
		None,
		White,
		Red,
		Blue,
		Green,
		length
	}

	static readonly Dictionary<ColorName,Color32> Colors = new Dictionary<ColorName,Color32>() {
		{ColorName.None, new Color32(255,255,255,100)},
		{ColorName.White, new Color32(255,255,255,255)},
		{ColorName.Red, new Color32(175,0,0,255)},
		{ColorName.Blue, new Color32(0,175,0,255)},
		{ColorName.Green, new Color32(0,0,175,255)}
    };

	ColorName myColor;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer>().material.color = Colors[myColor];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetColor(int color){
		if(color >= (int)ColorName.length){
			("色の範囲を超えています。(" + color + ") Whiteとして処理されます。").LogWarning();
			color = 0;
		}
		((ColorName)color + "に設定").Log();
		myColor = (ColorName)color;
	}
}
