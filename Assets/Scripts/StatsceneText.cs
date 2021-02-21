using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsceneText : MonoBehaviour {

	public Text cellsLit, bulletsFired, enemiesKilled;

	void Start () {
		int totalCellsPossible = summation (GameManager.Instance.initialMazeSize, GameManager.Instance.mazeSize);
		cellsLit.text = "Cells lit up: " + GameManager.Instance.cellsLit.ToString("N0") + " out of " + totalCellsPossible.ToString("N0") + " possible cells";
		bulletsFired.text = "Bullets fired: " + GameManager.Instance.bulletsFired.ToString ("N0");
		enemiesKilled.text = "Enemies killed: " + GameManager.Instance.enemiesKilled.ToString ("N0");
	}


	int summation(int start, int max){
		//this qill calculate all cells generated during gameplay. This is just a summation that starts at start(this value will be the size of the first maze,
		//by default this value is 5, though can be changed by the player if they want to start at a more difficult maze), and goes up to max(this will be the
		//size of the maze the player died on) and squares the value each time and adds it to the total.
		int total = 0;
		for (int i = start; i <= max; i++) {
			total += i * i;
		}


		return total;
	}
}
