using UnityEngine;
using System.Collections;

public class EatThings : MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter(Collision other){
		Destroy (other.gameObject);
	}
}
