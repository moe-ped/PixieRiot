using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public string name;
	
	public bool turn;
	
	public Transform ammo;
	
	public float ammoOffset;
	
	public float maxStrength = 10;

	public float strength = 0;
	
	public float sidebounds;
	
	public float lowbounds; 
	
	public float tss;

	public Transform crosshair;
	public Transform activeCrosshair;
	public Transform coneSprite;
	public Transform[] activeCone = new Transform[10];
	public float coneLength = 3;
	
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

	public void cone () {
		int current = (int) Mathf.Round(activeCone.Length * (strength/maxStrength) - 1);
		Vector3 position = transform.position + (coneLength * (strength/maxStrength) * Vector3.Normalize(Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position)));
		if (current > activeCone.Length) {
			current = activeCone.Length;
		}
		if (activeCone[current]) {
			Destroy(activeCone[current].gameObject);
		}
		activeCone[current] = (Transform) Instantiate(coneSprite, position, Quaternion.identity);
		activeCone[current].localScale = new Vector3(0.2f + strength/maxStrength, 0.2f + strength/maxStrength, 1);
		//follow
		for (int i=0; i<activeCone.Length; i++) {
			if (activeCone[i]) {
				float distance = Vector3.Distance(activeCone[i].position, transform.position);
				Vector3 direction = Vector3.Normalize(Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position));
				Vector3 iPosition = transform.position + direction * distance;
				activeCone[i].position = iPosition;
			}
		}
	}

	void destroyCone () {
		for (int i=0; i<activeCone.Length; i++) {
			if (activeCone[i]) {
				Destroy(activeCone[i].gameObject);
			}
		}
	}
	
	public void shoot () {
		Main main = Camera.main.GetComponent("Main") as Main;

		Vector2 aimPoint = Input.mousePosition;
		
		Vector3 position = Camera.main.WorldToScreenPoint (transform.position);
		Vector2 screenPosition = new Vector2(position.x, position.y);
		
		Vector2 force2d = aimPoint - new Vector2(screenPosition.x, screenPosition.y);

		if (Input.GetKey(KeyCode.Mouse0)) {
			if (strength < maxStrength) {
				strength += maxStrength * Time.deltaTime;
			}
			else {
				strength = maxStrength;
			}
			cone ();
			Debug.Log (Mathf.Round(strength/maxStrength * 100));
		}

		if (Input.GetKeyUp(KeyCode.Mouse0)) {

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
			destroyCone ();
			strength = 0;
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
