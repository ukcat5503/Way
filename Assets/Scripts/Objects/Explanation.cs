using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explanation : MonoBehaviour {

	public enum ControlType{
		None,
		MouseOver,

		GetMouseButton_Left,
		GetMouseButtonDown_Left,
		GetMouseButtonUp_Left,
		GetMouseButton_Right,
		GetMouseButtonDown_Right,
		GetMouseButtonUp_Right,

		GetMouseWheelUp,
		GetMouseWheelDown,
		GetMouseWheelEither,
	}

	[SerializeField]
	GameObject voiceWindow;

	[SerializeField]
	string[] message;

	[SerializeField]
	Vector3[] objPos;

	[SerializeField]
	Vector2[] rectPos;

	[SerializeField]
	Vector2[] rectSize;

	[SerializeField]
	ControlType[] controlType;	

	List<GameObject> objs;
	List<Rect> rect;

	Transform canvas;

	int currentObj;

	// Use this for initialization
	void Start () {
		objs = new List<GameObject>();
		rect = new List<Rect>();
		canvas = GameObject.Find("Canvas").transform;

		var length = message.Length;
		for (int i = 0; i < length; ++i){
			objs.Add(Instantiate(voiceWindow, objPos[i], Quaternion.identity) as GameObject);
			Rect r = new Rect();
			r.x = rectPos[i].x;
			r.y = rectPos[i].y;
			r.width = rectSize[i].x;
			r.height = rectSize[i].y;
			rect.Add(r);
			objs[i].transform.parent = canvas;
			objs[i].name = message[i];
			objs[i].SetActive(false);
		}
		objs[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(currentObj < rect.Count){
			if(rect[currentObj].Contains(Input.mousePosition) && isControl(controlType[currentObj])){
				Destroy(objs[currentObj]);
				++currentObj;
				if(currentObj < rect.Count){
					objs[currentObj].SetActive(true);
				}else{
					Destroy(gameObject);
				}
			}
		}
	}

	bool isControl(ControlType type){
		switch (type){
			case ControlType.None:						return false;
			case ControlType.MouseOver:					return true;

			case ControlType.GetMouseButton_Left:		return Input.GetMouseButton(0);
			case ControlType.GetMouseButtonDown_Left:	return Input.GetMouseButtonDown(0);
			case ControlType.GetMouseButtonUp_Left:		return Input.GetMouseButtonDown(0);	

			case ControlType.GetMouseButton_Right:		return Input.GetMouseButton(1);
			case ControlType.GetMouseButtonDown_Right:	return Input.GetMouseButtonDown(1);
			case ControlType.GetMouseButtonUp_Right:	return Input.GetMouseButtonDown(1);

			case ControlType.GetMouseWheelUp:			return Input.GetAxis("Mouse ScrollWheel") > 0;
			case ControlType.GetMouseWheelDown:			return Input.GetAxis("Mouse ScrollWheel") < 0;
			case ControlType.GetMouseWheelEither:		return Input.GetAxis("Mouse ScrollWheel") != 0;
		

			default: ("操作タイプが不明です。常に既定値falseを使用します。").LogWarning();
														return false;
		}
	}
}
