using UnityEngine;
using System.Collections;

public class TeleportToNewWorld : MonoBehaviour {

	private bool triggered = false;
	void OnTriggerEnter(Collider other){
		if (!triggered) {
			triggered = true;
			Application.LoadLevel(1);
		}
	}
}
