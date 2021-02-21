using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour {

	public GameObject stingrayPrefab;
	private GameObject enemy, player;
	private bool hasSpawned;

	void Start () {
		stingrayPrefab.GetComponent<SpriteRenderer> ().sortingOrder = 10;
		StartCoroutine (spawnNewEnemy ());
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	

	void Update () {
		if ((!enemy && hasSpawned) && GameManager.Instance.level > 5) {
			hasSpawned = false;
			Debug.Log ("Spawning new enemy");
			StartCoroutine(spawnNewEnemy ());
		}
	}


	public IEnumerator spawnNewEnemy(){
		//this waits for the maze to be spawned before spawning enemy, also giving some randomness to when enemies spawn in
		int waitTime = Random.Range (1, 3);
		yield return new WaitForSeconds (waitTime);
		hasSpawned = true;
		int spawnX = Random.Range (0, GetComponent<MazeGeneration>().width);
		int spawnY = Random.Range (0, GetComponent<MazeGeneration> ().height);

		while (spawnX == player.transform.position.x) {
			spawnX = Random.Range (0, GetComponent<MazeGeneration>().width);
		}
		while (spawnY == player.transform.position.y) {
			spawnY = Random.Range (0, GetComponent<MazeGeneration>().height);
		}

		Vector3 spawnPoint = new Vector3 (spawnX, spawnY, transform.position.z);
		enemy = Instantiate (stingrayPrefab, spawnPoint, Quaternion.identity, transform);
	}
}
