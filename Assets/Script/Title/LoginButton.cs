using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using MiniJSON;

public class LoginButton : MonoBehaviour {
	
	private TitleController tc;
	public bool p;

	void Start(){
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		p = true;
	}

	public void GoLogin(){
		if ((tc.user_name != "" && NameBox.IsAlphanumeric (tc.user_name))&&tc.room_no > 0 && tc.room_no <= 500 && p) {
			p = false;
			tc.p = false;
			Debug.Log("ログインします");
			Dictionary<string, string> post = new Dictionary<string, string>{ {"name" , tc.user_name} , {"room_no" , tc.room_no.ToString()} };
			WWWForm form = new WWWForm ();
			foreach (KeyValuePair<string,string> post_arg in post) {
				form.AddField (post_arg.Key, post_arg.Value);
			}
			string url = "http://192.168.3.83:"+tc.port.ToString()+"/users/login";
			WWW www = new WWW (url, form);
			StartCoroutine (WaitForRequest (www));
		} else if(p){
			Debug.Log("ログインできません");
			RoomBox.check(tc.room_no);
			NameBox.check(tc.user_name);
		}
	}

	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
			var json = MiniJSON.Json.Deserialize(www.text) as Dictionary<string,object>;
			
			tc.user_id = (int)(long)json ["user_id"];
			tc.play_id = (int)(long)json ["play_id"];

			GameObject.Find("FadeController").GetComponent<Fade>().Fade_p = false;
			yield return new WaitForSeconds(2f);
			Application.LoadLevel ("play");
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}
}
