using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	public int 			initialCount;
	public Transform	hasObject;
	public Transform	emptyBrick;
	public bool			isHidden;
	public GameObject	onTop = null;
	private int			count;
	public Transform	flower;

	private Animator 	animator;
	private bool		isEmpty;

	void Start(){
		if (isHidden) {
			foreach(Transform child in transform){
				Debug.Log("found child " + child);
				child.GetComponent<MeshRenderer>().enabled = false;
				child.GetComponent<SpriteRenderer>().enabled = false;
				//child. = false;
				//child.renderer.enabled = false;
			}
		}
		count = initialCount;
		animator = this.GetComponentInChildren<Animator>();
	}
	void Update(){
		AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
		if(info.nameHash == Animator.StringToHash("Base Layer.upBox")){
			animator.SetBool("doBump",false);
		}
	}
	public void marioHit(){
		//AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
		//Debug.Log ("this is A " + gameObject.tag + " at " + this.transform.position + " with a " + hasObject.GetType().ToString());
		if (onTop != null) {
			Destroy(onTop.gameObject);
		}
		if ( count > 0) {
			Vector3 spawnLoc = this.transform.position + /*new Vector3(11.69434f,1.748389f,2.006592f) +*/ new Vector3(0f,1,0f);
			if(hasObject.GetType().ToString() == "Mushroom" && (Mario.isBig || Mario.isFire)){
				Instantiate(flower, spawnLoc, Quaternion.identity);
			}
			Instantiate(hasObject, spawnLoc, Quaternion.identity);
			count--;
			if(count == 0){
				Vector3 emptyLoc = this.transform.position;
				emptyLoc.y -= .5f;
				emptyLoc.x += .5f;
				Instantiate (emptyBrick, emptyLoc, Quaternion.identity);
				isEmpty = true;
				animator.SetBool("doBump", true);
				Destroy (this.gameObject);
			}
			//Time.timeScale = 0;
		} 
		else if(Mario.isBig){
			Destroy(this.gameObject);
		}
		else{
			animator.SetBool("doBump", true);
		}
	}
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == "Enemy")
			onTop = other.gameObject;
	}
	void OnCollisionExit(Collision other){
		if(other.gameObject.tag == "Enemy")
			onTop = null;
	}

	void OnDestroy(){
		if (onTop != null) {
			Destroy(onTop.gameObject);
		}
		if (!isEmpty) {
			//do destruction animation
		}
	}
}
