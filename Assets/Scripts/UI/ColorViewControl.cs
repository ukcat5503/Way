﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorViewControl : MonoBehaviour {
	[SerializeField]
	GameObject[] circle;
	UnityEngine.UI.Image[] image;

	void Start(){
		image = new UnityEngine.UI.Image[circle.Length];

		for (int i = 0; i < circle.Length; ++i)
		{
			image[i] = circle[i].GetComponent<UnityEngine.UI.Image>();
		}
	}

	void Update (){
		for(int i = 0; i < circle.Length; ++i){
			Color color = PuzzleBlock.Colors[(PuzzleBlock.ColorName)(int)char.GetNumericValue(PuzzleManager.ColorOrder[(PuzzleManager.CurrentColor + i + 1) % PuzzleManager.ColorOrder.Length])];
			image[i].color = color;
		}
	}
}
