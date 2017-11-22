using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBlock : TurnBlockBase {

	[SerializeField]
	bool isVertical;

	const float kToleranceRotateFix = 1.0f;

	protected new void Update () {
		base.Update();
	}

	new void OnCollisionEnter(Collision other){
		var position = base.CalcStartPosition(other);
		isTouchSphere = true;
		if (sphereObjectInfo == null)
		{
			if (targetPoint[(int)position] != 0f)
			{
				var s = other.gameObject.GetComponent<PlayerController>();
				if (targetPoint[(int)position] == 180f)
				{
					s.RotationY(180);
				}
				else
				{
					// position.Log();
					var eular = other.transform.eulerAngles;

					bool isRotate = false;
					switch (position)
					{
						case StartPosition.North:
							isRotate = (eular.y < eular.y + kToleranceRotateFix && eular.y > eular.y - kToleranceRotateFix);
							eular.y = 180f;
							break;
						case StartPosition.South:
							isRotate = (eular.y < eular.y + kToleranceRotateFix && eular.y > eular.y - kToleranceRotateFix);
							eular.y = 0f;
							break;
						case StartPosition.West:
							isRotate = (eular.y < eular.y + kToleranceRotateFix && eular.y > eular.y - kToleranceRotateFix);
							eular.y = 90f;
							break;
						case StartPosition.East:
							isRotate = (eular.y < eular.y + kToleranceRotateFix && eular.y > eular.y - kToleranceRotateFix);
							eular.y = 270f;
							break;

					}

					// isRotate.Log();
					if (isRotate)
					{
						other.transform.eulerAngles = eular;
					}
				}
			}
		}
	}

	void OnCollisionStay(Collision other){
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