﻿// hides inherited member
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : ModelBase {

	public int CoordinateX;
	public int CoordinateY;
	public int CoordinateZ;

	public void SetCoordinates(int x, int y, int z){
		CoordinateX = x;
		CoordinateY = y;
		CoordinateZ = z;
	}

	PuzzleManager puzzleManager;

	public bool BreakWait = false;

	Vector3 prevPos;

	MeshRenderer meshRenderer;

	public enum ColorName{
		None,
		White,
		Red,
		Green,
		Blue,
		length
	}

	static readonly Dictionary<ColorName,Color32> Colors = new Dictionary<ColorName,Color32>() {
		{ColorName.None, new Color32(0,0,0,0)},
		{ColorName.White, new Color32(255,255,255,255)},
		{ColorName.Red, new Color32(175,0,0,255)},
		{ColorName.Green, new Color32(0,175,0,255)},
		{ColorName.Blue, new Color32(0,0,175,255)}
    };

	public ColorName MyColor;

	public void SetColorByInt(int color){
		if(color >= (int)ColorName.length){
			("色の範囲を超えています。(" + color + ") Whiteとして処理されます。").LogWarning();
			color = 0;
		}
		MyColor = (ColorName)color;
	}

	public Color GetColor(){
		return meshRenderer.material.color;
	}

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer>();
		ChangeMyColor(MyColor);
		puzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
	}
	
	public void ChangeMyColor(ColorName colorName){
		if(colorName == ColorName.None){
			BreakBlock();
		}else{
			MyColor = colorName;
			meshRenderer.material.color = Colors[MyColor];
		}
	}

	public void HitRayFromPlayer(){
		ChangeAroundColor();
	}

	public void ChangeAroundColor(){
		puzzleManager.ChangeColorTheBlock(CoordinateX, CoordinateY, CoordinateZ);
	}

	public void BreakBlock(){
		// GetComponent<BoxCollider>().enabled = false;
		puzzleManager.DestroyTheBlock(CoordinateX, CoordinateY, CoordinateZ);
	}
}
