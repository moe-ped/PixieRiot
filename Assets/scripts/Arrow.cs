using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	public float sidebounds;
	
	public float lowbounds; 

	public string type;
	public float explosionRadius = 3;
	public float explosionForce = 5;
	public Transform explosionPrefab;
	
	public float lt;

	public int dp = 1;

	public float impact = 1;

	// Use this for initialization
	void Start () {
		Main main = Camera.main.GetComponent("Main") as Main;
		main.settarget(gameObject);
		rigidbody2D.velocity = main.shotForce/40;
	}

	void OnDestroy () {
		renderer.enabled = false;
		rigidbody2D.isKinematic = true;
		collider2D.enabled = false;
		Destroy (particleSystem);
		if (type == "explosive" || type == "rc") {
			Debug.Log ("boom");
			Transform explosion = (Transform)Instantiate (explosionPrefab, transform.position, Quaternion.identity);
			CircleCollider2D circle = explosion.GetComponent("CircleCollider2D") as CircleCollider2D;
			circle.radius = explosionRadius;
			Explosion eScript = explosion.gameObject.GetComponent("Explosion") as Explosion;
			eScript.explosionForce = explosionForce;
		}
		Main main = Camera.main.GetComponent("Main") as Main;
		main.changeTurn();
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (!other.gameObject.GetComponent ("Ground")) {
			//other.gameObject.rigidbody2D.velocity = (rigidbody2D.velocity.normalized * impact)/10 + new Vector2 (rigidbody2D.velocity.x, 0).normalized;
		}
		if (lifetime() > 0.04) {
			//Destroy(rigidbody2D);
			//transform.parent = other.transform;
			die ();
		}
	}
	
	public void die () {
		float deathDelay = 0.01f;
		if (type == "explosive") {
			deathDelay = 3;
		}
		Destroy(this, deathDelay);
		Destroy(gameObject, deathDelay + 1);
	}
	
	public float lifetime () {
		lt += Time.deltaTime;
		return lt;
	}
	
	void FixedUpdate () {
		if (lifetime() > 0.02) {
			Main main = Camera.main.GetComponent("Main") as Main;
			int prefix = 1;
			if (rigidbody2D.velocity.y < 1) {
				prefix = -1;
			}
			float forceRotationZ = Vector3.Angle(Vector3.right, rigidbody2D.velocity) * prefix;
			transform.eulerAngles = new Vector3(0, 0, forceRotationZ);
			if (rigidbody2D.velocity.x < 1) {
				transform.localScale = new Vector3 (1, -1, 1);
			}
			if (transform.position.x > sidebounds || transform.position.x < -sidebounds) {
				die ();
			}
			if (transform.position.y < lowbounds) {
				die ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (type == "rc") {
			if (rigidbody2D.velocity.y < 0 && rigidbody2D.gravityScale != 10) {
				Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, 0);
				Debug.Log("rc activate" + newVelocity);
				newVelocity = newVelocity.normalized;
				rigidbody2D.gravityScale = 0;
				rigidbody2D.velocity = newVelocity*10;
			}
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				rigidbody2D.gravityScale = 10;
			}
			/*if (rigidbody2D.velocity.y < -20) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -20);
			}*/
		}
	}
}
