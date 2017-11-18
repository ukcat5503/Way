using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceText : MonoBehaviour {

	Text _text;
	RectTransform _rectTransform;
	public string Text;
	public Vector3 WorldPosition;

	static GameObject canvas;

	void Start () {
		if(canvas == null){
			canvas = GameObject.Find("Canvas");
		}
		transform.SetParent(canvas.transform);
		
		_text = GetComponent<Text>();
		_rectTransform = GetComponent<RectTransform>();

		_text.text = Text;
		_rectTransform.position = RectTransformUtility.WorldToScreenPoint (Camera.main, WorldPosition);
	}
	
	void Update () {
		_rectTransform.position = new Vector3(_rectTransform.position.x, _rectTransform.position.y + 5f,
		_rectTransform.position.z);
	}
}
