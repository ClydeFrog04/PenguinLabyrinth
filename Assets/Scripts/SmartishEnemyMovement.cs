using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartishEnemyMovement : MonoBehaviour {

	public float movementSpeed;
	private List<string> validNeighbors;
	private MazeGeneration maze;
	private const string NORTH = "north";
	private const string SOUTH = "south";
	private const string EAST = "east";
	private const string WEST = "west";
	private float waitTime;
	private string currentDirection;
	private GameObject guideCell;

	void Start () {
		maze = GameObject.FindGameObjectWithTag ("Maze").GetComponent<MazeGeneration> ();
		validNeighbors = new List<string> ();
		waitTime = 0f;
		currentDirection = NORTH;
		checkNeighbors ();
		
	}
	

	void Update (){
		checkNeighbors ();
		waitTime += Time.deltaTime;
		if (validNeighbors.Count >= 3 && waitTime > 2f) {
			validNeighbors.Remove (getOppositeDirection());
			waitTime = 0f;
			currentDirection = validNeighbors[Random.Range (0, validNeighbors.Count)];
		}


		switch (currentDirection) {
		case NORTH:
			Debug.Log (transform.rotation.eulerAngles.z);
			if (transform.rotation.eulerAngles.z != 0) {
				//transform.rotation.eulerAngles.z = 0;
				Quaternion.Euler (new Vector3 (0, 0, 0));

			}
			moveNorth ();
			break;
		case SOUTH:
//			if (transform.rotation.eulerAngles.z != 180) {
//				//Quaternion.Euler (new Vector3 (0, 0, 180));
//				transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
//			}
			moveSouth ();
			break;
		case EAST:
			moveEast ();
			break;
		case WEST:
			moveWest ();
			break;
		default:
			break;
		}
		//in case it ends up out of bounds
//		if (transform.position.x > maze.width)
//			transform.position.x = maze.width - 1;
//		
//		if (transform.position.x < 0)
//			transform.position.x = 0;
//
//		if (transform.position.y > maze.height)
//			transform.position.y = maze.height - 1;
//
//		if (transform.position.y < 0)
//			transform.position.y = 0;
		
	}

	string getOppositeDirection(){
		switch (currentDirection) {
		case NORTH:
			return SOUTH;
		case SOUTH:
			return NORTH;
		case EAST:
			return WEST;
		case WEST:
			return EAST;
		default:
			return null;
		}
	}




	void moveNorth(){
		transform.Translate (0, movementSpeed * Time.deltaTime, 0);
	}

	void moveSouth(){
		transform.Translate (0, (-1) * (movementSpeed * Time.deltaTime), 0);
		//transform.Rotate(0,0,90);
		//transform.Translate(0, movementSpeed * Time.deltaTime, 0);
	}

	void moveEast(){
		transform.Translate (movementSpeed * Time.deltaTime, 0, 0);
	}

	void moveWest(){
		transform.Translate ((-1)*(movementSpeed * Time.deltaTime), 0, 0);
	}



	//This will be used for centering the enemy in it's cell when needed
	void centerInCell(){
		float x = transform.position.x;
		float y = transform.position.y;
		//round to nearest int
		x += 0.5f;
		y += 0.5f;
		transform.position = new Vector3 ((int)x, (int)y, transform.parent.position.z);
	}




	void checkNeighbors(){
		validNeighbors.Clear ();
		guideCell = maze.cells [(int)(transform.position.x+0.5f), (int)(transform.position.y+0.5f)];
		CellManagement cellProps = guideCell.GetComponent<CellManagement> ();
		int currentX = (int)guideCell.transform.position.x;
		int currentY = (int)guideCell.transform.position.y;

		//check north neighbor. First check if in bounds, then check if there is a wall blocking or not.
		if(!cellProps.north && ((currentX >= 0 && currentX < maze.width) && (currentY+1 >= 0 && currentY+1 < maze.height))){
			validNeighbors.Add (NORTH);
		}
		//check south
		if(!cellProps.south && ((currentX >= 0 && currentX < maze.width) && (currentY-1 >= 0 && currentY-1 < maze.height))){
			validNeighbors.Add (SOUTH);
		}
		//check east
		if(!cellProps.east && ((currentX + 1 >= 0 && currentX + 1 < maze.width) && (currentY >= 0 && currentY < maze.height))){
			validNeighbors.Add (EAST);
		}
		//check west
		if(!cellProps.west && ((currentX - 1 >= 0 && currentX - 1 < maze.width) && (currentY >= 0 && currentY < maze.height))){
				validNeighbors.Add (WEST);
		}
	}


	void OnCollisionEnter2D(Collision2D collision){
		string tag = collision.gameObject.tag;

		if (tag.Equals ("Wall")) {
			checkNeighbors ();
			centerInCell ();
			if(validNeighbors.Contains(currentDirection)){
				validNeighbors.Remove(currentDirection);
			}
			if (validNeighbors.Count > 1) {
				validNeighbors.Remove (getOppositeDirection ());
			}
			currentDirection = validNeighbors[Random.Range (0, validNeighbors.Count)];
		}//end wall collision

		if (tag.Equals ("Bullet")) {
			GameManager.Instance.timeLeft += 10;
			GameManager.Instance.bulletsThatHit++;
			GameManager.Instance.enemiesKilled++;
			Destroy (collision.gameObject);
			Destroy (gameObject);

		}
		if (tag.Equals ("ShieldFish")) {
			GameManager.Instance.bulletsThatHit++;
			GameManager.Instance.enemiesKilled++;
			//Destroy (collision.gameObject);
			Destroy (gameObject);
		}
	}

}
