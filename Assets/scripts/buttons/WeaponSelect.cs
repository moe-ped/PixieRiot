using UnityEngine;
using System.Collections;

public class WeaponSelect : MonoBehaviour {

	public int team;
	public Transform[] buttons;
	Main main;

	public Color unavailable;
	public Color highlight;

	// Use this for initialization
	void Start () {
		main = Camera.main.GetComponent("Main") as Main;
		texturizeButtons ();
		colorizeButtons ();
	}

	void texturizeButtons () {
		for (int b=0; b<buttons.Length; b++) {
			buttons[b].gameObject.renderer.material.mainTexture = main.weapons[b].icon;
		}
	}

	public void colorizeButtons () {
		for (int b=0; b<buttons.Length; b++) {
			WeaponSlot weaponslot = buttons[b].GetComponent("WeaponSlot") as WeaponSlot;
			if (main.teams[team].selectedWeapon == b) {
				buttons[b].gameObject.renderer.material.color = Color.white;
				weaponslot.normal = Color.white;
			}
			else if (!main.weapons[b].available) {
				weaponslot.available = false;
				buttons[b].gameObject.renderer.material.color = unavailable;
				weaponslot.normal = unavailable;
			}
			else {
				weaponslot.available = true;
				buttons[b].gameObject.renderer.material.color = Color.grey;
				weaponslot.normal = Color.grey;
			}
		}
	}

	public void selectWeapon (int index) {
		main.teams[team].selectedWeapon = index;
	}

	public void thwart (bool greenLight) {
		main.MouseOver = greenLight;
	}
	
	// Update is called once per frame
	void Update () {
		if (main.turnChange) {
			colorizeButtons ();
		}
	}
}
