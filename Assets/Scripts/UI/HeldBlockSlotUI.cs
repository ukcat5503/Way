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
	Sprite[] heldObjSprite;
	[SerializeField]
	GameObject[] heldObject;

	[SerializeField]
	LayerMask targetLayer;


	void Awake () {
		instance = this;
		cursorGuideObject = Instantiate(cursorGuideObject) as GameObject;
		cursorGuideMeshRenderer = cursorGuideObject.GetComponentInChildren<MeshRenderer>();
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
		
		if(isContainLocalMap(mouseLocalPosition)){
			cursorGuideObject.transform.position = mouseWorldPosition;
			Cursor.visible = false;
		}else{
			cursorGuideObject.transform.position = new Vector3(-50f, -50f, -50f);
			Cursor.visible = true;
		}


		// 最初にUIクリック
		if(Input.GetMouseButtonDown(0) && ghostObject == null){
			/*
			ghostPos = screenToWorldPointPosition;
			var length = buttonPosition.Length;
			for (int i = 0; i < length; ++i){
				if (buttonPosition[i].Contains(Input.mousePosition)){
					ghostObject = Instantiate(heldObject[i]);
					ghostObject.transform.rotation = heldObject[i].transform.rotation;
					ghostObject.name = "Ghost Block [" + i + "]";
					ghostObject.transform.position = ghostPos;
					ghostObject.layer = 0;
					currentObjNumber = i;
					break;
				}
			}
			 */
		}

		// ドラッグ 位置更新
		if(Input.GetMouseButton(0) && ghostObject != null){
			ghostPos = screenToWorldPointPosition;

			// ghostObject.transform.position = ghostPos;

			var pos = new Vector3(
				(int)(ghostPos.x + 0.5f),
				-PuzzleManager.CurrentStage * PuzzleManager.kMapDepth,
				((int)(ghostPos.z + 0.5f )) + 0.25f
			);	// ワールド座標

			var mapPos = new Vector3(
				(int)(ghostPos.x + 0.5f),
				PuzzleManager.CurrentStage,
				(10 - (int)ghostPos.z + 0.5f)
			);	// ローカル座標

			ghostObject.transform.position = pos;
			("world: " + pos + " local:" + mapPos).Log();
		}
		
		// 置く判定
		if(Input.GetMouseButtonUp(0) && ghostObject != null){
			/*
			if(ghostObject != null){
				var pos = new Vector3(
					(int)(ghostPos.x + 0.5f),
					-PuzzleManager.CurrentStage * PuzzleManager.kMapDepth,
					((int)(ghostPos.z + 0.5f )) + 0.25f
				);	// ワールド座標

				var mapPos = new Vector3(
					(int)(ghostPos.x + 0.5f),
					PuzzleManager.CurrentStage,
					(10 - (int)ghostPos.z + 0.5f)
				);	// ローカル座標
				
				(mapPos + " / " + pos).Log();

				// (mapPos.x + "," + mapPos.z).Log();

				if(mapPos.x >= 0 && mapPos.x < PuzzleManager.kMapWidth && mapPos.z >= 0 && mapPos.z < PuzzleManager.kMapHeight ){
					var objs = Physics.OverlapSphere(pos, 0.05f, targetLayer);
					objs.Length.Log();
					var parentObj = GameObject.Find("Stage " + PuzzleManager.CurrentStage + "/Maps");
					if(objs.Length == 0 && parentObj){
						// (gameObject.name + " → " + mapPos + " [" + pos + "]").Log();
						("pos: " + pos).Log();
						ghostObject.transform.position = pos;
						ghostObject.gameObject.transform.parent = parentObj.transform;
						
						var num = --partsQty[currentObjNumber];
						string str;
						if(num > 0){
							str = "<color=blue>" + (num).ToString() + "</color>";
						}else if(num == 0){
							str = "<color=grey>0</color>";
						}else{
							str = "<color=red>" + (-num).ToString() + "</color>";
						}
						textObj[currentObjNumber].text = str;

						/* 
						// transform.position = objPos;
						// isAnimating = true;
						smoothMoveFrame = 0;
						targetPos = pos;
						//targetLocalPos = transform.InverseTransformDirection(targetPos - transform.position);
						targetLocalPos = targetPos - transform.position;
						ghostObject.layer = LayerMask.NameToLayer("Block");
						ghostObject = null;
					}else{
						Destroy(ghostObject);
					}
				}else{
					Destroy(ghostObject);
				}
				// Destroy(ghostObject);
				ghostObject = null;
			}
			*/	
		}
	}

	bool isContainLocalMap(Vector3 localPos){
		return localPos.x >= 0 &&
		 localPos.x < PuzzleManager.kMapWidth &&
		 localPos.z >= 0 &&
		 localPos.z < PuzzleManager.kMapHeight;
	}
}
