using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLighting : MonoBehaviour {

	private GameObject[] cells;
	private Color32 iceColor;
	public Sprite snowSprite;
	MazeGeneration maze;
	private int radius;//this will be the number of cells around the player get lit up

	void Start () {
		maze = GameObject.FindGameObjectWithTag ("Maze").GetComponent<MazeGeneration>();
		cells = GameObject.FindGameObjectsWithTag ("VisitedCell");
		iceColor = new Color32(109, 163, 249, 255);
		radius = 1;//the grid size will be radius+2. e.g. if radius is 1, the grid around the player will be a 3x3
		lightNeighbors ();

		foreach (GameObject cell in cells) {
			cell.tag = "Cell";
			SpriteRenderer cellSprite = cell.GetComponent<SpriteRenderer> ();
			cell.GetComponent<CellManagement> ().visited = false;
			cellSprite.color = Color.black;
		}
	}


	void Update () {
		lightNeighbors ();

		//only for testing
		if (Input.GetKeyDown (KeyCode.U)) {
			foreach (GameObject cell in cells) {
				cell.GetComponent<SpriteRenderer> ().color = iceColor;
			}
		}
	}


	void lightNeighbors(){
		int playerX = (int)(transform.position.x + 0.5f);
		int playerY = (int)(transform.position.y + 0.5f);


		//check if coord is valid
		if (playerX - radius < 0)
			playerX += radius;
		else if (playerX + radius > maze.width-radius)
			playerX -= radius;

		if (playerY - radius < 0)
			playerY += radius;
		else if (playerY + radius > maze.height-radius)
			playerY -= radius;


		for (int i = playerX - radius; i <= playerX + radius; i++) {
			for (int j = playerY - radius; j <= playerY + radius; j++) {
				//maze.cells [i, j].GetComponent<CellManagement> ().visited = true;
				GameObject cell = maze.cells [i,j];
				if (cell.tag.Equals ("Cell")) {
					cell.GetComponent<SpriteRenderer> ().color = iceColor;
					cell.GetComponent<SpriteRenderer>().sprite = snowSprite;
					maze.cells [i, j].tag = "VisitedCell";
					GameManager.Instance.cellsLit++;
					//Debug.Log ("Cell added to count" + GameManager.Instance.cellsLit);
				}
			}
		}

	}//end light neighbors


}
