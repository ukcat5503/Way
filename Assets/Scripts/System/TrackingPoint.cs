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
        checkHeadGesture();
    }

    void checkHeadGesture(){
        // 相対的にどのぐらい移動しているかを計算
        if(prevFrameRotate.x - transform.rotation.eulerAngles.x < -180f){
            // 下から上へ
            prevPointRelativeRotate.x += 360 + prevFrameRotate.x - transform.rotation.eulerAngles.x;
        }else if(prevFrameRotate.x - transform.rotation.eulerAngles.x > 180f){
            // 上から下へ
            prevPointRelativeRotate.x += 360 - prevFrameRotate.x - transform.rotation.eulerAngles.x;
        }else{
            prevPointRelativeRotate.x += prevFrameRotate.x - transform.rotation.eulerAngles.x;
        }
        
        if(prevFrameRotate.y - transform.rotation.eulerAngles.y < -180f){
            // 左から右へ
            prevPointRelativeRotate.y += 360 + prevFrameRotate.y - transform.rotation.eulerAngles.y;
        }else if(prevFrameRotate.y - transform.rotation.eulerAngles.y > 180f){
            // 右から左へ
            prevPointRelativeRotate.y += 360 - prevFrameRotate.y - transform.rotation.eulerAngles.y;
        }else{
            prevPointRelativeRotate.y += prevFrameRotate.y - transform.rotation.eulerAngles.y;
        }

        // 入力時間切れ
        if(++headGesturePassedFrame >= headGestureWaitFrame){
                headGestureCurrentCount = 0;
                prevPointRelativeRotate = new Vector3(0f, 0f, 0f);
                judgeType = JudgeType.None;
        }

        // 初回判定
        if(headGestureCurrentCount == 0){
            if(transform.rotation.eulerAngles.x - prevFrameRotate.x < -0.15f){
                movementDirection = MovementDirection.Up;
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                prevPointRelativeRotate = new Vector3(0f, 0f, 0f);

            }else if(transform.rotation.eulerAngles.x - prevFrameRotate.x > 0.15f){
                movementDirection = MovementDirection.Down;
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                prevPointRelativeRotate = new Vector3(0f, 0f, 0f);

            }else if(transform.rotation.eulerAngles.y - prevFrameRotate.y > 0.15f){
                movementDirection = MovementDirection.Right;
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                prevPointRelativeRotate = new Vector3(0f, 0f, 0f);

            }else if(transform.rotation.eulerAngles.y - prevFrameRotate.y < -0.15f){
                ++headGestureCurrentCount;
                headGesturePassedFrame = 0;
                movementDirection = MovementDirection.Left;
                prevPointRelativeRotate = new Vector3(0f, 0f, 0f);

            }
        }else{ // headGestureCurrentCount > 0
            switch(movementDirection){
                case MovementDirection.Left:
                    if(prevPointRelativeRotate.y < -headGestureDeviation && 
                    (prevPointRelativeRotate.x > -headGestureDeviation && 
                    prevPointRelativeRotate.x < headGestureDeviation)){
                        ++headGestureCurrentCount;
                        headGesturePassedFrame = 0;
                        movementDirection = MovementDirection.Right;
                        prevPointRelativeRotate = new Vector3(0f, 0f, 0f);
                    }
                break;

                case MovementDirection.Right:
                    if(prevPointRelativeRotate.y > headGestureDeviation && 
                    (prevPointRelativeRotate.x > -headGestureDeviation && 
                    prevPointRelativeRotate.x < headGestureDeviation)){
                        ++headGestureCurrentCount;
                        headGesturePassedFrame = 0;
                        movementDirection = MovementDirection.Left;
                        prevPointRelativeRotate = new Vector3(0f, 0f, 0f);
                        
                    }
                break;

                case MovementDirection.Up:
                    if(prevPointRelativeRotate.x < -headGestureDeviation && 
                    (prevPointRelativeRotate.y > -headGestureDeviation && 
                    prevPointRelativeRotate.y < headGestureDeviation)){
                        ++headGestureCurrentCount;
                        headGesturePassedFrame = 0;
                        movementDirection = MovementDirection.Down;
                        prevPointRelativeRotate = new Vector3(0f, 0f, 0f);
                        
                    }
                break;

                case MovementDirection.Down:
                     if(prevPointRelativeRotate.x > headGestureDeviation && 
                    (prevPointRelativeRotate.y > -headGestureDeviation && 
                    prevPointRelativeRotate.y < headGestureDeviation)){
                        ++headGestureCurrentCount;
                        headGesturePassedFrame = 0;
                        movementDirection = MovementDirection.Up;
                        prevPointRelativeRotate = new Vector3(0f, 0f, 0f);
                        
                    }
                break;

            }
        }

        DebugText.UpdateInfo("CheckAxis", movementDirection.ToString());
        DebugText.UpdateInfo("FrameCount", headGesturePassedFrame.ToString());
        DebugText.UpdateInfo("HeadCount", headGestureCurrentCount.ToString());
        DebugText.UpdateInfo("=======", "");
        

        DebugText.UpdateInfo("Relative", prevPointRelativeRotate);
        DebugText.UpdateInfo("rotate", transform.rotation.eulerAngles);

        prevFrameRotate = transform.rotation.eulerAngles;
    }
}