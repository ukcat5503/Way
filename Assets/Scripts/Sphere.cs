using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

	Rigidbody rigidbody;

	GameObject parent;

	const float kSpeed = 3f;

	int frame = 0;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();

		parent = transform.root.gameObject;
		
		(transform.forward * kSpeed).Log();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey (KeyCode.UpArrow)) {
            rigidbody.AddForce(0,0,kSpeed);
        }
        if (Input.GetKey (KeyCode.DownArrow)) {
            rigidbody.AddForce(0,0,-kSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
			var e = parent.transform.eulerAngles;
			e.y += kSpeed;
            parent.transform.eulerAngles = e;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
			var e = parent.transform.eulerAngles;
			e.y -= kSpeed;
            parent.transform.eulerAngles = e;
        }

		parent.transform.position += transform.localPosition;
		transform.localPosition = Vector3.zero;
	}
}