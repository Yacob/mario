using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	public int 			initialCoins; // Point of Interest
	public bool			hasCoins;
	public bool			has1Up;
	public bool			hasStar;
	public bool			hasMushroom;
	public bool			isHidden;

	private Animator animator;

	void Start(){
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
		Debug.Log (gameObject.tag);
		Debug.Log ("hi");
		if (hasCoins) {
		} else if (has1Up) {
		} else if (hasStar) {
		} else if (hasMushroom) {
		}
		Debug.Log (animator);
		animator.SetBool("doBump", true);

	}
}
