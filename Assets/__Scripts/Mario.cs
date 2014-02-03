using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float 		maxSpeed = 6;
	public float		acceleration = 2;
	public float		baseSpeed = 1;

	public float		jumpSpeed = 14;
	public float		jumpAcc = 12;

	public bool			grounded = true;
	public bool			jumping = false;

	private float		curSpeed = 0;
	public static bool	inCave = false;

	void Start(){
		UnityEngine.Time.fixedDeltaTime = 0.005f; 
		rigidbody.inertiaTensor = rigidbody.inertiaTensor + new Vector3 (0, 0, rigidbody.inertiaTensor.z * 100);
	}

	void Update () { // Every Frame
		//float h = Input.GetAxis ("Horizontal");
		//float v = Input.GetAxis ("Vertical");
		Vector3 vel = rigidbody.velocity;
		curSpeed = vel.x;

		int h = 0;

		if(Input.GetKey(KeyCode.LeftArrow)){
			h = -1;
		}
		else if (Input.GetKey(KeyCode.RightArrow)){
			h = 1;
		}

		//set sideways motion
		if (h == 0 && grounded) {
			curSpeed = 0;
		}
		else if (h != 0 && curSpeed == 0) {

			curSpeed = h*baseSpeed;
		}
		else if (grounded) {
			curSpeed = curSpeed + h*acceleration;
		}
		else if( h != 0){
			curSpeed = curSpeed + h*acceleration;			
		}

		if (Input.GetKeyDown (KeyCode.Space) ||
			Input.GetKeyDown (KeyCode.UpArrow) ||
			Input.GetKeyDown (KeyCode.W)) {
			if (grounded) {
					vel.y = jumpSpeed;
					jumping = true;
					grounded = false;
			}
		}
		if (jumping && !grounded) {
			if (Input.GetKey (KeyCode.Space) ||
				Input.GetKey (KeyCode.UpArrow) ||
				Input.GetKey (KeyCode.W)) {

				vel.y += jumpAcc * Time.deltaTime;
			}
			else{
				jumping = false;
			}
		}
		if(curSpeed > maxSpeed || curSpeed < -1*maxSpeed){
			curSpeed = h*maxSpeed;
		}
		vel.x = curSpeed;
		if ((Input.GetKeyDown (KeyCode.DownArrow) ||
				Input.GetKeyDown (KeyCode.S)) && PipeIn.canUseWarpPipeIn) {
				//teleport mario into cave
			inCave = true;
			Vector3 temp = new Vector3(-55.5f,-9.0f,0);
			transform.position += temp;
		}
		if ((Input.GetKeyDown (KeyCode.RightArrow) ||
		     Input.GetKeyDown (KeyCode.D)) && PipeOut.canUseWarpPipeOut) {
			//teleport mario out of cave
			inCave = false;
			Vector3 temp = new Vector3(155.0f,2.5f,0);
			transform.position = temp;

			
		}
		rigidbody.velocity = vel;


		Vector3 right = Vector3.Cross(-1*this.transform.forward,this.transform.up);
		Vector3 down = Vector3.Cross(-1*this.transform.forward,this.transform.right);
		Vector3 left = Vector3.Cross(-1*this.transform.forward,-1*this.transform.up);
		Vector3 up = Vector3.Cross(-1*this.transform.forward,-1*this.transform.right);
		
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

		Debug.DrawLine(botRight, botRight + (down*distance), Color.red);
		Debug.DrawLine(botLeft, botLeft + (down * distance), Color.red);

	}

	void OnCollisionEnter(Collision other) {
		Vector3 right = Vector3.Cross(-1*this.transform.forward,this.transform.up);
		Vector3 down = Vector3.Cross(-1*this.transform.forward,this.transform.right);
		Vector3 left = Vector3.Cross(-1*this.transform.forward,-1*this.transform.up);
		Vector3 up = Vector3.Cross(-1*this.transform.forward,-1*this.transform.right);
		
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
		
		RaycastHit edgeInfo1;
		RaycastHit edgeInfo2;
		RaycastHit centerInfo;

		//top
		bool hitTop = Physics.Raycast (topRight, up, out edgeInfo1, distance);
		hitTop = hitTop || Physics.Raycast (topLeft, up, out edgeInfo2, distance);
		hitTop = hitTop || Physics.Raycast (topCenter, up, out centerInfo, distance);

		if (hitTop) {
			if(other.gameObject.tag == "Brick"){
				other.gameObject.animation.Play();
			}
		}
		//left

		//bot
		bool hitBot = Physics.Raycast (botRight, down, out edgeInfo1, distance);
		hitBot = hitBot || Physics.Raycast (botLeft, down, out edgeInfo2, distance);

		//Debug.Log("hit the fucking goomba");

		if (hitBot) {
			if(other.gameObject.tag != "Enemy"){
				Debug.Log("gameObject = " + other.gameObject.tag);
				grounded = true;
				jumping = false;
			}
			else{
				Destroy(other.gameObject);
			}
		}

		
		//right
		//Debug.DrawLine(topRight, topRight + (rRay.direction * distance), Color.red);
		//Debug.DrawLine(botRight, botRight + (rRay.direction * distance), Color.red);
		

		//grounded = true;
		//jumping = false;
	}
	void OnCollisionExit(Collision other){
		Vector3 right = Vector3.Cross(-1*this.transform.forward,this.transform.up);
		Vector3 down = Vector3.Cross(-1*this.transform.forward,this.transform.right);
		Vector3 left = Vector3.Cross(-1*this.transform.forward,-1*this.transform.up);
		Vector3 up = Vector3.Cross(-1*this.transform.forward,-1*this.transform.right);
		
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
		
		RaycastHit edgeInfo1;
		RaycastHit edgeInfo2;
		RaycastHit centerInfo;
		
		//top
		bool hitTop = Physics.Raycast (topRight, up, out edgeInfo1, distance);
		hitTop = hitTop || Physics.Raycast (topLeft, up, out edgeInfo2, distance);
		hitTop = hitTop || Physics.Raycast (topCenter, up, out centerInfo, distance);
		
		/*if (hitTop) {
			if(other.gameObject.tag == "Brick"){
				other.gameObject.animation.Play();
			}
		}*/
		//left
		//Debug.DrawLine(topLeft, topLeft + (lRay.direction * distance), Color.red);
		//Debug.DrawLine(botLeft, botLeft + (lRay.direction * distance), Color.red);
		
		//bot
		bool hitBot = Physics.Raycast (botRight, down, out edgeInfo1, distance);
		hitBot = hitBot || Physics.Raycast (botLeft, down, out edgeInfo2, distance);

		if (!hitBot) {
			grounded = false;
		}

	}
}


















