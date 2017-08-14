﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [抽象クラス]
/// 主人公の目線との当たり判定を必要とするオブジェクトはすべてこのクラスを継承します。
/// 主人公と当たると.hit()が呼び出されます。
/// </summary>
abstract public class ModelBase : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		gameObject.layer = 8;	// レイヤーをプレイヤーのレイキャストが有効なものにする
	}

	public void HitRayFromPlayer(){
		"HitRayFromPlayerをoverrideして下さい。不要な場合は明示的に不要として下さい。".LogError();
	}
}
