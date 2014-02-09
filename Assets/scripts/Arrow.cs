using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	
	public float sidebounds;
	
	public float lowbounds; 
	
	public float lt;

	public int dp = 1;

	public float impact = 1;

	// Use this for initialization
	void Start () {
		Main main = Camera.main.GetComponent("Main") as Main;
		main.settarget(gameObject);
		rigidbody2D.velocity = main.shotForce/40;
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
		Main main = Camera.main.GetComponent("Main") as Main;
		main.changeTurn();
		main.turnTime = main.timePerTurn;
		Destroy(gameObject, 0.01f);
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
	
	}
}
