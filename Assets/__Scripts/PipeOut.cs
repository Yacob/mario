using UnityEngine;
using System.Collections;

public class PipeOut : MonoBehaviour {

	public static bool canUseWarpPipeOut = false;
	
	// Update is called once per frame
	void OnTriggerEnter(){
		Debug.Log("pipe on");
		canUseWarpPipeOut = true;
	}
	void OnTriggerExit(){
		canUseWarpPipeOut = false;
	}
}
