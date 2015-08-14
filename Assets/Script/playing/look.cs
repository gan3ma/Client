using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class look : MonoBehaviour {

	private Play_GameController gc;
	private float t;

	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ();
		t = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (gc.fs) {
			if(t>0.5f){
				if (gc.role < 0) {
					var me = this.gameObject.GetComponent<SpriteRenderer>();
					me.enabled = true;
				}else
					Destroy(this.gameObject);
			}else{
				t += Time.deltaTime;
			}
		}
	}
}
