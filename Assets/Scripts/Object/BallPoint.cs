using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPoint : MonoBehaviour {

	[SerializeField]
	GameObject ball;

	List<GameObject> ballList = new List<GameObject>();

	int frame = 0;

	// Update is called once per frame
	void Update () {
		if(++frame % 120 == 60){
			if(PuzzleManager.GetDroppedSphere() > 0){
				PuzzleManager.AddDroppedSphere(-1);
				var obj = Instantiate(ball, transform.position, Quaternion.identity) as GameObject; 
				ballList.Add(obj);
			}
		}
		
	}
}
