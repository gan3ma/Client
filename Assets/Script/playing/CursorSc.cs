using UnityEngine;
using System.Collections;

public class CursorSc : MonoBehaviour {

	private Play_GameController gc;

	// Use this for initialization
	void Start () {
		gc = GameObject.Find("Play_GameController").GetComponent<Play_GameController> ();
	}

	void OnMouseDown(){
		Debug.Log ("カーソル");
		gc.CursorDestroy ();
		if (gc.Cursor != null)
			Instantiate (gc.Cursor, this.transform.position - new Vector3 (0, 0, 1), Quaternion.identity);
		else
			Debug.Log ("ないです");
	}
}
