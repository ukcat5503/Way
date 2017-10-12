using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBlock : TurnBlockBase {

	[SerializeField]
	bool isVertical;

	protected new void Update () {
		base.Update();
	}

	new void OnCollisionEnter(Collision other){
		// base.OnCollisionEnter(other);
	}

	new void OnCollisionStay(Collision other){
		var pos = other.transform.position;
		if(isVertical){
			var difference = other.transform.position.x - transform.position.x;
			if(difference <= -0.1f){
				pos.x += 0.02f;
			}else if(difference >= 0.1f){
				pos.x -= 0.02f;
			}
		}else{
			var difference = other.transform.position.z - transform.position.z;
			if(difference <= -0.1f){
				pos.z += 0.02f;
			}else if(difference >= 0.1f){
				pos.z -= 0.02f;
			}
		}
		other.transform.position = pos;
	}

	new void OnCollisionExit(Collision other){
		base.OnCollisionExit(other);
	}
}