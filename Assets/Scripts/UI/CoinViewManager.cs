using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinViewManager : MonoBehaviour {

	[SerializeField]
	GameObject textObject;
	Text _text;

	int currentViewCoin;
	int fixedViewCoin;

	const int kAddViewCoinParFrame = 1;

	void Start(){
		_text = textObject.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		fixedViewCoin = PuzzleManager.MicroCoin;
		if(currentViewCoin < fixedViewCoin){
			currentViewCoin += kAddViewCoinParFrame;
		}else if(currentViewCoin > fixedViewCoin){
			currentViewCoin -= kAddViewCoinParFrame;
		}

		_text.text = (currentViewCoin).ToString();
	}
}
