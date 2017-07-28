﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面中央に表示されるインジケーター(マーカーとも言うかもしれない)を
/// 制御するクラス。
/// 基本的には１つしか存在してはならないものである。
/// </summary>
public class Indicator : MonoBehaviour {

	/// <summary>
	/// プログレスバーの現在の値
	/// </summary>
	
	public static bool IsWatchSomeone = false;
	static float ProgressState = 0f;
	const float kAddToProgressBar = 0.02f;
	const float kRemoveToProgressBar = 0.05f;

	Image childImage;

	void Awake(){
		// 既にインジケーターが生成されていればログを吐いてこのオブジェクトは消える
		if(GameObject.Find("Indicator").GetComponent<Indicator>() != null){
			"このオブジェクトは既に存在しています。".Log();
			// Destroy(gameObject);
		}
	}

	void Start(){
		childImage = gameObject.transform.FindChild("Top").GetComponent<Image>();
	}

	void Update(){
		// ゲージの変動
		if(IsWatchSomeone){
			ProgressState += kAddToProgressBar;
			if(ProgressState > 1f){
				ProgressState = 1f;
			}
		}else{
			ProgressState -= kRemoveToProgressBar;
			if(ProgressState < 0f){
				ProgressState = 0f;
			}
		}

		childImage.fillAmount = ProgressState;
	}
}
