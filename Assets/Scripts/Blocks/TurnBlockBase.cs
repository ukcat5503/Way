using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TurnBlockBase : MonoBehaviour {

	class ObjectInfo{
		public StartPosition startPosition;
		public GameObject obj;
		public ObjectInfo(StartPosition p, GameObject o){
			startPosition = p;
			obj = o;
		}
	}

	public enum StartPosition{
		Left,
		Up,
		Right,
		Down
	}

	float[] targetPoint = new float[4];
	Dictionary<int, ObjectInfo> SphereList = new Dictionary<int, ObjectInfo>();
	List<int> waitDelete = new List<int>();

	[SerializeField]
	float targerFromLeft, targerFromUp, targerFromRight, targerFromDown;
	

	// Use this for initialization
	void Start () {
		targetPoint[(int)StartPosition.Left] = targerFromLeft;
		targetPoint[(int)StartPosition.Up] = targerFromUp;
		targetPoint[(int)StartPosition.Right] = targerFromRight;
		targetPoint[(int)StartPosition.Down] = targerFromDown;
		
	}
	
	// Update is called once per frame
	void Update () {

		foreach (var item in SphereList)
		{
			item.Value.obj.transform.root.transform.Rotate(0,2,0);
			if(item.Value.obj.transform.root.transform.eulerAngles.y > 90){
				item.Value.obj.transform.root.transform.eulerAngles = new Vector3(0f, 90f, 0f);
				waitDelete.Add(item.Key);
			}
		}

		foreach (var item in waitDelete)
		{
			SphereList.Remove(item);
		}

			
		
		// other.transform.Rotate(0,-1,0);
	}
	

	void OnCollisionEnter(Collision other)
	{
		if(!SphereList.ContainsKey(other.gameObject.GetInstanceID())){
			StartPosition position;
			if(Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.z - other.transform.position.z)){
				if(transform.position.x - other.transform.position.x > 0){
					position = StartPosition.Left;
				}else{
					position = StartPosition.Right;
				}
			}else{
				if(transform.position.z - other.transform.position.z < 0){
					position = StartPosition.Up;
				}else{
					position = StartPosition.Down;
				}
			}
			SphereList.Add(other.gameObject.GetInstanceID(), new ObjectInfo(position, other.gameObject));
		}
	}
}