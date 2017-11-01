using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockManager : MonoBehaviour {

	[SerializeField]
	GameObject blocksParentObject;
	[SerializeField]
	GameObject parentPrefab;
	[SerializeField]
	GameObject[] childPrefabs;

	int frame = 0;

	void Update () {
		if(++frame % 30 == 0){
			var pos = new Vector3(-4.5f, 0f, 10f);
			var parent = Instantiate(parentPrefab, pos, Quaternion.identity) as GameObject;
			var child = Instantiate(childPrefabs[0], pos, childPrefabs[0].transform.rotation);
			parent.transform.parent = blocksParentObject.transform;
			child.transform.parent = parent.transform;

			float angle = Random.Range(130f,230f);
			var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
			shotVector.Log();
			parent.GetComponent<Rigidbody>().AddForce(shotVector * 100f);
		}
		
	}
}
