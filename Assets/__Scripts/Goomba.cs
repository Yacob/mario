using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour {

	public int		moveDir = 1;
	public int		moveSpd = 4;
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = rigidbody.velocity;
		vel.x = moveSpd * moveDir;
		rigidbody.velocity = vel;

	}
	void OnCollisionEnter(Collision col){

	}
	Vector3 getNormal(Vector3 a, Vector3 b){
		Vector3 c = Vector3.Cross (a, b);
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross(side1, side2).normalized;
	}
	void OnTriggerEnter(Collider other){
		Vector3 col = other.transform.position - this.transform.position;
		if (moveDir == 1) {
			if (col.x > 0) {
					moveDir = -1;
			}
		} else {
			if(col.x < 0){
				moveDir = 1;
			}
		}
		if (col.y > 0) {
			Debug.Log(other.tag);

			//hit from above
			if (other.tag == "Player"){
				Debug.Log("die");
				Destroy(this.gameObject);
			}
		}
	}
}


