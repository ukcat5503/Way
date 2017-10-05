﻿// hides inherited member
#pragma warning disable 0108

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSphere : ModelBase {

	[SerializeField]
	GameObject spherePrefab;
	MeshRenderer meshRenderer;



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
		if(meshRenderer == null){
			if(!(meshRenderer = GetComponent<MeshRenderer>())){
				"MeshRendererの取得に失敗しました。".LogWarning();
			}
		}
		ChangeMyColor(MyColor);
	}

	public void ChangeMyColor(PuzzleManager.ColorName colorName){
		if(meshRenderer == null){
			if(!(meshRenderer = GetComponent<MeshRenderer>())){
			"MeshRendererの取得に失敗しました。".LogWarning();
			}
		}
		MyColor = colorName;
		meshRenderer.material.color = PuzzleManager.Colors[MyColor];
	}

	public void HitRayFromPlayer(){
		GetComponent<Collider>().enabled = false;
		PuzzleManager.ChangeAroundColor(gameObject.GetInstanceID(), transform.position);
		PuzzleManager.SplitSphere(gameObject, MyColor);
		PuzzleManager.DeleteFromList(gameObject.GetInstanceID());
		Destroy(gameObject);
	}
}
