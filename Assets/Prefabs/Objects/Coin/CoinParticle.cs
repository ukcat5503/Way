using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour {

	[SerializeField]
	GameObject flyTargetObject;
	[SerializeField]
	GameObject particlePrefab;

	[SerializeField]
	public float coin;

	const float coinPerParticle = 0.1f;

	void Start () {
		int particleQty = (int)(coin / coinPerParticle);
		for (int i = 0; i < particleQty; ++i){
			var obj = Instantiate(particlePrefab) as GameObject;
			float angle = Random.Range(0f,360f);
			var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
			obj.GetComponent<Rigidbody>().AddForce(shotVector * 0.5f, ForceMode.Impulse);
		}
	}
}
