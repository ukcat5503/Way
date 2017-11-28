using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		Destroy(other.gameObject);
		PuzzleManager.NextStage(transform.parent.parent.gameObject);
	}
}