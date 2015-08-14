using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class TitleController : MonoBehaviour {
	
	public int port;
	public string user_name;
	public int room_no;
	public int user_id;
	public int play_id;
	public Sprite play,wait;
	public bool p = true;
	public GameObject RoomBar;
	private float t;
	private GameObject c;
	private float count = 1f;
	private Fade fader;

	void Start(){
		DontDestroyOnLoad (this);
		t = count+1f;
	}

	public void RoomBarDestroy() {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("RoomBar");
		foreach(GameObject obs in obstacles) {
			Destroy(obs);
		}
	}

	void Update(){
		if (t <= count)
			t += Time.deltaTime;
		if (p && t > count) {
			string url = "http://192.168.3.83:"+port.ToString()+"/plays/0/rooms";
			WWW www = new WWW (url);
			StartCoroutine (WaitForRequest (www));
			t = 0f;
		}
	}

	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
			RoomBarDestroy();
			var json = MiniJSON.Json.Deserialize(www.text) as Dictionary<string,object>;
			float pos = 30;
			for(int i=1;i<=json.Count;i++){
				var room = (Dictionary<string,object>)json[i.ToString()];
				Debug.Log((string)room["room_no"]+" "+(string)room["room_state"]);

				//うまいことインスタンス化したら完成
				c = GameObject.Find ("Content");
				var test = Instantiate (RoomBar,new Vector3(10,pos,0),Quaternion.identity) as GameObject;
				test.transform.SetParent(c.transform,false);

				var t = test.GetComponent<RoomChecker>();
				t.number.text = (string)room["room_no"];
				t.first.text = (string)room["first_player"];
				t.last.text = (string)room["last_player"];
				if((string)room["room_state"]=="playing"){
					t.state.text = "対戦中";
					test.GetComponent<Image>().sprite = play;
				}else{
					t.vs.text = "";
					t.state.text = "対戦者募集中";
					test.GetComponent<Image>().sprite = wait;
				}
				pos-=20;
			}
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}
}
