using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	public static GameManager Instance{
		get{
			if (instance == null) {
				GameObject gamemanager = new GameObject ("GameManager");
				gamemanager.AddComponent<GameManager> ();
				DontDestroyOnLoad (gamemanager);
			}
			return instance;
		}
	}//end Instance


	public int score { get; set; }
	public int lives { get; set; }
	public int level { get; set; }
	public int mazeSize{ get; set; }//this will be used to control the grid size of the maze
	public int cellsLit{ get; set; }
	public int bulletsFired{ get; set; }
	public int bulletsThatHit{ get; set; }
	public int enemiesKilled{ get; set; }
	public int initialMazeSize{ get; set; }//used for calculating cells lit up
	public float endLevelTime {get; set;} 
	public float cameraSize{get; set;}
	public float timeLeft { get; set; }
	public float shieldRegenTime{ get; set; }//only lets the player use their shield once every ten seconds
	public bool hudOn{get; set;}

	void Awake(){
		score = 0;
		lives = 3;
		level = 1;
		mazeSize = 5;
		cameraSize = 2.5f;
		timeLeft = 31f;
		endLevelTime = 15f;
		cellsLit = 0;
		bulletsFired = 0;
		shieldRegenTime = 10f;
		hudOn = true;
		instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

}//end GameManager