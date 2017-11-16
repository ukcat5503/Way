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

	void Start(){
		_text = textObject.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		fixedViewCoin = PuzzleManager.MicroCoin;
		if(currentViewCoin < fixedViewCoin){
			currentViewCoin += 1;
		}

		_text.text = (currentViewCoin).ToString() + "m";
	}
}
