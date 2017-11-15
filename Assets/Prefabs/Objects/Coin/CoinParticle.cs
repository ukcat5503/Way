using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour {

	class ParticleInfo{
		public GameObject Obj {get; private set;}
		public Rigidbody _Rigidbody {get; private set;}
		public SpriteRenderer _SpriteRenderer {get; private set;}
		

		public ParticleInfo(GameObject o, Rigidbody r, SpriteRenderer s){
			Obj = o;
			_Rigidbody = r;
			_SpriteRenderer = s;
		}
	}

	[SerializeField]
	GameObject flyTargetObject;
	[SerializeField]
	GameObject particlePrefab;

	[SerializeField]
	public float coin;

	const float coinPerParticle = 0.1f;

	List<ParticleInfo> objList;
	int frame = 0;

	void Start () {
		objList = new List<ParticleInfo>();

		int particleQty = (int)(coin / coinPerParticle);
		for (int i = 0; i < particleQty; ++i){

			var obj = Instantiate(particlePrefab) as GameObject;
			float angle = Random.Range(0f,360f);
			var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
			var r = obj.GetComponent<Rigidbody>();
			r.AddForce(shotVector * 0.7f, ForceMode.Impulse);
			var s = obj.GetComponent<SpriteRenderer>();
			objList.Add(new ParticleInfo(obj, r, s));
		}
	}

	void Update(){
		if(++frame >= 30){
			int length = objList.Count;
			for (int i = length - 1; i >= 0; --i){
				i.Log();
				objList[i]._Rigidbody.velocity = CloseToZero(objList[i]._Rigidbody.velocity);
				if(frame >= 90){
					objList[i].Obj.transform.position = Vector3.Lerp(objList[i].Obj.transform.position, flyTargetObject.transform.position, Time.deltaTime * 2f);
				}
				if(frame >= 120){
					objList[i]._SpriteRenderer.color = new Color(objList[i]._SpriteRenderer.color.r, objList[i]._SpriteRenderer.color.g, objList[i]._SpriteRenderer.color.b, objList[i]._SpriteRenderer.color.a - 0.05f);
					if(objList[i]._SpriteRenderer.color.a < 0){
						Destroy(objList[i].Obj);
						objList.RemoveAt(i);
					}
					
				}
			}
		}
		
	}

	Vector3 CloseToZero(Vector3 v3, float distance = 0.005f){
		if(v3 == Vector3.zero)	return Vector3.zero;
		if(v3.x > 0){
			v3.x = (v3.x - distance > 0 ? v3.x - distance : 0f);
		}else{
			v3.x = (v3.x + distance < 0 ? v3.x + distance : 0f);
		}
		if(v3.x > 0){
			v3.y = (v3.x - distance > 0 ? v3.y - distance : 0f);
		}else{
			v3.y = (v3.x + distance < 0 ? v3.y + distance : 0f);
		}
		if(v3.z > 0){
			v3.z = (v3.z - distance > 0 ? v3.z - distance : 0f);
		}else{
			v3.z = (v3.z + distance < 0 ? v3.z + distance : 0f);
		}

		return v3;
	}
}
