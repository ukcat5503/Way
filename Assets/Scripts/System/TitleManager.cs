using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	enum TitleState{
		FadeIn,
		Waiting,
		FadeOut
	}

	Image _image;

	float timeElapsed;

	TitleState state;

	string nextSceneName = "";

	[SerializeField]
	string ImageObjName;

	[SerializeField, Space(8)]
	string gameSceneName;
	[SerializeField]
	string logoSceneName;

	void Start(){
		state = TitleState.FadeIn;
		_image = GameObject.Find(ImageObjName).GetComponent<Image>();
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
	}

	void Update () {
		timeElapsed += Time.deltaTime;
		timeElapsed.Log();

		if(state == TitleState.Waiting && ( Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))){
			// ゲームへ
			nextSceneName = gameSceneName;
			state = TitleState.FadeOut;
		}else if(state == TitleState.Waiting && timeElapsed > 60){
			// 学校ロゴへ
			nextSceneName = logoSceneName;
			state = TitleState.FadeOut;
		}

		if(state == TitleState.FadeIn){
			fadeIn();
		}
		if(state == TitleState.FadeOut){
			fadeOut();
		}
	}

	void fadeIn(){
		var a = _image.color.a;
		a -= 1f / 60f;
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);

		if(_image.color.a < 0){
			state = TitleState.Waiting;
		}
	}

	void fadeOut(){
		var a = _image.color.a;
		a += 1f / 60f;
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);

		if(_image.color.a > 1){
			SceneManager.LoadScene(nextSceneName);
		}
	}
}
