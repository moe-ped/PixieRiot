using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {

	Main main;
	public int team = 0;

	float maxScore;

	public float startHeight;
	public float startWidth;

	// Use this for initialization
	void Start () {
		main = Camera.main.GetComponent ("Main") as Main;
		maxScore = main.maxScore;
	}

	void fill () {
		float score = main.teams[team].score;
		float percentage = score/maxScore;
		renderer.material.SetFloat("_Cutoff", 1-percentage);
	}

	void scale () {
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		float ratio = Screen.width / startWidth;
		Vector3 newScale = new Vector3 (Screen.width * 0.133645f, startHeight * ratio  * 0.133645f, 1);
		transform.localScale = newScale;
	}

	// Update is called once per frame
	void Update () {
		//better call from where points are added ...
		fill ();
		scale ();
	}
}
