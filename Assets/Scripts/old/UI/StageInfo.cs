﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(UnityEngine.UI.Text))]
public class StageInfo : MonoBehaviour {
	UnityEngine.UI.Text text;

	void Start () {
		text = GetComponent<UnityEngine.UI.Text>();
	}
	
	void Update () {
		text.text = "Stage: " +
		(checkDigit(PuzzleManager.CurrentStage) == 1 ? " " : "") + PuzzleManager.CurrentStage + "/" +
		(checkDigit(PuzzleManager.TotalStage) == 1 ? " " : "") + PuzzleManager.TotalStage;
	}

	int checkDigit(int num){
		return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);
	}
}
