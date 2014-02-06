using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform 	poi; // Point of Interest
	public float		u;
	public Vector3		offset = new Vector3(0,0,-5);
	public bool			lockCamera = false;
	

	void reCenter(){
		Vector3 curr = poi.position + offset;
		transform.position = curr;
		FallenToDeath.respawn = false;
	}


	// Update is called once per frame
	void Update () {
		if (FallenToDeath.respawn) {
			reCenter ();
		}
		if (Mario.inCave) {
			Vector3 cave = new Vector3 (0.0f, -8.75f, -5.0f);
			transform.position = cave;
		} 
		else {
			Vector3 poiV3 = poi.position + offset;
			Vector3 currPos = transform.position;
			currPos.y = 5.5f;
			if (currPos.x < 0) {
				Vector3 temp = new Vector3 (4.0f, 0, 0);
				transform.position += temp;
			}
			if (currPos.x <= poiV3.x) {
				Vector3 pos = (1 - u) * currPos + u * poiV3;
				pos.y = currPos.y;
				transform.position = pos;
			}
		}
	}

}
