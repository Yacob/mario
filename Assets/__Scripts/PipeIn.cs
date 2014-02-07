using UnityEngine;
using System.Collections;

public class PipeIn : MonoBehaviour {

	public static bool canUseWarpPipeIn = false;

	// Update is called once per frame
	void OnTriggerEnter(){
		canUseWarpPipeIn = true;
	}
	void OnTriggerExit(){
		canUseWarpPipeIn = false;
	}
}
