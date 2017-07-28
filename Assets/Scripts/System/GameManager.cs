﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲームの現在の状態を表す列挙です。
/// class GameManagerで使用します。
/// </summary>
public enum GameState{
    Title,
    Lobby,
    ShootingGame,
    length
}

/// <summary>
/// ゲームを実行する際の根底部分に当たるクラスです。
/// このゲームはこのスクリプトから始まり、制御され、終了します。
/// ミニゲームはゲームごとに別途Managerが存在する。
/// </summary>
public class GameManager : MonoBehaviour {

	/// <summary>
	/// GameStateに定義されている、シーンの数。
	/// </summary>
	const int kSceneCount = (int)GameState.length;

    /// <summary>
    /// ゲームのステータスを指定します。
    /// </summary>
    /// <value>The state.</value>
    GameState state;
    // public getter
    public GameState State{
        get { return this.state; }
    }


	// Use this for initialization
	void Start () {
        "test".Log();
        gameObject.transform.position.Log();
        gameObject.transform.rotation.Log();


		// TODO デバッグ用 実行環境で取り除くこと
        state = GameState.Lobby;
        changeScene();
	}

    

	/// <summary>
	/// Settings from device.
    /// デバイスの種類によって異なる設定を一括で設定します。
	/// </summary>
    void settingFromDevice(){
#if UNITY_EDITOR


#elif UNITY_IPHONE
        // 画面の向きを右にホームボタンの横画面に変更する
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
    Debug.LogWarning("Any other platform");
#endif
	}

	
    void changeScene(){
        
    }
}