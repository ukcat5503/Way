﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(BoxCollider))]
public class TurnBlockBase : MonoBehaviour {

	protected class ObjectInfo{
		public float currentRotate;
		public float targetRotate;
		public GameObject obj;
		public PlayerController sphere;

		public ObjectInfo(float c, float t, GameObject o, PlayerController s){
			currentRotate = c;
			targetRotate = t;
			obj = o;
			sphere = s;
		}
	}

	/// <summary>
	/// Z↑ X→を基準にしたときの方角
	/// </summary>
	public enum StartPosition{
		West,
		North,
		East,
		South
	}

	public enum RotateAngle {
		Zero = 0,
		Right90 = 90,
		Left90 = -90,
		Back = 180
	}

	public enum TurnAngle {
		NotTurn,
		TurnRight,
		TurnFlip,
		TurnLeft
	}

	public enum BlockType{
		NotTurn,
		TurnLeft,
		Move,
		Place,
		TurnRight,
	}

	public enum ClickEventType{
		LeftClick,
		RightClick,
		MiddleClick,
		WheelUp,
		WheelDown
	}


	RotateAngle ValueToRotateAngle(int n) {
		foreach (RotateAngle a in System.Enum.GetValues(typeof(RotateAngle)))
		{
			if ((int)a == n) return a;
		}
		("enumに指定する値がありませんでした。" + n).Log();
		return RotateAngle.Zero;
	}

	Collider _collider;

	protected int[] targetPoint = new int[4];
	protected ObjectInfo sphereObjectInfo = null;

	const float kMaxRange = 180f;
	[SerializeField]
	protected bool isTouchSphere;
	[HideInInspector]
	public bool CanMoveFromMouse;

	[UnityEngine.Serialization.FormerlySerializedAs("targetFromWest")]
	public RotateAngle TargetFromWest;
	[UnityEngine.Serialization.FormerlySerializedAs("targetFromNorth")]
	public RotateAngle TargetFromNorth;
	[UnityEngine.Serialization.FormerlySerializedAs("targetFromEast")]
	public RotateAngle TargetFromEast;
	[UnityEngine.Serialization.FormerlySerializedAs("targetFromSouth")]
	public RotateAngle TargetFromSouth;

	[SerializeField, Space(6), Header("1動作でどちらにブロックが動作するか")]
	BlockType turnBlockType;
	public void SetTurnBlockType(BlockType type){
		turnBlockType = type;
	}

	// 回転アニメーション系
	protected bool isAnimating = false;
	float targetAngle = 0f;
	float finalAngle = 0f;
	float finalViewAngle = 0f;
	public float currentAngle = 0f;
	bool leftRotate = false;
	const int kAnimationFrame = 20;
	const float kTolerancePotisionToStartRotate = 0.3f;
	

	// ドラッグの幻影系
	GameObject ghostObject;
	Vector3 ghostPos;

	// ドラッグ交換 スムース移動系
	int smoothMoveFrame = kSmoothMoveFrame + 1;
	Vector3 targetPos;
	Vector3 targetLocalPos;
	const int kSmoothMoveFrame = 10;
	const float	popupHeight = 2f / kSmoothMoveFrame * 0.75f;

	// ブロック回転のため
	const int kWaitRotateFrame = 30;
	int waitRotateCurrentFrame = kWaitRotateFrame;
	


	protected void Start () {
		/*
		if(Random.Range(0,2) == 1){
			CanMoveFromMouse = true;
		}
		*/
		CanMoveFromMouse = true;

		_collider = GetComponent<Collider>();
		var mesh = GetComponentsInChildren<MeshRenderer>()[0];
		switch (turnBlockType)
		{
			case BlockType.Place:
				mesh.material.color = PuzzleManager.PlaceColor;
				mesh.material.SetColor("_EmissionColor", new Color(2f,2f,2f));
			break;			
			case BlockType.NotTurn:
				mesh.material.color = PuzzleManager.NotTurnColor;
				mesh.material.SetColor("_EmissionColor", new Color(1f,1f,1f));
				break;
			case BlockType.TurnLeft:
				mesh.material.color = PuzzleManager.TurnColor;
				mesh.material.SetColor("_EmissionColor", new Color(2.5f,2.5f,2f));
				break;
			case BlockType.TurnRight:
				mesh.material.color = PuzzleManager.TurnRightColor;
				mesh.material.SetColor("_EmissionColor", new Color(2.5f,2.5f,2f));
				break;
			case BlockType.Move:
				mesh.material.color = PuzzleManager.MoveColor;
				mesh.material.SetColor("_EmissionColor", new Color(2.5f,2.5f,2.5f));
				break;
		}
		Setup();
	}
	
