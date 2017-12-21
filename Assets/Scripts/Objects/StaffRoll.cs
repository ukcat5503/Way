using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffRoll : MonoBehaviour {

	[SerializeField]
	GameObject textObj;
	GameObject canvas;

	Text obj;


	// Use this for initialization
	void Start () {
		canvas = GameObject.Find("Canvas");
		obj = Instantiate(textObj, transform.position, Quaternion.identity).GetComponent<Text>();
		obj.transform.SetParent(canvas.transform);
		obj.transform.localPosition = textObj.transform.position;
		obj.color = new Color(1,1,1,0);
	}

	void Update(){
		if(obj.color.a < 1f){
			obj.color = new Color(obj.color.r, obj.color.g, obj.color.b, obj.color.a + 0.005f);
		}
	}
}
