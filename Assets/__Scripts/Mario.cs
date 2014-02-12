using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float 			maxSpeed = 5.5f;
	public float			acceleration = 0.45f;
	public float			baseSpeed = 1;
	public float			sprintFactor = 1.55f;

	public float			jumpSpeed = 14f;
	public float			jumpAcc = 55f;

	public bool				grounded = true;
	public bool				jumping = false;
	public Transform		fireBall;
	private float			curSpeed = 0;
	private float			personalGravity = 95.15f;
	private bool			ChangeSize = false;		//keep mario from updating animation repeatitively

	private static float	noClip = 0f;
	public static bool		finished = false;
	public static bool		inCave = false;
	public static int		lives = 3;
	public static int		coins = 0;
	public static int		score = 0;
	public static float		time = 400f;
	public static bool		isBig = false;
	public static bool		isFire = false;
	public static bool 		dead = false;
	public static bool		respawn = false;
	public static bool		hitShroom = false;	
	public static bool		hitFire = false;
	public static bool		dmg = false;
	public static Vector3	respawnLoc = new Vector3(-4.0f, 0.0f, 0);
	public static bool		toTheNewWorldAway = false;
	public static int		numFire = 0;
	private static bool		reset = false;

	public static Mario me;	

	public GUIStyle customStyle;
	
	public static Animator marioAnim;

	public struct ScoreCounter{
		public int 		counter;
		public float 	decay;
	};

	public ScoreCounter SCount;


	void Awake(){
		finished = false;
		inCave = false;
		time = 400f;
		isBig = false;
		isFire = false;
		dead = false;
		respawn = false;
		hitShroom = false;
		hitFire = false;
		dmg = false;
		noClip = 0f;
		if(reset)
			transform.position = respawnLoc;
		numFire = 0;
	}
	void Start(){
		UnityEngine.Time.fixedDeltaTime = 0.005f; 
		rigidbody.inertiaTensor = rigidbody.inertiaTensor + new Vector3 (0, 0, rigidbody.inertiaTensor.z * 100);
		marioAnim = GetComponent<Animator>();
		me = this;
		SCount.counter++;
		//reset = false;
		//respawnLoc = new Vector3(-4.0f, 0.0f, 0);
	}
	void OnGUI(){
		GUI.Label (new Rect (0, 10, 100, 30), "Score: " + score.ToString(), customStyle);
		GUI.Label (new Rect (120, 10, 100, 30), "Coins: " + coins.ToString(), customStyle);
		GUI.Label (new Rect (220, 10, 100, 30), "Time " + ((int)time).ToString(), customStyle);
		GUI.Label (new Rect (340, 10, 100, 30), "Lives " + lives.ToString(), customStyle);
	}

	void Update () { // Every Frame
		bool bPressed;
		bool aPressed;
		bool aDown;
		bool bDown;
		bool downDown;

		if (ChangeSize) {
			marioAnim.SetBool ("ChangeSize", false);
			ChangeSize = false;
		}
		if (hitShroom) {
			hitShroom = false;
			marioAnim.SetBool ("ChangeSize", true);
			ChangeSize = true;
		}
		else if(dmg){
			dmg = false;
			marioAnim.SetBool ("ChangeSize", true);
			ChangeSize = true;
		}

		if (finished) {
			this.GameEnd ();
		}

		if (noClip > 0) {
			noClip -= Time.deltaTime;
			Physics.IgnoreLayerCollision(11,16, true);
		}
		else{
			Physics.IgnoreLayerCollision(11,16, false);
		}

		bPressed = (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.J));
		bDown = (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J));
		aDown = (Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.K));
		aPressed = (Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.K));
		downDown = (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S));

		time = time - Time.deltaTime*3;

		if(time <= 0){
			Respawn();
		}

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
		if ((h == 0 && grounded) || downDown) {
			if(curSpeed > 0){
				curSpeed -= acceleration;
				if(curSpeed < 0){
					curSpeed = 0;
				}
			}
			else {
				curSpeed += acceleration;
				if(curSpeed > 0){
					curSpeed = 0;
				}
			}
		}
		else if (h != 0 && curSpeed == 0) {
			curSpeed = h*baseSpeed;
		}
		else if (grounded || h != 0) {
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
		if(curSpeed > Mathf.Abs(h) * maxSpeed || curSpeed < Mathf.Abs(h) * -1 * maxSpeed){
			curSpeed = h*maxSpeed;
		}

		//------------Fire ---------
		if (bDown && isFire) {
			if(numFire < 2){
				shootFire();
			}
		}


		// ---------- Anim ----------

		if (isBig) {
			marioAnim.SetBool ("Big", true);
			Vector3 temp = new Vector3(0.875f, 2, 0.8f);
			((BoxCollider)this.GetComponent<BoxCollider>()).size = temp;
			temp = new Vector3(0, 1, 0);
			((BoxCollider)this.GetComponent<BoxCollider>()).center = temp;
		} 
		else {
			marioAnim.SetBool ("Big", false);
			Vector3 temp = new Vector3(0.875f, 1, 0.8f);
			((BoxCollider)this.GetComponent<BoxCollider>()).size = temp;
			temp = new Vector3(0, 0.5f, 0);
			((BoxCollider)this.GetComponent<BoxCollider>()).center = temp;
		}
		if (isFire) {
			//marioAnim.SetBool ("Fire", true);
		} 
		else {
			//marioAnim.SetBool ("Fire", false);
		}
		if (downDown) {
			marioAnim.SetBool ("DownDown", true);
		}
		else {
			marioAnim.SetBool ("DownDown", false);
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
			Mario.respawnLoc = new Vector3 (75.0f, 0.0f, 0);
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

		// ---------- New World ----------
		if (toTheNewWorldAway) {
			toTheNewWorldAway = false;
			Vector3 newWorld = new Vector3 (230.0f, 0.0f, 0);
			respawnLoc = new Vector3 (230.0f, 0.0f, 0);
			transform.position = newWorld;
		}
		if (Mario.respawn) {
			respawn = false;
			Respawn ();
		}


	}

	public void Respawn(){
		if(lives == 1){
			Dead ();
			return;
		}
		//Application.LoadLevel(0);
		Debug.Log ("respawning");
		lives--;
		time = 400;
		reset = true;
		Application.LoadLevel(Application.loadedLevel);

	}

	public void Dead(){
		reset = false;
		lives = 3;
		Application.LoadLevel(0);
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

	public static void takeDamage(){
		if(isFire){
			isFire = false;
			dmg = true;
			noClip = 4;
		}
		else if(isBig){
			isBig = false;
			dmg = true;
			noClip = 4;
		}
		else{
			Debug.Log("here I am");
			me.Respawn();
		}
	}
	void shootFire(){
		AnimatorStateInfo state =  marioAnim.GetCurrentAnimatorStateInfo(0);
		Vector3 offset = new Vector3();
		if (state.IsTag ("right"))
			offset.x = 1f;
		else
			offset.x = -1.5f;
		offset.y += 1f;
		Instantiate (fireBall, this.transform.position + offset, Quaternion.identity);
	}
}


















