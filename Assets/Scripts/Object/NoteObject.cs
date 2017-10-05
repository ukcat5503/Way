using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : SpriteBase {

	[SerializeField]
	SoundManager.SE se;

	SpriteRenderer sr;

	int frame = 0;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;
	}
	
	void Update () {
		++frame;
		base.Update();
	}

	void OnTriggerEnter(Collider other){
		if(frame > 120){
			sr.enabled = true;
			frame = 0;
			SoundManager.PlaySE(se);

		}
	}
}
