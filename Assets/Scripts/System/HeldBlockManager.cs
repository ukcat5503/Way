using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockManager : MonoBehaviour {

	[SerializeField]
	LayerMask blockParentLayerMask;
	[SerializeField]
	GameObject blocksParentObject;
	[SerializeField]
	GameObject parentPrefab;
	[SerializeField]
	GameObject[] childPrefabs;

	int frame = 0;

	void Update () {
		if(++frame % 20 == 0){
			Vector3 pos;
			{
				Collider[] objs;
				int count = 0;
				do{
					if(++count > 5){
						"どうしても空いていなかったのでスキップしたよ".Log();
						return;
					}
					pos = new Vector3(Random.Range(-2.5f,-6.5f), 0f, 11.5f);
					objs = Physics.OverlapBox(pos, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, blockParentLayerMask);
				} while (objs.Length > 0);
			}
			int blockType = Random.Range(0,childPrefabs.Length);

			var parent = Instantiate(parentPrefab, pos, Quaternion.identity) as GameObject;
			var child = Instantiate(childPrefabs[blockType], pos, childPrefabs[blockType].transform.rotation);
			parent.transform.parent = blocksParentObject.transform;
			child.transform.parent = parent.transform;

			// float angle = Random.Range(150f,210f);
			float angle = 180f;
			var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
			parent.GetComponent<Rigidbody>().AddForce(shotVector * 100f);
		}
	}
}
