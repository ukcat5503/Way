﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	Rigidbody rigid;
	
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
		Vector3 pv = (transform.forward * 10) + (transform.up * 10);
		rigid.AddForce(pv,ForceMode.VelocityChange);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
