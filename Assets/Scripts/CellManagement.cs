/*Helper script.
 * This will make setting a cell's properties cleaner
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManagement : MonoBehaviour {

	public bool visited, pathVisited;//the visited var is for generation, the pathVisited is for finding a path.
	public bool north, south, east, west;//this will control a cell's wall to set it as active or not in order to open a path

	void Start () {
		//visited = false;
		//north = true;
		//south = true;
		//east = true;
		//west = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (visited) {
			GetComponent<SpriteRenderer> ().color = Color.blue;
		}
		if (!north) {
			transform.GetChild (0).GetComponent<CapsuleCollider2D> ().enabled = false;
			transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;
		}
		if (!south) {
			transform.GetChild (1).GetComponent<CapsuleCollider2D> ().enabled = false;
			transform.GetChild (1).GetComponent<SpriteRenderer> ().enabled = false;
		}
		if (!east) {
			transform.GetChild (2).GetComponent<CapsuleCollider2D> ().enabled = false;
			transform.GetChild (2).GetComponent<SpriteRenderer> ().enabled = false;
		}
		if (!west) {
			transform.GetChild (3).GetComponent<CapsuleCollider2D> ().enabled = false;
			transform.GetChild (3).GetComponent<SpriteRenderer> ().enabled = false;
		}
	}
}
