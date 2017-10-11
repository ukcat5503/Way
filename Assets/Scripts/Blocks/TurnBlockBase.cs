using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class TurnBlockBase : MonoBehaviour {

	class ObjectInfo{
		public float currentRotate;
		public float targetRotate;
		public GameObject obj;
		public ObjectInfo(float c, float t, GameObject o){
			currentRotate = c;
			targetRotate = t;
			obj = o;
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
		Right90 = -90,
		Left90 = 90,
		Back = 180
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
	[SerializeField]
	bool turnBlock = false;
	bool isTouchSphere;

	[SerializeField, Space(6), Header("入って来た角度から見てどのように曲がるかを指定")]
	RotateAngle targetFromWest;
	[SerializeField]
	RotateAngle targetFromNorth;
	[SerializeField]
	RotateAngle targetFromEast;
	[SerializeField]
	RotateAngle targetFromSouth;
	

	// Use this for initialization
	void Start () {
		Setup();
	}
	
	// Update is called once per frame
	void Update () {
		// ブロック回転
		if (turnBlock && !isTouchSphere && Input.GetKeyDown(KeyCode.Return)){
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90f, transform.eulerAngles.z);


			targetFromSouth = ValueToRotateAngle(targetPoint[(int)StartPosition.West]);
			targetFromNorth = ValueToRotateAngle(targetPoint[(int)StartPosition.East]);
			targetFromWest = ValueToRotateAngle(targetPoint[(int)StartPosition.North]);
			targetFromEast = ValueToRotateAngle(targetPoint[(int)StartPosition.South]);

			Setup();
		}

		// ボール制御
		foreach (var item in SphereList)
		{
			item.Value.obj.transform.root.transform.Rotate(0,item.Value.targetRotate / 20,0);

			var target = item.Value.currentRotate + item.Value.targetRotate;
		
			bool isDelete = (item.Value.targetRotate < 0) ? item.Value.obj.transform.root.transform.eulerAngles.y < target : item.Value.obj.transform.root.transform.eulerAngles.y > target;
			// (target + " / " + item.Value.targetRotate + " / " + item.Value.obj.transform.root.transform.eulerAngles.y + " / " + isDelete).Log();
			if(isDelete){
				"消せ!".Log();
				item.Value.obj.transform.root.transform.eulerAngles = new Vector3(0f, target, 0f);
				waitDelete.Add(item.Key);
			}
		}
		foreach (var item in waitDelete)
		{
			SphereList.Remove(item);
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
				if(targetPoint[(int)position] == 180f || targetPoint[(int)position] == 180f){
					other.gameObject.transform.root.transform.Rotate(0,180,0);
				}else{
					("from " + position.ToString() + " / " + targetPoint[(int)position] + "する").Log();
					SphereList.Add(other.gameObject.GetInstanceID(), new ObjectInfo(other.gameObject.transform.root.transform.eulerAngles.y, targetPoint[(int)position], other.gameObject));
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