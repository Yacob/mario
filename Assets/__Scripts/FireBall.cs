using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	public int 		moveDir;
	public float	moveSpd;
	public int		vertSpeed;
	public int		upDown;
	public float	vertCeil;
	// Use this for initialization
	void Start () {
		moveDir = 1;
		AnimatorStateInfo state =  Mario.marioAnim.GetCurrentAnimatorStateInfo(0);
		string name = state.ToString();
		if (state.IsTag ("right"))
			moveDir = 1;
		else
			moveDir = -1;
		upDown = -1;
		vertSpeed = 6;
		moveSpd = 10;
		vertCeil = 100;
		Mario.numFire++;
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 newPos = this.transform.position;
		newPos.x = moveSpd * moveDir * Time.deltaTime;
		newPos.y = vertSpeed * upDown * Time.deltaTime;

		if (newPos.y > vertCap)
			newPos.y = vertCap;

		transform.position = newPos;*/
		Vector3 vel = this.rigidbody.velocity;
		vel.x = moveDir * moveSpd;
		if(this.transform.position.y > vertCeil){
			Debug.Log(vertCeil + " = cap, cur = " +this.transform.position.y );
			upDown = -1;
		}
		vel.y = upDown * vertSpeed;
		this.rigidbody.velocity = vel;
	
	}
	void OnCollisionEnter(Collision other){
		if(other.collider.tag == "Enemy" || other.collider.tag == "Shell"){
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}



		Vector3 right = Vector3.Cross(-1*this.transform.forward,this.transform.up);
		Vector3 down = Vector3.Cross(-1*this.transform.forward,this.transform.right);
		Vector3 left = Vector3.Cross(-1*this.transform.forward,-1*this.transform.up);
		Vector3 up = Vector3.Cross(-1*this.transform.forward,-1*this.transform.right);
		
		/*Ray rRay = new Ray (this.transform.position, right);
		Ray dRay = new Ray (this.transform.position, down);
		Ray lRay = new Ray (this.transform.position, left);
		Ray uRay = new Ray (this.transform.position, up);*/
		
		Vector3 center = this.collider.bounds.center;
		float distance = .05f;
		
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
		
		RaycastHit edgeInfo1 = new RaycastHit();
		RaycastHit edgeInfo2 = new RaycastHit();

		//top
		bool hitTopRight = Physics.Raycast (topRight, up, out edgeInfo1, distance);
		bool hitTopLeft = Physics.Raycast (topLeft, up, out edgeInfo2, distance);

		if(hitTopLeft||hitTopRight){
			Destroy (this.gameObject);
		}
		//bot
		bool hitBotRight = Physics.Raycast (botRight, down, out edgeInfo1, distance);
		bool hitBotLeft = Physics.Raycast (botLeft, down, out edgeInfo2, distance);
		
		if(hitBotLeft||hitBotRight){
			vertCeil = this.transform.position.y + 1;
			upDown = 1;
		}
		//left
		bool hitLeft = Physics.Raycast (topLeft, left, out edgeInfo1, distance);
		hitLeft = hitLeft || Physics.Raycast (botLeft, left, out edgeInfo2, distance);
		
		if (hitLeft) {
			Destroy(this.gameObject);
		}
		//right
		bool hitRight = Physics.Raycast (topRight, right, out edgeInfo1, distance);
		hitRight = hitRight || Physics.Raycast (botRight, right, out edgeInfo2, distance);
		
		if (hitRight) {
			Destroy(this.gameObject);
		}

	}
	void OnDestroy(){
		Mario.numFire--;
	}
}
