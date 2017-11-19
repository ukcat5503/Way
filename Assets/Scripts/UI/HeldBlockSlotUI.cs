using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockSlotUI : MonoBehaviour {

	RectTransform[] SlotObject;



	[SerializeField]
	int slotSize = 6;
	[SerializeField]
	GameObject slotPrefab;

	void Start () {
		SlotObject = new RectTransform[slotSize];

		var length = SlotObject.Length;
		for (int i = 0; i < length; ++i){
			SlotObject[i] = (Instantiate(slotPrefab, new Vector3(230f, -110f + -(i * 150), 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
			SlotObject[i].SetParent(transform, false);
			SlotObject[i].transform.name = "BlockSlot " + i;

			// 230 -140 
		}
	}
	
	void Update () {
		
	}
}
