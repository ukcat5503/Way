using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeft : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionStay(Collision other){
		other.transform.root.transform.Rotate(0,5,0);
		if(other.transform.root.transform.eulerAngles.y > 90){
			other.transform.root.transform.eulerAngles = new Vector3(0f, 90f, 0f);
		}
		// other.transform.Rotate(0,-1,0);
	}
}
