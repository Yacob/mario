using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {
	void OnCollisionEnter(Collision other){
		if (other.collider.tag == "Player") {
			Mario.hitFire = true;
			Mario.isFire = true;

			Destroy (this.gameObject);
			return;
		}
	}
}

