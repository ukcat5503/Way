using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeldBlockSlotUI : MonoBehaviour {

	static HeldBlockSlotUI instance;

	RectTransform[] SlotObject;

	[SerializeField]
	int slotLength;
	[SerializeField]
	GameObject slotPrefab;

	[SerializeField]
	Vector2 basePosition, slotSize, positionMargin;

	Rect[] buttonPosition;

	[SerializeField]
	GameObject rect;

	// ドラッグ時の幻影
	GameObject ghostObject = null;
	Vector3 ghostPos;
	int currentObjNumber;

	[SerializeField]
	Sprite[] heldObjSprite;
	[SerializeField]
	GameObject[] heldObject;

	[SerializeField]
	LayerMask targetLayer;

	public int[] partsQty;
	Text[] textObj;

	void Awake () {
		instance = this;
		SlotObject = new RectTransform[slotLength];
		buttonPosition = new Rect[slotLength];
		partsQty = new int[slotLength];
		textObj = new Text[slotLength];

		var length = SlotObject.Length;
		for (int i = 0; i < length; ++i){
			SlotObject[i] = (Instantiate(slotPrefab, new Vector3(60f, -110f + -(i * 150), 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
			SlotObject[i].SetParent(transform, false);
			SlotObject[i].transform.name = "BlockSlot " + i;
			buttonPosition[i] = new Rect(basePosition.x + (positionMargin.x * i), basePosition.y + (positionMargin.y * i), slotSize.x, slotSize.y);
			
			SlotObject[i].Find("BlockImage").GetComponent<Image>().sprite = heldObjSprite[i];
			textObj[i] = SlotObject[i].Find("BlockQty").GetComponent<Text>();
			textObj[i].text = "<color=grey>0</color>";

			if (false)
			{
				var obj = (Instantiate(rect, new Vector3(buttonPosition[i].x, buttonPosition[i].y, 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
				obj.SetParent(transform.parent);
				obj.sizeDelta = new Vector2(slotSize.x, slotSize.y);
			}
		}
	}
	
	void Update () {

		// 最初にUIクリック
		if(Input.GetMouseButtonDown(0) && ghostObject == null){
			// Vector3でマウス位置座標を取得する
			var position = Input.mousePosition;
			// Z軸修正
			position.z = 10f;
			// マウス位置座標をスクリーン座標からワールド座標に変換する
			var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
			// ワールド座標に変換されたマウス座標を代入
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
		}

		// ドラッグ 位置更新
		if(Input.GetMouseButton(0) && ghostObject != null){
			// Vector3でマウス位置座標を取得する
			var position = Input.mousePosition;
			// Z軸修正
			position.z = 10f;
			// マウス位置座標をスクリーン座標からワールド座標に変換する
			var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
			// ワールド座標に変換されたマウス座標を代入
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
						*/
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
		}
	}

	void refreshView(){
		for (int i = 0; i < partsQty.Length; ++i){
			var num = partsQty[i];
				string str;
				if(num > 0){
					str = "<color=blue>" + (num).ToString() + "</color>";
				}else if(num == 0){
					str = "<color=grey>0</color>";
				}else{
					str = "<color=red>" + (-num).ToString() + "</color>";
				}
			textObj[i].text = str;

		}
	}

	public static void ResetAndAddBlocks(int block1, int block2, int block3, int block4, int block5, int block6){
		Destroy(instance.ghostObject);

		instance.partsQty[0] = block1;
		instance.partsQty[1] = block2;
		instance.partsQty[2] = block3;
		instance.partsQty[3] = block4;
		instance.partsQty[4] = block5;
		instance.partsQty[5] = block6;

		instance.refreshView();
	}
}
