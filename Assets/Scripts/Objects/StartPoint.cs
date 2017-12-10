using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

	[SerializeField]
	float RotateY;

	public static GameObject CurrentObj = null;
	Collider currentCollider;
	Rigidbody currentRigidbody;
	PlayerController currentSphereController;

	bool playingAnimation = false;
	bool setStateToObj = false;
	float targetPosY = -1f;

	Transform parentTransform;

	int myStage;

	static float targetSize;

	

	// Use this for initialization
	void Start () {
		targetSize = PuzzleManager.SphereController.transform.localScale.x;
		parentTransform = transform.root;
		if(!int.TryParse(transform.parent.parent.name.Remove(0,6), out myStage)){
			("ステージ数を取得できませんでした。" + transform.parent.parent.name).LogError();
			Destroy(gameObject);
		}
		generate();
	}
	
	// Update is called once per frame
	void Update () {
		if(playingAnimation){
			CurrentObj.transform.position = new Vector3(CurrentObj.transform.position.x, CurrentObj.transform.position.y + 0.05f, CurrentObj.transform.position.z);
			var target = CurrentObj.transform.localScale.x + 0.08f;
			float scale = (target >= targetSize ? targetSize : CurrentObj.transform.localScale.x + 0.08f);

			CurrentObj.transform.localScale = new Vector3(scale, scale, scale);
			if(targetPosY < CurrentObj.transform.position.y){
				playingAnimation = false;
				setStateToObj = false;

				setStateToObj = true;
				currentCollider.enabled = true;
				currentRigidbody.isKinematic = false;
				currentSphereController.IsActive = true;
			}
		}

		if(CurrentObj == null){
			if(myStage == PuzzleManager.CurrentStage){
				generate();
			}
			
		}else{
			if(CurrentObj.transform.position.y < -3f + -(PuzzleManager.CurrentStage * PuzzleManager.kMapDepth)){
				DeleteSphere();
			}
		}
	}

	void generate(){
		var pos = transform.position;
		pos += PuzzleManager.SphereController.transform.position;
		pos.y += -PuzzleManager.kMapDepth;

		CurrentObj = Instantiate(PuzzleManager.SphereController, pos, Quaternion.identity) as GameObject;
		CurrentObj.name = "Player";
		CurrentObj.transform.parent = parentTransform;
		currentCollider = CurrentObj.GetComponent<Collider>();
		currentRigidbody = CurrentObj.GetComponent<Rigidbody>();
		currentSphereController = CurrentObj.GetComponent<PlayerController>();

		currentSphereController.RotationY(RotateY);
		CurrentObj.transform.localScale = new Vector3(0f, 0f, 0f);

		targetPosY = pos.y + PuzzleManager.kMapDepth;
		currentCollider.enabled = false;
		currentRigidbody.isKinematic = true;
		currentSphereController.IsActive = false;
		playingAnimation = true;
	}

	public static void DeleteSphere(){
		int addCoin = -(PuzzleManager.MicroCoin / 5) < 0 ? -(PuzzleManager.MicroCoin / 5) : 0;
		PuzzleManager.MicroCoin += addCoin;
		var text = (Instantiate(PuzzleManager.WorldSpaceText) as GameObject).GetComponent<WorldSpaceText>();
		text.Text = "Miss.";
		text.WorldPosition = new Vector3(CurrentObj.transform.position.x, -PuzzleManager.CurrentStage, CurrentObj.transform.position.z);
		++PuzzleManager.DeathCount;
		Destroy(CurrentObj);
		CurrentObj = null;
		SoundManager.PlaySE(SoundManager.SE.miss);
		// PuzzleManager.GenerateMap(PuzzleManager.StageData[PuzzleManager.CurrentStage], PuzzleManager.CurrentStage, true);

		PlayerController.IsSpeedUp = false;
	}
}
