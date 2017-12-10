using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffRoll : MonoBehaviour {

	[SerializeField]
	GameObject textObj;
	GameObject canvas;


	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		var obj = Instantiate(textObj, transform.position, Quaternion.identity).GetComponent<Text>();
		obj.transform.SetParent(canvas.transform);
		obj.transform.localPosition = textObj.transform.position;
		
	}
}
