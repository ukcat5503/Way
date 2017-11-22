using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockSlotUI : MonoBehaviour {

	RectTransform[] SlotObject;

	[SerializeField]
	int slotLength;
	[SerializeField]
	GameObject slotPrefab;

	[SerializeField]
	Vector2 basePosition, slotSize, positionMargin;

	Rect[] buttonPosition;

	[SerializeField]
	GameObject rect;

	void Start () {
		SlotObject = new RectTransform[slotLength];
		buttonPosition = new Rect[slotLength];

		var length = SlotObject.Length;
		for (int i = 0; i < length; ++i){
			SlotObject[i] = (Instantiate(slotPrefab, new Vector3(230f, -110f + -(i * 150), 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
			SlotObject[i].SetParent(transform, false);
			SlotObject[i].transform.name = "BlockSlot " + i;
			buttonPosition[i] = new Rect(basePosition.x + (positionMargin.x * i), basePosition.y + (positionMargin.y * i), slotSize.x, slotSize.y);

			if (true)
			{
				var obj = (Instantiate(rect, new Vector3(buttonPosition[i].x, buttonPosition[i].y, 0), Quaternion.identity) as GameObject).GetComponent<RectTransform>();
				obj.parent = transform.parent;
				obj.sizeDelta = new Vector2(slotSize.x, slotSize.y);
			}
		}
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			var length = buttonPosition.Length;
			for (int i = 0; i < length; ++i)
			{
				if (buttonPosition[i].Contains(Input.mousePosition))
				{
					("クリック obj:" + i).Log();
				}
			}
		}
	}


}
