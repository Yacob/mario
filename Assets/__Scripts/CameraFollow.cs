using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform 	poi; // Point of Interest
	public float		u;
	public Vector3		offset = new Vector3(0,0,-5);
	public bool			lockCamera = false;
	
	// Update is called once per frame
	void Update () {
		Vector3 poiV3 = poi.position + offset;
		Vector3 currPos = transform.position;
		currPos.y = 5.5f;

		if (currPos.x <= poiV3.x) {
			Vector3 pos = (1 - u) * currPos + u * poiV3;
			pos.y = currPos.y;
			transform.position = pos;
		}
		if (Mario.inCave) {
			Vector3 cave = new Vector3 (0.0f, -8.75f, -5.0f);
			transform.position = cave;
		}

	}
}
