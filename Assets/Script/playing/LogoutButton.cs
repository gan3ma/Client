﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System.Collections.Generic;
using MiniJSON;

public class LogoutButton : MonoBehaviour {

	private TitleController tc;
	public bool p;

	// Use this for initialization
	void Start () {
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoLogout(){
		if (p) {
			Debug.Log ("ログアウト");
			Dictionary<string, string> post = new Dictionary<string, string>{ {"play_id" , tc.play_id.ToString()} , {"user_id" , tc.user_id.ToString()} };
			WWWForm form = new WWWForm ();
			foreach (KeyValuePair<string,string> post_arg in post) {
				form.AddField (post_arg.Key, post_arg.Value);
			}
			string url = "http://192.168.3.83:3000/users/logout";
			WWW www = new WWW (url, form);
			StartCoroutine (WaitForRequest (www));
			p = false;
		} else {
			Debug.Log("戻る");
		}

		StartCoroutine (fader());
	}

	private IEnumerator fader(){
		GameObject.Find("FadeController").GetComponent<Fade>().Fade_p = false;
		yield return new WaitForSeconds (2f);
	//	GameObject.Find ("TitleController").GetComponent<TitleController> ().p = true;
		Application.LoadLevel ("title");
	}

	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}
}
