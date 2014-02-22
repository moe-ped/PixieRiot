using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Fontstretch : MonoBehaviour {

	public float relativeSize = 0.1f;

	// Use this for initialization
	void Start () {
		stretch ();
	}

	void OnRenderObject () {
		stretch ();
	}

	void stretch () {
		GUIText text = GetComponent ("GUIText") as GUIText;
		float size = relativeSize * Screen.height;
		text.fontSize = (int) size;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
