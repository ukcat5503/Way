/*
	ロゴ表示用スクリプト
	
	製作者：		小川晴生
	使用方法：	①新しいシーンにMainCameraだけがある状態にしてください
				②Hierarchy上にて新規でゲームオブジェクトを作り、このスクリプトとSpriteRendererをアタッチしてください。
				③使用したいロゴイメージをSpriteRendererのSpriteの登録してください。
				④LogoScriptのNextSceneNameに次に遷移するシーン名を入力してください。
	作成日：		２０１７年１２月１日
	バージョン：	1.1
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class LogoScript : MonoBehaviour {

	//シングルトン
	Camera cameraComponent;
	SpriteRenderer spriteRenderer;

	Color color = new Color( 0f, 0f, 0f, 0f );	//カラー
	float deltaTime = 0f;	//経過時間

	public string nextSceneName = "Stage1";	//次のシーン名

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		cameraComponent = FindObjectOfType<Camera> ();

		cameraComponent.clearFlags = CameraClearFlags.SolidColor;
		cameraComponent.transform.position = new Vector3 (0f, 0f, -10);
		cameraComponent.orthographic = true;

		float aspectScale = transform.localScale.x;	//アスペクト比に使うscale情報
		Vector2 spriteSize = spriteRenderer.bounds.size;	//画像のサイズ
		float widthByHight = (float)Screen.width / (float)Screen.height;	//縦横比

		if (Screen.width < spriteSize.x * 100) {
			aspectScale *= widthByHight * 0.6f;
		}
		transform.localScale = new Vector3 (aspectScale, aspectScale, 1f);

		spriteRenderer.color = color;
		cameraComponent.backgroundColor = color;
	}
	
	void Update () {
		deltaTime += Time.deltaTime;

		if (deltaTime < 1f) {
			color.a = color.b = color.g = color.r = deltaTime / 1f;
			UpdateColor ();
		} else if(deltaTime > 4f){
			color.a = color.b = color.g = color.r = 1f - (deltaTime - 4f) / 1f;
			UpdateColor ();
			if (deltaTime > 5f) {
				SceneManager.LoadScene (nextSceneName);
			}
		}
	}
		
	void UpdateColor(){
		spriteRenderer.color = color;
		cameraComponent.backgroundColor = color;
	}
}