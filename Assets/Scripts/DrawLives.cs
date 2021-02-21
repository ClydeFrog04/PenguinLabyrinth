using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLives : MonoBehaviour {
	private int localLives;// = GameManager.Instance.lives;
	private Text livesText;

	void Start () {
		localLives = GameManager.Instance.lives;
		livesText = GetComponent<Text> ();
		livesText.text = "Lives left: " + GameManager.Instance.lives;
	}
	

	void Update () {
		if (localLives != GameManager.Instance.lives) {
			livesText.text = "Lives left: " + GameManager.Instance.lives;
		}
	}
}
