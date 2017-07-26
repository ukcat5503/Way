using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera manager.
/// カメラの総合的な操作を司る。
/// </summary>
public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// 左右の目に対するカメラ
    /// </summary>
    Camera eyeLeft, eyeRight;

    /// <summary>
    /// 両眼視差距離 この値/2づつ左右のカメラが横にずれる
    /// </summary>
    const float kBinocularDisparity = 0.4f;

    // Use this for initialization
    void Start()
    {
        // 自身の下に配置されているカメラを取得する
        eyeLeft = gameObject.transform.FindChild("EyeLeft").GetComponent<Camera>();
        eyeRight = gameObject.transform.FindChild("EyeRight").GetComponent<Camera>();



		settingCameraFromDevice();
    }

    void Update()
    {
        cameraControl();
    }

    /// <summary>
    /// Settings the camera from device.
    /// デバイスの種類によって カメラの設定を変更します
    /// </summary>
    void settingCameraFromDevice()
    {
#if UNITY_EDITOR
        // カメラは１つで良いのでRightを無効化する
        eyeRight.gameObject.SetActive(false);

#elif UNITY_IPHONE
        // カメラの位置をずらす
        Vector3 myPos = transform.position;
		eyeLeft.gameObject.transform.position = new Vector3(myPos.x - (kBinocularDisparity / 2f), myPos.y, myPos.z);
		eyeRight.gameObject.transform.position = new Vector3(myPos.x + (kBinocularDisparity / 2f), myPos.y, myPos.z);
		eyeLeft.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
		eyeRight.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);

        // ジャイロを有効に
        Input.gyro.enabled = true;

#else
    Debug.LogWarning("Any other platform");

#endif
	}


    void cameraControl(){
#if UNITY_EDITOR
		Vector3 keyState = new Vector3(0f, 0f, 0f);
		if (Input.GetKey(KeyCode.LeftArrow)) --keyState.y;
		if (Input.GetKey(KeyCode.RightArrow)) ++keyState.y;
		if (Input.GetKey(KeyCode.UpArrow)) --keyState.x;
		if (Input.GetKey(KeyCode.DownArrow)) ++keyState.x;
		keyState.z = 0f;
        gameObject.transform.Rotate(keyState);

#elif UNITY_IPHONE
        if (Input.gyro.enabled){
        Quaternion direction = Input.gyro.attitude;
            gameObject.transform.localRotation = Quaternion.Euler(90, 0, 0) * (new Quaternion(-direction.x, -direction.y, direction.z, direction.w));
        }
#endif
	}

}
