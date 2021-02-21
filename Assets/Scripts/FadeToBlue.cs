using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlue : MonoBehaviour {
	
	[Range(0f, 1f)]
	public float fadeToBlueAmount = 0f;
	private SpriteRenderer sprite;
	public bool startFade;
	public float fadeSpeed = 0.05f;


	void Start(){
		sprite = GetComponent<SpriteRenderer> ();
		Color c = sprite.material.color;

		c.g = 1f;
		c.b = 1f;
		c.r = 1f;

		sprite.material.color = c;
	}

	void Update(){
		if (startFade) {
			StartCoroutine (fadeToBlue ());
			//Destroy (this);
		}
	}


	IEnumerator fadeToBlue(){
		//float check = 1f;
		for (float i = 1f; i > fadeToBlueAmount; i -= 0.05f) {
			Color c = sprite.material.color;

			c.g = i;
			c.b = i;

			sprite.material.color = c;
			yield return new WaitForSeconds (fadeSpeed);
			Debug.Log (i);
		}
	}

}
