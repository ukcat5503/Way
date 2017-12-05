using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Collider _collider;

	public bool IsActive = false;

	const float kSpeedPerSecond = 1.75f;

	public static TurnBlockBase.StartPosition Direction;
	public static Vector3 Pos;

	public static bool IsSpeedUp = false;

	int frame = 0;

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
		if(!PlayerController.IsSpeedUp && ++frame % 60 == 0){
			if(PuzzleManager.IsConnectToGoalBlock(PlayerController.Pos, PlayerController.Direction)){
				PlayerController.IsSpeedUp = true;
			}
		}


		Pos = transform.position;

		// 移動方向求める
		float angleDir = transform.eulerAngles.y * (Mathf.PI / 180.0f);
		Vector3 dir = new Vector3 (Mathf.Cos (angleDir), Mathf.Sin (angleDir), 0.0f);
		if(dir.x > 0.9f){
			Direction = TurnBlockBase.StartPosition.North;
		}else if(dir.x < -0.9f){
			Direction = TurnBlockBase.StartPosition.South;
		}else if(dir.y < -0.9f){
			Direction = TurnBlockBase.StartPosition.West;
		}else if(dir.y > 0.9f){
			Direction = TurnBlockBase.StartPosition.East;
		}

		if(IsActive){
			var radian = (transform.eulerAngles.y) * Mathf.Deg2Rad;
			Vector2 vector = new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
			if(IsSpeedUp){
				transform.position = new Vector3(
					(vector.x * kSpeedPerSecond * 4 / 60f) + transform.position.x,
					transform.position.y,
					(vector.y * kSpeedPerSecond * 4 / 60f) + transform.position.z
				);
			}else{
				transform.position = new Vector3(
					(vector.x * kSpeedPerSecond / 60f) + transform.position.x,
					transform.position.y,
					(vector.y * kSpeedPerSecond / 60f) + transform.position.z
				);
			}
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