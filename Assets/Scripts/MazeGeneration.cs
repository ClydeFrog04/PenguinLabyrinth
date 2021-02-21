//My own solution for generating a maze. Many tries using algorithms I found online didn't work so finally just sat down and figured it out on my own.
//This is a depth first maze generation.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour {

	public GameObject cellPrefab, guideCell, playerPrefab, goalFishPrefab;
	public GameObject[,] cells;
	private int unvisitedCells;
	private Stack pathHistory;
	private CellManagement cellProps;//the cellProperties variable was needed because for whatever reason, if I set the variables in the cellManagement script, then edit them here, they don't get updated. But with it they do. I don't know...
	public int width, height;
	public bool useMazeSize, generateMaze;



	void Awake () {
		pathHistory = new Stack ();
		//initialize grid for maze
		if (useMazeSize) {
			width = GameManager.Instance.mazeSize;
			height = GameManager.Instance.mazeSize;
		}
		cells = new GameObject[width, height];
		GameObject parent = GameObject.FindGameObjectWithTag ("Maze");
		Debug.Log ("Level: " + (GameManager.Instance.mazeSize-4));


		//fill maze with unvisited cell prefabs
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Vector3 spawnPoint = new Vector3 (x, y, parent.transform.position.z);
				cells [x, y] = Instantiate (cellPrefab, spawnPoint, Quaternion.identity, parent.transform);
				cells [x, y].tag = "UnvisitedCell";
				cellProps = cells [x, y].GetComponent<CellManagement> ();
				cellProps.visited = false;
				cellProps.north = true;
				cellProps.south = true;
				cellProps.east = true;
				cellProps.west = true;

			}
		}//end grid for maze initialization
		unvisitedCells = GameObject.FindGameObjectsWithTag("UnvisitedCell").Length;

		//guideCell = cells [width / 2, height / 2];//center. These are really easy paths to the fish, though sometimes long.
		//guideCell = cells [0, height - 1];//top left. Moderate difficulty
		guideCell = cells[Random.Range((int)(width*0.75f),width), Random.Range((int)(width*0.75f),height)];//Random cell. These are the most difficult paths

		//comment out following lines to not generate maze
		if (generateMaze) {
			createMaze ();
			spawnPlayer ();
			spawnFish ();
		}



	}

	void spawnFish (){
		//spawn goalFish at a random point within 5 cells from the border
		int fishX, fishY;
		fishX = Random.Range (0, 10) % 2 == 0 ? Random.Range (0, 5) : Random.Range (width - 5, width);
		fishY = Random.Range (0, 10) % 2 == 0 ? Random.Range (0, 5) : Random.Range (height - 5, height);


		while (fishX == (int)GameObject.FindGameObjectWithTag ("Player").transform.position.x) {
			fishX = Random.Range (0, 10) % 2 == 0 ? Random.Range (0, 5) : Random.Range (width - 5, width);
		}
		while (fishY == (int)GameObject.FindGameObjectWithTag ("Player").transform.position.y) {
			fishY = Random.Range (0, 10) % 2 == 0 ? Random.Range (0, 5) : Random.Range (height - 5, height);
		}
		Vector3 fishSpawnPoint = new Vector3 (fishX, fishY, transform.position.z);
		Instantiate (goalFishPrefab, fishSpawnPoint, Quaternion.identity);
	}

	void spawnPlayer(){
		//spawn player
		if (GameObject.FindGameObjectWithTag ("Player") == null) {
			Vector3 playerSpawnPoint = new Vector3 (width / 2, height / 2, transform.position.z);
			Instantiate (playerPrefab, playerSpawnPoint, Quaternion.identity);
		}  else {
			Vector3 playerSpawnPoint = new Vector3 (width / 2, height / 2, transform.position.z);
			GameObject.FindGameObjectWithTag ("Player").transform.position = playerSpawnPoint;
		}
		GameObject.FindGameObjectWithTag ("Player").GetComponent<FindPath> ().enabled = true;
		//setup camera for position control
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.AddComponent<CameraPositionController> ();
	}


	void manualGenerate(){
		//this is for testing
		trySouth();
		tryEast ();
		trySouth ();
		tryWest ();
		tryNorth ();
		tryEast ();
		trySouth ();
		trySouth ();
		tryEast ();
		tryNorth ();
		tryEast ();
		tryNorth ();
		tryNorth ();
		tryEast ();
		trySouth ();
		trySouth ();
	}




	void createMaze(){
		//the main logic for generating the maze
		while (unvisitedCells > 0) {//while there are unvisited cells
			if (getValidNeighborCount () > 0) {//if that cell has unvisited neighbors
				//pick a random direction and try moving there
				int direction = Random.Range (0, 4);
				switch (direction) {
				case 0:
					tryNorth ();
					break;
				case 1:
					trySouth ();
					break;
				case 2:
					tryEast ();
					break;
				case 3:
					tryWest ();
					break;
				default:
					break;
				}
			}//end if have valid neighbors
			else {
				if (pathHistory.Count > 0) {
					guideCell.tag = "VisitedCell";//this is a dead end cell. Mark it as visited that keep moving
					guideCell = pathHistory.Pop () as GameObject;
					guideCell.tag = "VisitedCell";
				}  else
					break;
			}
			unvisitedCells = GameObject.FindGameObjectsWithTag ("UnvisitedCell").Length;
		}
	}//end createMaze


	//these methods will check to see if direction given is valid, if it is it will set all properties acordingly, if not it will do nothing.
	void tryNorth(){
		int currentX = (int)guideCell.transform.position.x;
		int currentY = (int)guideCell.transform.position.y+1;
		if ((currentX >= 0 && currentX < width) && (currentY >= 0 && currentY < height)) {
			//coords are in bounds, now check if cell at that position has been visited or not
			if (cells [currentX, currentY].GetComponent<CellManagement> ().visited == false) {
				pathHistory.Push (guideCell);
				guideCell.tag = "VisitedCell";
				cellProps = guideCell.GetComponent<CellManagement> ();
				cellProps.visited = true;
				cellProps.north = false;
				cellProps = cells [currentX, currentY].GetComponent<CellManagement> ();
				cellProps.visited = true;
				cellProps.south = false;
				guideCell = cells [currentX, currentY];
			}
		}
	}
	void trySouth(){
		int currentX = (int)guideCell.transform.position.x;
		int currentY = (int)guideCell.transform.position.y - 1;
		if((currentX >= 0 && currentX < width) && (currentY >= 0 && currentY < height)){
			if (cells [currentX, currentY].GetComponent<CellManagement> ().visited == false) {
				pathHistory.Push (guideCell);
				guideCell.tag = "VisitedCell";
				//it gets a bit confusing but here's how it works:
				cellProps = guideCell.GetComponent<CellManagement>();//set cellProps to be the props of the guideCell
				cellProps.visited = true;//mark it visited
				cellProps.south = false;//get rid of it's south wall because we are moving south. This will always be the direction we are moving
				cellProps = cells[currentX,currentY].GetComponent<CellManagement>();//set cellProps to the new cell
				cellProps.visited = true;//mark this one as visited
				cellProps.north = false;//get rid of it's NORTH wall since we just came from the north. This will always be the opposite direction we are moving
				guideCell = cells [currentX, currentY];//only set new guideCell if we can move. Otherwise try again
			}
		}

	}
	void tryEast(){
		int currentX = (int)guideCell.transform.position.x+1;
		int currentY = (int)guideCell.transform.position.y;
		if ((currentX >= 0 && currentX < width) && (currentY >= 0 && currentY < height)) {
			if (cells [currentX, currentY].GetComponent<CellManagement> ().visited == false) {
				pathHistory.Push (guideCell);
				guideCell.tag = "VisitedCell";
				cellProps = guideCell.GetComponent<CellManagement> ();
				cellProps.visited = true;
				cellProps.east = false;
				cellProps = cells [currentX, currentY].GetComponent<CellManagement> ();
				cellProps.visited = true;
				cellProps.west = false;
				guideCell = cells [currentX, currentY];
			}
		}

	}
	void tryWest(){
		int currentX = (int)guideCell.transform.position.x-1;
		int currentY = (int)guideCell.transform.position.y;
		if ((currentX >= 0 && currentX < width) && (currentY >= 0 && currentY < height)) {
			if (cells [currentX, currentY].GetComponent<CellManagement> ().visited == false) {
				pathHistory.Push (guideCell);
				guideCell.tag = "VisitedCell";
				cellProps = guideCell.GetComponent<CellManagement> ();
				cellProps.visited = true;
				cellProps.west = false;
				cellProps = cells [currentX, currentY].GetComponent<CellManagement> ();
				cellProps.visited = true;
				cellProps.east = false;
				guideCell = cells [currentX, currentY];
			}
		}
	}


	int getValidNeighborCount(){
		//this checks the neighbors of the guidecell to see if they have bee visted or not, and checks if the possible neighbor is actually in the maze or not.
		int currentX = (int)guideCell.transform.position.x;
		int currentY = (int)guideCell.transform.position.y;
		int validCellCount = 0;

		//check north
		if (((currentX >= 0 && currentX < width) && (currentY+1 >= 0 && currentY+1 < height))) {
			//cell is in the maze
			GameObject testSubject = cells[currentX, currentY+1];
			if (!testSubject.GetComponent<CellManagement> ().visited) {
				validCellCount++;
			}
		}
		//check south
		if (((currentX >= 0 && currentX < width) && (currentY-1 >= 0 && currentY-1 < height))) {
			//cell is in the maze
			GameObject testSubject = cells[currentX, currentY-1];
			if (!testSubject.GetComponent<CellManagement> ().visited) {
				validCellCount++;
			}
		}
		//check east
		if (((currentX+1 >= 0 && currentX+1 < width) && (currentY >= 0 && currentY < height))) {
			//cell is in the maze
			GameObject testSubject = cells[currentX+1, currentY];
			if (!testSubject.GetComponent<CellManagement> ().visited) {
				validCellCount++;
			}
		}
		//check west
		if (((currentX-1 >= 0 && currentX-1 < width) && (currentY >= 0 && currentY < height))) {
			//cell is in the maze
			GameObject testSubject = cells[currentX-1, currentY];
			if (!testSubject.GetComponent<CellManagement> ().visited) {
				validCellCount++;
			}
		}
		return validCellCount;
	}

}
