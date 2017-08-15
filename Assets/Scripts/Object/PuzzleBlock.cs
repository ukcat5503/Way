﻿// hides inherited member
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : ModelBase {

	public Vector3 Coordinate;

	PuzzleManager puzzleManager;
	

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
		{ColorName.White, new Color32(255,255,255,100)},
		{ColorName.Red, new Color32(175,0,0,255)},
		{ColorName.Blue, new Color32(0,175,0,255)},
		{ColorName.Green, new Color32(0,0,175,255)}
    };

	ColorName myColor;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<MeshRenderer>().material.color = Colors[myColor];
		puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HitRayFromPlayer(){
		BreakBlock();
	}

	public void BreakBlock(){
		transform.name.Log();
		// puzzleManager.DestroyAroundDesignation(Coordinate);
		Destroy(gameObject);
	}

	public void SetColor(int color){
		if(color >= (int)ColorName.length){
			("色の範囲を超えています。(" + color + ") Whiteとして処理されます。").LogWarning();
			color = 0;
		}
		myColor = (ColorName)color;
	}
}
