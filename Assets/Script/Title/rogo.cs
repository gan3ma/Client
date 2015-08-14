using UnityEngine;
using System.Collections;

public class rogo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (sss ());
	}
	
	// Update is called once per frame
	void Update () {

	}

	private IEnumerator sss(){
		yield return new WaitForSeconds(0.5f);
		GameObject.Find("FadeController").GetComponent<Fade>().Fade_p = true;
		yield return new WaitForSeconds(3f);
		GameObject.Find("FadeController").GetComponent<Fade>().Fade_p = false;
		yield return new WaitForSeconds(2f);
		Application.LoadLevel ("first_title");
	}
}