	virtual protected void Update () {
		if(++waitRotateCurrentFrame == kWaitRotateFrame){
			_collider.enabled = true;
		}

		if(isAnimating){
			rotateY(targetAngle / kAnimationFrame);

			if(!leftRotate){
				if(currentAngle > finalAngle){
					rotateYFixedValue(finalViewAngle);
					isAnimating = false;
				}
			}else{
				if(currentAngle < finalAngle){
					rotateYFixedValue(finalViewAngle);
					isAnimating = false;
				}
			}
		}

		// スムース移動
		if (++smoothMoveFrame < kSmoothMoveFrame)
		{
			var pos = targetLocalPos / kSmoothMoveFrame;
			if(smoothMoveFrame < kSmoothMoveFrame / 2){
				
				pos.y += popupHeight * (kSmoothMoveFrame / 2 - (smoothMoveFrame % kSmoothMoveFrame / 2));
			}else{
				pos.y -= popupHeight * (smoothMoveFrame % kSmoothMoveFrame / 2);
			}
			transform.position += pos;
		}else if (smoothMoveFrame == kSmoothMoveFrame)
		{
			transform.position = targetPos;
		}

		// ボール制御
		if(sphereObjectInfo != null){
			bool isDelete = (sphereObjectInfo.currentRotate < sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate) ?
			// 右折
			sphereObjectInfo.sphere.RotateY >= sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate:
			// 左折
			sphereObjectInfo.sphere.RotateY <= sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate;
			if (isDelete){
				sphereObjectInfo.obj.transform.eulerAngles = new Vector3(0f, sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate, 0f);
				// (sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate).Log();
				sphereObjectInfo = null;
			}
			else
			{
				var localPos = sphereObjectInfo.obj.transform.position - transform.position;
				// localPos.Log();
				// 回転していい位置まで行っていれば回転スタート
				if((localPos.x >= -kTolerancePotisionToStartRotate && localPos.x < kTolerancePotisionToStartRotate) && (localPos.z >= -kTolerancePotisionToStartRotate && localPos.z < kTolerancePotisionToStartRotate)){
					if(PlayerController.IsSpeedUp){
						sphereObjectInfo.sphere.RotationY(sphereObjectInfo.targetRotate / 2);
					}else{
						sphereObjectInfo.sphere.RotationY(sphereObjectInfo.targetRotate / 10);
					}
				}
				// sphereObjectInfo.sphere.RotationY(sphereObjectInfo.targetRotate / 17);
			}
		}
		
	}

	void TurnBlock(int turnCount)
	{
		TargetFromEast = ValueToRotateAngle(targetPoint[(int)StartPosition.East]);
		TargetFromNorth = ValueToRotateAngle(targetPoint[(int)StartPosition.North]);
		TargetFromWest = ValueToRotateAngle(targetPoint[(int)StartPosition.West]);
		TargetFromSouth = ValueToRotateAngle(targetPoint[(int)StartPosition.South]);
		for (;turnCount > 0; --turnCount)
		{
			var temp = TargetFromEast;
			TargetFromEast = TargetFromNorth;
			TargetFromNorth = TargetFromWest;
			TargetFromWest = TargetFromSouth;
			TargetFromSouth = temp;
		}
	}

	void Setup()
	{
		targetPoint[(int)StartPosition.West] = (int)TargetFromWest;
		targetPoint[(int)StartPosition.East] = (int)TargetFromEast;
		targetPoint[(int)StartPosition.North] = (int)TargetFromNorth;
		targetPoint[(int)StartPosition.South] = (int)TargetFromSouth;
	}

	[ContextMenu("Output")]
	public void Output()
	{
		(
			"name: " + gameObject.name + "\n" +
			"←: " + targetPoint[(int)StartPosition.West] + "\n" +
			"↑: " + targetPoint[(int)StartPosition.North] + "\n" +
			"→: " + targetPoint[(int)StartPosition.East] + "\n" +
			"↓: " + targetPoint[(int)StartPosition.South] + "\n"
		).Log();
	}

