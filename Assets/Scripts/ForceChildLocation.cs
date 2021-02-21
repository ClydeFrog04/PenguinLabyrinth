using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceChildLocation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent) {
			transform.position = transform.parent.transform.position;
		}
	}
}
