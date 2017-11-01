using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockParent : MonoBehaviour {

	const float kDestroyHeight = -2f;

	void Update () {
		if(transform.position.y < kDestroyHeight){
			Destroy(gameObject);
		}
	}
}
