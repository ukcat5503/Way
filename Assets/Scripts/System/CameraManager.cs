using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera manager.
/// カメラの総合的な操作を司る。
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    LayerMask targetLayerMask;

    /// <summary>
    /// 左右の目に対するカメラ
    /// </summary>
    Camera eyeLeft, eyeRight;

    static float downSpeed = 0.01f;
	static float leftOverDown = 0f;

    /*
    /// <summary>
    /// 両眼視差距離 この値/2づつ左右のカメラが横にずれる
    /// </summary>
    [SerializeField]
    const float kBinocularDisparity = 0f;
    */

    // Use this for initialization
    void Start()
    {
		/*
        // 自身の下に配置されているカメラを取得する
        eyeLeft = gameObject.transform.Find("EyeLeft").GetComponent<Camera>();
        eyeRight = gameObject.transform.Find("EyeRight").GetComponent<Camera>();

		settingCameraFromDevice();

		 */
    }

    void Update()
    {
        //cameraControl();

        if (Input.GetMouseButtonDown(0)) {
            sendEventToHit(TurnBlockBase.ClickEventType.LeftClick);
        }

        if (Input.GetMouseButtonDown(1)) {
            sendEventToHit(TurnBlockBase.ClickEventType.RightClick);
        }

        if (Input.GetMouseButtonDown(2)) {
            sendEventToHit(TurnBlockBase.ClickEventType.MiddleClick);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f){
            sendEventToHit(TurnBlockBase.ClickEventType.WheelUp);
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f){
            sendEventToHit(TurnBlockBase.ClickEventType.WheelDown);
        }


        if(leftOverDown > 0f){
            PuzzleManager.CameraObject.transform.position = new Vector3(PuzzleManager.CameraObject.transform.position.x, PuzzleManager.CameraObject.transform.position.y - downSpeed, PuzzleManager.CameraObject.transform.position.z);
			
			leftOverDown = leftOverDown - downSpeed > 0f ? leftOverDown - downSpeed : 0f;
		}
    }

    void sendEventToHit(TurnBlockBase.ClickEventType type){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 100f, targetLayerMask)) {
            var s = hit.transform.gameObject.GetComponent<TurnBlockBase>();
            if(s != null){
                s.ClickObject(type);
            }
        }
    }

    /// <summary>
    /// Settings the camera from device.
    /// デバイスの種類によって カメラの設定を変更します
    /// </summary>
    void settingCameraFromDevice()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // カメラは１つで良いのでRightを無効化する
        eyeRight.gameObject.SetActive(false);

#elif UNITY_IOS
        // カメラの位置をずらす
        Vector3 myPos = transform.position;
        /* 
		eyeLeft.gameObject.transform.position = new Vector3(myPos.x - (kBinocularDisparity / 2f), myPos.y, myPos.z);
		eyeRight.gameObject.transform.position = new Vector3(myPos.x + (kBinocularDisparity / 2f), myPos.y, myPos.z);
        */
		eyeLeft.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
		eyeRight.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);

        // ジャイロを有効に
        Input.gyro.enabled = true;

#else
    "Any other platform".LogWarning();

#endif
	}


    void cameraControl(){
// #if UNITY_EDITOR || UNITY_STANDALONE
        // マウスの移動でカメラ回転
        transform.rotation = Quaternion.Euler(
            gameObject.transform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * 2f,
            gameObject.transform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * 2f,
            0f
        );
 
 /*
#elif UNITY_IPHONE
        // ジャイロを利用してカメラ回転
        if (Input.gyro.enabled){
        Quaternion direction = Input.gyro.attitude;
            gameObject.transform.localRotation = Quaternion.Euler(90, 0, 0) * (new Quaternion(-direction.x, -direction.y, direction.z, direction.w));
        }
#endif
 */
    }

    public static void CameraDown(float distance){
        leftOverDown += distance;
		PuzzleManager.HeldBlockSlot.transform.position = new Vector3(PuzzleManager.HeldBlockSlot.transform.position.x, PuzzleManager.HeldBlockSlot.transform.position.y - distance, PuzzleManager.HeldBlockSlot.transform.position.z);
	}

}