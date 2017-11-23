using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockSlotUI : MonoBehaviour {

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

	int? clickingObject = null;

	// ドラッグ時の幻影
	GameObject ghostObject = null;
	Vector3 ghostPos;

	[SerializeField]
	GameObject block;

	void Start () {
		SlotObject = new RectTransform[slotLength];
		buttonPosition = new Rect[slotLength];

		var length = SlotObject.Length;
		for (int i = 0; i < length; ++i){
			SlotObject[i] = (Instantiate(slotPrefab, new Vector3(230f, -110f + -(i * 150), 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
			SlotObject[i].SetParent(transform, false);
			SlotObject[i].transform.name = "BlockSlot " + i;
			buttonPosition[i] = new Rect(basePosition.x + (positionMargin.x * i), basePosition.y + (positionMargin.y * i), slotSize.x, slotSize.y);

			if (true)
			{
				var obj = (Instantiate(rect, new Vector3(buttonPosition[i].x, buttonPosition[i].y, 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
				obj.SetParent(transform.parent);
				obj.sizeDelta = new Vector2(slotSize.x, slotSize.y);
			}
		}
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			var length = buttonPosition.Length;
			for (int i = 0; i < length; ++i)
			{
				if (buttonPosition[i].Contains(Input.mousePosition))
				{
					("クリック obj:" + i).Log();
					clickingObject = i;
				}
			}
		}

		if(Input.GetMouseButton(0) && clickingObject != null){
		
		// Vector3でマウス位置座標を取得する
		var position = Input.mousePosition;
		// Z軸修正
		position.z = 10f;
		// マウス位置座標をスクリーン座標からワールド座標に変換する
		var screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(position);
		// ワールド座標に変換されたマウス座標を代入
		ghostPos = screenToWorldPointPosition;

			ghostPos.Log();

			if(ghostObject == null){
				// var obj = gameObject.transform.GetChild(0).GetChild(0).gameObject;
				ghostObject = Instantiate(block);
				ghostObject.transform.rotation = block.transform.rotation;
				ghostObject.name = "Ghost Block";
			}
			ghostObject.transform.position = ghostPos;
		}

		if(Input.GetMouseButtonUp(0) && clickingObject != null){
			if(ghostObject != null){
				var mapPos = new Vector3((int)(ghostObject.transform.position.x + 0.5f), PuzzleManager.CurrentStage, (10 - (int)ghostObject.transform.position.z));	// 相対位置
				var pos = new Vector3((int)(ghostObject.transform.position.x + 0.5f), 0, ((int)(ghostObject.transform.position.z + 0.5f )) + 0.25f);	// 絶対位置
				
				if(mapPos.x >= 0 && mapPos.x < PuzzleManager.MapSize && mapPos.y >= 0 && mapPos.y < PuzzleManager.MapSize ){
					var objs = Physics.OverlapSphere(pos, 0.05f);
					if(objs.Length == 0 && GameObject.Find("Stage " + PuzzleManager.CurrentStage)){
						(gameObject.name + " → " + mapPos + " [" + pos + "]").Log();

						ghostObject.transform.position = mapPos;
						ghostObject.gameObject.transform.parent = GameObject.Find("Stage " + PuzzleManager.CurrentStage).transform;
						/*
						childCollider.enabled = true;
						childCollider.gameObject.transform.parent = GameObject.Find("Stage " + PuzzleManager.CurrentStage).transform;
						childCollider.gameObject.GetComponent<TurnBlockBase>().CanMoveFromMouse = true;
						childCollider.gameObject.GetComponent<TurnBlockBase>().enabled = true;
						*/

						/* 
						// transform.position = objPos;
						// isAnimating = true;
						smoothMoveFrame = 0;
						targetPos = pos;
						//targetLocalPos = transform.InverseTransformDirection(targetPos - transform.position);
						targetLocalPos = targetPos - transform.position;
						*/
					}else{
					}
				}else{
				}
				// Destroy(ghostObject);
				ghostObject = null;
			}	
		}
	}
}
