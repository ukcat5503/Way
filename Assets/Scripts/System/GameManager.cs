﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲームの現在の状態を表す列挙です。
/// class GameManagerで使用します。
/// </summary>
enum GameState{
    Title,
    Lobby,
    ShootingGame,
    length
}

/// <summary>
/// ゲームを実行する際の根底部分に当たるクラスです。
/// このゲームはこのスクリプトから始まり、制御され、終了します。
/// </summary>
public class GameManager : MonoBehaviour {

    private const int kSceneCount = (int)GameState.length;

    private GameState state { get; set; }

	// Use this for initialization
	void Start () {
		// TODO デバッグ用

#if UNITY_EDITOR
		

#elif UNITY_IPHONE
        switch (Screen.orientation)
        {
            // 縦画面のとき
            case ScreenOrientation.Portrait:
                // 左回転して左向きの横画面にする
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
            // 上下反転の縦画面のとき
            case ScreenOrientation.PortraitUpsideDown:
                // 右回転して左向きの横画面にする
                Screen.orientation = ScreenOrientation.LandscapeRight;
                break;
        }
#else
    Debug.Log("Any other platform");
#endif

        state = GameState.Lobby;
        changeScene();
	}
	
    void changeScene(){
        
    }
}