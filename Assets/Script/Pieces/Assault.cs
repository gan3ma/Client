﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System.Collections.Generic;
using MiniJSON;

public class Assault : MonoBehaviour {

	private Play_GameController gc;
	private MoveTypeSc move;
	private PosBox posbox;
	public bool p;
	
	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ();
		move = GameObject.Find ("Play_GameController").GetComponent<MoveTypeSc> ();
		posbox = this.GetComponent<PosBox> ();
	}
	
	void OnMouseDown(){
		move.LoadDestroy ();
		if(p)
			move.P_Assault (posbox.num,posbox.x, posbox.y);
		else 
			move.Assault (posbox.num,posbox.x, posbox.y);
	}
}