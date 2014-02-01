using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float		speed = 6;
	public float		jumpSpeed = 18;
	public float		jumpAcc = 10;

	public bool			grounded = true;
	public bool			jumping = false;

	void Start(){
		rigidbody.inertiaTensor = rigidbody.inertiaTensor + new Vector3 (0, 0, rigidbody.inertiaTensor.z * 100);
	}

	void FixedUpdate () { // Every Frame
		float h = Input.GetAxis ("Horizontal");
		//float v = Input.GetAxis ("Vertical");

		Vector3 vel = rigidbody.velocity;
		vel.x = h * speed;

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
		}
		if ((Input.GetKeyDown (KeyCode.DownArrow) ||
				Input.GetKeyDown (KeyCode.S)) && Pipe.canUseWarpPipe) {
				//teleport mario
			Vector3 temp = new Vector3(-55.5f,-9.0f,0);
			transform.position += temp;

		}
		rigidbody.velocity = vel;
			
	}

	void OnCollisionEnter(Collision other) {
		//Debug.Log ("gameobject enter " + other.gameObject.tag);
		grounded = true;
		jumping = false;
	}
	void OnCollisionExit(Collision other) {
		grounded = false;
		//Debug.Log ("gameobject exit " + other.gameObject.tag);
	}
	void OnCollisionStay(Collision other){
		grounded = true;
	}
}


















