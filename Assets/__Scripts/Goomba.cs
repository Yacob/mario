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
	void onCollisionEnter(Collision col){
		//Vector3 diff = this.transform.position - col.gameObject.transform.position;
		Vector3 direction = getNormal (this.transform.position, col.gameObject.transform.position);
		if (direction.x == moveDir)	moveDir *= -1;
	}
	Vector3 getNormal(Vector3 a, Vector3 b){
		Vector3 c = Vector3.Cross (a, b);
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross(side1, side2).normalized;
	}
}
