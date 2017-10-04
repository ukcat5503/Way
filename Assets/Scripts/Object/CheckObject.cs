using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObject : MonoBehaviour {
	List<MeshRenderer> objList = new List<MeshRenderer>();

	[SerializeField]
	int frameToTransparency = 60;

	void Update () {
		for (int i = objList.Count - 1; i >= 0; --i)
		{
			if(objList[i] != null){
				objList[i].material.color = new Color(objList[i].material.color.r, objList[i].material.color.g, objList[i].material.color.b, objList[i].material.color.a - (1f / frameToTransparency));
				if(objList[i].material.color.a < 0){
					Destroy(objList[i].gameObject);
					objList.RemoveAt(i);
				}
			}else{
				"MeshRendererが取得されていませんでした。".LogWarning();
				objList.RemoveAt(i);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		MeshRenderer m;
		if((m = other.GetComponent<MeshRenderer>()) == null){
			"MeshRendererの取得に失敗しました".LogWarning();
			Destroy(other.gameObject);
		}else{
			objList.Add(m);
			other.isTrigger = false;
		}
		
    }
}
