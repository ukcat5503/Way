using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceWindow : MonoBehaviour {
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		text.text = transform.name;
	}
}
