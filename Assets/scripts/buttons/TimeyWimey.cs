using UnityEngine;
using System.Collections;

public class TimeyWimey : MonoBehaviour {

	public Texture2D[] timerSprites; 

	// Use this for initialization
	void Start () {
		Main main = Camera.main.GetComponent("Main") as Main;
		float fps = 36/main.timePerTurn;
	}
	
	// Update is called once per frame
	void Update () {
		Main main = Camera.main.GetComponent("Main") as Main;
		UISpriteAnimation animation = gameObject.GetComponent ("UISpriteAnimation") as UISpriteAnimation;
		if (main.turnTime >= main.timePerTurn) {
//			animation.mIndex = 0;
		}
	}
}
