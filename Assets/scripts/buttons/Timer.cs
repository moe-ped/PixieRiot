using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Main main = Camera.main.GetComponent("Main") as Main;
		UILabel lbl = GetComponent<UILabel>();
		string timerTime = "" + Mathf.Round(main.turnTime);
		if (main.turnTime > main.timePerTurn) {
			timerTime = "";
		}
		lbl.text = timerTime;
	}
}
