using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Select : MonoBehaviour {

	SpriteRenderer[] blocks = new SpriteRenderer[3];
	SpriteRenderer allow;


	int frame = 0;

	// Use this for initialization
	void Start () {
		blocks[0] = transform.Find("Block1").GetComponent<SpriteRenderer>();
		blocks[1] = transform.Find("Block2").GetComponent<SpriteRenderer>();
		blocks[2] = transform.Find("Block3").GetComponent<SpriteRenderer>();
		allow = transform.Find("AllowUp").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

		if(frame > 90 && frame <= 120){
			blocks[1].transform.position = new Vector3(blocks[1].transform.position.x, blocks[1].transform.position.y + (0.6f / 30f), blocks[1].transform.position.z);
			blocks[2].transform.position = new Vector3(blocks[2].transform.position.x, blocks[2].transform.position.y + (0.6f / 30f), blocks[2].transform.position.z);
			allow.transform.position = new Vector3(allow.transform.position.x, allow.transform.position.y + (1f / 30f), allow.transform.position.z);

			if(frame < 105){
				blocks[0].color = new Color(blocks[0].color.r, blocks[0].color.g, blocks[0].color.b, blocks[0].color.a - (1f / 15f));
				allow.color = new Color(allow.color.r, allow.color.g, allow.color.b, allow.color.a + (1f / 15f));
			}else if(frame == 105){
				blocks[0].color = new Color(blocks[0].color.r, blocks[0].color.g, blocks[0].color.b, 0);
				blocks[0].transform.position = new Vector3(blocks[0].transform.position.x, blocks[0].transform.position.y - (0.6f * 2f), blocks[0].transform.position.z);
			}else{
				blocks[0].color = new Color(blocks[0].color.r, blocks[0].color.g, blocks[0].color.b, blocks[0].color.a + (1f / 15f));
				allow.color = new Color(allow.color.r, allow.color.g, allow.color.b, allow.color.a - (1f / 15f));
			}

			if(frame == 120){
				allow.transform.position = new Vector3(allow.transform.position.x, allow.transform.position.y - 1f, allow.transform.position.z);
			}
		}

		if(frame > 210 && frame <= 240){
			blocks[2].transform.position = new Vector3(blocks[2].transform.position.x, blocks[2].transform.position.y + (0.6f / 30f), blocks[2].transform.position.z);
			blocks[0].transform.position = new Vector3(blocks[0].transform.position.x, blocks[0].transform.position.y + (0.6f / 30f), blocks[0].transform.position.z);
			allow.transform.position = new Vector3(allow.transform.position.x, allow.transform.position.y + (1f / 30f), allow.transform.position.z);

			if(frame < 225){
				blocks[1].color = new Color(blocks[1].color.r, blocks[1].color.g, blocks[1].color.b, blocks[1].color.a - (1f / 15f));
				allow.color = new Color(allow.color.r, allow.color.g, allow.color.b, allow.color.a + (1f / 15f));
			}else if(frame == 225){
				blocks[1].color = new Color(blocks[1].color.r, blocks[1].color.g, blocks[1].color.b, 0);
				blocks[1].transform.position = new Vector3(blocks[1].transform.position.x, blocks[1].transform.position.y - (0.6f * 2f), blocks[1].transform.position.z);
			}else{
				blocks[1].color = new Color(blocks[1].color.r, blocks[1].color.g, blocks[1].color.b, blocks[1].color.a + (1f / 15f));
				allow.color = new Color(allow.color.r, allow.color.g, allow.color.b, allow.color.a - (1f / 15f));
			}

			if(frame == 240){
				allow.transform.position = new Vector3(allow.transform.position.x, allow.transform.position.y - 1f, allow.transform.position.z);
			}
		}

		if(frame > 330 && frame <= 360){
			blocks[0].transform.position = new Vector3(blocks[0].transform.position.x, blocks[0].transform.position.y + (0.6f / 30f), blocks[0].transform.position.z);
			blocks[1].transform.position = new Vector3(blocks[1].transform.position.x, blocks[1].transform.position.y + (0.6f / 30f), blocks[1].transform.position.z);
			allow.transform.position = new Vector3(allow.transform.position.x, allow.transform.position.y + (1f / 30f), allow.transform.position.z);

			if(frame < 345){
				blocks[2].color = new Color(blocks[2].color.r, blocks[2].color.g, blocks[2].color.b, blocks[2].color.a - (1f / 15f));
				allow.color = new Color(allow.color.r, allow.color.g, allow.color.b, allow.color.a + (1f / 15f));
			}else if(frame == 345){
				blocks[2].color = new Color(blocks[2].color.r, blocks[2].color.g, blocks[2].color.b, 0);
				blocks[2].transform.position = new Vector3(blocks[2].transform.position.x, blocks[2].transform.position.y - (0.6f * 2f), blocks[2].transform.position.z);
			}else{
				blocks[2].color = new Color(blocks[2].color.r, blocks[2].color.g, blocks[2].color.b, blocks[2].color.a + (1f / 15f));
				allow.color = new Color(allow.color.r, allow.color.g, allow.color.b, allow.color.a - (1f / 15f));
			}

			if(frame == 360){
				allow.transform.position = new Vector3(allow.transform.position.x, allow.transform.position.y - 1f, allow.transform.position.z);
			}
		}
		
		++frame;

		if(frame > 360){
			// リセット処理
			blocks[0].color = new Color(blocks[0].color.r, blocks[0].color.g, blocks[0].color.b, 1f);
			blocks[1].color = new Color(blocks[1].color.r, blocks[1].color.g, blocks[1].color.b, 1f);
			blocks[2].color = new Color(blocks[2].color.r, blocks[2].color.g, blocks[2].color.b, 1f);

			allow.transform.position = new Vector3(1.1f, -0.2f, 0f);
			blocks[0].transform.position = new Vector3(-1f, 0.6f, 0f);
			blocks[1].transform.position = new Vector3(-1f, 0f, 0f);
			blocks[2].transform.position = new Vector3(-1f, -0.6f, 0f);
			frame = 0;
		}
	}
}
