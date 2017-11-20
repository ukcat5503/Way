﻿using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour {

	class ParticleInfo{
		public GameObject Obj {get; private set;}
		public Rigidbody _Rigidbody {get; private set;}
		public SpriteRenderer _SpriteRenderer {get; private set;}
		public int Coin {get; private set;}
		public int DestroyFrame {get; private set;}

		public ParticleInfo(GameObject o, Rigidbody r, SpriteRenderer s, int c){
			Obj = o;
			_Rigidbody = r;
			_SpriteRenderer = s;
			Coin = c;
			DestroyFrame = Random.Range(100, 150);
		}
	}

	[SerializeField]
	GameObject flyTargetObject;
	[SerializeField]
	GameObject particlePrefab;

	[SerializeField]
	public int microCoin;

	const int coinPerParticle = 1;

	bool isIgnition = false;

	List<ParticleInfo> objList;
	int frame = 0;
	SpriteRenderer _spriteRenderer;

	void Start () {
		objList = new List<ParticleInfo>();

		float color = 3f + microCoin / 50;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.material.SetColor("_EmissionColor", new Color(color,color,0f));
	}

	void Update(){
		if(isIgnition){
			if(++frame >= 30){
				int length = objList.Count;
				for (int i = length - 1; i >= 0; --i){
					objList[i]._Rigidbody.velocity = CloseToZero(objList[i]._Rigidbody.velocity);
					if(frame >= objList[i].DestroyFrame / 1.5f){
						objList[i].Obj.transform.position = Vector3.Lerp(objList[i].Obj.transform.position, flyTargetObject.transform.position, Time.deltaTime * 2f);
					}
					if(frame >= objList[i].DestroyFrame){
						objList[i]._SpriteRenderer.color = new Color(objList[i]._SpriteRenderer.color.r, objList[i]._SpriteRenderer.color.g, objList[i]._SpriteRenderer.color.b, objList[i]._SpriteRenderer.color.a - 0.05f);
						if(objList[i]._SpriteRenderer.color.a < 0){
							Destroy(objList[i].Obj);
							PuzzleManager.MicroCoin += objList[i].Coin;
							objList.RemoveAt(i);
						}
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

	void OnTriggerEnter(Collider other){
		if(!isIgnition){
			_spriteRenderer.enabled = false;
			isIgnition = true;
			int particleQty = microCoin / coinPerParticle;
			for (int i = particleQty; i > 0; --i){

				var obj = Instantiate(particlePrefab, transform.position, Quaternion.identity) as GameObject;
				float angle = Random.Range(0f,360f);
				var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
				var r = obj.GetComponent<Rigidbody>();
				r.AddForce(shotVector * Random.Range(0.5f, 1.5f), ForceMode.Impulse);
				var s = obj.GetComponent<SpriteRenderer>();
				int coinQty = 1;
				// 全部大きくなると見栄えが悪いので少し小さいのも出す
				if(i > 150){
					coinQty = 100;
					i -= 99;
					obj.transform.localScale = new Vector3(5f, 5f, 5f);
				}else if(i > 15){
					coinQty = 10;
					i -= 9;
					obj.transform.localScale = new Vector3(2f, 2f, 2f);
				}
				obj.name = coinQty + "Coin.";
				objList.Add(new ParticleInfo(obj, r, s, coinQty));
			}
		}
	}
}
