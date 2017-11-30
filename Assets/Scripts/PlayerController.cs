using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Collider _collider;

	public bool IsActive = false;

	const float kSpeedPerSecond = 2f;

	[SerializeField]
	LayerMask targerLayer;

	[SerializeField]
	float rotateY = 0;
	public float RotateY{
		get
		{
			return rotateY;
		}
	}

	void Start()
	{
		_collider = GetComponent<Collider>();
	}

	// Update is called once per frame
	void Update (){
		if(IsActive){
			var radian = (transform.eulerAngles.y) * Mathf.Deg2Rad;
			Vector2 vector = new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
			transform.position = new Vector3(
				(vector.x * kSpeedPerSecond / 60f) + transform.position.x,
				transform.position.y,
				(vector.y * kSpeedPerSecond / 60f) + transform.position.z
			);
		}

		RaycastHit hit;
		var isHit = Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hit, 100f, targerLayer);
		if (!isHit){
			_collider.enabled = false;
		}
	}

	public void RotationY(float rotate)
	{
		rotateY += rotate;
		transform.Rotate(0, rotate, 0);
	}
}