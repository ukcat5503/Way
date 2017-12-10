using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundUIManager : MonoBehaviour {

	class BackGroundPartsInfo{
		public GameObject obj;
		public Image _image;
		public int deleteStartFrame;
		public float speed;
		public bool isVisible = false;
		public BackGroundPartsInfo(GameObject o, Image im, int deleteFrame, float s){
			obj = o;
			_image = im;
			deleteStartFrame = deleteFrame;
			speed = s;
		}
	}
	
	[SerializeField]
	GameObject backgroundParts;

	[SerializeField]
	Vector3 partsAngle;	

	[SerializeField]
	Color[] colorList;

	List<BackGroundPartsInfo> list = new List<BackGroundPartsInfo>();

	int frame = 0;

	Vector2 minInstantiatePosition = new Vector2(-1050f,-640f);
	Vector2 maxInstantiatePosition = new Vector2(500f,540f);

	int minDeleteFrame = 90;
	int maxDeleteFrame = 180;

	float minSpeed = 120f / 60f;
	float maxSpeed = 360f / 60f;

	float minSize = 0.25f;
	float maxSize = 2f;

	float targetAlphaColor = 0.1f;

	void Update () {

		if(++frame % 2 == 0){
			var obj = Instantiate(backgroundParts, new Vector3(0,0,0), Quaternion.Euler(partsAngle)) as GameObject;

			obj.transform.SetParent(transform);

			float size = Random.Range(minSize, maxSize);
			obj.transform.localScale = new Vector3(size, size, size);

			Vector3 pos = new Vector3(Random.Range(minInstantiatePosition.x, maxInstantiatePosition.x), Random.Range(minInstantiatePosition.y, maxInstantiatePosition.y), 0);
			obj.transform.localPosition = pos;
			
			var img = obj.GetComponent<Image>();
			var color = Random.Range(0, colorList.Length);
			img.color = new Color(colorList[color].r, colorList[color].g, colorList[color].b, 0f);

			int delete = frame + Random.Range(minDeleteFrame, maxDeleteFrame);
			list.Add(new BackGroundPartsInfo(obj, obj.GetComponent<Image>(), delete, Random.Range(minSpeed, maxSpeed)));
		}

		int length = list.Count;

		for (int i = length - 1; i >= 0; --i){
			if(frame > list[i].deleteStartFrame){
				var c = list[i]._image.color;
				c.a -= targetAlphaColor / 60f;
				list[i]._image.color = c;

				if(list[i]._image.color.a < 0){
					Destroy(list[i].obj);
					list.RemoveAt(i);
				}
			}else{
				if(!list[i].isVisible){
					var c = list[i]._image.color;
					c.a += targetAlphaColor / 60f;
					list[i]._image.color = c;

					if(list[i]._image.color.a > targetAlphaColor){
						list[i].isVisible = true;
					}
				}
			}
			list[i].obj.transform.position = new Vector3(list[i].obj.transform.position.x + list[i].speed, list[i].obj.transform.position.y, list[i].obj.transform.position.z);
		}
	}
}
