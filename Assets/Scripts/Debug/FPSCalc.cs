﻿using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// デバッグ用 FSPを表示するだけ
/// </summary>
public class FPSCalc : MonoBehaviour
{
    private int frameCount;
    private float prevTime;

    // FPSをDebugLogで出力するかどうか。 使うとログが荒れるけど正確な値が見られる。
    public bool UseFpsToDebugLog;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        frameCount = 0;
        prevTime = 0.0f;
    }

    void Update()
    {
        ++frameCount;
        float time = Time.realtimeSinceStartup - prevTime;

        // 変更頻度
        if (time >= 0.25f)
        {
            if (UseFpsToDebugLog)
                (frameCount / time + "fps").Log();

            DebugText.UpdateInfo("FPS", (frameCount / time + "fps"));

            //フレームカウントを戻して秒数を更新
            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }
}