using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float 		maxSpeed = 5.5f;
	public float		acceleration = 0.45f;
	public float		baseSpeed = 1;
	public float		sprintFactor = 2.5f;

	public float		jumpSpeed = 18.5f;
	public float		jumpAcc = 57.5f;

	public bool			grounded = true;
	public bool			jumping = false;
	private float		curSpeed = 0;
	private bool		bPressed = false;
	private bool		aPressed = false;
	private bool		aDown = false;
	public float		personalGravity = 95.15f;
	private bool		ChangeSize = false;		//keep mario from updating animation repeatitively

	public static bool	finished = false;
	public static bool	inCave = false;
	public static int	lives = 3;
	public static int	score = 0;
	public static float	time = 400f;
	public static bool	isBig = true;
	public static bool 	dead = false;
	public static bool	respawn = false;


	Animator marioAnim;


	void Start(){
		UnityEngine.Time.fixedDeltaTime = 0.005f; 
		rigidbody.inertiaTensor = rigidbody.inertiaTensor + new Vector3 (0, 0, rigidbody.inertiaTensor.z * 100);
		marioAnim = GetComponent<Animator>();
	}
	void OnGUI(){
		GUI.Label (new Rect (285, 90, 100, 30), score.ToString());
		GUI.Label (new Rect (385, 90, 100, 30), "x  " + lives.ToString());
		GUI.Label (new Rect (800, 90, 100, 30), ((int)time).ToString());

	}

	void Update () { // Every Frame

		if (ChangeSize) {
			marioAnim.SetBool ("ChangeSize", false);
			ChangeSize = false;
		}

		if (finished) {
			this.GameEnd ();
		}


		bPressed = (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.J));
		aDown = (Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.K));
		aPressed = (Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.K));

		time = time - 1 * Time.deltaTime*3;

		Vector3 vel = rigidbody.velocity;
		curSpeed = vel.x;

		float h = 0.0f;

		if(Input.GetKey(KeyCode.LeftArrow)){
			h = -1;
			if(bPressed){
				h *= sprintFactor;
			}
		}
		else if (Input.GetKey(KeyCode.RightArrow)){
			h = 1;
			if(bPressed){
				h *= sprintFactor;
			}

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

		// ---------- Jumping ----------
		if (aDown) {
			if (grounded) {
					vel.y = jumpSpeed;
					jumping = true;
					grounded = false;
			}
		}
		if (jumping && !grounded) {
			if (aPressed) {
				vel.y += jumpAcc * Time.deltaTime;
			}
			else{
				jumping = false;
			}
		}

		// Max speed
		if(curSpeed > maxSpeed || curSpeed < -1*maxSpeed){
			curSpeed = h*maxSpeed;
		}
		//Debug.Log("H is " + h + " and curSpeed is " + curSpeed);


		// ---------- Anim ----------

		if (isBig) {
			marioAnim.SetBool ("Big", true);
		} 
		else {
			marioAnim.SetBool ("Big", false);
		}
		if (curSpeed > 0) {
			marioAnim.SetBool ("RightDown", true);
			marioAnim.SetBool ("LeftDown", false);
		}
		else if (curSpeed < 0) {
			marioAnim.SetBool ("LeftDown", true);
			marioAnim.SetBool ("RightDown", false);
		}

		marioAnim.SetFloat("Speed", Mathf.Abs(curSpeed));
		if (curSpeed == 0) {
			marioAnim.SetBool ("Idle", true);
		} 
		else {
			marioAnim.SetBool ("Idle", false);
		}

		if (grounded) {
			marioAnim.SetBool ("Jumping", false);
		} 
		else {
			marioAnim.SetBool ("Jumping", true);
		}

		// ---------- respawn ----------
		if (respawn) {
			this.Respawn ();
		} 
		else if (dead) {
			this.Dead ();
		}


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
		topCenter.y += this.collider.bounds.size.y / 2 - distance/2;
		
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

		RaycastHit edgeInfo1;
		RaycastHit edgeInfo2;

		bool hitBotRight = Physics.Raycast (botRight, down, out edgeInfo1, distance);
		bool hitBotLeft = Physics.Raycast (botLeft, down, out edgeInfo2, distance);
		bool hitBot = hitBotRight || hitBotLeft; 
		
		if (!hitBot) {
			vel.y -= personalGravity*Time.deltaTime;
		}

		vel.x = curSpeed;
		rigidbody.velocity = vel;

		//   ---------- Pipes ----------
		bool DownPressed =  Input.GetKey(KeyCode.DownArrow) ||
				Input.GetKey(KeyCode.S) ||
				Input.GetKeyDown(KeyCode.DownArrow) ||
				Input.GetKeyDown(KeyCode.S);
		
		if (DownPressed && PipeIn.canUseWarpPipeIn) {
			//teleport mario into cave
			inCave = true;
			Vector3 temp = new Vector3(-55.5f,-9.0f,0);
			transform.position += temp;
			SetSpawn.respawnLoc = "secondRespawn";
		}
		bool RightPressed =  Input.GetKey(KeyCode.RightArrow) ||
				Input.GetKey(KeyCode.D) ||
				Input.GetKeyDown(KeyCode.RightArrow) ||
				Input.GetKeyDown(KeyCode.D);
		if (RightPressed && PipeOut.canUseWarpPipeOut) {
			//teleport mario out of cave
			inCave = false;
			Vector3 temp = new Vector3(155.0f,2.5f,0);
			transform.position = temp;
		}
	}

	public void Respawn(){
		string caseSwitch = SetSpawn.respawnLoc;
		//Debug.Log (caseSwitch);
		switch (caseSwitch) {
			case "firstRespawn":
				Vector3 temp = new Vector3 (-4.0f, 0.0f, 0);
				transform.position = temp;
				break;
			case "secondRespawn":
				Vector3 temp2 = new Vector3 (75.0f, 0.0f, 0);
				transform.position = temp2;
				break;
			default:
				Vector3 temp3 = new Vector3 (-4.0f, 0.0f, 0);
				transform.position = temp3;
				break;
		}
		lives--;
	}

	public void Dead(){
		//Destroy (this.gameObject);
	}

	public void GameEnd(){
		this.renderer.enabled = false;
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
		topCenter.y += this.collider.bounds.size.y / 2 - distance/2;

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
			if(hitTopCenter && centerInfo.collider.gameObject.tag == "Brick"){
				BrickScript brick = centerInfo.collider.gameObject.GetComponent<BrickScript>();
				brick.marioHit();
			}
		}
		//left

		//bot
		bool hitBotRight = Physics.Raycast (botRight, down, out edgeInfo1, distance);
		bool hitBotLeft = Physics.Raycast (botLeft, down, out edgeInfo2, distance);
		bool hitBot = hitBotRight || hitBotLeft; 

		if (hitBot) {
			Vector3 vel = rigidbody.velocity;
			string tag1 = "none"; 
			string tag2 = "none";

			if(hitBotLeft)
				tag2 = edgeInfo2.collider.tag;
			if(hitBotRight)
				tag1 = edgeInfo1.collider.tag;

			if(tag1 == "Enemy"){
				vel.y += 5;
				Destroy(edgeInfo1.collider.gameObject);

			}
			else if(tag2 == "Enemy"){
				vel.y += 5;
				Destroy(edgeInfo2.collider.gameObject);
			}
			else if(tag1 == "Shell" || tag2 == "Shell"){
				vel.y += 5;
			}
			else{
				grounded = true;
				jumping = false;
			}
			rigidbody.velocity = vel;
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


















