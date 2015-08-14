using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NameBox : MonoBehaviour {

	private TitleController tc;
	private InputField i;
	private static string o_name;

	// Use this for initialization
	void Start () {
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		i = this.GetComponent<InputField> ();
	}

	public static bool IsAlphanumeric(string target){
		return new Regex("^[0-9a-zA-Z]+$").IsMatch(target);
	}

	public static void check(string name){
		if(!(name != "" && IsAlphanumeric (name)))
			GameObject.Find ("Wname").GetComponent<SpriteRenderer> ().enabled = true;
		else
			GameObject.Find ("Wname").GetComponent<SpriteRenderer> ().enabled = false;
		o_name = name;
	}

	// Update is called once per frame
	void Update () {
		tc.user_name = i.text;
		if(o_name != tc.user_name)
			GameObject.Find ("Wname").GetComponent<SpriteRenderer> ().enabled = false;
	}
}
