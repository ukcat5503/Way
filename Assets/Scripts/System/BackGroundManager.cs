using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour {

	class BackGroundPartsInfo{
		public GameObject obj;
		public SpriteRenderer _spriteRenderer;
		public int deleteStartFrame;
		public float speed;
		public bool isVisible = false;
		public BackGroundPartsInfo(GameObject o, SpriteRenderer sr, int deleteFrame, float s){
			obj = o;
			_spriteRenderer = sr;
			deleteStartFrame = deleteFrame;
			speed = s;
		}
	}
	
	[SerializeField]
	GameObject backgroundParts;

	[SerializeField]
	Vector3 partsAngle;	

	List<BackGroundPartsInfo> list = new List<BackGroundPartsInfo>();

	int frame = 0;

	Vector2 minInstantiatePosition = new Vector2(-12f,-7f);
	Vector2 maxInstantiatePosition = new Vector2(20f,17f);

	int minDeleteFrame = 90;
	int maxDeleteFrame = 180;

	float minSpeed = 0.2f / 60f;
	float maxSpeed = 1.2f / 60f;

	float minSize = 2f;
	float maxSize = 5f;

	float targetAlphaColor = 0.1f;

	void Update () {

		if(++frame % 2 == 0){
			var obj = Instantiate(backgroundParts, new Vector3(0,0,0), Quaternion.Euler(partsAngle)) as GameObject;

			obj.transform.parent = transform;

			float size = Random.Range(minSize, maxSize);
			obj.transform.localScale = new Vector3(size, size, size);

			Vector3 pos = new Vector3(Random.Range(minInstantiatePosition.x, maxInstantiatePosition.x), Random.Range(minInstantiatePosition.y, maxInstantiatePosition.y), 0);
			obj.transform.localPosition = pos;
			

			int delete = frame + Random.Range(minDeleteFrame, maxDeleteFrame);
			list.Add(new BackGroundPartsInfo(obj, obj.GetComponent<SpriteRenderer>(), delete, Random.Range(minSpeed, maxSpeed)));
		}

		int length = list.Count;

		for (int i = length - 1; i >= 0; --i){
			if(frame > list[i].deleteStartFrame){
				var c = list[i]._spriteRenderer.color;
				c.a -= targetAlphaColor / 60f;
				list[i]._spriteRenderer.color = c;

				if(list[i]._spriteRenderer.color.a < 0){
					Destroy(list[i].obj);
					list.RemoveAt(i);
				}
			}else{
				if(!list[i].isVisible){
					var c = list[i]._spriteRenderer.color;
					c.a += targetAlphaColor / 60f;
					list[i]._spriteRenderer.color = c;

					if(list[i]._spriteRenderer.color.a > targetAlphaColor){
						list[i].isVisible = true;
					}
				}
			}
			list[i].obj.transform.position = new Vector3(list[i].obj.transform.position.x + list[i].speed, list[i].obj.transform.position.y, list[i].obj.transform.position.z);
		}
		
	}
}
