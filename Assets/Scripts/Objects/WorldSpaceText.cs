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

	int frame;

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
		if(++frame > 50){
			_text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - 0.02f);
		}
		if(_text.color.a < 0){
			Destroy(gameObject);
		}
		_rectTransform.position = new Vector3(_rectTransform.position.x, _rectTransform.position.y + 5f,
		_rectTransform.position.z);
	}
}
