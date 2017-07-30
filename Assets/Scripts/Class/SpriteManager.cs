﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]

/// <summary>
/// スプライトを表示するクラスです。
/// </summary>
public class SpriteManager : MonoBehaviour {

	/// <summary>
	/// SpriteRendererをキャッシュしておくため
	/// </summary>
	SpriteRenderer spriteRenderer;

	/// <summary>
	/// アニメーションに使用するスプライトの配列
	/// </summary>
	[SerializeField]
	Sprite[] animationSprite = new Sprite[0];

	/// <summary>
	/// アニメーション時、何フレームでスプライトを変更するか
	/// </summary>
	[SerializeField]
	int animationPerFrame = 0;

	/// <summary>
	/// オブジェクトが常にカメラの方向を向くか
	/// </summary>
	[SerializeField]
	bool useBillBoard;	

	/// <summary>
	/// アニメーション時、変更してから何フレーム目か。
	/// </summary>
	int currentFrameCount = 0;

	/// <summary>
	/// アニメーション時、現在何枚目の画像か
	/// </summary>
	int currentSprite = 0;

	// Use this for initialization
	void Start () {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

		if(animationSprite.Length == 1){
			spriteRenderer.sprite = animationSprite[0];

		}else if(animationPerFrame == 0 && animationSprite.Length > 1){
			"animationPerFrameが0かつ、アニメーションのスプライトが複数設定されています。\nanimationPerFrame = 1で実行されます。".LogWarning();
			animationPerFrame = 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// ２枚以上なければアニメーションの必要なし
		if(animationSprite.Length >= 2){
			// アニメーションすべきフレームになったらスプライトを変更
			if(++currentFrameCount >= animationPerFrame){
				if(++currentSprite >= animationSprite.Length){
					currentSprite = 0;
				}
				spriteRenderer.sprite = animationSprite[currentSprite];
				currentFrameCount = 0;
			}
		}

		if(useBillBoard){
			Vector3 p = Camera.main.transform.position;
			p.y = transform.position.y;
			transform.LookAt (p);
		}
	}
}