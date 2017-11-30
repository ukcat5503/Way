using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinkers : MonoBehaviour {
	Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();

		var c = image.color;
		c.a = 1f;
		image.color = c;
	}
	
	// Update is called once per frame
	void Update () {
		var c = image.color;
		c.a -= 0.008f;
		image.color = c;

		if(c.a <= 0){
			Destroy(gameObject);
		}
	}
}
