 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
	public float movementSpeed;
	public float seconds;
	public GameObject bulletPrefab;
	public bool facingNorth, facingSouth, facingEast, facingWest;//will be used to control which direction bullet faces and moves, as well as animations on the player
	private SpriteRenderer hit;//this will be used to keep the player from being able to move while respawing after being hit by an enemy
	private MazeGeneration maze;
	private Color32 iceColor, markColor;

	void Start(){
		movementSpeed = 5;
		seconds = 1f;
		iceColor = new Color32(109, 163, 249, 255);
		markColor = new Color32 (244, 188, 66, 255);
		maze = GameObject.FindGameObjectWithTag ("Maze").GetComponent<MazeGeneration> ();
		hit = GetComponent<SpriteRenderer> ();

		GameObject[] bullets = new GameObject [10];
		for (int i = 0; i < 10; i++) {
			bullets [i] = Instantiate (bulletPrefab, transform.position, Quaternion.identity, transform);
			bullets[i].tag = "ShieldFish";
			bullets [i].transform.localScale = new Vector3 (0.66f, 0.66f, 0.66f);
			Destroy (bullets [i].GetComponent<BulletMovement> ());
		}
		StartCoroutine(spawnShield (bullets, 0.05f));

	}

	void Update () {
		GameManager.Instance.timeLeft -= Time.deltaTime;
		seconds += Time.deltaTime;
		GameManager.Instance.shieldRegenTime += Time.deltaTime;

		if (seconds >= 1) {
			//Debug.Log ((int)GameManager.Instance.timeLeft);
			seconds = 0f;
		}
		if (GameManager.Instance.timeLeft <= 0) {
			Debug.Log ("Game over!");
			Debug.Log ("Total cells lit up: " + GameManager.Instance.cellsLit);
			Debug.Log ("Levels passed: " + (GameManager.Instance.mazeSize-5));
			GetComponent<SpriteRenderer> ().enabled = false;
			Destroy (this);
			//GameManager.Instance.score = 0;
			//GameManager.Instance.lives = 3;
			//GameManager.Instance.level = 1;
			//GameManager.Instance.mazeSize = 5;
			//GameManager.Instance.cameraSize = 2.5f;
			//GameManager.Instance.timeLeft = 31f;
			//GameManager.Instance.cellsLit = 0;
			SceneManager.LoadScene ("End", LoadSceneMode.Single);
			Cursor.visible = true;
		}

		//instead of setFacing, just change sprite here/play animation
		if ((Input.GetKey (KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && hit.enabled) {
			transform.Translate (new Vector3 (0, -movementSpeed * Time.deltaTime, 0));
			setFacing ("south");
		} else if ((Input.GetKey (KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && hit.enabled) {
			transform.Translate (new Vector3 (0, movementSpeed * Time.deltaTime, 0));
			setFacing ("north");
		} else if ((Input.GetKey (KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && hit.enabled) {
			transform.Translate (new Vector3 (-movementSpeed * Time.deltaTime, 0, 0));
			setFacing ("west");
		} else if ((Input.GetKey (KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && hit.enabled) {
			transform.Translate (new Vector3 (movementSpeed * Time.deltaTime, 0, 0));
			setFacing ("east");
		}

		//debugging controls. Might keep them, might not. This will be important information kind of like the F3 menu in minecraft.
		if (Input.GetKeyDown (KeyCode.I)) {
			Debug.Log (Camera.main.orthographicSize);
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			if (Time.timeScale == 1) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}
		}


		//firing controls
		if ((((Input.GetKey (KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown (KeyCode.Space))&& hit.enabled) && GameManager.Instance.shieldRegenTime >= 10) {
			GameManager.Instance.shieldRegenTime = 0;
			GameObject[] bullets = new GameObject [10];
			for (int i = 0; i < 10; i++) {
				bullets [i] = Instantiate (bulletPrefab, transform.position, Quaternion.identity, transform);
				bullets[i].tag = "ShieldFish";
				bullets [i].transform.localScale = new Vector3 (0.66f, 0.66f, 0.66f);
				Destroy (bullets [i].GetComponent<BulletMovement> ());
			}
			StartCoroutine(spawnShield (bullets));
		}

		else if (Input.GetKeyDown (KeyCode.Space) && hit.enabled) {
			GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity);
			GameManager.Instance.bulletsFired++;
			if (!facingNorth && !facingSouth && !facingEast && !facingWest) {
				bullet.GetComponent<BulletMovement> ().movementSpeed = 0;
				StartCoroutine (spinFish (bullet));
			}
			if (facingNorth) {
				bullet.GetComponent<BulletMovement> ().north = true;
				bullet.transform.Rotate (0, 0, -90);
			}
			if (facingSouth) {
				bullet.GetComponent<BulletMovement> ().south = true;
				bullet.transform.Rotate (0, 0, 90);
			}
			if (facingEast) {
				bullet.GetComponent<BulletMovement> ().east = true;
				bullet.transform.Rotate (0, 0, 180);
				bullet.GetComponent<SpriteRenderer> ().flipY = true;
				//bullet.transform.position = Vector3.Reflect(bullet.transform.position, Vector3.right);
			}
			if (facingWest) {
				bullet.GetComponent<BulletMovement> ().west = true;
			}
		}
			

		//cheats
		//this may end up being a power up or something later, but right now it's just to help me mark dead ends so I can test big mazes quicker
		if (Input.GetKeyDown (KeyCode.M) || Input.GetKeyDown(KeyCode.E)) {
			float x = transform.position.x;
			float y = transform.position.y;
			//round to nearest int
			x += 0.5f;
			y += 0.5f;
			GameObject currentCell = GameObject.FindGameObjectWithTag ("Maze").GetComponent<MazeGeneration> ().cells [(int)x, (int)y];
			currentCell.GetComponent<CellManagement> ().visited = false;

			Color32 cellColor = currentCell.GetComponent<SpriteRenderer> ().color;
			currentCell.GetComponent<SpriteRenderer>().color = cellColor.Equals (iceColor) ? markColor : iceColor;
			//currentCell.GetComponent<SpriteRenderer> ().color = markColor;
		}

		if (Input.GetKey (KeyCode.C) && Input.GetKey(KeyCode.H)) {
			Debug.Log ("Cheat");
			try{
				GameObject fish = GameObject.FindGameObjectWithTag ("GoalFish");
				transform.position = fish.transform.position;
			}catch(NullReferenceException exception){
				Debug.Log ("No object found" + exception.StackTrace);
			}
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			GameManager.Instance.timeLeft += 30;
		}

		//this one is being used so I can light up all of the cells in the scene very easily and precisely. Just disables my collider so I can go through walls
		if (Input.GetKeyDown (KeyCode.X)) {
			CircleCollider2D collider = GetComponent<CircleCollider2D> ();
			collider.enabled = !collider.enabled;
		}


		//scene controls
		if (Input.GetKeyDown (KeyCode.Q)) {
			Cursor.visible = true;
			returnToMenu ();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}//end update

	IEnumerator spinFish(GameObject bullet){
		int rotateAmount = 3;//degrees of rotation
		for (int i = 0; i < 360/rotateAmount; i++) {
			if (bullet != null) {
				bullet.transform.Rotate (0, 0, rotateAmount);
				yield return new WaitForSeconds (0.01f);
			}
		}
		Destroy (bullet);
		gameObject.layer = 11;
	}

	IEnumerator spinFish(GameObject bullet, int rotateAmount){
		for (int i = 0; i < 360/rotateAmount; i++) {
			if (bullet != null) {
				bullet.transform.Rotate (0, 0, rotateAmount);
				yield return new WaitForSeconds (0.01f);
			}
		}
		Destroy (bullet);
		gameObject.layer = 11;
	}

	IEnumerator spawnShield(GameObject[] bullets){
		gameObject.layer = 12;
		for (int i = 0; i < bullets.Length; i++) {
			if (bullets [i] != null) {
				bullets [i].gameObject.layer = 13;
				bullets [i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
				bullets [i].AddComponent<ForceChildLocation> ();
				StartCoroutine (spinFish (bullets [i]));
				yield return new WaitForSeconds (0.1f);
			}
		}
		//gameObject.layer = 11;
	}

	IEnumerator spawnShield(GameObject[] bullets, float waitTime){
		gameObject.layer = 12;
		for (int i = 0; i < bullets.Length; i++) {
			if (bullets [i] != null) {
				bullets [i].gameObject.layer = 13;
				bullets [i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
				bullets [i].AddComponent<ForceChildLocation> ();
				StartCoroutine (spinFish (bullets [i], 6));
				yield return new WaitForSeconds (waitTime);
			}
		}
		//gameObject.layer = 11;
	}


	void setFacing(string direction){
		facingNorth = false;
		facingSouth = false;
		facingEast = false;
		facingWest = false;

		switch (direction) {
		case "north":
			facingNorth = true;
			break;
		case "south":
			facingSouth = true;
			break;
		case "east":
			facingEast = true;
			break;
		case "west":
			facingWest = true;
			break;
		default:
			Debug.Log ("Not facing any direction");
			break;
		}
	}


	void returnToMenu(){
		/*
		 score = 0;
		lives = 3;
		level = 1;
		mazeSize = 5;
		cameraSize = 2.5f;
		timeLeft = 31f;
		cellsLit = 0;
		 */
		GameManager.Instance.score = 0;
		GameManager.Instance.lives = 3;
		GameManager.Instance.level = 1;
		GameManager.Instance.mazeSize = 5;
		GameManager.Instance.cameraSize = 2.5f;
		GameManager.Instance.timeLeft = 31f;
		GameManager.Instance.cellsLit = 0;
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}


	IEnumerator respawnPlayer(GameObject player){
		player.GetComponent<SpriteRenderer> ().enabled = false;
		player.GetComponent<CircleCollider2D> ().enabled = false;
		GameManager.Instance.lives--;

		yield return new WaitForSeconds (1);
		player.transform.position = new Vector3 (maze.width / 2, maze.height / 2, player.transform.position.z);
		player.GetComponent<SpriteRenderer> ().enabled = true;
		player.GetComponent<CircleCollider2D> ().enabled = true;


		GameObject[] bullets = new GameObject [10];
		for (int i = 0; i < 10; i++) {
			bullets [i] = Instantiate (bulletPrefab, transform.position, Quaternion.identity, transform);
			bullets[i].tag = "ShieldFish";
			bullets [i].transform.localScale = new Vector3 (0.66f, 0.66f, 0.66f);
			Destroy (bullets [i].GetComponent<BulletMovement> ());
		}
		StartCoroutine(spawnShield (bullets, 0.05f));
	}

	void OnCollisionEnter2D(Collision2D collision){
		string tag = collision.gameObject.tag;

		if (tag.Equals ("Enemy")) {
			StartCoroutine (respawnPlayer (gameObject));
		}
	}


}
