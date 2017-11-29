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
	Color normalStateColor, pickingStateColor, deleteStateColor, disableStateColor;

	// 選択中のブロック情報
	bool isPicking;
	int currentObj;
	Vector3 pickingObjectWorldPos;

	// ブロックのホイール
	Image[] images;
	RectTransform[] imageRectTranTransforms;
	Vector3[] imageBasePosition;
	Vector2[] imageBaseScale;
	const float kWheelSensitivity = 0f;
	float wheelValue;

	// ルーレット移動時のアニメーション
	const int kAnimationFrame = 10;
	int animationCurrentFrame = kAnimationFrame;
	bool upToAnimation;

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

		images = new Image[5];
		imageRectTranTransforms = new RectTransform[images.Length];
		imageBasePosition = new Vector3[images.Length];
		imageBaseScale = new Vector2[images.Length];


		var length = images.Length;
		for (int i = 0; i < length; ++i){
			images[i] = transform.Find("currentObj_" + i).GetComponent<Image>();
			imageRectTranTransforms[i] = images[i].GetComponent<RectTransform>();
			imageBasePosition[i] = imageRectTranTransforms[i].localPosition;
			imageBaseScale[i] = imageRectTranTransforms[i].sizeDelta;
			images[i].sprite = heldObjSprite[calcFixedIndex(currentObj + i - 2)];
		}
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
			(10 - (int)(screenToWorldPointPosition.z + 0.5f))
		);

		if(!isPicking){
			if(isContainLocalMap(mouseLocalPosition)){
				cursorGuideObject.transform.position = mouseWorldPosition;
				Cursor.visible = false;
			}else{
				cursorGuideObject.transform.position = new Vector3(-50f, -50f, -50f);
				Cursor.visible = true;
			}

		}else if(animationCurrentFrame++ <= kAnimationFrame){
			playAnimation();
			
		}else{
			wheelValue += Input.GetAxis("Mouse ScrollWheel");
			if(wheelValue > kWheelSensitivity){
				wheelValue = 0f;
				wheelScrollImage(true);
				upToAnimation = true;
				animationCurrentFrame = 0;

			}else if(wheelValue < -kWheelSensitivity){
				wheelValue = 0f;
				wheelScrollImage();
				upToAnimation = false;
				animationCurrentFrame = 0;
			}
		}

		var objs = Physics.OverlapSphere(cursorGuideObject.transform.position, 0.05f, targetLayer);
		if(objs.Length > 0){
			// 既存のブロックを選択
			cursorGuideMeshRenderer.material.color = deleteStateColor;

			// 右クリックでブロック削除
			if(Input.GetMouseButton(1)){
				var length = objs.Length;
				for (int i = 0; i < length; ++i){
					if(objs[i].gameObject.layer == LayerMask.NameToLayer("Block")){
						Destroy(objs[i].gameObject);
					}
				}
			}

		}else if(isPicking){
			// 新規ブロックを選択中
			cursorGuideMeshRenderer.material.color = pickingStateColor;

			if(Input.GetMouseButtonDown(0)){
				// 左クリックでブロック設置
				isPicking = false;
				cursorGuideMeshRenderer.material.color = normalStateColor;

				var parentObj = GameObject.Find("Stage " + PuzzleManager.CurrentStage + "/Maps");
				if(objs.Length == 0 && parentObj){
					var obj = Instantiate(heldObject[calcFixedIndex(currentObj)], pickingObjectWorldPos, heldObject[calcFixedIndex(currentObj)].transform.rotation) as GameObject;
					obj.transform.parent = parentObj.transform;
					var turn = obj.GetComponent<TurnBlockBase>();
					if(turn != null){
						turn.SetTurnBlockType(TurnBlockBase.BlockType.Place);
					}
				}
				transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, new Vector3(-50f, -50f, -50f));

			}else if(Input.GetMouseButtonDown(1)){
				// 右クリックで選択解除
				isPicking = false;
				
				transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, new Vector3(-50f, -50f, -50f));
			}

		}else{
			// 何も選択してない
			cursorGuideMeshRenderer.material.color = normalStateColor;

			// 最初にUIクリック
			if(Input.GetMouseButtonDown(0) && !isPicking){
				if(isContainLocalMap(mouseLocalPosition)){
					isPicking = true;
					cursorGuideMeshRenderer.material.color = pickingStateColor;
					pickingObjectWorldPos = mouseWorldPosition;

					transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, mouseWorldPosition);
				}
			}
		}
	}

	void wheelScrollImage(bool isMoveUp = false){
		if(isMoveUp)	++currentObj;
		else			--currentObj;
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

	void playAnimation(){
		if(upToAnimation){
			if(animationCurrentFrame == kAnimationFrame){
			var length = imageRectTranTransforms.Length;
			for (int i = 0; i < length; ++i){
				imageRectTranTransforms[i].localPosition = imageBasePosition[i];
				imageRectTranTransforms[i].sizeDelta = imageBaseScale[i];
				images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1f);
				images[i].sprite = heldObjSprite[calcFixedIndex(currentObj + i - 2)];
				}
			}else{
				// 1 → 0
				imageRectTranTransforms[1].transform.position = new Vector3(
					imageRectTranTransforms[1].transform.position.x,
					imageRectTranTransforms[1].transform.position.y + 90 / kAnimationFrame,
					imageRectTranTransforms[1].transform.position.z
				);
				imageRectTranTransforms[1].sizeDelta = new Vector2(
					imageRectTranTransforms[1].sizeDelta.x - 40 / kAnimationFrame ,
					imageRectTranTransforms[1].sizeDelta.y - 40 / kAnimationFrame
				);
			
				// 2 → 1
				imageRectTranTransforms[2].transform.position = new Vector3(
					imageRectTranTransforms[2].transform.position.x,
					imageRectTranTransforms[2].transform.position.y + 130 / kAnimationFrame,
					imageRectTranTransforms[2].transform.position.z
				);
				imageRectTranTransforms[2].sizeDelta = new Vector2(
					imageRectTranTransforms[2].sizeDelta.x - 50 / kAnimationFrame,
					imageRectTranTransforms[2].sizeDelta.y - 50 / kAnimationFrame
				);

				// 3 → 2
				imageRectTranTransforms[3].transform.position = new Vector3(
					imageRectTranTransforms[3].transform.position.x,
					imageRectTranTransforms[3].transform.position.y + 130 / kAnimationFrame,
					imageRectTranTransforms[3].transform.position.z
				);
				imageRectTranTransforms[3].sizeDelta = new Vector2(
					imageRectTranTransforms[3].sizeDelta.x + 50 / kAnimationFrame,
					imageRectTranTransforms[3].sizeDelta.y + 50 / kAnimationFrame
				);

				// 4 → 3
				imageRectTranTransforms[4].transform.position = new Vector3(
					imageRectTranTransforms[4].transform.position.x,
					imageRectTranTransforms[4].transform.position.y + 90 / kAnimationFrame,
					imageRectTranTransforms[4].transform.position.z
				);
				imageRectTranTransforms[4].sizeDelta = new Vector2(
					imageRectTranTransforms[4].sizeDelta.x + 40 / kAnimationFrame,
					imageRectTranTransforms[4].sizeDelta.y + 40 / kAnimationFrame
				);

				if(animationCurrentFrame <= kAnimationFrame / 2){
					// 0を徐々に消す
					images[0].color = new Color(images[0].color.r, images[0].color.g, images[0].color.b, images[0].color.a - images[0].color.a / (kAnimationFrame / 2));

					if(animationCurrentFrame == kAnimationFrame / 2){
						imageRectTranTransforms[0].localPosition = imageBasePosition[4];
						images[0].sprite = heldObjSprite[calcFixedIndex(currentObj + 2)];
					}
				}else{
					images[0].color = new Color(images[0].color.r, images[0].color.g, images[0].color.b, images[0].color.a + images[0].color.a / (kAnimationFrame / 2));
				}
			}
		}else{
			if(animationCurrentFrame == kAnimationFrame){
			var length = imageRectTranTransforms.Length;
			for (int i = 0; i < length; ++i){
				imageRectTranTransforms[i].localPosition = imageBasePosition[i];
				imageRectTranTransforms[i].sizeDelta = imageBaseScale[i];
				images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1f);
				images[i].sprite = heldObjSprite[calcFixedIndex(currentObj + i - 2)];
				}
			}else{
				// 0 → 1
				imageRectTranTransforms[0].transform.position = new Vector3(
					imageRectTranTransforms[0].transform.position.x,
					imageRectTranTransforms[0].transform.position.y - 90 / kAnimationFrame,
					imageRectTranTransforms[0].transform.position.z
				);
				imageRectTranTransforms[0].sizeDelta = new Vector2(
					imageRectTranTransforms[0].sizeDelta.x + 40 / kAnimationFrame ,
					imageRectTranTransforms[0].sizeDelta.y + 40 / kAnimationFrame
				);
			
				// 1 → 2
				imageRectTranTransforms[1].transform.position = new Vector3(
					imageRectTranTransforms[1].transform.position.x,
					imageRectTranTransforms[1].transform.position.y - 130 / kAnimationFrame,
					imageRectTranTransforms[1].transform.position.z
				);
				imageRectTranTransforms[1].sizeDelta = new Vector2(
					imageRectTranTransforms[1].sizeDelta.x + 50 / kAnimationFrame,
					imageRectTranTransforms[1].sizeDelta.y + 50 / kAnimationFrame
				);

				// 2 → 3
				imageRectTranTransforms[2].transform.position = new Vector3(
					imageRectTranTransforms[2].transform.position.x,
					imageRectTranTransforms[2].transform.position.y - 130 / kAnimationFrame,
					imageRectTranTransforms[2].transform.position.z
				);
				imageRectTranTransforms[2].sizeDelta = new Vector2(
					imageRectTranTransforms[2].sizeDelta.x - 50 / kAnimationFrame,
					imageRectTranTransforms[2].sizeDelta.y - 50 / kAnimationFrame
				);

				// 3 → 4
				imageRectTranTransforms[3].transform.position = new Vector3(
					imageRectTranTransforms[3].transform.position.x,
					imageRectTranTransforms[3].transform.position.y - 90 / kAnimationFrame,
					imageRectTranTransforms[3].transform.position.z
				);
				imageRectTranTransforms[3].sizeDelta = new Vector2(
					imageRectTranTransforms[3].sizeDelta.x - 40 / kAnimationFrame,
					imageRectTranTransforms[3].sizeDelta.y - 40 / kAnimationFrame
				);

				if(animationCurrentFrame <= kAnimationFrame / 2){
					// 4を徐々に消す
					images[4].color = new Color(images[4].color.r, images[4].color.g, images[4].color.b, images[4].color.a - images[4].color.a / (kAnimationFrame / 2));

					if(animationCurrentFrame == kAnimationFrame / 2){
						imageRectTranTransforms[4].localPosition = imageBasePosition[0];
						images[4].sprite = heldObjSprite[calcFixedIndex(currentObj - 2)];
					}
				}else{
					images[4].color = new Color(images[4].color.r, images[4].color.g, images[4].color.b, images[4].color.a + images[4].color.a / (kAnimationFrame / 2));
				}
			}
		}
	}
}
