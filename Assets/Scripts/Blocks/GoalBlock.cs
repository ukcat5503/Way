using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		PuzzleManager.NextStage(transform.parent.parent.gameObject);
	}
}