using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldBlockManager : MonoBehaviour {

	/*
	[SerializeField]
	LayerMask blockParentLayerMask;
	*/
	[SerializeField]
	GameObject blocksParentObject;
	[SerializeField]
	GameObject parentPrefab;
	[SerializeField]
	GameObject[] childPrefabs;
	
	float[] generatePosX = {-7f, -6f, -5f, -4f, -3f, -2f};

	static HeldBlockManager instance;

	void Awake()
	{
		instance = this;
	}

	public static void GenerateBlocks(){
		// まずブロック全削除
		int length = PuzzleManager.StageData[PuzzleManager.StageNumber].HeldBlocks.Count;

		for (int i = 0; i < length; ++i){
			Vector3 pos = new Vector3(instance.generatePosX[i % instance.generatePosX.Length], 1f, 11.5f + (i / instance.generatePosX.Length));
			int blockType = PuzzleManager.StageData[PuzzleManager.StageNumber].HeldBlocks[i];
			var parent = Instantiate(instance.parentPrefab, pos, Quaternion.identity) as GameObject;

			GameObject child;
			try
			{
				child = Instantiate(instance.childPrefabs[blockType], pos, instance.childPrefabs[blockType].transform.rotation);
			}
			catch (System.IndexOutOfRangeException e)
			{
				("指定されたブロックはありません: " + blockType + "\n" + e).Log();

				Destroy(parent);
				continue;
			}
			parent.transform.parent = instance.blocksParentObject.transform;
			child.transform.parent = parent.transform;

			// float angle = Random.Range(150f,210f);
			float angle = 180f;
			var shotVector = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad),0f , Mathf.Cos(angle * Mathf.Deg2Rad));
			parent.GetComponent<Rigidbody>().AddForce(shotVector * 100f);
			child.GetComponent<TurnBlockBase>().enabled = false;
		}
	}
}
