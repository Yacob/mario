using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		Mario.score += (int)Mario.time;
		Mario.finished = true;
	}
}
