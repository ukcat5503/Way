using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WakeUpApplication : MonoBehaviour {

	[SerializeField]
	GameObject[] instantiateObjects;

	[SerializeField]
	string nextSceneName;

	void Awake(){
		Application.targetFrameRate = 60;
	}
	
	void Start () {
		foreach (var item in instantiateObjects)
		{
			Instantiate(item);
		}
	}
	
	void LateUpdate () {
		// 各オブジェクトのStartやUpdateを１フレームだけ待ちたいのでLateで遷移処理する。
		SceneManager.LoadScene(nextSceneName);
	}
}
