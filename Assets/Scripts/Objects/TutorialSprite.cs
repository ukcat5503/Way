using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSprite : SpriteBase {
	[SerializeField]
	int visibleFrame, invisibleFrame, animateFrame;

	bool visible;

	[SerializeField]
	int frame;


	new void Start () {
		base.Start();

		var color = spriteRenderer.color;
		color.a = 0f;
		spriteRenderer.color = color;
	}
	
	new void Update () {
		base.Update();

		++frame;

		if(visibleFrame < frame && !visible){
			var color = spriteRenderer.color;
			color.a = spriteRenderer.color.a + (1f / animateFrame);
			spriteRenderer.color = color;
			if(color.a > 1f){
				visible = true;
			}
		}
		if(visible && invisibleFrame != 0 && invisibleFrame < frame && spriteRenderer.color.a > 0f){
			var color = spriteRenderer.color;
			color.a = spriteRenderer.color.a - (1f / animateFrame);
			spriteRenderer.color = color;
		}
	}
}
