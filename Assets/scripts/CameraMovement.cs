using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject target;

	public float wobblyness = 5;

	public float zoomFactor = -10;

	public float zoomSpeed = 5;

	public float zoomWobblyness = 0.5f;

	public float zoomLockClose = 3;
	public float zoomLockFar = 20;

	public void follow () {
		if (!target) {
			Vector3 ololol = -transform.position;
			Vector3 lolz = new Vector3 (ololol.x, ololol.y, 0);
			rigidbody.velocity = lolz;
		}
		else {
			//screenpos.: WorldToScreenPoint (transform.position)
			Vector3 distance = camera.WorldToScreenPoint(target.transform.position) - camera.WorldToScreenPoint(transform.position);
			distance -= new Vector3(Screen.width/2, Screen.height/2, 0);
			distance /= wobblyness;
			Vector3 planeDistance = new Vector3 (distance.x, distance.y, 0);
			rigidbody.velocity = planeDistance;
		}
	}

	public void zoom () {
		float distance = zoomFactor - transform.position.z;
		distance /= zoomWobblyness;
		Vector3 linearDistance = new Vector3 (rigidbody.velocity.x, rigidbody.velocity.y, distance);
		rigidbody.velocity = linearDistance;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		follow ();
		if(Input.GetAxis("Mouse ScrollWheel") != 0) {
			if (zoomFactor + Input.GetAxis("Mouse ScrollWheel") > -zoomLockFar && zoomFactor + Input.GetAxis("Mouse ScrollWheel") < -zoomLockClose) {
				zoomFactor += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
			}
			else if (zoomFactor + Input.GetAxis("Mouse ScrollWheel") > -zoomLockFar) {
				zoomFactor = -zoomLockClose;
			}
			else if (zoomFactor + Input.GetAxis("Mouse ScrollWheel") < -zoomLockClose) {
				zoomFactor = -zoomLockFar;
			}
		}
		zoom ();
	}
}
