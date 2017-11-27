using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeldBlockSlotUI : MonoBehaviour {

	static HeldBlockSlotUI instance;

	// ドラッグ時の幻影
	GameObject ghostObject = null;
	Vector3 ghostPos;
	int currentObjNumber;

	// カーソル位置のガイド
	[SerializeField]
	GameObject cursorGuideObject;
	MeshRenderer cursorGuideMeshRenderer;
	[SerializeField]
	Color normalStateColor, pickingStateColor;

	// 選択中のブロック情報
	Vector2 pickingWorldPos;
	bool isPicking;

	[Space(8), SerializeField]
	Sprite[] heldObjSprite;
	[SerializeField]
	GameObject[] heldObject;

	[SerializeField]
	LayerMask targetLayer;


	void Awake () {
		instance = this;
		cursorGuideObject = Instantiate(cursorGuideObject) as GameObject;
		cursorGuideMeshRenderer = cursorGuideObject.GetComponentInChildren<MeshRenderer>();
		cursorGuideMeshRenderer.material.color = normalStateColor;
	}
	
	void Update () {
		// マウス位置演算
		var position = Input.mousePosition;
		position.z = 10f;
		var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);

		var mouseWorldPosition = new Vector3(
			(int)(screenToWorldPointPosition.x + 0.5f),
			-PuzzleManager.CurrentStage * PuzzleManager.kMapDepth,
			((int)(screenToWorldPointPosition.z + 0.5f )) + 0.25f
		);

		var mouseLocalPosition = new Vector3(
			(int)(screenToWorldPointPosition.x + 0.5f),
			PuzzleManager.CurrentStage,
			(10 - (int)screenToWorldPointPosition.z + 0.5f)
		);

		if(!isPicking){
			if(isContainLocalMap(mouseLocalPosition)){
				cursorGuideObject.transform.position = mouseWorldPosition;
				Cursor.visible = false;
			}else{
				cursorGuideObject.transform.position = new Vector3(-50f, -50f, -50f);
				Cursor.visible = true;
			}
		}

		// 最初にUIクリック
		if(Input.GetMouseButtonDown(0) && !isPicking){
			isPicking = true;
			cursorGuideMeshRenderer.material.color = pickingStateColor;
		}else if(Input.GetMouseButtonDown(0) && isPicking){
			isPicking = false;
			cursorGuideMeshRenderer.material.color = normalStateColor;

		}
	}

	bool isContainLocalMap(Vector3 localPos){
		return localPos.x >= 0 &&
		 localPos.x < PuzzleManager.kMapWidth &&
		 localPos.z >= 0 &&
		 localPos.z < PuzzleManager.kMapHeight;
	}
}
