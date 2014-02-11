using UnityEngine;
using System.Collections;

public class NewWorldRespawn : MonoBehaviour {

	public float		x_coord = 230.0f;
	public float		y_coord = 0.0f;
	public float		z_coord = 0.0f;
	void OnTriggerEnter(Collider other){
		Mario.respawnLoc = new Vector3 (x_coord, y_coord, z_coord);
	}
}
