using UnityEngine;
using System.Collections;

public class FallenToDeath : MonoBehaviour {

	public static bool 	dead = false;
	public static bool	respawn = false;

	void OnTriggerEnter(){
		if (Mario.lives <= 1) {
			dead = true;
			respawn = false;
		}
		else {
			Debug.Log("dead?");
			respawn = true;
		}
	}
	

}
