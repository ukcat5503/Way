﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Stage")){
			SoundManager.PlaySE(SoundManager.SE.GenerateBuilding);
			GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().StageClear();
			Destroy(gameObject);
		}
    }
}
