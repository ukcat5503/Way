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
	GameObject debugRect;
	[SerializeField]
	GameObject voiceWindow;

	[SerializeField]
	string[] message;

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
			objs.Add(Instantiate(voiceWindow, new Vector3(rectPos[i].x, rectPos[i].y, 0f), Quaternion.identity) as GameObject);
			Rect r = new Rect();
			r.x = rectPos[i].x;
			r.y = rectPos[i].y;
			r.width = rectSize[i].x;
			r.height = rectSize[i].y;
			rect.Add(r);
			objs[i].transform.parent = canvas;
			objs[i].SetActive(false);
			if(debugRect != null){
				var rt = Instantiate(debugRect).GetComponent<RectTransform>();
				rt.transform.position = new Vector3(r.x - r.width, r.y - (r.height / 2), 0f);
				rt.sizeDelta = new Vector2(r.width, r.height);
				rt.transform.parent = canvas;
			}
		}
		objs[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(currentObj < rect.Count){
			if(rect[currentObj].Contains(Input.mousePosition)){
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
			case ControlType.GetMouseWheelEither:			return Input.GetAxis("Mouse ScrollWheel") != 0;
		

			default: ("操作タイプが不明です。常に既定値falseを使用します。").LogWarning();
														return false;
		}
	}
}
