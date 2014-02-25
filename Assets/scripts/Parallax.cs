using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	//between 0 and 2 (1 is midground) 
	public float distanceFromWall;

	float startXPosition;
	float cameraXPosition;

	Vector3 startScale;

	// Use this for initialization
	void Start () {
		startXPosition = transform.position.x;
		startScale = transform.localScale;
	}

	void parallax () {
		//position
		cameraXPosition = Camera.main.transform.position.x;
		float newXPosition = startXPosition + (cameraXPosition * (1 - distanceFromWall));
		transform.position = new Vector3 (newXPosition, transform.position.y, transform.position.z);
		//scale
		float camHeight = Camera.main.orthographicSize;
		float relationToNormal = camHeight / 7;
		//hmmm ... actually works kind of the other way round when zooming IN -> makeshift fix
		if (relationToNormal > 1) {
			transform.localScale = startScale * ((1 * distanceFromWall) + (relationToNormal * (1 - distanceFromWall)));
		}
		else {
			transform.localScale = startScale;
		}
	}

	void FixedUpdate () {
		parallax ();
	}
}
