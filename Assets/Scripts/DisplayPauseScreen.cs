using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPauseScreen : MonoBehaviour {

	private Text pauseText;

	void Start () {
		pauseText = GetComponent<Text> ();
		if (Time.timeScale == 1) {
			pauseText.enabled = false;
			transform.parent.GetComponent<Image> ().enabled = false;
		}
	}


	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			if (Time.timeScale == 1) {
				pauseText.enabled = false;
				transform.parent.GetComponent<Image> ().enabled = false;
			} else {
				pauseText.enabled = true;
				transform.parent.GetComponent<Image> ().enabled = true;
			}
		}
	}
}
