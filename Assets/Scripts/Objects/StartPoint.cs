using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

	[SerializeField]
	GameObject sphere;
	[SerializeField]
	float RotateY;

	GameObject currentObj;
	Collider currentCollider;
	Rigidbody currentRigidbody;
	SphereController currentSphereController;

	bool playingAnimation = false;
	float targetPosY = 0f;

	Transform parentTransform;

	// Use this for initialization
	void Start () {
		parentTransform = transform.root.root;
		generate();
	}
	
	// Update is called once per frame
	void Update () {
		if(playingAnimation){
			currentObj.transform.position = new Vector3(currentObj.transform.position.x, currentObj.transform.position.y + 0.06f, currentObj.transform.position.z);
			var target = currentObj.transform.localScale.x + 0.08f;
			float scale = (target >= 1.0f ? 1.0f : currentObj.transform.localScale.x + 0.08f);
			
			currentObj.transform.localScale = new Vector3(scale, scale, scale);
			if(targetPosY < currentObj.transform.position.y){
				playingAnimation = false;
				currentCollider.enabled = true;
				currentRigidbody.isKinematic = false;
				currentSphereController.IsActive = true;
			}
		}

		if(currentObj == null){
			generate();
		}else{
			if(currentObj.transform.position.y < -30f){
				deleteSphere();
			}
		}
	}

	void generate(){
		var pos = transform.position;

		currentObj = Instantiate(sphere, pos, Quaternion.identity) as GameObject;
		currentObj.transform.parent = parentTransform;
		currentCollider = currentObj.GetComponent<Collider>();
		currentRigidbody = currentObj.GetComponent<Rigidbody>();
		currentSphereController = currentObj.GetComponent<SphereController>();

		currentSphereController.RotationY(RotateY);
		currentObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

		targetPosY = transform.position.y + 0.6f;
		currentCollider.enabled = false;
		currentRigidbody.isKinematic = true;
		currentSphereController.IsActive = false;
		playingAnimation = true;
	}

	void deleteSphere(){
		Destroy(currentObj);
		currentObj = null;
	}
}
