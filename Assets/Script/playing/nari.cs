using UnityEngine;
using System.Collections;

public class nari : MonoBehaviour {

	public GameObject boss;
	public bool Pro;
	private nari_back n;

	// Use this for initialization
	void Start () {
		n = boss.GetComponent<nari_back> ();
	}

	void OnMouseDown(){
		if (Pro) {
			n.p = 1;
		} else {
			n.p = -1;
		}
	}
}
