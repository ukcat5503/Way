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
	Color normalStateColor, deleteStateColor, disableStateColor;

	// 選択中のブロック情報
	int currentObj;

	// UIの表示状況
	const int uiVisibleFrame = 60;
	int uiVisibleCurrentFrame = uiVisibleFrame;

	// ブロックのホイール
	Image slotImages;
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

	int targerStage = 0;


	void Awake () {
		instance = this;
		cursorGuideObject = Instantiate(cursorGuideObject) as GameObject;
		cursorGuideMeshRenderer = cursorGuideObject.GetComponentInChildren<MeshRenderer>();
		cursorGuideMeshRenderer.material.color = normalStateColor;

		slotImages = transform.Find("BackGround").GetComponent<Image>();

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

		if(animationCurrentFrame++ <= kAnimationFrame){
			playAnimation();
			
		}else if(isContainLocalMap(mouseLocalPosition)){
			var uiPos = mouseWorldPosition;
			uiPos.x -= 1f;
			if(mouseLocalPosition.x < 3){
				// 画面端の時はUIを逆側にする
				uiPos.x += 4f;
			}
			if(mouseLocalPosition.z == 0){
				// Uiが上端ならすこしずらす
				uiPos.z -= 2f;
				
			}else if(mouseLocalPosition.z == 1){
				// Uiが上端ならすこしずらす
				uiPos.z -= 1f;

			}else if(mouseLocalPosition.z == 8){
				// uiが下端なら少しずらす
				uiPos.z += 0.5f;

			}else if(mouseLocalPosition.z == 9){
				// uiが下端なら少しずらす
				uiPos.z += 1f;
			}
			mouseLocalPosition.z.Log();

			wheelValue += Input.GetAxis("Mouse ScrollWheel");
			if(wheelValue > kWheelSensitivity){
				SoundManager.PlaySE(SoundManager.SE.move);
				wheelValue = 0f;
				wheelScrollImage(true);
				upToAnimation = true;
				animationCurrentFrame = 0;
				slotUiVisible(uiPos);

			}else if(wheelValue < -kWheelSensitivity){
				SoundManager.PlaySE(SoundManager.SE.move);
				wheelValue = 0f;
				wheelScrollImage();
				upToAnimation = false;
				animationCurrentFrame = 0;
				slotUiVisible(uiPos);

			}
		}

		// 選択していない時 カーソルを動かす
		if(isContainLocalMap(mouseLocalPosition)){
			cursorGuideObject.transform.position = mouseWorldPosition;
			Cursor.visible = false;
		}else{
			cursorGuideObject.transform.position = new Vector3(-50f, -50f, -50f);
			Cursor.visible = true;
		}

		// スロットそのもののアニメーション
		if(++uiVisibleCurrentFrame > uiVisibleFrame){
			var alpha = slotImages.color.a - (1f / uiVisibleFrame * 3);
			slotImages.color = new Color(1f, 1f, 1f, alpha);

			var length = images.Length;
			for (int i = 0; i < length; ++i){
				images[i].color = new Color(1f, 1f, 1f, alpha);
			}
		}
		
		// 選択中かのステータス達
		bool isSelectObj;
		bool isSelectUserPlaceBlock;
		GameObject selectBlock;
		{
			var objs = Physics.OverlapSphere(cursorGuideObject.transform.position, 0.05f, targetLayer);
			isSelectObj = objs.Length != 0;
			if(isSelectObj){
				selectBlock = objs[0].gameObject;
				isSelectUserPlaceBlock = selectBlock.layer == LayerMask.NameToLayer("Block");
				if(isSelectUserPlaceBlock){
					cursorGuideMeshRenderer.material.color = deleteStateColor;
				}else{
					cursorGuideMeshRenderer.material.color = disableStateColor;
				}
			}else{
				selectBlock = null;
				isSelectUserPlaceBlock = false;
				cursorGuideMeshRenderer.material.color = normalStateColor;
			}
		}

		
		if(isSelectObj){
			// 右クリックでブロック削除
			if(Input.GetMouseButton(1)){
				if(isSelectUserPlaceBlock){
					Destroy(selectBlock);
					SoundManager.PlaySE(SoundManager.SE.cancel);
				}
			}
		}

		if(Input.GetMouseButton(0)){
			// 左クリックでブロック設置
			var parentObj = GameObject.Find("Stage " + PuzzleManager.CurrentStage + "/Maps");
			if(!isSelectObj && isContainLocalMap(mouseLocalPosition)){
				var obj = Instantiate(heldObject[calcFixedIndex(currentObj)], mouseWorldPosition, heldObject[calcFixedIndex(currentObj)].transform.rotation) as GameObject;
				obj.transform.parent = parentObj.transform;
				var turn = obj.GetComponent<TurnBlockBase>();
				if(turn != null){
					turn.SetTurnBlockType(TurnBlockBase.BlockType.Place);
				}
				PuzzleManager.AddTotalBlockText(-1);
				SoundManager.PlaySE(SoundManager.SE.push);

			}
			transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, new Vector3(-50f, -50f, -50f));
		}
	}

	void slotUiVisible(Vector3 pos){
		transform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, pos);

		uiVisibleCurrentFrame = 0;

		slotImages.color = new Color(1f, 1f, 1f, 1f);
		var length = images.Length;
		for (int i = 0; i < length; ++i){
			images[i].color = new Color(1f, 1f, 1f, 1f);
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
		// スクロールアニメーション
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
