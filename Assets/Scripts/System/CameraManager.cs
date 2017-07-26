#define BIONOCULAR_DISPARITY 0.3f


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

    /// <summary>
    /// Settings the camera from device.
    /// デバイスの種類によって カメラの設定を変更します
    /// </summary>
    void settingCameraFromDevice()
    {
#if UNITY_EDITOR
        // カメラは１つで良いのでRightを無効化する
        eyeRight.gameObject.SetActive(false);
        // #elif UNITY_IPHONE
        // カメラの位置をずらす
        Vector3 myPos = transform.position;
		eyeLeft.gameObject.transform.position = new Vector3(myPos.x - (kBinocularDisparity / 2f), myPos.y, myPos.z);
		eyeRight.gameObject.transform.position = new Vector3(myPos.x + (kBinocularDisparity / 2f), myPos.y, myPos.z);

        // TODO カメラは規定で１つしか表示されないように変更する それをここで複数になるように指定する(Viewport rectとか)

#else
    Debug.LogWarning("Any other platform");
#endif
	}
}
