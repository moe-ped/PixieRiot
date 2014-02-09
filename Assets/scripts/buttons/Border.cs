using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Border : MonoBehaviour {

	public float startHeight;
	public float startWidth;

	// Use this for initialization
	void Start () {

	}

	void OnEnable ()
	{
		scale ();
	}

	void scale () {
		float aspectRatio = (float)Screen.width / (float)Screen.height;
		float ratio = Screen.width / startWidth;
		Vector3 newScale = new Vector3 (Screen.width, startHeight * ratio);
		transform.localScale = newScale;
	}

	// Update is called once per frame
	void Update () {
		scale ();
	}
}
