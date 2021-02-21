using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHud : MonoBehaviour {


	void Start () {
		if (!GameManager.Instance.hudOn) {
			this.gameObject.SetActive (false);
		}
	}

}
