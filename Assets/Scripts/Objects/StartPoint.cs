using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

	[SerializeField]
	float RotateY;

	static GameObject currentObj = null;
	Collider currentCollider;
	Rigidbody currentRigidbody;
	PlayerController currentSphereController;

	bool playingAnimation = false;
	float targetPosY = 0f;

	Transform parentTransform;

	int myStage;

	static float targetSize;

	[SerializeField]
	GameObject worldSpaceText;

	// Use this for initialization
	void Start () {
		targetSize = PuzzleManager.SphereController.transform.localScale.x;
		parentTransform = transform.root;
		if(!int.TryParse(transform.parent.parent.name.Remove(0,6), out myStage)){
			("ステージ数を取得できませんでした。" + transform.parent.parent.name).LogError();
			Destroy(gameObject);
		}
		// generate();
	}
	
	// Update is called once per frame
	void Update () {
		if(playingAnimation){
			currentObj.transform.position = new Vector3(currentObj.transform.position.x, currentObj.transform.position.y + 0.05f, currentObj.transform.position.z);
			var target = currentObj.transform.localScale.x + 0.08f;
			float scale = (target >= targetSize ? targetSize : currentObj.transform.localScale.x + 0.08f);

			"dasadsdasdsaads".Log();
			
			currentObj.transform.localScale = new Vector3(scale, scale, scale);
			if(targetPosY < currentObj.transform.position.y){
				playingAnimation = false;
				currentCollider.enabled = true;
				currentRigidbody.isKinematic = false;
				currentSphereController.IsActive = true;
			}
		}

		if(currentObj == null){
			if(myStage == PuzzleManager.CurrentStage){
				generate();
			}
			
		}else{
			if(currentObj.transform.position.y < -3f + -(PuzzleManager.CurrentStage * PuzzleManager.kMapDepth)){
				deleteSphere();
			}
		}
	}

	void generate(){
		var pos = transform.position;
		pos += PuzzleManager.SphereController.transform.position;
		pos.y += -PuzzleManager.kMapDepth;

		currentObj = Instantiate(PuzzleManager.SphereController, pos, Quaternion.identity) as GameObject;
		currentObj.transform.parent = parentTransform;
		currentCollider = currentObj.GetComponent<Collider>();
		currentRigidbody = currentObj.GetComponent<Rigidbody>();
		currentSphereController = currentObj.GetComponent<PlayerController>();

		currentSphereController.RotationY(RotateY);
		currentObj.transform.localScale = new Vector3(0f, 0f, 0f);

		targetPosY = pos.y + PuzzleManager.kMapDepth;
		currentCollider.enabled = false;
		currentRigidbody.isKinematic = true;
		currentSphereController.IsActive = false;
		playingAnimation = true;
	}

	void deleteSphere(){
		int addCoin = -(PuzzleManager.MicroCoin / 5) < 0 ? -(PuzzleManager.MicroCoin / 5) : 0;
		PuzzleManager.MicroCoin += addCoin;
		var text = (Instantiate(worldSpaceText) as GameObject).GetComponent<WorldSpaceText>();
		text.Text = "Reject!!";
		if(addCoin < 0){
			text.Text += "\n    " + addCoin + "Coin";
		}
		
		text.WorldPosition = new Vector3(currentObj.transform.position.x, -PuzzleManager.CurrentStage, currentObj.transform.position.z);
		Destroy(currentObj);
		currentObj = null;
	}
}
