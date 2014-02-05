using UnityEngine;
using System.Collections;

public class SetSpawn : MonoBehaviour {
	public static string respawnLoc = "firstRespawn";
	void OnTriggerEnter(Collider other){
		respawnLoc = "secondRespawn";
	}
}
