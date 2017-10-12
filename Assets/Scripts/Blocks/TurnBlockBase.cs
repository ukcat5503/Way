using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TurnBlockBase : MonoBehaviour {

	class ObjectInfo{
		public float currentRotate;
		public float targetRotate;
		public GameObject obj;
		public SphereController sphere;

		public ObjectInfo(float c, float t, GameObject o, SphereController s){
			currentRotate = c;
			targetRotate = t;
			obj = o;
			sphere = s;
		}
	}

	/// <summary>
	/// Z↑ X→を基準にしたときの方角
	/// </summary>
	public enum StartPosition{
		West,
		North,
		East,
		South
	}

	public enum RotateAngle {
		Zero = 0,
		Right90 = 90,
		Left90 = -90,
		Back = 180
	}

	public enum TurnAngle {
		NotTurn,
		TurnRight,
		TurnFlip,
		TurnLeft
	}


	RotateAngle ValueToRotateAngle(int n) {
		foreach (RotateAngle a in System.Enum.GetValues(typeof(RotateAngle)))
		{
			if ((int)a == n) return a;
		}
		("enumに指定する値がありませんでした。" + n).Log();
		return RotateAngle.Zero;
	}



	int[] targetPoint = new int[4];
	Dictionary<int, ObjectInfo> SphereList = new Dictionary<int, ObjectInfo>();
	List<int> waitDelete = new List<int>();

	const float kMaxRange = 180f;
	bool isTouchSphere;

	[SerializeField, Space(6), Header("入って来た角度から見てどのように曲がるかを指定")]
	RotateAngle targetFromWest;
	[SerializeField]
	RotateAngle targetFromNorth;
	[SerializeField]
	RotateAngle targetFromEast;
	[SerializeField]
	RotateAngle targetFromSouth;

	[SerializeField, Space(6), Header("1動作でどちらにブロックが動作するか")]
	TurnAngle turnBlockAngle;


	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		// ブロック回転
		if (turnBlockAngle != TurnAngle.NotTurn && !isTouchSphere && Input.GetKeyDown(KeyCode.Return)){
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90f * (int)turnBlockAngle, transform.eulerAngles.z);

			TurnBlock((int)turnBlockAngle);
			Setup();
		}

		// ボール制御
		foreach (var item in SphereList)
		{
			bool isDelete = (item.Value.currentRotate < item.Value.currentRotate + item.Value.targetRotate) ?
				item.Value.sphere.RotateY > item.Value.currentRotate + item.Value.targetRotate:
				item.Value.sphere.RotateY < item.Value.currentRotate + item.Value.targetRotate;
			isDelete.Log();
			if (isDelete){
				(item.Value.currentRotate + item.Value.targetRotate).Log();
				item.Value.obj.transform.eulerAngles = new Vector3(0f, item.Value.currentRotate + item.Value.targetRotate, 0f);
				waitDelete.Add(item.Key);
			}
			else
			{
				item.Value.sphere.RotationY(item.Value.targetRotate / 20);
			}
		}
		foreach (var item in waitDelete)
		{
			SphereList.Remove(item);
		}

		waitDelete.Clear();
	}

	void TurnBlock(int turnCount)
	{
		targetFromEast = ValueToRotateAngle(targetPoint[(int)StartPosition.East]);
		targetFromNorth = ValueToRotateAngle(targetPoint[(int)StartPosition.North]);
		targetFromWest = ValueToRotateAngle(targetPoint[(int)StartPosition.West]);
		targetFromSouth = ValueToRotateAngle(targetPoint[(int)StartPosition.South]);
		for (;turnCount > 0; --turnCount)
		{
			var temp = targetFromEast;
			targetFromEast = targetFromNorth;
			targetFromNorth = targetFromWest;
			targetFromWest = targetFromSouth;
			targetFromSouth = temp;
		}
	}

	void Setup()
	{
		targetPoint[(int)StartPosition.West] = (int)targetFromWest;
		targetPoint[(int)StartPosition.East] = (int)targetFromEast;
		targetPoint[(int)StartPosition.North] = (int)targetFromNorth;
		targetPoint[(int)StartPosition.South] = (int)targetFromSouth;
	}

	[ContextMenu("Output")]
	public void Output()
	{
		(
			"name: " + gameObject.name + "\n" +
			"←: " + targetPoint[(int)StartPosition.West] + "\n" +
			"↑: " + targetPoint[(int)StartPosition.North] + "\n" +
			"→: " + targetPoint[(int)StartPosition.East] + "\n" +
			"↓: " + targetPoint[(int)StartPosition.South] + "\n"
		).Log();
	}
	

	void OnCollisionEnter(Collision other)
	{
		if(!SphereList.ContainsKey(other.gameObject.GetInstanceID())){
			StartPosition position;
			if(Mathf.Abs(transform.position.x - other.transform.position.x) > Mathf.Abs(transform.position.z - other.transform.position.z)){
				if(transform.position.x - other.transform.position.x > 0){
					position = StartPosition.West;
				}else{
					position = StartPosition.East;
				}
			}else{
				if(transform.position.z - other.transform.position.z < 0){
					position = StartPosition.North;
				}else{
					position = StartPosition.South;
				}
			}
			// (position.ToString() + "から来た").Log();
			if(targetPoint[(int)position] != 0f){
				var s = other.gameObject.GetComponent<SphereController>();
				if (targetPoint[(int)position] == 180f){
					s.RotationY(180);
				}else{
					("from " + position.ToString() + " / " + targetPoint[(int)position] + "する" + " 現在:" + s.RotateY + " 目標:" + (s.RotateY + targetPoint[(int)position])).Log();
					SphereList.Add(other.gameObject.GetInstanceID(), new ObjectInfo(s.RotateY, targetPoint[(int)position], other.gameObject, s));
				}
			}
		}
	}
	void OnCollisionStay(Collision other)
	{
		isTouchSphere = true;
	}

	void OnCollisionExit(Collision other)
	{
		isTouchSphere = false;
	}
}