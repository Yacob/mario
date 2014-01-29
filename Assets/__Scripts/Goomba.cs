using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour {

	public bool		moveRight;
	public int		moveSpd = 4;
	private int		moveDir;
	
	//Start is called at beginning
	void Start(){
		if (moveRight) {
			moveDir = 1;
		} else {
			moveDir = -1;
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 vel = rigidbody.velocity;
		vel.x = moveSpd * moveDir;
		rigidbody.velocity = vel;
	}

	Vector3 getNormal(Vector3 a, Vector3 b){
		Vector3 c = Vector3.Cross (a, b);
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross(side1, side2).normalized;
	}
	void OnTriggerEnter(Collider other){
		Vector3 dir = other.gameObject.transform.position - this.transform.position;
		Ray ray = new Ray (this.transform.position, dir);
		RaycastHit hitInfo;
		float distance = Vector3.Distance(this.transform.position, other.gameObject.transform.position) + 9999999;
		
		Debug.DrawLine(ray.origin, ray.origin + (ray.direction * distance), Color.red);

		if (!collider.Raycast (ray, out hitInfo, distance)) {
			return;
		}

		Vector3 hitNormal = hitInfo.normal;
		//hitNormal = hitInfo.transform.TransformDirection(hitNormal);

		if(hitNormal == hitInfo.transform.up)
		{
			Debug.Log("hit top");
		}
		if(hitNormal == -1*hitInfo.transform.up)
		{
			Debug.Log("hit bottom");
		}
		if(hitNormal == hitInfo.transform.right)
		{
			Debug.Log("hit right");
		}
		if(hitNormal == -1*hitInfo.transform.right)
		{
			Debug.Log("hit left");
		}


		/*Vector3 hitTrigger = other.bounds.center - this.transform.position;
		Debug.Log (hitTrigger.x + " " + hitTrigger.y);
		Vector3 col = other.transform.position - this.transform.position;
		if (moveDir == 1) { //this needs work
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
		}*/
	}
}


