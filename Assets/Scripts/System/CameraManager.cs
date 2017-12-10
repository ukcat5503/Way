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

    static float downSpeed = 0.02f;
	static float leftOverDown = 0f;

    void Update()
    {
        if(leftOverDown > 0f){
            PuzzleManager.CameraObject.transform.position = new Vector3(PuzzleManager.CameraObject.transform.position.x, PuzzleManager.CameraObject.transform.position.y - downSpeed, PuzzleManager.CameraObject.transform.position.z);
			
			leftOverDown = leftOverDown - downSpeed > 0f ? leftOverDown - downSpeed : 0f;
		}
    }

    public static void CameraDown(float distance){
        leftOverDown += distance;
		PuzzleManager.HeldBlockSlot.transform.position = new Vector3(PuzzleManager.HeldBlockSlot.transform.position.x, PuzzleManager.HeldBlockSlot.transform.position.y - distance, PuzzleManager.HeldBlockSlot.transform.position.z);
	}

}