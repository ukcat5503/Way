using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		"ゴール".Log();
	}
}
