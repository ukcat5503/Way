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
            if(transform.rotation.eulerAngles.x - prevFrameRotate.x < -0.15f){
                movementDirection = MovementDirection.Up;
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                prevPointRotate = transform.rotation.eulerAngles;

            }else if(transform.rotation.eulerAngles.x - prevFrameRotate.x > 0.15f){
                movementDirection = MovementDirection.Down;
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                prevPointRotate = transform.rotation.eulerAngles;

            }else if(transform.rotation.eulerAngles.y - prevFrameRotate.y > 0.15f){
                movementDirection = MovementDirection.Right;
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                prevPointRotate = transform.rotation.eulerAngles;

            }else if(transform.rotation.eulerAngles.y - prevFrameRotate.y < -0.15f){
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                movementDirection = MovementDirection.Left;
                prevPointRotate = transform.rotation.eulerAngles;

            }
        }else{
            if(headGesturePassedFrame >= headGestureWaitFrame){
                // 入力時間切れ
                headGestureCurrentCount = 0;

            }else{
                switch(movementDirection){
                    case MovementDirection.Left:
                        if(transform.rotation.eulerAngles.y - prevFrameRotate.y > headGestureDeviation && 
                        ((prevPointRotate.x + headGestureDeviation) > transform.rotation.eulerAngles.x || 
                        (prevPointRotate.x - headGestureDeviation) < transform.rotation.eulerAngles.x)){
                            ++headGestureCurrentCount;
                            headGesturePassedFrame = 0;
                            movementDirection = MovementDirection.Right;
                        }
                    break;

                    case MovementDirection.Right:
                        if(transform.rotation.eulerAngles.y - prevFrameRotate.y < -headGestureDeviation){
                            ++headGestureCurrentCount;
                            headGesturePassedFrame = 0;
                            movementDirection = MovementDirection.Left;
                        }
                    break;

                    case MovementDirection.Down:
                        if(transform.rotation.eulerAngles.x - prevFrameRotate.x < -headGestureDeviation){
                            ++headGestureCurrentCount;
                            headGesturePassedFrame = 0;
                            movementDirection = MovementDirection.Up;
                        }
                    break;

                    case MovementDirection.Up:
                        if(transform.rotation.eulerAngles.x - prevFrameRotate.x > headGestureDeviation){
                            ++headGestureCurrentCount;
                            headGesturePassedFrame = 0;
                            movementDirection = MovementDirection.Down;
                        }
                    break;
                }
            }
        }

        float x = transform.rotation.eulerAngles.x - prevFrameRotate.x;
        float y = transform.rotation.eulerAngles.y - prevFrameRotate.y;

        DebugText.UpdateInfo("HeadState-X", (x < 0 ? x.ToString("F6") : " " + (x.ToString("F6"))));
        DebugText.UpdateInfo("HeadState-Y", (y < 0 ? y.ToString("F6") : " " + (y.ToString("F6"))));
        DebugText.UpdateInfo("CheckAxis", movementDirection.ToString());
        DebugText.UpdateInfo("FrameCount", headGesturePassedFrame.ToString());
        DebugText.UpdateInfo("HeadCount", headGestureCurrentCount.ToString());

        prevFrameRotate = transform.rotation.eulerAngles;
    }
}