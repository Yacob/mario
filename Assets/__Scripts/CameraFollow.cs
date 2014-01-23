﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform 	poi; // Point of Interest
	public float		u;
	public Vector3		offset = new Vector3(0,0,-5);
	
	// Update is called once per frame
	void Update () {
		Vector3 poiV3 = poi.position + offset;
		Vector3 currPos = transform.position;
		Vector3 pos = (1-u)*currPos + u*poiV3;
		transform.position = pos;

		//if y position is < some value we raise camera so that the ground is bottom of screen
		//same thing for the ceiling
	}
}
