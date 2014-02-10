using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	public int 			initialCount;
	public Transform	hasObject;
	public Transform	emptyBrick;
	public bool			isHidden;
	public GameObject	onTop = null;
	private int			count;

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
		animator = gameObject.GetComponent<Animator>();
	}
	void Update(){
		AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
		if(info.nameHash == Animator.StringToHash("Base Layer.upBox")){
			animator.SetBool("doBump",false);
		}
	}
	public void marioHit(){
		AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
		Debug.Log ("this is A " + gameObject.tag + " at " + this.transform.position);
		if ( count > 0) {
			Vector3 spawnLoc = this.transform.position + new Vector3(0f,.5f,0f);
			Instantiate(hasObject, spawnLoc, Quaternion.identity);
			count--;
			if(count == 0){
				Instantiate (emptyBrick, this.transform.position, Quaternion.identity);
				isEmpty = true;
				Destroy (this.gameObject);
			}
		} 
		else if(Mario.isBig){
			Destroy(this.gameObject);
		}
		if (onTop != null) {
			Destroy(onTop.gameObject);
		}
		animator.SetBool("doBump", true);


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
