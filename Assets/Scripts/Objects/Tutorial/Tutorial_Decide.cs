using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Decide : MonoBehaviour {

	SpriteRenderer[] blocks = new SpriteRenderer[3];
	SpriteRenderer mouse;
	SpriteRenderer leftButton;
	SpriteRenderer cursor;


	int frame = 0;

	// Use this for initialization
	void Start () {
		blocks[0] = transform.Find("Block1").GetComponent<SpriteRenderer>();
		blocks[1] = transform.Find("Block2").GetComponent<SpriteRenderer>();
		blocks[2] = transform.Find("Block3").GetComponent<SpriteRenderer>();

		mouse = transform.Find("Mouse").GetComponent<SpriteRenderer>();
		leftButton = transform.Find("Mouse/LeftButton").GetComponent<SpriteRenderer>();
		cursor = transform.Find("Cursor").GetComponent<SpriteRenderer>();

		initialize();
	}
	
	// Update is called once per frame
	void Update () {
		// マウスカーソル
		if(frame > 30 && frame <= 60){
			mouse.transform.position = new Vector3(mouse.transform.position.x + (0.75f / 30f), mouse.transform.position.y, mouse.transform.position.z);
		}
		if(frame > 150 && frame <= 180){
			mouse.transform.position = new Vector3(mouse.transform.position.x + (0.75f / 30f), mouse.transform.position.y, mouse.transform.position.z);
		}
		if(frame > 270 && frame <= 300){
			mouse.transform.position = new Vector3(mouse.transform.position.x + (0.75f / 30f), mouse.transform.position.y, mouse.transform.position.z);
		}
		if(frame > 390 && frame <= 420){
			mouse.transform.position = new Vector3(mouse.transform.position.x + (0.75f / 30f), mouse.transform.position.y, mouse.transform.position.z);
		}

		// 半透明カーソル移動
		if(frame == 45){
			cursor.transform.position = new Vector3(cursor.transform.position.x + 0.75f, cursor.transform.position.y, cursor.transform.position.z);
		}
		if(frame == 165){
			cursor.transform.position = new Vector3(cursor.transform.position.x + 0.75f, cursor.transform.position.y, cursor.transform.position.z);
		}
		if(frame == 285){
			cursor.transform.position = new Vector3(cursor.transform.position.x + 0.75f, cursor.transform.position.y, cursor.transform.position.z);
		}
		if(frame == 415){
			cursor.transform.position = new Vector3(cursor.transform.position.x + 0.75f, cursor.transform.position.y, cursor.transform.position.z);
		}

		// ブロック表示
		if(frame == 60){
			blocks[0].enabled = true;
			leftButton.enabled = true;
		}
		if(frame == 65){
			leftButton.enabled = false;
		}

		if(frame == 180){
			blocks[1].enabled = true;
			leftButton.enabled = true;
		}
		if(frame == 185){
			leftButton.enabled = false;
		}

		if(frame == 300){
			blocks[2].enabled = true;
			leftButton.enabled = true;
		}
		if(frame == 305){
			leftButton.enabled = false;
		}

		++frame;
		if(frame > 500){
			// 初期化
			initialize();
		}
	}

	void initialize(){
		blocks[0].enabled = false;
		blocks[1].enabled = false;
		blocks[2].enabled = false;
		leftButton.enabled = false;

		mouse.transform.position = new Vector3(-1.5f, -0.5f, 0f);
		cursor.transform.position = new Vector3(-1.5f, 0.69f, 0f);
		frame = 0;
	}
}
