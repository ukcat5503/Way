using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	Image _image;

	float timeElapsed;

	string nextSceneName = "";

	[SerializeField]
	string ImageObjName;

	[SerializeField, Space(8)]
	string gameSceneName;
	[SerializeField]
	string logoSceneName;

	void Start(){
		_image = GameObject.Find(ImageObjName).GetComponent<Image>();
	}

	void Update () {
		timeElapsed += Time.deltaTime;
		timeElapsed.Log();

		if(nextSceneName == "" && ( Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))){
			// ゲームへ
			"ゲームへ".Log();
			nextSceneName = gameSceneName;
		}else if(nextSceneName == "" && timeElapsed > 60){
			// 学校ロゴへ
			nextSceneName = logoSceneName;
		}

		if(nextSceneName != ""){
			blackOut();
		}
	}

	void blackOut(){
		var c = _image.color.a;
		c += 1f / 60f;
		_image.color = new Color(0, 0, 0, c);

		if(_image.color.a > 1){
			SceneManager.LoadScene(nextSceneName);
		}
	}
}
