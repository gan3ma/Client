using UnityEngine;
using System.Collections;

public class nari_back : MonoBehaviour {

	public int p;
	public bool click;
	public float t;

	// Use this for initialization
	void Start () {
		p = 0;
		t = 0f;
		click = false;
	}

	void Update(){
		if (t > 0.1f) {
			if (Input.GetMouseButtonDown (0))
				click = true;
		} else
			t += Time.deltaTime;

		if (click) {
			if(t>0.2f)
				Destroy(this.gameObject);
			else
				t += Time.deltaTime;
		}
	}
}
