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

	//in case of no target
	Vector3 noTarget;

	public void follow () {
		if (!target) {
			Vector3 distance = camera.WorldToScreenPoint(noTarget) - camera.WorldToScreenPoint(transform.position);
			distance -= new Vector3(Screen.width/2, Screen.height/2, 0);
			distance /= wobblyness;
			Vector3 planeDistance = new Vector3 (distance.x, distance.y, 0);
			rigidbody.velocity = planeDistance;
		}
		else {
			//screenpos.: WorldToScreenPoint (transform.position)
			Vector3 distance = camera.WorldToScreenPoint(target.transform.position) - camera.WorldToScreenPoint(transform.position);
			if (target.GetComponent("Player")) {
				float xoffset = (Screen.width * -zoomFactor /20) - Screen.width/20;
				float yoffset = (Screen.height * -zoomFactor /20) - Screen.height/20;

				Vector2 direction = (Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0));

				float xprefix = direction.x/500;
				float yprefix = direction.y/500;

				xoffset *= xprefix;
				yoffset *= yprefix;

				distance.x += xoffset;
				distance.y += yoffset;
			}
			distance -= new Vector3(Screen.width/2, Screen.height/2, 0);
			distance /= wobblyness;
			Vector3 planeDistance = new Vector3 (distance.x, distance.y, 0);
			rigidbody.velocity = planeDistance;
			noTarget = target.transform.position;
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
