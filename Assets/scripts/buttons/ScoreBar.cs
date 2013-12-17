using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {

	float initialScaley = 1;

	public int side;

	public float height;

	float initialPositiony;

	// Use this for initialization
	void Start () {
		initialScaley = transform.localScale.y;
		initialPositiony = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		Main main = Camera.main.GetComponent("Main") as Main;
		float scorePercent = main.teams[side].score/main.maxScore;
		float newScaley = initialScaley * scorePercent;
		transform.position = new Vector3 (transform.position.x, initialPositiony - (height/2 * (1-scorePercent)), transform.position.z); 
		transform.localScale = new Vector3 (transform.localScale.x, newScaley, transform.localScale.z);
	}
}
