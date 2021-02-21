using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLeftAI : MonoBehaviour {

	private Vector3 spawnPoint;
	private string currentDirection;
	private float movementSpeed;


	void Start () {
		currentDirection = "south";
		movementSpeed = 1f;
		moveSouth ();
		//MazeGeneration mazeGen = transform.parent.GetComponent<MazeGeneration> ();
		//spawnPoint = new Vector3 (0, mazeGen.height - 1, transform.parent.position.z);
		GameObject fish = GameObject.FindGameObjectWithTag("GoalFish");
		spawnPoint = new Vector3 (fish.transform.position.x, fish.transform.position.y, transform.parent.position.z);
		transform.position = spawnPoint;
		GetComponent<SpriteRenderer> ().sortingOrder = 5;
	}

	void Update () {

		switch (currentDirection) {
		case "north":
			moveNorth ();
			break;
		case "south":
			moveSouth ();
			break;
		case "east":
			moveEast ();
			break;
		case "west":
			moveWest ();
			break;
		default:
			Debug.Log ("No direction");
			break;
		}
		
	}
	//need to center frog in current cell before moving
	void centerInCell(){
		float x = transform.position.x;
		float y = transform.position.y;
		//round to nearest int
		x += 0.5f;
		y += 0.5f;
		transform.position = new Vector3 ((int)x, (int)y, transform.parent.position.z);
	}

	void moveNorth(){
		currentDirection = "north";
		//centerInCell ();
		transform.Translate(new Vector3 (0, movementSpeed * Time.deltaTime, 0));
	}

	void moveSouth(){
		currentDirection = "south";
		//centerInCell ();
		transform.Translate (new Vector3 (0, -movementSpeed * Time.deltaTime, 0));

	}

	void moveEast(){
		currentDirection = "east";
		//centerInCell ();
		transform.Translate (new Vector3 (movementSpeed * Time.deltaTime, 0, 0));
	}

	void moveWest(){
		currentDirection = "west";

		transform.Translate (new Vector3 (-movementSpeed * Time.deltaTime, 0, 0));
	}


	void getRandomDirection(){
		switch (Random.Range (0, 4)) {
		case 0:
			currentDirection = "north";
			break;
		case 1:
			currentDirection = "south";
			break;
		case 2:
			currentDirection = "east";
			break;
		case 3:
			currentDirection = "west";
			break;
		default:
			break;
		}

	}

	void OnCollisionEnter2D(Collision2D collision){
		string tag = collision.gameObject.tag;
		if (tag.Equals ("Wall")) {
			getRandomDirection ();
			switch (currentDirection) {
			case "north":
				//Debug.Log (currentDirection);
				currentDirection = "west";
				centerInCell ();
				moveWest ();
				break;
			case "south":
				//Debug.Log (currentDirection);
				currentDirection = "east";
				centerInCell ();
				moveEast ();
				break;
			case "east":
				//Debug.Log (currentDirection);
				currentDirection = "north";
				centerInCell ();
				moveNorth ();
				break;
			case "west":
				//Debug.Log (currentDirection);
				currentDirection = "south";
				centerInCell ();
				moveSouth ();
				break;
			default:
				Debug.Log ("No direction");
				break;
			}
		}
	}
}
