using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public bool		coinUsed = false;

	void OnTriggerEnter(){
		if (!coinUsed) {
			coinUsed = true;
			Mario.score += 100;
			GUI.Label (new Rect (this.transform.position.x + 10, this.transform.position.y - 10, 20, 30), "100");
			Destroy (this.gameObject);
		}
	}
}
