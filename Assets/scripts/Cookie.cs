using UnityEngine;
using System.Collections;

public class Cookie : MonoBehaviour {

	public AudioClip hitSound;
	public AudioClip crackSound;

	void OnCollisionEnter2D (Collision2D other) {
		if (other.relativeVelocity.magnitude > 10) {
			audio.PlayOneShot(crackSound);
			Destroy(gameObject);
		}
		else if (other.relativeVelocity.magnitude > 2) {
			audio.PlayOneShot(hitSound);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
