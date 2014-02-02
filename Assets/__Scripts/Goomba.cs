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
		Vector3 right = Vector3.Cross(-1*this.transform.forward,this.transform.up);
		Vector3 down = Vector3.Cross(-1*this.transform.forward,this.transform.right);
		Vector3 left = Vector3.Cross(-1*this.transform.forward,-1*this.transform.up);
		Vector3 up = Vector3.Cross(-1*this.transform.forward,-1*this.transform.right);

		Vector3 center = this.transform.position;
		float distance = .1f;

		
		Vector3 topRight = center;
		topRight.y += this.collider.bounds.size.y / 2 - distance/2;
		topRight.x += this.collider.bounds.size.x / 2 - distance/2;

		Vector3 topLeft = center;
		topLeft.y += this.collider.bounds.size.y / 2 - distance/2;
		topLeft.x -= this.collider.bounds.size.x / 2 - distance/2;

		Vector3 botRight = center;
		botRight.y -= this.collider.bounds.size.y / 2 - distance/2;
		botRight.x += this.collider.bounds.size.x / 2 - distance/2;

		Vector3 botLeft = center;
		botLeft.y -= this.collider.bounds.size.y / 2 - distance/2;
		botLeft.x -= this.collider.bounds.size.x / 2 - distance/2;
		
		Ray rRay = new Ray (this.transform.position, right);
		Ray dRay = new Ray (this.transform.position, down);
		Ray lRay = new Ray (this.transform.position, left);
		Ray uRay = new Ray (this.transform.position, up);
		
		//RaycastHit hitInfo;

		//top
		Debug.DrawLine(topRight, topRight + (uRay.direction * distance), Color.red);
		Debug.DrawLine(topLeft, topLeft + (uRay.direction * distance), Color.red);

		//left
		Debug.DrawLine(topLeft, topLeft + (lRay.direction * distance), Color.red);
		Debug.DrawLine(botLeft, botLeft + (lRay.direction * distance), Color.red);

		//bot
		Debug.DrawLine(botRight, botRight + (dRay.direction * distance), Color.red);
		Debug.DrawLine(botLeft, botLeft + (dRay.direction * distance), Color.red);

		//right
		Debug.DrawLine(topRight, topRight + (rRay.direction * distance), Color.red);
		Debug.DrawLine(botRight, botRight + (rRay.direction * distance), Color.red);

		
		
		
	}

	/*Vector3 getNormal(Vector3 a, Vector3 b){
		Vector3 c = Vector3.Cross (a, b);
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross(side1, side2).normalized;
	}*/
	void OnCollisionEnter(Collision other){
		Vector3 right = Vector3.Cross(-1*this.transform.forward,this.transform.up);
		Vector3 down = Vector3.Cross(-1*this.transform.forward,this.transform.right);
		Vector3 left = Vector3.Cross(-1*this.transform.forward,-1*this.transform.up);
		Vector3 up = Vector3.Cross(-1*this.transform.forward,-1*this.transform.right);

		/*Ray rRay = new Ray (this.transform.position, right);
		Ray dRay = new Ray (this.transform.position, down);
		Ray lRay = new Ray (this.transform.position, left);
		Ray uRay = new Ray (this.transform.position, up);*/

		Vector3 center = this.collider.bounds.center;
		float distance = .1f;

		Vector3 topRight = center;
		topRight.y += this.collider.bounds.size.y / 2 - distance/2;
		topRight.x += this.collider.bounds.size.x / 2 - distance/2;
		
		Vector3 topCenter = center;
		topRight.y += this.collider.bounds.size.y / 2 - distance/2;
		
		Vector3 topLeft = center;
		topLeft.y += this.collider.bounds.size.y / 2 - distance/2;
		topLeft.x -= this.collider.bounds.size.x / 2 - distance/2;
		
		Vector3 botRight = center;
		botRight.y -= this.collider.bounds.size.y / 2 - distance/2;
		botRight.x += this.collider.bounds.size.x / 2 - distance/2;
		
		Vector3 botLeft = center;
		botLeft.y -= this.collider.bounds.size.y / 2 - distance/2;
		botLeft.x -= this.collider.bounds.size.x / 2 - distance/2;

		RaycastHit edgeInfo1 = new RaycastHit();
		RaycastHit edgeInfo2 = new RaycastHit();
		RaycastHit centerInfo = new RaycastHit();

		//top
		bool hitTop = Physics.Raycast (topRight, up, out edgeInfo1, distance);
		hitTop = hitTop || Physics.Raycast (topLeft, up, out edgeInfo2, distance);
		hitTop = hitTop || Physics.Raycast (topCenter, up, out centerInfo, distance);
		
		if (hitTop) {
			if(other.gameObject.tag == "Player"){
				Debug.Log("Goomba Killed");
				Destroy(this.gameObject);
			}
		}
		//left
		bool hitLeft = Physics.Raycast (topLeft, left, out edgeInfo1, distance);
		hitLeft = hitLeft || Physics.Raycast (botLeft, left, out edgeInfo2, distance);

		if (hitLeft) {
			if(moveDir == -1){
				moveDir*=-1;
			}
		}
		//right
		bool hitRight = Physics.Raycast (topRight, right, out edgeInfo1, distance);
		hitRight = hitRight || Physics.Raycast (botRight, right, out edgeInfo2, distance);

		if (hitRight) {
			if(moveDir == 1){
				moveDir*=-1;
			}
		}



		//Vector3 dir = 
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


