using UnityEngine;
using System.Collections;

public class TurnChecker : MonoBehaviour {

	private play_Wrapper wp;
	private TitleController tc;
	
	public int turn_p;

	// Use this for initialization
	void Start () {
		wp = GameObject.Find ("Play_GameController").GetComponent<play_Wrapper> ();
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		turn_p = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (turn_p != wp.turn_p) {
			if (wp.turn_p != tc.user_id) {
				this.transform.position = new Vector3(0,0,-5);
			} else {
				this.transform.position = new Vector3(0,0,1);
			}
			turn_p = wp.turn_p;
		}
	}
}
