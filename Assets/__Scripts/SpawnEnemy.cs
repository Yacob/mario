using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
	public float 	x;
	public float 	y;
	private Vector3	loc;
	public bool canSpawn = true;
	public Transform enemy;
	// Use this for initialization
	void Start () {
		loc = new Vector3 (x,y,0f);
	}
	
	// Update is called once per frame
	void OnTriggerEnter(){
		if(canSpawn){
			canSpawn = false;
			Debug.Log ("spawning");
			Object baddie = Instantiate(enemy, new Vector3(x,y,0f), Quaternion.identity);
			//((Goomba)baddie).moveRight = false;
		}
	}
}
