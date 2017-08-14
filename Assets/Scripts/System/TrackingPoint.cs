﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラにアタッチするスクリプト。
/// Rayを飛ばし、何と衝突しているかを判定できます。
/// </summary>
public class TrackingPoint : MonoBehaviour {

    enum MovementDirection{
        Up,
        Down,
        Left,
        Right
    }

    enum JudgeType{
        None,
        Yes,
        No
    }

    MovementDirection movementDirection;

    JudgeType judgeType;

    /// <summary>
    /// マウスの向いている方向を表すRay
    /// </summary>
    Ray ray;

    /// <summary>
    /// 前フレームで選択していたオブジェクトのユニークID
    /// </summary>
    int prevHit = 0;
    int count = 0;

    /// <summary>
    /// 選択対象のレイヤー
    /// </summary>
    public LayerMask TargetLayer;

    /// <summary>
    /// 前回の移動地点の場所を記録しておくため
    /// </summary>
    Vector3 prevPointRelativeRotate;

    /// <summary>
    /// 前フレームの場所を記録しておくため
    /// </summary>
    Vector3 prevFrameRotate;

    /// <summary>
    /// 何回やったらヘッドジェスチャーと認識するか
    /// </summary>
    int headGestureCount = 4;

    /// <summary>
    /// 現在何回ジェスチャーをしたか。
    /// </summary>
    int headGestureCurrentCount = 0;

    /// <summary>
    /// 一回のアクションは何秒まで入力許容されるか。
    /// </summary>
    int headGestureWaitFrame = 90;

    // 前回のジェスチャーから何フレーム経過したか
    int headGesturePassedFrame = 0;

    /// <summary>
    /// どのぐらいの距離を移動したら１回アクションしたとみなすか
    /// </summary>
    float headGestureDistance = 3f;

    /// <summary>
    /// 違う軸にどのぐらいまで移動しているなら許容されるか ±指定
    /// </summary>
    float headGestureDeviation = 10f;

    
    void Update(){

        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0f, true);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000.0f, TargetLayer)) {
            Indicator.IsWatchSomeone = true;

            if(Indicator.ProgressState){
                if(prevHit == hit.collider.gameObject.GetInstanceID()){
                Debug.Log("[" + ++count + "] selecting: " + hit.collider.gameObject.name);
                // この方法は結構重いので一発で無効にする 改善できたら改善したい
                hit.collider.SendMessageUpwards("HitRayFromPlayer");
                hit.collider.enabled = false;

                }else{
                    prevHit = hit.collider.gameObject.GetInstanceID();
                }
            }else{
                count = 0;
            }
            
        }else{
            Indicator.IsWatchSomeone = false;
        }
    }
}