using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLeftSprite : MonoBehaviour {

	[SerializeField]
	GameObject pivot;
	[SerializeField]
	GameObject upAllow;

	Vector3 startPosition;
	SpriteRenderer _spriteRenderer;

	int frame = 0;

	void Start(){
		startPosition = upAllow.transform.position;
		_spriteRenderer = upAllow.GetComponent<SpriteRenderer>();
	}

	const int kRotateFrame = 40;
	const float kRotateSpeed = 90f / kRotateFrame;
	const float kMovePosition = 0.6f;

	// Update is called once per frame
	void Update () {
		if(frame % 60 == 0){
			upAllow.transform.position = startPosition;
			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0f);
		}

		if(frame % 60 >= 0 && frame % 60 < kRotateFrame){
			pivot.transform.eulerAngles = new Vector3(pivot.transform.eulerAngles.x, pivot.transform.eulerAngles.y, pivot.transform.eulerAngles.z + kRotateSpeed);

			upAllow.transform.position = new Vector3(upAllow.transform.position.x, upAllow.transform.position.y, upAllow.transform.position.z + (kMovePosition / kRotateFrame));
		}

		if(frame % 60 >= 0 && frame % 60 < kRotateFrame / 4){
			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a + (1f / (float)kRotateFrame * (float)4));
		}else if(frame % 60 >= kRotateFrame - (kRotateFrame / 4) && frame % 60 < kRotateFrame){
			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a - (1f / (float)kRotateFrame * (float)4));
		}

		++frame;
	}
}
