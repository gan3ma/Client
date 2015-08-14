using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomBox : MonoBehaviour {

	private TitleController tc;
	private InputField i;
	private int d;
	private static int o_num;

	// Use this for initialization
	void Start () {
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		i = this.GetComponent<InputField> ();
	}

	public static void check(int num){
		if(!(num > 0  && num <= 500))
			GameObject.Find("Wroom").GetComponent<SpriteRenderer>().enabled = true;
		else
			GameObject.Find("Wroom").GetComponent<SpriteRenderer>().enabled = false;
		o_num = num;
	}

	// Update is called once per frame
	void Update () {
		if (int.TryParse(i.text,out d)) {
			if(o_num != d)
				GameObject.Find("Wroom").GetComponent<SpriteRenderer>().enabled = false;
			tc.room_no = d;
		} else {
			tc.room_no = -1;
		}
	}
}
