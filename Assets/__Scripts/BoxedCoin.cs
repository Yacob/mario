using UnityEngine;
using System.Collections;

public class BoxedCoin : MonoBehaviour {
	private float timer;
	// Use this for initialization
	void Start () {
		Animator anim = GetComponentInChildren<Animator> ();
		//anim.Play (0);
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
		timer = info.length;
	}
	
	// Update is called once per frame
	void Update () {
		Animator anim = GetComponentInChildren<Animator> ();
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
		if(info.IsTag("done"))
		   Destroy(this.gameObject);
	}
	void OnDestroy(){
		Mario.coins++;
		Mario.score += 100;
	}
}
