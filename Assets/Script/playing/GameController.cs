using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class GameController : MonoBehaviour {

	private string url;
	private Wrapper W;
	public GameObject field;
	public GameObject[] clone = new GameObject[40];
	//Dictionary<string, string> p = new Dictionary<string, string>{ {"name" , "gan3ma2"} , {"room_no" , "252"} };
	//Dictionary<string, string> t = new Dictionary<string, string>{ {"play_id" , "7"} , {"user_id" , "38"}};

	// Use this for initialization
	void Start () {
		W = GameObject.Find ("Wrapper").GetComponent<Wrapper> ();
		//p ["name"] = "gan3ma";
		url = "http://192.168.3.83:3000/plays/7/pieces";
		W.GET (url);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
