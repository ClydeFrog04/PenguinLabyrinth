using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {

	public bool north, south, east, west;
	public float movementSpeed;


	void Start () {
		
	}


	void Update () {
		transform.Translate (new Vector3 ((-1)*movementSpeed * Time.deltaTime, 0, 0));//Originally planned on using nsew vars to handle movement direction but 
		//since I am just rotating the "bullet", the movement translate can be the same for all
	}


	void OnCollisionEnter2D(Collision2D collision){
		string tag = collision.gameObject.tag;

		if (tag.Equals ("Wall")) {
			Destroy(gameObject);
		}
	}
}
