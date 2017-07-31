﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面を管理するスクリプト
/// </summary>
public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// 現状は何もないのですぐにロビーに
		GameManager.ChangeScene(GameState.Lobby);
	}
}
