using UnityEngine;
using System.Collections;

public class FallenToDeath : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
				Mario.respawn = true;
		}
	}

}
