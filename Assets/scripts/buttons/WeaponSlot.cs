using UnityEngine;
using System.Collections;

public class WeaponSlot : MonoBehaviour {

	WeaponSelect weaponSelect;

	public Color normal = new Color (0.7f, 0.3f, 0.3f);

	public bool available = true;

	void Start () {
		weaponSelect = transform.parent.GetComponent ("WeaponSelect") as WeaponSelect;
	}

	void OnMouseDown () {
		for (int i=0; i<weaponSelect.buttons.Length; i++) {
			if (weaponSelect.buttons[i] == this.transform && available) {
				weaponSelect.selectWeapon(i);
				weaponSelect.colorizeButtons ();
			}
		}
	}

	//disable throwing in main
	void OnMouseEnter () {
		weaponSelect.thwart (true);
		if (available) {
			renderer.material.color = Color.green;
		}
	}

	void OnMouseExit () {
		weaponSelect.thwart (false);
		if (available) {
			renderer.material.color = normal;
		}
	}

	void Update () {

	}
}
