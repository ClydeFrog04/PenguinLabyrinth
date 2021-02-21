using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebugging : MonoBehaviour {

	private GameObject player, goal;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		goal = GameObject.FindGameObjectWithTag ("GoalFish");
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Tab)) {
			player.transform.position = goal.transform.position;
		}
	}

}
