﻿// hides inherited member
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : ModelBase {

	public Vector3 Coordinate;

	PuzzleManager puzzleManager;

	public bool BreakWait = false;
	

	public enum ColorName{
		White,
		Red,
		Blue,
		Green,
		length
	}

	static readonly Dictionary<ColorName,Color32> Colors = new Dictionary<ColorName,Color32>() {
		// {ColorName.None, new Color32(0,0,0,100)},
		{ColorName.White, new Color32(255,255,255,255)},
		{ColorName.Red, new Color32(175,0,0,255)},
		{ColorName.Blue, new Color32(0,175,0,255)},
		{ColorName.Green, new Color32(0,0,175,255)}
    };

	public ColorName MyColor;

	public void SetColorByInt(int color){
		if(color >= (int)ColorName.length){
			("色の範囲を超えています。(" + color + ") Whiteとして処理されます。").LogWarning();
			color = 0;
		}
		MyColor = (ColorName)color;
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer>().material.color = Colors[MyColor];
		puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HitRayFromPlayer(){
		BreakBlock();
	}

	public void BreakBlock(){
		// 散り散りになる処理
		
		puzzleManager.DestroyTheBlock(Coordinate);
	}


}
