﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlockFragment : MonoBehaviour {

	[SerializeField]
	float deleteHeight = -15f;

	// Update is called once per frame
	void Update () {
		if(deleteHeight > transform.position.y){
			Destroy(gameObject);
		}
	}
}
