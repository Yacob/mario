using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float 		maxSpeed = 6;
	public float		acceleration = 2;
	public float		baseSpeed = 1;

	public float		jumpSpeed = 19;
	public float		jumpAcc = 50;

	public bool			grounded = true;
	public bool			jumping = false;

	private float		curSpeed = 0;
	public static bool	inCave = false;
	public static int	lives = 3;

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



		//Respawns
		if (FallenToDeath.respawn) {
			string caseSwitch = SetSpawn.respawnLoc;
			switch(caseSwitch){
				case "firstRespawn":
					vel.x = 0;;
					Vector3 temp = new Vector3 (-4.0f, 0.0f, 0);
					transform.position = temp;
					break;
				case "secondRespawn":
					vel.x = 0;
					Vector3 temp2 = new Vector3(75.0f, 0.0f, 0);
					transform.position = temp2;
					break;
				default:
					//huh
					break;
			}
		}
		if (FallenToDeath.dead) {
			//destroy mario
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
		bool hitTopRight = Physics.Raycast (topRight, up, out edgeInfo1, distance);
		bool hitTopLeft = Physics.Raycast (topLeft, up, out edgeInfo2, distance);
		bool hitTopCenter = Physics.Raycast (topCenter, up, out centerInfo, distance);

		bool hitTop = hitTopLeft || hitTopRight || hitTopCenter;

		if (hitTop) {
			if(hitTopCenter && edgeInfo2.collider.tag == "Brick"){
				other.gameObject.animation.Play();
			}
		}
		//left

		//bot
		bool hitBotRight = Physics.Raycast (botRight, down, out edgeInfo1, distance);
		bool hitBotLeft = Physics.Raycast (botLeft, down, out edgeInfo2, distance);
		bool hitBot = hitBotRight || hitBotLeft; 

		if (hitBot) {
			string tag1 = "none"; 
			string tag2 = "none";
			if(hitBotLeft)
				tag1 = edgeInfo2.collider.tag;
			if(hitBotRight)
				tag2 = edgeInfo1.collider.tag;

			if(tag1 == "Enemy")
				Destroy(edgeInfo1.collider.gameObject);
			else if(tag2 == "Enemy")
				Destroy(edgeInfo1.collider.gameObject);
			else{
				grounded = true;
				jumping = false;
			}
		}

		
		//right
		//Debug.DrawLine(topRight, topRight + (rRay.direction * distance), Color.red);
		//Debug.DrawLine(botRight, botRight + (rRay.direction * distance), Color.red);
		

		//grounded = true;
		//jumping = false;
		//Pipes
		bool DownPressed =  Input.GetKey(KeyCode.DownArrow) ||
		                    Input.GetKey(KeyCode.S) ||
		                    Input.GetKeyDown(KeyCode.DownArrow) ||
		 					Input.GetKeyDown(KeyCode.S);

		 if (DownPressed && PipeIn.canUseWarpPipeIn) {
			//teleport mario into cave
			inCave = true;
			Vector3 temp = new Vector3(-55.5f,-9.0f,0);
			transform.position += temp;
		}
		if ((Input.GetKey (KeyCode.RightArrow) ||
		     Input.GetKey (KeyCode.D)) && PipeOut.canUseWarpPipeOut) {
			//teleport mario out of cave
			inCave = false;
			Vector3 temp = new Vector3(155.0f,2.5f,0);
			transform.position = temp;
		}
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


















