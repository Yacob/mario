using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public bool		coinUsed = false;

	void OnTriggerEnter(){
		if (!coinUsed) {
			Mario.coins++;
		}
		Destroy (this.gameObject);
	}
}
