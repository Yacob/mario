using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour {

	public static bool canUseWarpPipe = false;
	
	// Update is called once per frame
	void OnTriggerEnter(){
		canUseWarpPipe = true;
	}
	void OnTriggerExit(){
		canUseWarpPipe = false;
	}
}
