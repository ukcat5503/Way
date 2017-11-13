using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
	
	// Update is called once per frame
	void Update ()
	{
		if(IsActive){

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