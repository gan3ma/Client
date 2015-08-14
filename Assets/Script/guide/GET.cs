using UnityEngine;
using System.Collections;

public class GET : MonoBehaviour {

	private Play_GameController gc;
	private PosBox pb;
	private nari_back n;

	// Use this for initialization
	void Start () {
		gc = GameObject.Find ("Play_GameController").GetComponent<Play_GameController> ();
		pb = this.GetComponent<PosBox> ();
		n = null;
	}
	
	void Update(){
		if (n != null) {
			if(n.p>0) Pro();
			else if(n.p<0) NotPro();
		}
	}

	bool ProCheck(int type){
		//駒のプレハブ 0:王 1:飛車 3:角 5:金 6:銀 8:桂馬 10:香車 12:歩  (+1で成る・+14で敵サイド)
		if (type == 1 || type == 3 || type == 6 || type == 8 || type == 10 || type == 12)
			return false;
		return true;
	}

	void NotPro(){
		gc.GetIt (pb.num, pb.x, pb.y, false);
	}
	
	void Pro(){
		gc.GetIt (pb.num, pb.x, pb.y, true);
	}
	
	void OnMouseDown(){
		Debug.Log ("取る < "+pb.num+" position (x = "+pb.x+" : y = "+pb.y+")");
		if (gc.actor [pb.num, 0]==0||gc.actor [pb.num, 0]==5) {
			NotPro ();
		}else if (gc.actor [pb.num, 1] != 0 && (pb.y < 3 || gc.actor[pb.num,2]<4)) { //持ち駒でない&& (敵陣である || 敵陣から動く)
			if((((0<=pb.num&&pb.num<18)||(19<=pb.num&&pb.num<=22))&&pb.y==0)||(23<=pb.num&&pb.num<=26&&pb.y<2)||ProCheck(gc.actor[pb.num, 0])){ //最前線の歩・香車または上2列の桂馬
				Pro (); //強制的に成る
			}else{
				var g = Instantiate(gc.nariButton, gc.guide[pb.x+pb.y*9].transform.position-new Vector3(0,0.18f,1),Quaternion.identity) as GameObject;
				n = g.GetComponent<nari_back>();
			}
		} else { //持ち駒だったり敵陣じゃなかったり
			if(ProCheck(gc.actor[pb.num, 0])) Pro(); //すでに成ってたら成る
			else NotPro(); //成らない
		}
	}
}
