using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float explosionForce = 0;

	public float lifetime = 0.3f;

	void OnTriggerStay2D (Collider2D other) {
		Vector2 force = (other.gameObject.transform.position - transform.position).normalized/Vector2.Distance(other.gameObject.transform.position, transform.position) * explosionForce;
		other.gameObject.rigidbody2D.AddForce(force);
		Destroy (this, 0.3f);
	}

	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
