using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinViewManager : MonoBehaviour {

	[SerializeField]
	GameObject textObject;
	Text _text;

	void Start(){
		_text = textObject.GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		// PuzzleManager.Coin.Log();
		_text.text = ((int)(PuzzleManager.Coin * 1000f + 0.1f)).ToString();
	}
}
