using UnityEngine;
using System.Collections;

public class C_Boss : MonoBehaviour {

	public float speed;
	public float size;
	public bool p;

	private TitleController tc;

	void Start(){
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
	}

	void Update(){
		if (p && tc.play_id > 0) {
			p = false;
			tc.p = true;
		} else if (p) {
			p = false;
		}
		this.transform.localScale = new Vector3 (size, size, 1);
	}

}
