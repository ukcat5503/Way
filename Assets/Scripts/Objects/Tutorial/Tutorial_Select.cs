using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Select : MonoBehaviour {

	SpriteRenderer[] Blocks = new SpriteRenderer[3];


	int frame = 0;

	// Use this for initialization
	void Start () {
		Blocks[0] = transform.Find("Block1").GetComponent<SpriteRenderer>();
		Blocks[1] = transform.Find("Block2").GetComponent<SpriteRenderer>();
		Blocks[2] = transform.Find("Block3").GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		if(frame == 0){
			// 初期化
			
		}

		if(frame > 90 && frame <= 120){
			Blocks[1].transform.position = new Vector3(Blocks[1].transform.position.x, Blocks[1].transform.position.y + (0.6f / 30f), Blocks[1].transform.position.z);
			Blocks[2].transform.position = new Vector3(Blocks[2].transform.position.x, Blocks[2].transform.position.y + (0.6f / 30f), Blocks[2].transform.position.z);

			if(frame < 105){
				Blocks[0].color = new Color(Blocks[0].color.r, Blocks[0].color.g, Blocks[0].color.b, Blocks[0].color.a - (1f / 15f));
			}else if(frame == 105){
				Blocks[0].color = new Color(Blocks[0].color.r, Blocks[0].color.g, Blocks[0].color.b, 0);
				Blocks[0].transform.position = new Vector3(Blocks[0].transform.position.x, Blocks[0].transform.position.y - (0.6f * 2f), Blocks[0].transform.position.z);
			}else{
				Blocks[0].color = new Color(Blocks[0].color.r, Blocks[0].color.g, Blocks[0].color.b, Blocks[0].color.a + (1f / 15f));
			}
		}

		if(frame > 210 && frame <= 240){
			Blocks[2].transform.position = new Vector3(Blocks[2].transform.position.x, Blocks[2].transform.position.y + (0.6f / 30f), Blocks[2].transform.position.z);
			Blocks[0].transform.position = new Vector3(Blocks[0].transform.position.x, Blocks[0].transform.position.y + (0.6f / 30f), Blocks[0].transform.position.z);

			if(frame < 225){
				Blocks[1].color = new Color(Blocks[1].color.r, Blocks[1].color.g, Blocks[1].color.b, Blocks[1].color.a - (1f / 15f));
			}else if(frame == 225){
				Blocks[1].color = new Color(Blocks[1].color.r, Blocks[1].color.g, Blocks[1].color.b, 0);
				Blocks[1].transform.position = new Vector3(Blocks[1].transform.position.x, Blocks[1].transform.position.y - (0.6f * 2f), Blocks[1].transform.position.z);
			}else{
				Blocks[1].color = new Color(Blocks[1].color.r, Blocks[1].color.g, Blocks[1].color.b, Blocks[1].color.a + (1f / 15f));
			}
		}

		if(frame > 330 && frame <= 360){
			Blocks[0].transform.position = new Vector3(Blocks[0].transform.position.x, Blocks[0].transform.position.y + (0.6f / 30f), Blocks[0].transform.position.z);
			Blocks[1].transform.position = new Vector3(Blocks[1].transform.position.x, Blocks[1].transform.position.y + (0.6f / 30f), Blocks[1].transform.position.z);

			if(frame < 345){
				Blocks[2].color = new Color(Blocks[2].color.r, Blocks[2].color.g, Blocks[2].color.b, Blocks[2].color.a - (1f / 15f));
			}else if(frame == 345){
				Blocks[2].color = new Color(Blocks[2].color.r, Blocks[2].color.g, Blocks[2].color.b, 0);
				Blocks[2].transform.position = new Vector3(Blocks[2].transform.position.x, Blocks[2].transform.position.y - (0.6f * 2f), Blocks[2].transform.position.z);
			}else{
				Blocks[2].color = new Color(Blocks[2].color.r, Blocks[2].color.g, Blocks[2].color.b, Blocks[2].color.a + (1f / 15f));
			}
		}
		
		++frame;

		if(frame > 360){
			// リセット処理
			Blocks[0].color = new Color(Blocks[0].color.r, Blocks[0].color.g, Blocks[0].color.b, 1f);
			Blocks[1].color = new Color(Blocks[1].color.r, Blocks[1].color.g, Blocks[1].color.b, 1f);
			Blocks[2].color = new Color(Blocks[2].color.r, Blocks[2].color.g, Blocks[2].color.b, 1f);

			Blocks[0].transform.position = new Vector3(-1f, 0.6f, 0f);
			Blocks[1].transform.position = new Vector3(-1f, 0f, 0f);
			Blocks[2].transform.position = new Vector3(-1f, -0.6f, 0f);
			frame = 0;
		}
	}
}
