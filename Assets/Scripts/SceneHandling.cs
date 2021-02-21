using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class SceneHandling : MonoBehaviour {
	public InputField difficultyField;
	public Toggle hudOnTog;

	public void loadGame(){
		GameManager.Instance.score = 0;
		GameManager.Instance.lives = 3;
		GameManager.Instance.level = 1;
		GameManager.Instance.mazeSize = 5;
		GameManager.Instance.cameraSize = 2.5f;
		GameManager.Instance.timeLeft = 31f;
		GameManager.Instance.cellsLit = 0;
		GameManager.Instance.bulletsFired = 0;
		GameManager.Instance.endLevelTime = 15f;
		GameManager.Instance.hudOn = hudOnTog.isOn;
		//first strip any non numeric input from the string, then parse an integer from it for use.
		string userInput = Regex.Replace (difficultyField.text.ToString (), @"[^\d]", string.Empty);
		if (userInput.Length <= 0)
			userInput = "5";
		int mazeSize = int.Parse (userInput);

		Debug.Log (mazeSize + " Maze size");
		if (mazeSize > 30)
			mazeSize = 30;
		else if (mazeSize < 5)
			mazeSize = 5;

		if (mazeSize >= 10 && mazeSize < 20) {
			GameManager.Instance.timeLeft = 61f;
			GameManager.Instance.endLevelTime = 20f;
		} else if (mazeSize >= 20 && mazeSize < 30) {
			GameManager.Instance.timeLeft = 91f;
			GameManager.Instance.endLevelTime = 30f;
		} else if (mazeSize >= 30) {
			GameManager.Instance.timeLeft = 101f;
			GameManager.Instance.endLevelTime = 60f;
		}

		GameManager.Instance.initialMazeSize = mazeSize;
		GameManager.Instance.mazeSize = (int)mazeSize;
		SceneManager.LoadScene ("Main", LoadSceneMode.Single);
		Cursor.visible = false;
	}

	public void loadMenu(){
		SceneManager.LoadScene ("Menu", LoadSceneMode.Single);
	}

	public void loadControls(){
		SceneManager.LoadScene ("Controls", LoadSceneMode.Single);
	}

	public void loadEnd(){
		SceneManager.LoadScene ("End", LoadSceneMode.Single);
		Cursor.visible = true;
	}

	public void loadStats(){
		SceneManager.LoadScene("Stats", LoadSceneMode.Single);
		Cursor.visible = true;
		//Scene statScene = SceneManager.GetSceneByName ("Stats");
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}
}
