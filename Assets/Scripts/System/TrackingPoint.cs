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

    MovementDirection movementDirection;

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
    Vector3 prevPointRotate;

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
    int headGestureWaitFrame = 30;

    // 前回のジェスチャーから何フレーム経過したか
    int headGesturePassedFrame = 0;

    /// <summary>
    /// どのぐらいの距離を移動したら１回アクションしたとみなすか
    /// </summary>
    float headGestureDistance = 20f;

    /// <summary>
    /// 違う軸にどのぐらいまで移動しているなら許容されるか ±指定
    /// </summary>
    float headGestureDeviation = 20f;

    
    void Start(){
        DebugText.AddInfo("HeadState");
        DebugText.AddInfo("CheckAxis");
        DebugText.AddInfo("FrameCount");
        DebugText.AddInfo("HeadCount");
    }

    void Update(){

        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0f, true);

        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000.0f, TargetLayer)) {
            Indicator.IsWatchSomeone = true;

            if(Indicator.ProgressState){
                if(prevHit == hit.collider.gameObject.GetInstanceID()){
                Debug.Log("[" + ++count + "] selecting: " + hit.collider.gameObject.name);
                hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
                hit.collider.enabled = false;

                }else{
                    prevHit = hit.collider.gameObject.GetInstanceID();
                }
            }else{
                count = 0;
            }


            // hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
            // TODO hitからスクリプトを得たいけどgetcomponent使いたくない
            //  → 見続けたときだけ判定出すから負荷的にok?
            // CollisionObject a = hit.collider.gameObject;
        }else{
            Indicator.IsWatchSomeone = false;
        }
        checkHeadGesture();
    }


    void checkHeadGesture(){
        ++headGesturePassedFrame;
        if(headGestureCurrentCount == 0){
            if(transform.rotation.eulerAngles.x - prevFrameRotate.x > 0.15f){
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                movementDirection = MovementDirection.Right;
            }else if(transform.rotation.eulerAngles.x - prevFrameRotate.x < -0.15f){
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                movementDirection = MovementDirection.Left;
            }
        }else{
            if(headGesturePassedFrame >= headGestureWaitFrame){
                // 入力時間切れ
                headGestureCurrentCount = 0;
            }else{
                switch(movementDirection){
                    case MovementDirection.Left:
                        if(transform.rotation.eulerAngles.x - prevFrameRotate.x > 0.15f){
                            ++headGestureCurrentCount;
                            headGesturePassedFrame = 0;
                            movementDirection = MovementDirection.Right;
                        }
                    break;

                    case MovementDirection.Right:
                        if(transform.rotation.eulerAngles.x - prevFrameRotate.x < -0.15f){
                            ++headGestureCurrentCount;
                            headGesturePassedFrame = 0;
                            movementDirection = MovementDirection.Left;
                        }
                    break;
                }

            }
        }


        DebugText.UpdateInfo("HeadState-X", (transform.rotation.eulerAngles.x - prevFrameRotate.x).ToString("F6"));
        DebugText.UpdateInfo("HeadState-Y", (transform.rotation.eulerAngles.y - prevFrameRotate.y).ToString("F6"));
        DebugText.UpdateInfo("CheckAxis", movementDirection.ToString());
        DebugText.UpdateInfo("FrameCount", headGesturePassedFrame.ToString());
        DebugText.UpdateInfo("HeadCount", headGestureCurrentCount.ToString());

        prevFrameRotate = transform.rotation.eulerAngles;
    }
}