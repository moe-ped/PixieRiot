using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float explosionForce = 0;

	void OnTriggerStay2D (Collider2D other) {
		Vector2 force = (other.gameObject.transform.position - transform.position).normalized/Vector2.Distance(other.gameObject.transform.position, transform.position) * explosionForce;
		other.gameObject.rigidbody2D.AddForce(force);
	}

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
