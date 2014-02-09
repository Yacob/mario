using UnityEngine;
using System.Collections;

public class FallenToDeath : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			if (Mario.lives <= 1) {
				Mario.dead = true;
				Mario.respawn = false;
			} 
			else {
				Debug.Log ("dead?");
				Mario.respawn = true;
			}
		}
	}

}
