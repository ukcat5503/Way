﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// ゲームの現在の状態を表す列挙です。
/// class GameManagerで使用します。
/// ゲーム内に存在しているシーンと同じ名前をつけること。
/// </summary>
public enum GameState{
    Title,
    length
}

/// <summary>
/// ゲームを実行する際の根底部分に当たるクラスです。
/// このゲームはこのスクリプトから始まり、制御され、終了します。
/// ミニゲームはゲームごとに別途Managerが存在する。
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// 自身のシーン
    /// </summary>
    static Scene myScene;

	/// <summary>
	/// GameStateに定義されている、シーンの数。
	/// </summary>
	const int kSceneCount = (int)GameState.length;

    /// <summary>
    /// ゲームのステータスを指定します。
    /// </summary>
    /// <value>The state.</value>
    static GameState state;

    void Awake(){
        Application.targetFrameRate = 60;
    }

	// Use this for initialization
	void Start () {
        myScene = SceneManager.GetActiveScene();

        "test".Log();
        gameObject.transform.position.Log();
        gameObject.transform.rotation.Log();

        // タイトル画面を追加する
        changeScene(GameState.Title);
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

    /// <summary>
    /// シーンを指定したものに変更します
    /// </summary>
    static void changeScene(GameState changeState){
        // GameManager以外のシーンをすべて削除する
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.name != myScene.name){
                SceneManager.UnloadSceneAsync(scene); 
            }
        }

        // シーンを追加する
        state = changeState;
        SceneManager.LoadScene(changeState.ToString(), LoadSceneMode.Additive);
    }
}