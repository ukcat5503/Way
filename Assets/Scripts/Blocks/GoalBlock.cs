using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		"ゴール".Log();
		CameraManager.CameraDown(PuzzleManager.MapHeight);
		Destroy(transform.parent.parent.gameObject);
	}
}