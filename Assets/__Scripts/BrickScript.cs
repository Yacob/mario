using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	public int 			initialCoins; // Point of Interest
	public bool			hasCoins;
	public bool			has1Up;
	public bool			hasStar;
	public bool			hasMushroom;
	public bool			isHidden;

	void OnTriggerEnter(){
				//check if trigger on bottom of brick otherwise do nothing
				//then check to see if anything is colliding with trigger on top of brick and if its an enemy kill it.
				//If it is a shroom pop it up
				//then check to see if (Mario.isBig == true).  If so destroy brick or cover with blue or something
	}
}
