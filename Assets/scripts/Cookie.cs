﻿using UnityEngine;
using System.Collections;

public class Cookie : MonoBehaviour {

	public AudioClip hitSound;
	public AudioClip crackSound;

	public Transform dust;

	public int hp = 3;

	void OnCollisionEnter2D (Collision2D other) {
		Main main = Camera.main.GetComponent("Main") as Main;
		if (other.relativeVelocity.magnitude > 10) {
			audio.PlayOneShot(crackSound);
			if (other.gameObject.GetComponent ("Arrow")) {
				Arrow arrow = other.gameObject.GetComponent ("Arrow") as Arrow;
				hp -= arrow.dp;
			}
			else {
				hp -= 1;
			}
			if (transform.parent) {
				if (transform.parent.name == "Fort1") {
					main.teams[1].score++;
				}
				else if (transform.parent.name == "Fort2") {
					main.teams[0].score++;
				}
			}
			if (hp <= 0) {
				Instantiate(dust, transform.position, Quaternion.identity);
				float initVolume = main.audio.volume;
				main.audio.volume = 0.5f;
				main.audio.PlayOneShot(main.cookieDieSound);
				main.audio.volume = initVolume;
				Destroy(gameObject);
			}
		}
		else if (other.relativeVelocity.magnitude > 2) {
			audio.PlayOneShot(hitSound);
		}
	}

	void checkMoving () {
		CameraMovement mov = Camera.main.GetComponent("CameraMovement") as CameraMovement;
		if (rigidbody2D.velocity.magnitude > 0.2f) {
			if (!mov.target || rigidbody2D.velocity.magnitude > mov.target.rigidbody2D.velocity.magnitude) {
				mov.target = this.gameObject;
			}
		}
	}

	// Use this for initialization
	void Start () {
		Main main = Camera.main.GetComponent ("Main") as Main;
		renderer.material.color = main.blockColors[Random.Range(0, main.blockColors.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		checkMoving ();
	}
}
