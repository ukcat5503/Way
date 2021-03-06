﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : TurnBlockBase
{
	enum MoveDirection
	{
		West,
		North,
		East,
		South
	}

	[SerializeField]
	MoveDirection moveDirection;

	[SerializeField]
	LayerMask targetLayer;

	// bool isAnimating = false;
	Vector3 movePos;
	Vector3 moveLocalPos;
	int currentFrame = kAnimationFrame + 1;

	const int kAnimationFrame = 20;

	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();

		if (++currentFrame < kAnimationFrame)
		{
			transform.position += moveLocalPos / kAnimationFrame;
		}else if (currentFrame == kAnimationFrame)
		{
			transform.position = movePos;
		}
	}

	Vector3 GetCollidePosition()
	{
		Vector3 vector = Vector3.zero;
		switch (moveDirection) {
			case MoveDirection.West:
				vector.x = -1;
			break;
			case MoveDirection.East:
				vector.x = 1;
			break;
			case MoveDirection.North:
				vector.z = 1;
			break;
			case MoveDirection.South:
				vector.z = -1;
			break;
		}

		Vector3 checkpos = transform.position;
		for(int i = 0; i < 50; ++i)
		{
			checkpos += vector;
			var objs = Physics.OverlapSphere(checkpos, 0.25f, targetLayer);



			if (objs.Length != 0 || checkpos.x < 0 || checkpos.x >= PuzzleManager.kMapWidth || checkpos.z < 0 || checkpos.z >= PuzzleManager.kMapHeight + 1)
			{
				checkpos -= vector;
				break;
			}
		}
		return checkpos;
	}

	void changeDirection()
	{
		switch (moveDirection)
		{
			case MoveDirection.West:
				moveDirection = MoveDirection.East;
				break;
			case MoveDirection.East:
				moveDirection = MoveDirection.West;
				break;
			case MoveDirection.North:
				moveDirection = MoveDirection.South;
				break;
			case MoveDirection.South:
				moveDirection = MoveDirection.North;
				break;
		}
	}

	new void OnCollisionExit(Collision other){
		isTouchSphere = false;
		if(sphereObjectInfo != null){
			sphereObjectInfo.obj.transform.eulerAngles = new Vector3(0f, sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate, 0f);
			sphereObjectInfo = null;
		}
		
		if (currentFrame > kAnimationFrame)
		{
			isAnimating = true;
			currentFrame = 0;
			movePos = GetCollidePosition();
			moveLocalPos = transform.InverseTransformDirection(movePos - transform.position);
			changeDirection();
		}
	}
}
