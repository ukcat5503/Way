﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlockParticle : MonoBehaviour {

	public Color CubeColor;

	int count = 0;

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystemRenderer>().material.color = CubeColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(++count > 300){
			Destroy(gameObject);
		}
	}
}
