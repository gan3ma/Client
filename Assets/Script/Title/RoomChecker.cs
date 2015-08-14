using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MiniJSON;

public class RoomChecker : MonoBehaviour {
	
	public Text number,first,last,state,vs;
	private float t;
	public float Dcount;

	// Use this for initialization
	void Start () {

	}

	bool DobleClick(){
		return t < Dcount;
	}

	// Update is called once per frame
	void Update () {
		if (t < Dcount)
			t += Time.deltaTime;
	}

	public void Hi(){
		if (DobleClick ()) {
			Debug.Log("ダブルクリック");
			var tc = GameObject.Find("TitleController").GetComponent<TitleController>();
			tc.room_no = int.Parse(number.text);
			if(!NameBox.IsAlphanumeric(tc.user_name)&&state.text=="対戦中"){
				tc.user_name = "Watcher";
			}
			GameObject.Find("Button").GetComponent<LoginButton>().GoLogin();
		} else {
			t = 0f;
		}
	}
}
