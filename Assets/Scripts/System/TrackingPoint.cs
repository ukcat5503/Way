﻿﻿using System.Collections;
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
    /// 前フレームで選択していたオブジェクトのユニークID
    /// </summary>
    int prevHit = 0;
    int count = 0;

    /// <summary>
    /// 選択対象のレイヤー
    /// </summary>
    public LayerMask TargetLayer;

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


            // hit.collider.enabled = false;
        }else{
            Indicator.IsWatchSomeone = false;
        }
    }
}