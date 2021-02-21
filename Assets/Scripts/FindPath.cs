//this script will find a path from one maze cell to another
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour {

	//these will be the cells to create a path between. I plan to use this script primarilly for enemy path finding AI
	private GameObject startCell, endCell, guideCell;
	private MazeGeneration maze;
	private Stack pathHistory;
	private bool pathFound;
	private List<string> validNeighbors;
	private const string NORTH = "north";
	private const string SOUTH = "south";
	private const string EAST = "east";
	private const string WEST = "west";



	// Use this for initialization
	void Start () {
		maze = GameObject.FindGameObjectWithTag ("Maze").GetComponent<MazeGeneration> ();
		pathFound = false;
		validNeighbors = new List<string> ();
		pathHistory = new Stack ();
		//findPath ();
		//drawSolution ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			findPath ();
			drawSolution ();
		}
	}


	void findPath(){
		Debug.Log ("********NEW PATH********");
		//once I get this working I may create a similar function that has the game objects passed in, so I'm not calling findwithtag every frame.
		//this will only be a problem when attached to enemies, since the player position will always be changing.
		startCell = GameObject.FindGameObjectWithTag ("Player");//side note, this really only needs to be initialized once, because it will update the players position without this being called multiple times
		endCell = GameObject.FindGameObjectWithTag ("GoalFish");
		guideCell = maze.cells[(int)(startCell.transform.position.x+0.5f),(int)(startCell.transform.position.y+0.5f)];
		pathHistory.Push (guideCell);
		checkNeighbors ();


		//begin path finding logic.
		int manualBreak = 0;
		while (!guideCell.Equals (endCell)) {
		//while(guideCell.transform.position.x != endCell.transform.position.x && guideCell.transform.position.y != endCell.transform.position.y){
			//Debug.Log (string.Format ("guideX = {0}, guideY = {1}", guideCell.transform.position.x, guideCell.transform.position.y));
			Debug.Log("Guide cell coords: " + guideCell.transform.position.x + "," + guideCell.transform.position.y);
			Debug.Log ("End cell coords: " + endCell.transform.position.x + "," + endCell.transform.position.y);
			int guideX = (int)guideCell.transform.position.x;
			int guideY = (int)guideCell.transform.position.y;
			int endX = (int)endCell.transform.position.x;
			int endY = (int)endCell.transform.position.y;
			if((guideX == endX)&&(guideY == endY)) break;
			checkNeighbors ();
			if (guideCell.Equals (endCell)) {
				pathFound = true;
				break;
			}

			else if (validNeighbors.Count > 0) {
				switch (validNeighbors [0]) {
				case NORTH:
					//Debug.Log (NORTH);
					moveNorth ();
					break;
				case SOUTH:
					//Debug.Log (SOUTH);
					moveSouth ();
					break;
				case EAST:
					//Debug.Log (EAST);
					moveEast ();
					break;
				case WEST:
					//Debug.Log (WEST);
					moveWest ();
					break;
				default:
					break;
				}
			} else if (guideCell.Equals (endCell)) {
				Debug.Log ("$$$$$$$$$$$$$$$$$$Path FOUND");
				pathFound = true;
				break;
			} else if (pathHistory.Count <= 0) {
				Debug.Log ("Path history empty- breaking here");
//				guideCell = maze.cells [maze.width / 2, maze.height / 2];
//				checkNeighbors ();
//				if (validNeighbors.Count <= 0)
//					break;
				break;
			} 
//			else if (!pathFound && pathHistory.Count <= 0) {
//				guideCell = maze.cells [maze.width / 2, maze.height / 2];
//			}
			else {
				//Debug.Log (pathHistory.Count + " PATH COUNT");
				guideCell = pathHistory.Pop () as GameObject;
			}
			manualBreak++;
//			if (manualBreak >= 20) {
//				break;
//			}
		}


	}

	void manualTest(){
		moveNorth ();
		moveNorth ();
		moveEast ();
		moveEast ();
		moveSouth ();
		moveSouth ();
		moveWest ();
		moveWest ();
	}


	void drawSolution(){
		Debug.Log (pathHistory.Count + " Path length");
		while (pathHistory.Count > 0) {
			GameObject pathCell = pathHistory.Pop () as GameObject;
			pathCell.tag = "PathCell";
			pathCell.GetComponent<SpriteRenderer> ().color = Color.green;
		}
	}

	void moveNorth(){
		int newX = (int)guideCell.transform.position.x;
		int newY = (int)guideCell.transform.position.y + 1;
		guideCell.GetComponent<CellManagement> ().pathVisited = true;
		guideCell = maze.cells [newX, newY];
		pathHistory.Push (guideCell);
		checkNeighbors ();
	}

	void moveSouth(){
		int newX = (int)guideCell.transform.position.x;
		int newY = (int)guideCell.transform.position.y - 1;
		guideCell.GetComponent<CellManagement> ().pathVisited = true;
		guideCell = maze.cells [newX, newY];
		pathHistory.Push (guideCell);
		checkNeighbors ();
	}

	void moveEast(){
		int newX = (int)guideCell.transform.position.x + 1;
		int newY = (int)guideCell.transform.position.y;
		guideCell.GetComponent<CellManagement> ().pathVisited = true;
		guideCell = maze.cells [newX, newY];
		pathHistory.Push (guideCell);
		checkNeighbors ();
	}

	void moveWest(){
		int newX = (int)guideCell.transform.position.x - 1;
		int newY = (int)guideCell.transform.position.y;
		guideCell.GetComponent<CellManagement> ().pathVisited = true;
		guideCell = maze.cells [newX, newY];
		pathHistory.Push (guideCell);
		checkNeighbors ();
	}


	void checkNeighbors(){
		validNeighbors.Clear ();
		CellManagement cellProps = guideCell.GetComponent<CellManagement> ();
		int currentX = (int)guideCell.transform.position.x;
		int currentY = (int)guideCell.transform.position.y;

		//check north neighbor. First check if in bounds, then check if there is a wall blocking or not.
		if(!cellProps.north && ((currentX >= 0 && currentX < maze.width) && (currentY+1 >= 0 && currentY+1 < maze.height))){
			//only add if neighbor not pathVisited yet
			if (!maze.cells [currentX, currentY + 1].GetComponent<CellManagement> ().pathVisited) {
				validNeighbors.Add (NORTH);
			}
		}
		//check south
		if(!cellProps.south && ((currentX >= 0 && currentX < maze.width) && (currentY-1 >= 0 && currentY-1 < maze.height))){
			//only add if neighbor not pathVisited yet
			if (!maze.cells [currentX, currentY - 1].GetComponent<CellManagement> ().pathVisited) {
				validNeighbors.Add (SOUTH);
			}
		}
		//check east
		if(!cellProps.east && ((currentX + 1 >= 0 && currentX + 1 < maze.width) && (currentY >= 0 && currentY < maze.height))){
			//only add if neighbor not pathVisited yet
			if (!maze.cells [currentX + 1, currentY].GetComponent<CellManagement> ().pathVisited) {
				validNeighbors.Add (EAST);
			}
		}
		//check west
		if(!cellProps.west && ((currentX - 1 >= 0 && currentX - 1 < maze.width) && (currentY >= 0 && currentY < maze.height))){
			//only add if neighbor not pathVisited yet
			if (!maze.cells [currentX - 1, currentY].GetComponent<CellManagement> ().pathVisited) {
				validNeighbors.Add (WEST);
			}
		}
		Debug.Log (validNeighbors.Count + " valid neighbors to " + string.Format ("({0},{1})", guideCell.transform.position.x, guideCell.transform.position.y));
	}
}