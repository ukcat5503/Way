﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [抽象クラス]
/// 主人公の目線との当たり判定を必要とするオブジェクトはすべてこのクラスを継承します。
/// 主人公と当たると.hit()が呼び出されます。
/// </summary>
public class CollisionObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.layer = 8;	// レイヤーをプレイヤーのレイキャストが有効なものにする
	}

	public void Hit(){
		Debug.Log("hoge");
	}
	
}
