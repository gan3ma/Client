using UnityEngine;
using System.Collections;

public class AthersB : MonoBehaviour {

	public int type;
	private MoveTypeSc mt;
	private play_Wrapper pw;

	// Use this for initialization
	void Start () {
		mt = GameObject.Find ("Play_GameController").GetComponent<MoveTypeSc> ();
		pw = GameObject.Find ("Play_GameController").GetComponent<play_Wrapper> ();
	}

	void OnMouseDown(){
		Debug.Log (type+"を置きたいけどあと" + pw.hand [type] + "個");
		if (pw.hand [type] > 0)
			mt.PutAthers (pw.putNum [type, 0]);
		else
			mt.LoadDestroy ();
	}
}
