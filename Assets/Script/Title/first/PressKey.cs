using UnityEngine;
using System.Collections;

public class PressKey : MonoBehaviour {

	private bool p;
	public float speed;
	public GameObject Press;

	// Use this for initialization
	void Start () {
		p = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (p&&Input.GetKey (KeyCode.Space)) {
			p = false;
			Destroy(Press);
			StartCoroutine (GoTitle());
		}
	}

	private IEnumerator GoTitle(){
		while (this.transform.position.y < 4.1f) {
			this.transform.position += new Vector3 (0, speed, 0);
			yield return new WaitForSeconds(0.001f);
		}
		this.transform.position = new Vector3 (-1, 4.1f, -2);
		yield return new WaitForSeconds(0.3f);
		GameObject.Find ("TitleController").GetComponent<TitleController> ().p = true;
		Application.LoadLevel ("title");
	}
}
