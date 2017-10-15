using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour {

	GameObject child;

	Rigidbody rigidBody;

	public bool IsActive = false;

	const float kSpeedPerSecond = 2.5f;

	[SerializeField]
	float rotateY = 0;
	public float RotateY{
		get
		{
			return rotateY;
		}
	}

	// Use this for initialization
	void Start () {
		child = transform.Find("SphereBody").gameObject;
		// RotationY(180f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(IsActive){
			child.transform.Rotate(kSpeedPerSecond * 2f, 0, 0);

			var radian = (transform.eulerAngles.y) * Mathf.Deg2Rad;
			Vector2 vector = new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
			transform.position = new Vector3(
				(vector.x * kSpeedPerSecond / 60f) + transform.position.x,
				transform.position.y,
				(vector.y * kSpeedPerSecond / 60f) + transform.position.z
			);
		}
	}

	public void RotationY(float rotate)
	{
		rotateY += rotate;
		transform.Rotate(0, rotate, 0);
	}
}