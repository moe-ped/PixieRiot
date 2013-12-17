using UnityEngine;
using System.Collections;

//container class for teams
[System.Serializable]
	public class Teams
	{
		public string name;
		public GameObject[] players;
		public float score;
	}

public class Main : MonoBehaviour {

	public AudioClip pixyDieSound;
	public AudioClip cookieDieSound;

	public float maxScore = 10;

	public Vector2 shotForce = Vector2.zero;

	public float turnTime;
	public float timePerTurn = 30;
	
	public int turn = 0;
	
	public int[] player = new int[2];
	
	//public int winner = 0;
	
	public Teams[] teams;
	
	/*public void sort (GameObject[] array) {
		int check = 0;
		while (check < array.Length) {
			if (array[check] == null) {
				int sub = check;
				while (sub <= array.Length) {
					if (sub == null) {
						sub++;	
					}
				}
				if (array[sub] != null) {
					array[check] = array[sub];
					array[sub] = null;
				}
			}
			check++;
		}
	}*/

	public void changeTurn () {
		StartCoroutine ("changeTurnCo");
	}

	public void countdown () {
		if (turnTime <= timePerTurn) {
			turnTime -= Time.deltaTime;
		}
	}

	public IEnumerator changeTurnCo () {
		yield return new WaitForSeconds (0.2f);
		deselectAll ();
		int turn0 = turn;
		turn++;
		if (turn >= teams.Length) {
					turn = 0;
			}
		while (teams[turn] == null) {
			turn++;
			if (turn >= teams.Length) {
				turn = 0;
			}
			if (turn0 == turn){

				//yield return;
			}
		}
		changePlayer ();
	}
	
	public void changePlayer () {
		player[turn]++;
		int i = 0;
		if (player[turn] >= teams[turn].players.Length) {
			player[turn] = 0;
		}
		while (teams[turn].players[player[turn]] == null) {
			player[turn]++;
			if (player[turn] >= teams[turn].players.Length) {
				player[turn] = 0;
			}
			if (i >= teams[turn].players.Length){
				//win?
				return;
			}
			i++;
		}
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
	
	void OnGUI () {
		/*if (winner > 0) {
			GUI.Label(new Rect(Screen.width/2 - 40, Screen.height/2 - 10, 80, 20), "Player " + winner + " wins");
		}
		if (turn == 0) {
			int strength = Mathf.RoundToInt(shotForce.magnitude/10);
			
			int prefix = 1;
			if (shotForce.y < 1) {
				prefix = -1;
			}
			int angle = Mathf.RoundToInt(Vector3.Angle(Vector3.right, shotForce) * prefix);
			if (angle == -90) {
				angle = 0;
			}
			
			GUI.Label(new Rect(10, 10, 80, 20), "Strength: " + strength);
			GUI.Label(new Rect(100, 10, 80, 20), "Angle: " + angle + "°");
		}
		else {
			int strength = Mathf.RoundToInt(shotForce.magnitude/10);
			
			int prefix = 1;
			if (shotForce.y < 1) {
				prefix = -1;
			}
			int angle = Mathf.RoundToInt(Vector3.Angle(-Vector3.right, shotForce) * prefix);
			if (angle == -90) {
				angle = 0;
			}
			
			GUI.Label(new Rect(Screen.width - 180, 10, 80, 20), "Strength: " + strength);
			GUI.Label(new Rect(Screen.width - 90, 10, 80, 20), "Angle: " + angle + "°");
		}*/
	}

	// Use this for initialization
	void Start () {
		audio.Play();
		turnTime = timePerTurn;
		changeTurn ();
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
