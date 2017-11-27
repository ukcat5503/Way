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
	bool isPicking;
	int currentObj;
	Vector3 pickingObject;

	// ブロックのホイール
	Image imageTop2, imageTop1, image0, imageUnder1, imageUnder2;
	const float kWheelSensitivity = 0f;
	float wheelValue;


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

		imageTop2 = transform.Find("currentObj+2").GetComponent<Image>();
		imageTop1 = transform.Find("currentObj+1").GetComponent<Image>();
		image0 = transform.Find("currentObj").GetComponent<Image>();
		imageUnder1 = transform.Find("currentObj-1").GetComponent<Image>();
		imageUnder2 = transform.Find("currentObj-2").GetComponent<Image>();

		imageTop2.sprite = heldObjSprite[calcFixedIndex(currentObj - 2)];
		imageTop1.sprite = heldObjSprite[calcFixedIndex(currentObj - 1)];
		image0.sprite = heldObjSprite[calcFixedIndex(currentObj)];
		imageUnder1.sprite = heldObjSprite[calcFixedIndex(currentObj + 1)];
		imageUnder2.sprite = heldObjSprite[calcFixedIndex(currentObj + 2)];
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
		}else{
			wheelValue += Input.GetAxis("Mouse ScrollWheel");
			if(wheelValue > kWheelSensitivity){
				wheelValue = 0f;
				wheelScrollImage(true);

			}else if(wheelValue < -kWheelSensitivity){
				wheelValue = 0f;
				wheelScrollImage();
			}
		}

		// 最初にUIクリック
		if(Input.GetMouseButtonDown(0) && !isPicking){
			isPicking = true;
			cursorGuideMeshRenderer.material.color = pickingStateColor;
			pickingObject = mouseWorldPosition;

			transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, mouseWorldPosition);
		}else if(Input.GetMouseButtonDown(0) && isPicking){
			isPicking = false;
			cursorGuideMeshRenderer.material.color = normalStateColor;


			var obj = Instantiate(heldObject[calcFixedIndex(currentObj)], pickingObject, heldObject[calcFixedIndex(currentObj)].transform.rotation) as GameObject;

			transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, new Vector3(-50f, -50f, -50f));

			
		}
	}

	void wheelScrollImage(bool isMoveUp = false){
		if(isMoveUp)	++currentObj;
		else			--currentObj;

		imageTop2.sprite = heldObjSprite[calcFixedIndex(currentObj - 2)];
		imageTop1.sprite = heldObjSprite[calcFixedIndex(currentObj - 1)];
		image0.sprite = heldObjSprite[calcFixedIndex(currentObj)];
		imageUnder1.sprite = heldObjSprite[calcFixedIndex(currentObj + 1)];
		imageUnder2.sprite = heldObjSprite[calcFixedIndex(currentObj + 2)];
	}

	int calcFixedIndex(int index){
		var i = index % heldObjSprite.Length;
		return i >= 0 ? i : heldObjSprite.Length + i;
	}

	bool isContainLocalMap(Vector3 localPos){
		return localPos.x >= 0 &&
		 localPos.x < PuzzleManager.kMapWidth &&
		 localPos.z >= 0 &&
		 localPos.z < PuzzleManager.kMapHeight;
	}
}
