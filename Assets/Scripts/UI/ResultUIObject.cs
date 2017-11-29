using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIObject : MonoBehaviour {
	
	Image backGround;
	Text Result;

	Text PlaceBlock;
	Text DeathCount;
	Text Rank;

	Text PlaceBlockValue;
	Text DeathCountValue;
	Text RankValue;

	int count;
	int frame;

	void Start () {

		backGround = transform.Find("BackGround").GetComponent<Image>();
		Result = transform.Find("Result").GetComponent<Text>();
		
		PlaceBlock = transform.Find("PlaceBlock").GetComponent<Text>();
		DeathCount = transform.Find("DeathCount").GetComponent<Text>();
		Rank = transform.Find("Rank").GetComponent<Text>();

		PlaceBlockValue = transform.Find("PlaceBlockValue").GetComponent<Text>();
		DeathCountValue = transform.Find("DeathCountValue").GetComponent<Text>();
		RankValue = transform.Find("RankValue").GetComponent<Text>();


		backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 0f);
		Result.color = new Color(Result.color.r, Result.color.g, Result.color.b, 0f);

		PlaceBlock.color = new Color(PlaceBlock.color.r, PlaceBlock.color.g, PlaceBlock.color.b, 0f);
		DeathCount.color = new Color(DeathCount.color.r, DeathCount.color.g, DeathCount.color.b, 0f);
		Rank.color = new Color(Rank.color.r, Rank.color.g, Rank.color.b, 0f);

		PlaceBlockValue.color = new Color(PlaceBlockValue.color.r, PlaceBlockValue.color.g, PlaceBlockValue.color.b, 0f);
		DeathCountValue.color = new Color(DeathCountValue.color.r, DeathCountValue.color.g, DeathCountValue.color.b, 0f);
		RankValue.color = new Color(RankValue.color.r, RankValue.color.g, RankValue.color.b, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(count == 0){
			backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, backGround.color.a + 0.01f);
			if(backGround.color.a > 1f){
				++count;
			}
		}else if(count == 1){
			Result.color = new Color(Result.color.r, Result.color.g, Result.color.b, Result.color.a + 0.08f);
			if(Result.color.a > 1f){
				++count;
			}
		}else if(count == 2){
			PlaceBlock.color = new Color(PlaceBlock.color.r, PlaceBlock.color.g, PlaceBlock.color.b, PlaceBlock.color.a + 0.05f);
			if(PlaceBlock.color.a > 1f){
				++count;
			}
		}else if(count == 3){
			DeathCount.color = new Color(DeathCount.color.r, DeathCount.color.g, DeathCount.color.b, DeathCount.color.a + 0.05f);
			if(DeathCount.color.a > 1f){
				++count;
			}
		}else if(count == 4){
			Rank.color = new Color(Rank.color.r, Rank.color.g, Rank.color.b, Rank.color.a + 0.05f);
			if(Rank.color.a > 1f){
				++count;
			}
		}else if(count == 5){
			PlaceBlockValue.color = new Color(PlaceBlockValue.color.r, PlaceBlockValue.color.g, PlaceBlockValue.color.b, PlaceBlockValue.color.a + 0.05f);
			if(PlaceBlockValue.color.a > 1f){
				++count;
			}
		}else if(count == 6){
			DeathCountValue.color = new Color(DeathCountValue.color.r, DeathCountValue.color.g, DeathCountValue.color.b, DeathCountValue.color.a + 0.05f);
			if(DeathCountValue.color.a > 1f){
				++count;
			}
		}else if(count == 7){
			if(++frame > 120){
				++count;
			}
		}else if(count == 8){
			RankValue.color = new Color(RankValue.color.r, RankValue.color.g, RankValue.color.b, RankValue.color.a + 0.05f);
			if(RankValue.color.a > 1f){
				++count;
			}
		}
	}
}
