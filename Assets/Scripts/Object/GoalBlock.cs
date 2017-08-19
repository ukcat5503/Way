﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Stage")){
			GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>().StageClear();
			Destroy(gameObject);
		}
    }
}
