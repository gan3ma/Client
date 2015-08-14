using UnityEngine;
using System.Collections;

public class TurnGuide : MonoBehaviour {

	public bool p;

	// Update is called once per frame
	void Update () {
		if (p && !GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ().exit) {
			p = false;
			StartCoroutine (StartGuide ());
		} else if(p){
			Destroy(this.gameObject);
		}
	}

	private IEnumerator StartGuide(){
		int times = 30;
		float Wait = 0.001f;
		float scales = 1f / times;
		for (int i=0; i<times; i++) {
			this.transform.localScale += new Vector3 (0, scales, 0);
			yield return new WaitForSeconds(Wait);
		}
		yield return new WaitForSeconds(1f);
		for (int i=0; i<times; i++) {
			this.transform.localScale -= new Vector3 (0, scales, 0);
			yield return new WaitForSeconds(Wait);
		}
		Destroy (this.gameObject);
	}
}
