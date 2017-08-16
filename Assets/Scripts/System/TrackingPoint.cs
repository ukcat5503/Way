﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラにアタッチするスクリプト。
/// Rayを飛ばし、何と衝突しているかを判定できます。
/// </summary>
public class TrackingPoint : MonoBehaviour {

    /// <summary>
    /// マウスの向いている方向を表すRay
    /// </summary>
    Ray ray;


    /// <summary>
    /// 選択対象のレイヤー
    /// </summary>
    public LayerMask TargetLayer;

    void Update(){

        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0f, true);
        RaycastHit hit;
        if(isDecide() && Physics.Raycast(ray, out hit, 1000.0f, TargetLayer)) {
            hit.collider.SendMessageUpwards("HitRayFromPlayer");
        }
    }


    bool isDecide(){
#if UNITY_EDITOR
        return Input.GetKeyUp(KeyCode.Return);
#elif UNITY_IOS
        "未設定".LogWarning();
        return false;
#else
        return false;
#endif
    }
}