using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {

	public Transform flaggyMario;
	public float y;
	public static bool used = false;


	void OnTriggerEnter(Collider other){

		if (!used) {
			used = true;
			Mario.score += (int)Mario.time;
			Mario.finished = true;
			Object flagSlider = Instantiate (flaggyMario, new Vector3 (191.0f, y, 0f), Quaternion.identity);
		}
	}
}
