using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

	Rigidbody rigidbody;

	GameObject parent;

	const float kSpeed = 1f;

	int frame = 0;

	public int Case = 0;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();

		parent = transform.root.gameObject;
		parent.transform.Rotate(0,180,0);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce(0,0,kSpeed);

		parent.transform.position += transform.localPosition;
		transform.localPosition = Vector3.zero;
	}
}