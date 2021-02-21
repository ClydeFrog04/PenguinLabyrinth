using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour {

	/*
		//parent camera to player
	GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
	camera.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
	GameObject player = GameObject.FindGameObjectWithTag ("Player");
	camera.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, camera.transform.position.z);
	//camera.transform.position = GameObject.FindGameObjectWithTag ("Player").transform.position + offset;
	*/
	void Start () {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		camera.transform.parent = player.transform;
		camera.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, camera.transform.position.z);

		//as of right now, a camera size of 2.5 seems to be a good fit. This might change later on though, or maybe even by each level

		Camera.main.orthographicSize = GameManager.Instance.cameraSize;//2.5f;

	}

	void Update(){
		if (Input.GetKey (KeyCode.Equals)) {
			Camera.main.orthographicSize -= 0.1f;
		} else if (Input.GetKey (KeyCode.Minus)) {
			Camera.main.orthographicSize += 0.1f;
		}
	}
	

}
