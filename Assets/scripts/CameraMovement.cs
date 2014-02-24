using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	public GameObject target;

	float zoomFactor = 1;
	public float zoomSpeed = 1;

	public float minZoom = 0.25f;

	float groundLevel;

	Vector3 position;
	
	// Use this for initialization
	void Start () {
		position = transform.position;
		groundLevel = Camera.main.ScreenToWorldPoint (Vector3.zero).y;
	}
	
	void move () {
		Vector3 tPosition = target.transform.position;
		tPosition = new Vector3 (tPosition.x, position.y, -10);
		position = tPosition;
	}

	void zoom () {
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			zoomFactor += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
		}
		if (zoomFactor < minZoom) {
			zoomFactor = minZoom;
		}
		//make it smooth!
		camera.orthographicSize = 7 * 1/zoomFactor;
		//horizontal positioning
		float camHeight = groundLevel + camera.orthographicSize;
		position = new Vector3 (position.x, camHeight, position.z);
		//vertical positioning
		if (target && target.GetComponent ("Player")) {
			float prefix = target.transform.localScale.x;
			float direction = -prefix * camera.orthographicSize*1.2f * (Screen.width/Screen.height);
			position = new Vector3 (target.transform.position.x + direction, position.y, position.z);
		}
	}

	void smoothMove () {
		if (target && target.GetComponent("Player")) {
			transform.position = Vector3.Lerp (transform.position, position, 0.04f);
		}
		else {
			transform.position = position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			move ();
		}
		smoothMove ();
		zoom ();
	}
}

