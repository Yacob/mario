using UnityEngine;
using System.Collections;

public class Mario : MonoBehaviour {
	public float		speed = 6;
	public float		jumpSpeed = 18;
	public float		jumpAcc = 10;

	public bool			grounded = true;
	public bool			jumping = false;

	void Update () { // Every Frame
			float h = Input.GetAxis ("Horizontal");
			float v = Input.GetAxis ("Vertical");

			Vector2 vel = rigidbody2D.velocity;
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
		rigidbody2D.velocity = vel;
			
	}

	void OnTriggerEnter2D(Collider2D other) {
		grounded = true;
		jumping = false;
	}
	void OnTriggerExit2D(Collider2D other) {
		grounded = false;
	}
}


















