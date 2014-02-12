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
	public Transform projectile;
	public Sprite icon;
}

public class Main : MonoBehaviour {

	public Ammo[] weapons;

	public AudioClip pixyDieSound;
	public AudioClip cookieDieSound;

	public Color[] blockColors = new Color[5];

	public float maxScore = 10;

	public Vector2 shotForce = Vector2.zero;

	public float turnTime;
	public float timePerTurn = 30;
	
	public int turn = 0;
	
	public int[] player = new int[2];
	
	//public int winner = 0;
	
	public Teams[] teams;

	public void changeTurn () {
		StartCoroutine ("changeTurnCo");
	}

	public void countdown () {
		CameraMovement mov = this.GetComponent("CameraMovement") as CameraMovement;
		if (turnTime <= timePerTurn && mov.target && mov.target.GetComponent("Player")) {
			turnTime -= Time.deltaTime;
		}
	}

	public IEnumerator changeTurnCo () {
		yield return new WaitForSeconds (1);
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

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		audio.Play();
		turnTime = timePerTurn;
		//changeTurn ();
		selectPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		countdown ();
		if (turnTime <= 0) {
			changeTurn ();
			turnTime = timePerTurn;
		}
		if(Input.GetKeyDown(KeyCode.Tab)) {
			changePlayer();
		}
	}
}
