//this script will make whatever object it is applied to invisible while in a dark cell. That way the player can't see them until they have revealed the particular
//cell
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWhileDark : MonoBehaviour {
	private int currentX;
	private int currentY;
	private MazeGeneration maze;
	// Use this for initialization
	void Start () {
		currentX = (int)(transform.position.x + 0.5f);
		currentY = (int)(transform.position.y + 0.5f);
		maze = GameObject.FindGameObjectWithTag ("Maze").GetComponent<MazeGeneration> ();

	}
	
	// Update is called once per frame
	void Update () {
		currentX = (int)(transform.position.x + 0.5f);
		currentY = (int)(transform.position.y + 0.5f);
		if (maze.cells [currentX, currentY].GetComponent<SpriteRenderer> ().color.Equals (Color.black)) {
			GetComponent<SpriteRenderer> ().enabled = false;
		} else
			GetComponent<SpriteRenderer> ().enabled = true;
	}
}
