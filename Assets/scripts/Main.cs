using UnityEngine;
using System.Collections;

//container class for teams
[System.Serializable]
public class Teams
{
	public string name;
	public GameObject[] players;
	public float score;
	public int selectedWeapon = 0;
}

[System.Serializable]
public class Ammo
{
	public int availabilityCounter;
	public bool available = false;
	public Transform projectile;
	public Texture2D icon;
}

public class Main : MonoBehaviour {

	public Ammo[] weapons;

	public AudioClip pixyDieSound;
	public AudioClip cookieDieSound;

	public Color[] blockColors = new Color[5];

	public float maxScore = 10;

	public Vector2 shotForce = Vector2.zero;
	
	public int turn = 0;

	public Camera pointerCam;
	
	public int[] player = new int[2];
	
	//public int winner = 0;
	
	public Teams[] teams;

	public Transform cursorPrefab;
	Transform cursor;

	public bool turnChange = false;
	public bool MouseOver = false;

	public void changeTurn () {
		StartCoroutine ("changeTurnCo");
	}

	public IEnumerator changeTurnCo () {
		//ammo counters
		for (int i=0; i<weapons.Length; i++) {
			weapons[i].availabilityCounter--;
			if (weapons[i].availabilityCounter <= 0) {
				weapons[i].available = true;
			}
		}
		turnChange = true;
		yield return new WaitForSeconds (1);
		turnChange = false;
		deselectAll ();
		turn++;
		if (turn >= teams.Length) {
					turn = 0;
			}
		while (teams[turn] == null) {
			turn++;
			if (turn >= teams.Length) {
				turn = 0;
			}
		}
		changePlayer ();
	}
	
	public void changePlayer () {
		player[turn]++;
		if (player[turn] >= teams[turn].players.Length) {
			player[turn] = 0;
		}
		while (teams[turn].players[player[turn]] == null) {
			player[turn]++;
			if (player[turn] >= teams[turn].players.Length) {
				player[turn] = 0;
			}
		}
		selectPlayer ();
	}

	public void selectPlayer () {
		Player playerscript = teams[turn].players[player[turn]].GetComponent("Player") as Player;
		playerscript.audio.PlayOneShot(playerscript.selectSound[Random.Range(0, 9)]);
		playerscript.turn = true;
	}

	public void settarget (GameObject target) {
		CameraMovement mov = this.GetComponent("CameraMovement") as CameraMovement;
		mov.target = target;
	}

	void deselectAll () {
		for (int t=0; t < teams.Length; t++) {
			int remaining = teams[t].players.Length;
			for (int p=0; p < teams[t].players.Length; p++) {
				if (teams[t].players[p]) {
					Player playerscript = teams[t].players[p].GetComponent("Player") as Player;
					playerscript.turn = false;
				}
				else {
					remaining --;
				}
				if (remaining <= 0 || teams[1-t].score >= maxScore) {
					Application.LoadLevel("Win" + (2-t));
				}
			}
		}
	}

	void customCursor () {
		//not in OnGUI because the rotation won't work
		Screen.showCursor = false;
		cursor = (Transform)Instantiate (cursorPrefab, transform.position, Quaternion.identity);
		cursor.transform.parent = transform;
	}

	void cursorPosition () {
		CameraMovement mov = GetComponent ("CameraMovement") as CameraMovement;
		Vector3 screenPosition = Input.mousePosition;
		//offset, texture width = 174
		Vector3 offset = cursor.transform.right.normalized * 87/pointerCam.orthographicSize;
		screenPosition -= offset;
		Vector3 mousePosition = pointerCam.ScreenToWorldPoint(screenPosition);
		mousePosition = new Vector3 (mousePosition.x, mousePosition.y, 10);
		cursor.transform.position = mousePosition;
		//rotation
		if (mov.target && mov.target.GetComponent ("Player")) {
			Vector3 cursorDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mov.target.transform.position;
			cursorDirection = new Vector3 (cursorDirection.x, cursorDirection.y, 0);
			cursor.transform.right = cursorDirection.normalized;
		} else {
			cursor.transform.right = new Vector3(-1, 1, 0);
		}
	}

	// Use this for initialization
	void Start () {
		weapons[0].available = true;
		customCursor ();
		audio.Play();
		//changeTurn ();
		selectPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		cursorPosition ();
		if(Input.GetKeyDown(KeyCode.Tab)) {
			changePlayer();
		}
	}
}
