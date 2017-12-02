using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(RectTransform))]
[RequireComponent (typeof(Image))]
/// <summary>
/// ロゴを表示するためのクラスです。
/// シーンに配置されるとすぐに動作します。
/// 常に60fpsで動くことを前提に設計しています。
/// </summary>
public class DisplayLogo : MonoBehaviour {

	[SerializeField]
	int takeFrameToFadein = 60;
	[SerializeField]
	int viewImageDurationFrame = 60 * 3;
	[SerializeField]
	int takeFrameToFadeout = 60;

	[SerializeField, Space(8)]
	string targetCanvasName = "Canvas";
	
	int frame;

	RectTransform canvasRectTransform;
	RectTransform _rectTransform;
	Image _image;

	void Start () {
		// 必要なオブジェクトなどを参照
		if(!(canvasRectTransform = GameObject.Find(targetCanvasName).GetComponent<RectTransform>())){
			Debug.LogWarning(targetCanvasName + "オブジェクトが見つかりませんでした。");
			Destroy(gameObject);
		}

		if(!(_rectTransform = GetComponent<RectTransform>())){
			Debug.LogWarning(targetCanvasName + "UnityEngine.RectTransformが取得できませんでした。");
			Destroy(gameObject);
		}

		if(!(_image = GetComponent<Image>())){
			Debug.LogWarning("UnityEngine.UI.Imageが取得できませんでした。");
			Destroy(gameObject);
		}

		// Canvasの親にする
		_rectTransform.SetParent(canvasRectTransform);

		// 位置とかサイズの設定
		_rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		_rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		_rectTransform.pivot = new Vector2(0.5f, 0.5f);
		_rectTransform.sizeDelta = new Vector2(1920, 1080);
		_rectTransform.localPosition = new Vector3(0f, 0f, 0f);

		// 画像を消しておく
		_image.color = new Color(0f, 0f, 0f, 1f);
	}
	
	void Update () {
		if(frame < takeFrameToFadein){
			// フェードイン処理
			var c = _image.color.r;
			c += 1f / takeFrameToFadein;
			_image.color = new Color(c, c, c, 1f);

		}else if(frame > takeFrameToFadein + viewImageDurationFrame){
			// フェードアウト処理
			var c = _image.color.r;
			c -= 1f / takeFrameToFadeout;
			_image.color = new Color(c, c, c, 1f);
			if(_image.color.r < 0f){
				// 画像消えきったらオブジェクトを消す
				Destroy(gameObject);
			}
		}
		++frame;
	}
}
