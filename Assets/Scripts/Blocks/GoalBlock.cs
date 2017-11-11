using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		"ゴール".Log();
		++PuzzleManager.StageNumber;
		CameraManager.CameraDown(PuzzleManager.MapHeight);
		HeldBlockManager.GenerateBlocks();
		Destroy(transform.parent.parent.gameObject);
	}
}