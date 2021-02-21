using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldAvailable : MonoBehaviour {


	void Update () {
		if (GameManager.Instance.shieldRegenTime < 10) {
			GetComponent<Text> ().enabled = false;
		} else {
			GetComponent<Text> ().enabled = true;
		}
	}
}
