﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnObject : MonoBehaviour {

	public float TurnParFrameAngle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y + TurnParFrameAngle,0);
	}
}
