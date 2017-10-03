﻿// hides inherited member
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSphere : ModelBase {

	[SerializeField]
	int instantiateQty = 1;

	[SerializeField]
	GameObject spherePrefab;

	MeshRenderer meshRenderer;

	public enum ColorName{
		None,
		White,
		Red,
		Green,
		Blue,
		length
	}

	public static Vector2[] instantiatePotision = new Vector2[4] {
		new Vector2(-1,-1),
		new Vector2(-1,1),
		new Vector2(1,1),
		new Vector2(1,-1)
	};

	public static readonly Dictionary<ColorName,Color32> Colors = new Dictionary<ColorName,Color32>() {
		{ColorName.None, new Color32(0,0,0,255)},
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
		for (int i = 0; i < instantiateQty; ++i)
		{
			var size = transform.localScale.x * 0.5f;
			Vector3 pos = new Vector3((instantiatePotision[i].x * size / 2) + transform.position.x, (instantiatePotision[i].y * size / 2) + transform.position.y, transform.position.z);
			
			var obj = Instantiate(spherePrefab, pos, Quaternion.identity) as GameObject;
			obj.transform.localScale = new Vector3(size, size, size);
			obj.transform.parent = transform.root.gameObject.transform;
		}

		Destroy(gameObject);
	}

	public void ChangeAroundColor(){
		// puzzleManager.ChangeColorTheBlock(CoordinateX, CoordinateY, CoordinateZ);
	}

	public void BreakBlock(){
		// puzzleManager.DestroyTheBlock(CoordinateX, CoordinateY, CoordinateZ);
	}
}