	void rotateY(float angle){
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle,transform.eulerAngles.z);
		currentAngle += angle;
	}

	void rotateYFixedValue(float angle){
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle,transform.eulerAngles.z);
		// currentAngle = angle;
	}

	protected StartPosition CalcStartPosition(Collision other){
		StartPosition position;
		if(Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.z - other.transform.position.z)){
			if(transform.position.x - other.transform.position.x > 0){
				position = StartPosition.West;
			}else{
				position = StartPosition.East;
			}
		}else{
			if(transform.position.z - other.transform.position.z < 0){
				position = StartPosition.North;
			}else{
				position = StartPosition.South;
			}
		}
		// (position.ToString() + "から来た").Log();
		return position;
	}

	/*
	public void ClickObject(ClickEventType type){
		// (transform.name + "をクリック" + "  type: " + type).Log();
		clickAction(type);
	}
	*/
	
	virtual protected void clickAction(ClickEventType type){
		if (!isAnimating && turnBlockType != BlockType.NotTurn && turnBlockType != BlockType.Place && !isTouchSphere){
			TurnAngle turnBlockAngle;
			switch (type)
			{
				case ClickEventType.WheelUp:
					targetAngle = -90f;
					leftRotate = true;
					turnBlockAngle = TurnAngle.TurnLeft;
				break;
				case ClickEventType.WheelDown:
					targetAngle = 90f;
					leftRotate = false;
					turnBlockAngle = TurnAngle.TurnRight;
				break;
				default:
				return;
			}

			isAnimating = true;

			finalAngle = currentAngle + targetAngle;
			finalViewAngle = transform.eulerAngles.y + targetAngle;

			TurnBlock((int)turnBlockAngle);
			Setup();
		}
	}

	/*
	public bool ChangeBlock(Vector3 originPos){
		if(CanMoveFromMouse && !isTouchSphere){
			// transform.position = originPos;
				smoothMoveFrame = 0;
				targetPos = originPos;
				//targetLocalPos = transform.InverseTransformDirection(targetPos - transform.position);
				targetLocalPos = targetPos - transform.position;
			return true;
		}
		return false;
	}
	*/
	
	virtual protected void OnCollisionEnter(Collision other){
		isTouchSphere = true;
		if (sphereObjectInfo == null){
			var position = CalcStartPosition(other);
			var s = other.gameObject.GetComponent<PlayerController>();
			if (targetPoint[(int)position] == 180f){
				if(PlayerController.IsTurnFromPrevBlock){
					// 脱線する
					PlayerController.IsTurnFromPrevBlock = false;
					s.BreakPlayer("Break!!");
				}else{
					PlayerController.IsTurnFromPrevBlock = false;
					s.RotationY(180);
				}
			}else{
				// 90f || -90f
				PlayerController.IsTurnFromPrevBlock = true;
				sphereObjectInfo = new ObjectInfo(s.RotateY, targetPoint[(int)position], other.gameObject, s);
			}
		}
	}

	virtual protected void OnCollisionExit(Collision other){
		isTouchSphere = false;
		if(sphereObjectInfo != null){
			sphereObjectInfo.obj.transform.eulerAngles = new Vector3(0f, sphereObjectInfo.currentRotate + sphereObjectInfo.targetRotate, 0f);
			sphereObjectInfo = null;
		}

		// ブロック移動
		TurnAngle turnBlockAngle;
		switch (turnBlockType){
			case BlockType.TurnLeft:
				targetAngle = -90f;
				leftRotate = true;
				turnBlockAngle = TurnAngle.TurnLeft;
			break;
			case BlockType.TurnRight:
				targetAngle = 90f;
				leftRotate = false;
				turnBlockAngle = TurnAngle.TurnRight;
			break;
			default:	return;
		}
			isAnimating = true;

			finalAngle = currentAngle + targetAngle;
			finalViewAngle = transform.eulerAngles.y + targetAngle;

			TurnBlock((int)turnBlockAngle);
			Setup();
			waitRotateCurrentFrame = 0;
			_collider.enabled = false;
	}
}