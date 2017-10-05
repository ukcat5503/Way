using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTextController : MonoBehaviour {
	void Update () {
		DebugText.UpdateInfo("Object", PuzzleManager.GetPuzzleListCount());
		DebugText.UpdateInfo("Point", PuzzleManager.GetDroppedSphere());
	}
}
