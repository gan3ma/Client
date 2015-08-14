using UnityEngine;
using System.Collections;

public class C_back : MonoBehaviour {

	private C_Boss boss;
	private float size = 29f;
	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("C_backs").GetComponent<C_Boss> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localPosition += new Vector3(boss.speed, 0, 0);
		if (this.transform.localPosition.x > size)
			this.transform.localPosition = new Vector3 (-size, 0, 10);
		if (this.transform.localPosition.x < -size)
			this.transform.localPosition = new Vector3 (size, 0, 10);
	}
}
