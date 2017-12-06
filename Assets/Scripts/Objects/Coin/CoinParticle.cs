using System.Collections.Generic;
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
	GameObject particlePrefab;

	int microCoin;

	const int coinPerParticle = 1;
	bool isIgnition;

	List<ParticleInfo> objList;
	SpriteRenderer _spriteRenderer;

	void Start () {
		objList = new List<ParticleInfo>();
		microCoin = 50;

		float color = 3f + microCoin / 10;
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.material.SetColor("_EmissionColor", new Color(color,color,0f));
	}

	void Update(){
		if(isIgnition && objList.Count == 0){
			Destroy(gameObject);
		}
	}


	void OnTriggerEnter(Collider other){
		if(!isIgnition){
			_spriteRenderer.enabled = false;
			isIgnition = true;
			++PuzzleManager.StageData[PuzzleManager.CurrentStage].CurrentCoinQty;

			SoundManager.PlaySE(SoundManager.SE.coin);

			int particleQty = microCoin / coinPerParticle;
			for (int i = particleQty; i > 0; --i){

				var obj = Instantiate(particlePrefab, transform.position, Quaternion.identity) as GameObject;
				var script = obj.GetComponent<ParticleChild>();
				script._Rigidbody = obj.GetComponent<Rigidbody>();
				script._SpriteRenderer = obj.GetComponent<SpriteRenderer>();

				float angle = Random.Range(0f,360f);
				var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
				script._Rigidbody.AddForce(shotVector * Random.Range(0.5f, 1.5f), ForceMode.Impulse);

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
				script.Coin = coinQty;

				obj.name = coinQty + "Coin.";
				// obj.transform.parent = transform;
			}
		}
	}
}
