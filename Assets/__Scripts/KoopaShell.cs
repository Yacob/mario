using UnityEngine;
using System.Collections;

public class KoopaShell : MonoBehaviour {

	public float	timer = 6;
	public int		moveSpd = 2;
	private int		moveDir;
	private bool	moving = false;
	
	//Start is called at beginning
	void Start(){
		
	}

	// Update is called once per frame
	void Update () {

		Vector3 vel = rigidbody.velocity;
		if(moving){
			vel.x = moveSpd * moveDir;
			rigidbody.velocity = vel;
			timer = 6;
		}
		else{
			timer -= 1*Time.deltaTime;
			vel.x = 0;
			rigidbody.velocity = vel;
		}

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
		topRight.x += (this.collider.bounds.size.x / 2 - distance/2);
		
		Vector3 topCenter = center;
		topCenter.y += this.collider.bounds.size.y / 2 - distance/2;
		
		Vector3 topLeft = center;
		topLeft.y += this.collider.bounds.size.y / 2 - distance/2;
		topLeft.x -= (this.collider.bounds.size.x / 2 - distance/2);
		
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
		bool hitTopRight = Physics.Raycast (topRight, up, out edgeInfo1, distance);
		bool hitTopLeft = Physics.Raycast (topLeft, up, out edgeInfo2, distance);
		bool hitTopCenter = Physics.Raycast (topCenter, up, out centerInfo, distance);
		
		if (hitTopRight) {
			if(edgeInfo1.collider.tag == "Player"){
				moveDir = -1;
				moving = true;
				Vector3 vel = edgeInfo1.collider.gameObject.rigidbody.velocity;
				vel.y += .3f;
				edgeInfo1.collider.gameObject.rigidbody.velocity = vel;
			}
		}
		else if(hitTopLeft){
			if(edgeInfo2.collider.tag == "Player"){
				Debug.Log("topLeft");
				moveDir = 1;
				moving = true;
				Vector3 vel = edgeInfo1.collider.gameObject.rigidbody.velocity;
				vel.y += .3f;
				edgeInfo1.collider.gameObject.rigidbody.velocity = vel;

			}
		}

		string tag1 = "";
		string tag2 = "";
		string tag3 = "";
		bool playerUp = false;
		
		if (hitTopRight) {
			tag1 = edgeInfo1.collider.tag;
		}
		if (hitTopLeft) {
			tag1 = edgeInfo2.collider.tag;
		}
		if (hitTopCenter) {
			tag1 = centerInfo.collider.tag;
		}
		if (tag1 == "Player") {
			playerUp = true;
		}
		else if(tag2 == "Player"){
			playerUp = true;
		}
		else if(tag3 == "Player"){
			playerUp = true;
		}
		
		if (other.collider.tag == "Player" && !playerUp) {
			Mario.takeDamage();
		}


		//left
		bool hitLeft = Physics.Raycast (topLeft, left, out edgeInfo1, distance);
		hitLeft = hitLeft || Physics.Raycast (botLeft, left, out edgeInfo2, distance);

		if (hitLeft && !moving) {
			moving = true;
			moveDir = 1;
		} else if (hitLeft) {
			moveDir = 1;
		}
		//right
		bool hitRight = Physics.Raycast (topRight, right, out edgeInfo1, distance);
		hitRight = hitRight || Physics.Raycast (botRight, right, out edgeInfo2, distance);

		if (hitRight && !moving) {
			moving = true;
			moveDir = -1;
		} else if (hitRight) {
			moveDir = -1;
		}

	}
	void OnDestroy(){
		
	}
}




