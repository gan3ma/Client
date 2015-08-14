using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NamePleat : MonoBehaviour {

	public int type;
	private play_Wrapper wp;
	private Play_GameController gc;
	private Text t;

	// Use this for initialization
	void Start () {
		wp = GameObject.Find ("Play_GameController").GetComponent<play_Wrapper> ();
		gc = GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ();
		t = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(gc.role>1)
			t.text = wp.names [1-type];
		else
			t.text = wp.names [type];
	}
}
