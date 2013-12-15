using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	public AudioClip hitSound;
	
	void OnCollisionEnter2D (Collision2D other) {
		if (other.relativeVelocity.magnitude > 2) {
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
