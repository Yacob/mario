using UnityEngine;
using System.Collections;

public class PipeIn : MonoBehaviour {

	public static bool canUseWarpPipeIn = false;

	// Update is called once per frame
	void OnTriggerEnter(){
		Debug.Log("In pipe on");
		canUseWarpPipeIn = true;
	}
	void OnTriggerExit(){
		Debug.Log("In pipe off");
		canUseWarpPipeIn = false;
	}
}
