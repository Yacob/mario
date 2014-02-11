using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	public int 		moveDir;
	public float	bounceSpeed;
	public float	moveSpd;
	public int		vertSpeed;
	public int		upDown;
	public float	vertCeil;
	private float	vertCap = 100;
	// Use this for initialization
	void Start () {
		AnimatorStateInfo state =  Mario.marioAnim.GetCurrentAnimatorStateInfo(0);
		string name = state.ToString();
		if (name.Contains("Right"))
			moveDir = 1;
		else
			moveDir = -1;
		upDown = -1;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curPos = this.transform.position;
		Vector3 newPos = this.transform.position;
		newPos.x = moveSpd * moveDir * Time.deltaTime;
		newPos.y = vertSpeed * upDown * Time.deltaTime;

		if (newPos.y > vertCap)
			newPos.y = vertCap;

		transform.position = newPos;
	
	}
	void OnCollisionEnter(Collision other){
		if(other.collider.tag == "Enemy"){
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
			this.vertCap = this.transform.position.y + .5f;
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

	}
	void OnDestroy(){
		Mario.numFire--;
	}
}
