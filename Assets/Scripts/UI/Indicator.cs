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
	/// 何かに視線が合っているか
	/// </summary>	
	private static bool isWatchSomeone = false;
	/// <summary>
	/// 何かに視線が合っているか
	/// </summary>
	public static bool IsWatchSomeone{
		set{ isWatchSomeone = value; }
	}
	
	/// <summary>
	/// プログレスバーの現在の状況 0.0~1.0
	/// </summary>
    static float progressState = 0f;
	/// <summary>
	/// プログレスバーがたまりきっている(選択確定)状態か
	/// </summary>
	/// <returns>trueで選択中</returns>
    public static bool ProgressState{
        get { return Indicator.progressState >= 1f; }
    }
	/// <summary>
	/// プログレスバーが１フレームでどのぐらい増加するか
	/// </summary>
	const float kAddToProgressBar = 0.02f;
	/// <summary>
	/// プログレスバーが１フレームでどのぐらい減少するか
	/// </summary>
	const float kRemoveToProgressBar = 0.05f;

	/// <summary>
	/// プログレスバーで操作するimageコンポーネント
	/// </summary>
	Image childImage;

	void Awake(){
		// 既にインジケーターが生成されていればログを吐いてこのオブジェクトは消える
		/* TODO 未実装
		if(GameObject.Find("Indicator").GetComponent<Indicator>() != null){
			"このオブジェクトは既に存在しています。".Log();
			Destroy(gameObject);
		}
		*/
	}

	void Start(){
		// 子要素のimageコンポーネントを取得しておく
		childImage = gameObject.transform.FindChild("Top").GetComponent<Image>();
	}

	void Update(){
		// ゲージの変動
		if(isWatchSomeone){
			progressState += kAddToProgressBar;
			if(progressState > 1f){
				progressState = 1f;
			}
		}else{
			progressState -= kRemoveToProgressBar;
			if(progressState < 0f){
				progressState = 0f;
			}
		}

		// 画像の表示を変更する
		childImage.fillAmount = progressState;
	}
}