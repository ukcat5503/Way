using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour {
	
	SpriteRenderer[] _spriteRenderers;

	bool isClearing = false;

	// Use this for initialization
	void Start () {
		_spriteRenderers = new SpriteRenderer[2];
		var length = _spriteRenderers.Length;
		for (int i = 0; i < length; ++i){
			_spriteRenderers[i] = GameObject.Find("bg_" + i).GetComponent<SpriteRenderer>();
		}
	}
	
	void Update () {
		var length = _spriteRenderers.Length;
		for (int i = 0; i < length; ++i){
			_spriteRenderers[i].color = new Color(_spriteRenderers[i].color.r, _spriteRenderers[i].color.g, _spriteRenderers[i].color.b, _spriteRenderers[i].color.a + (isClearing ? -0.02f: +0.02f));
		}

		if(!isClearing && _spriteRenderers[0].color.a >= 1){
			isClearing = true;
		}else if(isClearing && _spriteRenderers[0].color.a <= 0){
			isClearing = false;
		}
	}
}
