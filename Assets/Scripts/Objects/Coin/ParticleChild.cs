using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleChild : MonoBehaviour {

	public Rigidbody _Rigidbody;
	public SpriteRenderer _SpriteRenderer;
	public int Coin;
	int destroyFrame;

	int frame = 0;


	void Start () {
		destroyFrame = Random.Range(100, 150);
	}
	
	void Update () {
		if(++frame >= 30){
			_Rigidbody.velocity = CloseToZero(_Rigidbody.velocity);
			if(frame >= destroyFrame / 1.5f && StartPoint.CurrentObj != null){
				transform.position = Vector3.Lerp(transform.position, StartPoint.CurrentObj.transform.position, Time.deltaTime * 5f);
			}
			if(frame >= destroyFrame){
				_SpriteRenderer.color = new Color(_SpriteRenderer.color.r, _SpriteRenderer.color.g, _SpriteRenderer.color.b, _SpriteRenderer.color.a - 0.05f);
				if(_SpriteRenderer.color.a < 0){
					Destroy(gameObject);
					PuzzleManager.MicroCoin += Coin;
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