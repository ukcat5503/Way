﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : SpriteBase {

	[SerializeField]
	SoundManager.SE se;

	int frame = 0;

	new void Start () {
		base.Start();
		spriteRenderer.enabled = false;
	}
	
	new void Update () {
		base.Update();

		++frame;
	}

	void OnTriggerEnter(Collider other){
		// if(spriteRenderer.enabled == false){
			spriteRenderer.enabled = true;
			SoundManager.PlaySE(se);
		// }

	}
}