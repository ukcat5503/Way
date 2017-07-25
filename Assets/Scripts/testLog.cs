﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLog : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("initialization " + name);
		LogPlus.Load("update " + name);

        Debug.LogWarning("ほげ");

	}
	
	// Update is called once per frame
	void Update () {
    }
}