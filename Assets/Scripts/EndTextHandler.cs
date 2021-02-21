using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTextHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string output = "You completed: " + (GameManager.Instance.level - 1) + " levels";
		Debug.Log (GameManager.Instance.level);
		GetComponent<Text> ().text = output;
	}
}
