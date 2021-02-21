using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTime : MonoBehaviour {
	private Text timeLeft;


	void Start () {
		timeLeft = GetComponent<Text> ();
	}

	void Update () {
		int seconds = (int)GameManager.Instance.timeLeft%60;
		int minutes = (int)GameManager.Instance.timeLeft / 60;
		string time = minutes + ":" + seconds.ToString("00"); 
		timeLeft.text = (string.Format ("Time left: {0}", time));
	}
}
