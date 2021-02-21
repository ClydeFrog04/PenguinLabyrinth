using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour {

	public GameObject mazePrefab;

	void OnCollisionEnter2D(Collision2D collsion){
		string tag = collsion.gameObject.tag;

		if (tag.Equals ("GoalFish")) {
			GameManager.Instance.timeLeft += GameManager.Instance.endLevelTime;//15f;//give the player more time every maze completed
			Debug.Log ("YOU WIN!");
			GameManager.Instance.level++;
			Destroy (GameObject.FindGameObjectWithTag (tag));
			//GameObject.Find ("Maze").GetComponent<MazeGeneration> ().spawnNewEnemy ();
			//Destroy (GameObject.FindGameObjectWithTag ("Maze"));
			GameManager.Instance.mazeSize++;
			if (GameManager.Instance.mazeSize % 5 == 0) {
				GameManager.Instance.cameraSize += 1f;
			}
			//moved to cell lighting controller
			//GameManager.Instance.cellsLit += GameObject.FindGameObjectsWithTag ("VisitedCell").Length;
			//GameManager.Instance.cellsLit += 1000;

			SceneManager.LoadScene ("Main", LoadSceneMode.Single);
			//Instantiate (mazePrefab, new Vector3 (0, 0, 0), Quaternion.identity);
		}

		if (tag.Equals ("Enemy")) {
			if (GameManager.Instance.lives < 0) {
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
		}
	}
}
