using UnityEngine;
using System.Collections;


public class RotateVrCamera : MonoBehaviour
{

    public GameObject Head;

    // Use this for initialization
    void Start()
    {
#if UNITY_IPHONE
        Input.gyro.enabled = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
		Vector3 keyState = new Vector3(0f, 0f, 0f);
		if (Input.GetKey(KeyCode.LeftArrow)) --keyState.y;
		if (Input.GetKey(KeyCode.RightArrow)) ++keyState.y;
		if (Input.GetKey(KeyCode.UpArrow)) --keyState.x;
		if (Input.GetKey(KeyCode.DownArrow)) ++keyState.x;
        keyState.z = 0f;
		Head.transform.Rotate(keyState);

#elif UNITY_IPHONE
        if (Input.gyro.enabled){
        Quaternion direction = Input.gyro.attitude;
            Head.transform.localRotation = Quaternion.Euler(90, 0, 0) * (new Quaternion(-direction.x, -direction.y, direction.z, direction.w));
        }
#endif
	}
}