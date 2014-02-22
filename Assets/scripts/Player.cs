﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public string name;
	
	public bool turn;

	public bool ko = false;

	public AudioClip hitSound;
	public AudioClip throwSound;
	public AudioClip chargeThrowSound;
	public AudioClip[] selectSound;
	
	public float ammoOffset;

	Animator animator;
	
	public float maxStrength = 10;

	public float strength = 0;
	
	public float sidebounds;
	
	public float lowbounds; 
	
	public float tss;

	public float idleTime = 5;
	float idleCountDown;

	public Transform crosshair;
	public Transform activeCrosshair;
	public Transform coneSprite;
	public Transform dust;
	public Transform glow;
	Transform activeGlow;
	Transform[] activeCone = new Transform[30];
	public float coneLength = 3;
	
	//test ... some part appears to be missing hmmm
	public Particle testicle;

	// Use this for initialization
	void Start () {
		//desynchronize animation state
		idleCountDown = Random.Range (0.0f, 5.0f);
		animator = transform.GetChild(0).GetComponent("Animator") as Animator;
	}
	
	void OnCollisionEnter2D (Collision2D other) {
		Main main = Camera.main.GetComponent("Main") as Main;
		if (other.relativeVelocity.magnitude > 10) {
			ko = true;
			animator.SetInteger("animationIndex", 1);
			audio.PlayOneShot(hitSound);
			//for special ammo
			if (other.gameObject.GetComponent ("Arrow")) {
				Arrow arrow = other.gameObject.GetComponent ("Arrow") as Arrow;
				//arrow.doSpecialStuff() :)
			}
		}
	}
	
	public void timeSinceShot () {
		if (tss >= 0) {
			tss += Time.deltaTime;
		}
	}

	//obsolete
	public void moveCrosshair () {
		if (!activeCrosshair && turn) {
			activeCrosshair = Instantiate(crosshair) as Transform;
		}
		else if (turn) {
			Vector2 aimPoint = Input.mousePosition;
			
			Vector3 position = Camera.main.WorldToScreenPoint (transform.position);
			Vector2 screenPosition = new Vector2(position.x, position.y);
			
			Vector2 pointer = aimPoint - new Vector2(screenPosition.x, screenPosition.y);
			activeCrosshair.position = transform.position + new Vector3(pointer.normalized.x, pointer.normalized.y, 0)*3;
			activeCrosshair.right = pointer;
		}
		else if (activeCrosshair) {
			Destroy(activeCrosshair.gameObject);
		}
	}

	//obsolete
	public void cone () {
		int current = (int) Mathf.Round(activeCone.Length * (strength/maxStrength) - 1);
		Vector3 position = transform.position + (coneLength * (strength/maxStrength) * Vector3.Normalize(Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position)));
		if (current > activeCone.Length - 1) {
			current = activeCone.Length - 1;
		}
		else if (current < 0) {
			current = 0;
		}
		if (!activeCone[current]) {
			activeCone[current] = (Transform) Instantiate(coneSprite, position, Quaternion.identity);
			activeCone[current].localScale = new Vector3(0.2f + strength/maxStrength, 0.2f + strength/maxStrength, 1);
			activeCone[current].transform.parent = transform;
		}
		//follow
		for (int i=0; i<activeCone.Length; i++) {
			if (activeCone[i]) {
				float distance = Vector3.Distance(activeCone[i].position, transform.position);
				Vector3 direction = Vector3.Normalize(Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position));
				Vector3 iPosition = transform.position + direction * distance;
				activeCone[i].position = iPosition;
				activeCone[i].gameObject.renderer.material.color = new Color (1 , 1- (strength/maxStrength), 0);
			}
		}
	}

	//obsolete
	void destroyCone () {
		for (int i=0; i<activeCone.Length; i++) {
			if (activeCone[i]) {
				Destroy(activeCone[i].gameObject);
			}
		}
	}
	
	public void shoot () {
		Main main = Camera.main.GetComponent("Main") as Main;

		Transform ammo = main.weapons[main.teams[main.turn].selectedWeapon].projectile;

		Vector2 aimPoint = Input.mousePosition;
		
		Vector3 position = Camera.main.WorldToScreenPoint (transform.position);
		Vector2 screenPosition = new Vector2(position.x, position.y);
		
		Vector2 force2d = aimPoint - new Vector2(screenPosition.x, screenPosition.y);

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			animator.SetInteger("animationIndex", 3);
			audio.PlayOneShot(chargeThrowSound);
		}

		if (Input.GetKey(KeyCode.Mouse0)) {
			if (strength < maxStrength) {
				strength += maxStrength * Time.deltaTime;
			}
			else {
				animator.speed = 0;
				strength = maxStrength;
			}
			//cone ();
		}

		if (Input.GetKeyUp(KeyCode.Mouse0)) {
			animator.speed = 1;
			animator.SetInteger("animationIndex", 4);
			audio.Stop();
			audio.PlayOneShot(throwSound);

			main.shotForce = force2d.normalized * strength;
			
			int prefix = 1;
			if (force2d.y < 1) {
					prefix = -1;
			}
			float initialAmmoRotationZ = Vector2.Angle(Vector2.right, force2d) * prefix;
			Quaternion initialAmmoRotation = Quaternion.Euler (0, 0, initialAmmoRotationZ);

			Vector2 startPosition = force2d.normalized*ammoOffset;
			Instantiate(ammo, transform.position + new Vector3(startPosition.x,startPosition.y, 0), initialAmmoRotation);
			destroyCone ();
			strength = 0;
			tss = 0;
			turn = false;
		}
		int lookdirx = 1;
		if (force2d.x > 1) {
			lookdirx = -1;
		}
		transform.localScale = new Vector3(lookdirx*Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}

	void selectIdle () {
		float randomNumber = Random.Range (0, 10);
		if (randomNumber == 9) {
			animator.SetInteger("idleIndex", 1);
		}
		else if (randomNumber == 8) {
			animator.SetInteger("idleIndex", 2);
		}
		else {
			animator.SetInteger("idleIndex", 0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CameraMovement mov = Camera.main.GetComponent("CameraMovement") as CameraMovement;
		idleCountDown -= Time.deltaTime;
		if (idleCountDown <= 0) {
			selectIdle ();
			idleCountDown = idleTime;
		}
		Main main = Camera.main.GetComponent("Main") as Main;

		if (turn) {
			//make animation for selected pixy
			animator.SetInteger("animationIndex", 0);
			if (ko) {
				animator.SetInteger("animationIndex", 0);
				ko = false;
				main.changeTurn ();
			}
			else if (!main.turnChange && mov.target == this.gameObject) { 
				shoot();
			}
		}

		//moveCrosshair();

		timeSinceShot();
		
		if(Input.GetKeyDown(KeyCode.Tab) && turn){
			turn = false;
		}
		
		if (tss > 0.04) {
			main.shotForce = Vector3.zero;
			tss = -1;
		}
		
		if (transform.position.y < lowbounds) {
			if (turn) {
				main.changeTurn ();
			}
			Destroy(gameObject);	
		}
		
		if (turn && !ko) {
			if (!mov.target || mov.target.GetComponent("Player")) {
				mov.target = this.gameObject;
			}
			if (!activeGlow) {
				activeGlow = (Transform) Instantiate(glow, transform.position + Vector3.forward/10, Quaternion.identity);
				activeGlow.transform.parent = transform;
			}
		}
		else if (activeGlow) {
			Destroy (activeGlow.gameObject);
		}
	}
}
