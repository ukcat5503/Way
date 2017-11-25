using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockParent : MonoBehaviour {

	const float kDestroyHeight = -2f;

	Collider _collider;
	Collider childCollider;
	Rigidbody _rigidbody;

	[SerializeField]
	int frame = 0;

	// ドラッグの幻影系
	GameObject ghostObject;
	Vector3 ghostPos;

	// ドラッグ交換 スムース移動系
	int smoothMoveFrame = kSmoothMoveFrame + 1;
	Vector3 targetPos;
	Vector3 targetLocalPos;
	const int kSmoothMoveFrame = 10;
	const float	popupHeight = 0.3f;

	void Start(){
		_collider = GetComponent<Collider>();
		_rigidbody = GetComponent<Rigidbody>();
		childCollider = gameObject.transform.GetChild(0).GetComponent<Collider>();
	}

	void Update () {
		++frame;
		if (++smoothMoveFrame < kSmoothMoveFrame){
			var pos = targetLocalPos / kSmoothMoveFrame;
			if(smoothMoveFrame < kSmoothMoveFrame / 2){
				
				pos.y += popupHeight * (kSmoothMoveFrame / 2 - (smoothMoveFrame % kSmoothMoveFrame / 2));
			}else{
				pos.y -= popupHeight * (smoothMoveFrame % kSmoothMoveFrame / 2);
			}
			transform.position += pos;
		}else if (smoothMoveFrame == kSmoothMoveFrame){
			transform.position = targetPos;
			childCollider.enabled = true;
			childCollider.gameObject.transform.parent = GameObject.Find("Stage " + PuzzleManager.CurrentStage).transform;
			childCollider.gameObject.GetComponent<TurnBlockBase>().CanMoveFromMouse = true;
			childCollider.gameObject.GetComponent<TurnBlockBase>().enabled = true;
		}else if(transform.position.y < kDestroyHeight){
			// 流れきったら破棄
			// OnMouseUp();
			Destroy(gameObject);
		}else if(frame > 450 && ghostObject == null){
			Destroy(gameObject);
		}
	}
	/*
	void switchGhost(bool state){
		_collider.enabled = !state;
		_rigidbody.isKinematic = state;
		childCollider.enabled = !state;
		if(!state){
			float angle = 180f;
			var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
			_rigidbody.AddForce(shotVector * 100f);
		}
	}

	void OnMouseDrag(){
		switchGhost(true);

		Vector3 objectPointInScreen = Camera.main.WorldToScreenPoint(this.transform.position);

		Vector3 mousePointInScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, objectPointInScreen.z);
		
		Vector3 mousePointInWorld = Camera.main.ScreenToWorldPoint(mousePointInScreen);
		mousePointInWorld.y = this.transform.position.y;
		ghostPos = mousePointInWorld;

		if(ghostObject == null){
			var obj = gameObject.transform.GetChild(0).GetChild(0).gameObject;
			ghostObject = Instantiate(obj);
			ghostObject.transform.rotation = obj.transform.rotation;
			ghostObject.name = "Ghost Block";
			var m = ghostObject.GetComponent<MeshRenderer>();
			m.material.color = new Color(m.material.color.r, m.material.color.g, m.material.color.b, 0.3f);
		}
		ghostObject.transform.position = ghostPos;
    }

	void OnMouseUp() {
		if(ghostObject != null){
			var mapPos = new Vector3((int)(ghostObject.transform.position.x + 0.5f), 0, (10 - (int)ghostObject.transform.position.z));	// 相対位置
			var pos = new Vector3((int)(ghostObject.transform.position.x + 0.5f), 0, ((int)(ghostObject.transform.position.z + 0.5f )) + 0.25f);	// 絶対位置
			if(mapPos.x >= 0 && mapPos.x < PuzzleManager.kMapWidth && mapPos.z >= 0 && mapPos.z < PuzzleManager.kMapHeight){
				var objs = Physics.OverlapSphere(pos, 0.05f);
				if(objs.Length == 0 && GameObject.Find("Stage " + PuzzleManager.CurrentStage)){
					// (gameObject.name + " → " + pos).Log();
					// transform.position = objPos;
					// isAnimating = true;
					smoothMoveFrame = 0;
					targetPos = pos;
					//targetLocalPos = transform.InverseTransformDirection(targetPos - transform.position);
					targetLocalPos = targetPos - transform.position;
				}else{
				switchGhost(false);
				}
			}else{
				switchGhost(false);
			}
			Destroy(ghostObject);
			ghostObject = null;
			
		}
    }
	 */
}
