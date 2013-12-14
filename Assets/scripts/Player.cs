using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public string name;
	
	public bool turn;
	
	public Transform ammo;
	
	public float ammoOffset;
	
	public float strength;
	
	public float sidebounds;
	
	public float lowbounds; 
	
	public float tss;

	public Transform crosshair;
	public Transform activeCrosshair;
	
	//test
	public Particle testparticle;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter2D (Collision2D other) {
		Main main = Camera.main.GetComponent("Main") as Main;
		if (other.relativeVelocity.magnitude > 10) {
			Destroy(gameObject);
		}
	}
	
	public void timeSinceShot () {
		if (tss >= 0) {
			tss += Time.deltaTime;
		}
	}

	public void moveCrosshair () {
		if (!activeCrosshair && turn) {
			activeCrosshair = Instantiate(crosshair) as Transform;
		}
		else if (turn) {
			Vector2 aimPoint = Input.mousePosition;
			
			Vector3 position = Camera.main.WorldToScreenPoint (transform.position);
			Vector2 screenPosition = new Vector2(position.x, position.y);
			
			Vector2 pointer = aimPoint - new Vector2(screenPosition.x, screenPosition.y);
			activeCrosshair.position = transform.position + new Vector3(pointer.normalized.x, pointer.normalized.y, 0);
			activeCrosshair.right = pointer;
		}
		else if (activeCrosshair) {
			Destroy(activeCrosshair.gameObject);
		}
	}
	
	public void shoot () {
		Main main = Camera.main.GetComponent("Main") as Main;

		Vector2 aimPoint = Input.mousePosition;
		
		Vector3 position = Camera.main.WorldToScreenPoint (transform.position);
		Vector2 screenPosition = new Vector2(position.x, position.y);
		
		Vector2 force2d = aimPoint - new Vector2(screenPosition.x, screenPosition.y);

		if (Input.GetKeyDown(KeyCode.Mouse0)) {

			main.shotForce = force2d.normalized * strength;
			
			int prefix = 1;
			if (force2d.y < 1) {
					prefix = -1;
			}
			float initialAmmoRotationZ = Vector2.Angle(Vector2.right, force2d) * prefix;
			Quaternion initialAmmoRotation = Quaternion.Euler (0, 0, initialAmmoRotationZ);

			Debug.Log(screenPosition);
			Vector2 startPosition = force2d.normalized*ammoOffset;
			Instantiate(ammo, transform.position + new Vector3(startPosition.x,startPosition.y, 0), initialAmmoRotation);
			//main.turn = enemyID;
			tss = 0;
			turn = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Main main = Camera.main.GetComponent("Main") as Main;

		if (turn) {
			shoot();
		}

		moveCrosshair();

		timeSinceShot();
		
		if(Input.GetKeyDown(KeyCode.Tab) && turn){
			turn = false;
		}
		
		if (tss > 0.04) {
			main.shotForce = Vector3.zero;
			tss = -1;
		}
		
		if (transform.position.x > sidebounds || transform.position.x < -sidebounds || transform.position.y < lowbounds) {
				Destroy(gameObject);	
		}
		
		if (turn) {
			main.settarget(gameObject);
		}
	}
}
