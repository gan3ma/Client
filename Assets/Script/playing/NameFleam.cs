using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameFleam : MonoBehaviour {

	private play_Wrapper wp;
	private TitleController tc;
	private Play_GameController gc;
	private SpriteRenderer sp;

	public bool p;
	public int turn_p;
	
	// Use this for initialization
	void Start () {
		wp = GameObject.Find ("Play_GameController").GetComponent<play_Wrapper> ();
		gc = GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ();
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		sp = this.GetComponent<SpriteRenderer> ();
		turn_p = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (gc.fs&&gc.role < 0) {
			if ((wp.turn_p != gc.player && p) || (wp.turn_p == gc.player && !p)) {
				sp.enabled = false;
			} else {
				sp.enabled = true;
			}
			turn_p = wp.turn_p;
		}else if (turn_p != wp.turn_p) {
			if ((wp.turn_p!=tc.user_id&&p)||(wp.turn_p==tc.user_id&&!p)) {
				sp.enabled = false;
			} else {
				sp.enabled = true;
			}
			turn_p = wp.turn_p;
		}
	}
}
