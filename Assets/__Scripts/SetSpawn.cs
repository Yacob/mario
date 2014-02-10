using UnityEngine;
using System.Collections;

public class SetSpawn : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		Mario.respawnLoc = new Vector3 (75.0f, 0.0f, 0);
	}
}
