﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

	Rigidbody rigid;

	GameObject player;
	
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
		player = GameObject.Find("VRHead");
		transform.LookAt(player.transform);
		Vector3 pv = (-transform.forward * 15) + (transform.up * 10);
		rigid.AddForce(pv,ForceMode.VelocityChange);
	}
}
