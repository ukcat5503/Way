﻿// hides inherited member
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSphere : ModelBase {

	[SerializeField]
	GameObject spherePrefab;
	MeshRenderer meshRenderer;

	public static readonly Dictionary<PuzzleManager.ColorName,Color32> Colors = new Dictionary<PuzzleManager.ColorName,Color32>() {
		{PuzzleManager.ColorName.None, new Color32(0,0,0,255)},
		{PuzzleManager.ColorName.White, new Color32(255,255,255,255)},
		{PuzzleManager.ColorName.Red, new Color32(175,0,0,255)},
		{PuzzleManager.ColorName.Green, new Color32(0,175,0,255)},
		{PuzzleManager.ColorName.Blue, new Color32(0,0,175,255)}
    };

	public PuzzleManager.ColorName MyColor;

	public void SetColorByInt(int color){
		if(color >= (int)PuzzleManager.ColorName.length){
			("色の範囲を超えています。(" + color + ") Whiteとして処理されます。").LogWarning();
			color = 0;
		}
		MyColor = (PuzzleManager.ColorName)color;
	}

	public Color GetColor(){
		return meshRenderer.material.color;
	}

	// Use this for initialization
	void Start () {
		if(!(meshRenderer = GetComponent<MeshRenderer>())){
			"MeshRendererの取得に失敗しました。".LogWarning();
		}
		ChangeMyColor(MyColor);
	}

	public void ChangeMyColor(PuzzleManager.ColorName colorName){
		if(colorName == PuzzleManager.ColorName.None){
		}else{
			MyColor = colorName;
			meshRenderer.material.color = Colors[MyColor];
		}
	}

	public void HitRayFromPlayer(){
		GetComponent<Collider>().enabled = false;
		Vector3 pos = transform.position;
		var color = MyColor;

		PuzzleManager.ChangeAroundColor(pos);
		PuzzleManager.SplitSperer(gameObject, MyColor);
		PuzzleManager.DeleteFromList(gameObject.GetInstanceID());
		Destroy(gameObject);
	}
}
