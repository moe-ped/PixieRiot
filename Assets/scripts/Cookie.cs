using UnityEngine;
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
				main.teams[main.turn].score++;
				hp -= arrow.dp;
			}
			else {
				hp -= 1;
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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//placeholder
		renderer.material.color = new Color(1, (float)hp/3, (float)hp/3);
	}
}
