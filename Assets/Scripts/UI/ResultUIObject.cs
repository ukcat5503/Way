using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultUIObject : MonoBehaviour {
	
	Image BackGround;
	Image BackGroundBlack;
	Text Result;

	Text PlaceBlock;
	Text DeathCount;
	Text Rank;

	Text PlaceBlockValue;
	Text DeathCountValue;
	Text RankValue;

	int count;
	int frame;
	int score;

	int[] scoreBorder;
	string[] scoreChar;
	[SerializeField]
	Color[] scoreColor;

	void Start () {

		BackGround = transform.Find("BackGround").GetComponent<Image>();
		BackGroundBlack = transform.Find("BackGroundBlack").GetComponent<Image>();
		Result = transform.Find("Result").GetComponent<Text>();
		
		PlaceBlock = transform.Find("PlaceBlock").GetComponent<Text>();
		DeathCount = transform.Find("DeathCount").GetComponent<Text>();
		Rank = transform.Find("Rank").GetComponent<Text>();

		PlaceBlockValue = transform.Find("PlaceBlockValue").GetComponent<Text>();
		DeathCountValue = transform.Find("DeathCountValue").GetComponent<Text>();
		RankValue = transform.Find("RankValue").GetComponent<Text>();


		BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, 0f);
		BackGroundBlack.color = new Color(BackGroundBlack.color.r, BackGroundBlack.color.g, BackGroundBlack.color.b, 0f);
		Result.color = new Color(Result.color.r, Result.color.g, Result.color.b, 0f);
		

		PlaceBlock.color = new Color(PlaceBlock.color.r, PlaceBlock.color.g, PlaceBlock.color.b, 0f);
		DeathCount.color = new Color(DeathCount.color.r, DeathCount.color.g, DeathCount.color.b, 0f);
		Rank.color = new Color(Rank.color.r, Rank.color.g, Rank.color.b, 0f);

		PlaceBlockValue.color = new Color(PlaceBlockValue.color.r, PlaceBlockValue.color.g, PlaceBlockValue.color.b, 0f);
		DeathCountValue.color = new Color(DeathCountValue.color.r, DeathCountValue.color.g, DeathCountValue.color.b, 0f);
		

		// スコアボーダー
		scoreBorder = new int[] { 1, 0, -30, -60, -100 };
		scoreChar = new string[] {"S", "A", "B", "C", "D", "E" };


		//スコア計算
		score = PuzzleManager.RequirementBlockQty - PuzzleManager.PlaceBlockQty - (PuzzleManager.DeathCount * 10);

		PlaceBlockValue.text = PuzzleManager.PlaceBlockQty.ToString() + "/" + PuzzleManager.RequirementBlockQty.ToString();
		DeathCountValue.text = PuzzleManager.DeathCount.ToString();

		RankValue.text = scoreChar[scoreChar.Length - 1];
		RankValue.color = new Color(scoreColor[scoreColor.Length - 1].r, scoreColor[scoreColor.Length - 1].g, scoreColor[scoreColor.Length - 1].b, 0f);
		for (int i = 0; i < scoreBorder.Length; ++i){
			if (scoreBorder[i] < score)
			{
				RankValue.text = scoreChar[i];
				RankValue.color = new Color(scoreColor[i].r, scoreColor[i].g, scoreColor[i].b, 0f);
				break;
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if(count == 0){
			BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, BackGround.color.a + 0.01f);
			if(BackGround.color.a > 1f){
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
			if(++frame > 90){
				++count;
			}
		}else if(count == 8){
			RankValue.color = new Color(RankValue.color.r, RankValue.color.g, RankValue.color.b, RankValue.color.a + 0.05f);
			if(RankValue.color.a > 1f){
				++count;
			}
		}else if(count == 9){
			if(Input.GetMouseButtonUp(0)){
				++count;
			}
		}else if(count == 10){
			BackGroundBlack.color = new Color(BackGroundBlack.color.r, BackGroundBlack.color.g, BackGroundBlack.color.b, BackGroundBlack.color.a + 0.01f);
			if(BackGroundBlack.color.a > 1f){
				SceneManager.LoadScene("Loading");
			}
		}
	}
}
