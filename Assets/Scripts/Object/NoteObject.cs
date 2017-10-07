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
		base.Update();

		++frame;
	}

	void OnTriggerEnter(Collider other){
		/*
		if(frame > 120){
			sr.enabled = true;
			frame = 0;
			SoundManager.PlaySE(se);

		}
		*/
		if(sr.enabled == false){
			sr.enabled = true;
			SoundManager.PlaySE(se);
		}

	}
}
