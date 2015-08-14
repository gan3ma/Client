﻿using UnityEngine;
using System.Collections;

public class soldier : MonoBehaviour {

	private Play_GameController gc;
	private MoveTypeSc move;
	private PosBox posbox;
	
	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ();
		move = GameObject.Find ("Play_GameController").GetComponent<MoveTypeSc> ();
		posbox = this.GetComponent<PosBox> ();
	}
	
	void OnMouseDown(){
		move.LoadDestroy ();
		move.Soldier (posbox.num,posbox.x, posbox.y);
	}
}