﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(PuzzleManager.StageData[PuzzleManager.CurrentStage].IsCollectAllCoin()){
			Destroy(other.gameObject);
			PuzzleManager.NextStage(transform.parent.parent.gameObject);
			SoundManager.PlaySE(SoundManager.SE.clear);
			PlayerController.IsSpeedUp = false;
			PlayerController.IsTurnFromPrevBlock = false;
		}
		else
		{
			other.GetComponent<PlayerController>().BreakPlayer("Collect all coins!!");
		}
	}
}