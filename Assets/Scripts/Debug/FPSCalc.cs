﻿using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// デバッグ用 FSPを表示するだけ
/// </summary>
public class FPSCalc : MonoBehaviour
{
    private int _frameCount;
    private float _prevTime;

    // uGUIのテキスト格納用 (使わないなら無しでok)
    Text text;
    // FPSをDebugLogで出力するかどうか。 使うとログが荒れるけど正確な値が見られる。
    public bool UseFpsToDebugLog;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        _frameCount = 0;
        _prevTime = 0.0f;

		text = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        ++_frameCount;
        float time = Time.realtimeSinceStartup - _prevTime;

        // 0.5秒ごとに変更
        if (time >= 0.5f)
        {
            if (UseFpsToDebugLog)
                (_frameCount / time + "fps").Log();

            if (text)
                //少数2位で四捨五入
                text.text = Math.Round(_frameCount / time, 1, MidpointRounding.AwayFromZero) + "fps";

            //フレームカウントを戻して秒数を更新
            _frameCount = 0;
            _prevTime = Time.realtimeSinceStartup;
        }
    }
}