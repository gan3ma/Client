using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MiniJSON;

public class play_Wrapper : MonoBehaviour {

	public string[] names = new string[2];
	public int first_n;
	public int turn;
	public int turn_p;
	public int[] hand = new int[28];
	public int[,] putNum = new int[14,18]; //置ける駒ID (クライアント基準)
	public Text[] sugoihito = new Text[2];
	public GameObject CursorMove;
	public GameObject CM;
	public Text TurnC;
	public Sprite[] TurnGuideSprite = new Sprite[2];
	public GameObject turnguide;

	private TitleController tc;
	private Play_GameController gc;

	// Use this for initialization
	void Start () {
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		gc = this.GetComponent<Play_GameController> ();
		turn = 0;
		turn_p = -1;
		names [0] = "ﾏｯﾁﾝｸﾞ中...";
		names [1] = "ﾏｯﾁﾝｸﾞ中...";
	}

	//試合状況確認
	public void CheckPlay(){
		string url = "http://192.168.3.83:"+tc.port.ToString()+"/plays/"+tc.play_id.ToString();
		WWW www = new WWW (url);
		StartCoroutine (GetPlay(www));
	}

	//試合状況確認コルーチン
	private IEnumerator GetPlay(WWW www){
		yield return www;
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
		
		var json = MiniJSON.Json.Deserialize(www.text) as Dictionary<string,object>;
		if (!gc.fs && (string)json ["state"] == "playing") {
			CheckRole ();
		} else if ((string)json ["state"] == "finish"||(string)json ["state"] == "exit") {
			gc.exit = true;
		} else if (gc.fs&&(int)(long)json ["turn_count"] != turn) {
			FieldData ();
			turn = (int)(long)json ["turn_count"];
			turn_p = (int)(long)json["turn_player"];
			TurnC.text = turn.ToString();
		} else
			gc.p = true;
	}

	//立場確認
	public void CheckRole(){
		string url = "http://192.168.3.83:"+tc.port.ToString()+"/plays/"+tc.play_id.ToString()+"/users";
		WWW www = new WWW (url);
		StartCoroutine (GetRole(www));
	}

	//立場確認コルーチン
	private IEnumerator GetRole(WWW www){
		yield return www;
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}

		var json = MiniJSON.Json.Deserialize(www.text) as Dictionary<string,object>;

		var fp = (Dictionary<string,object>)json ["first_player"];
		names [0] = (string)fp ["name"];
		first_n = (int)(long)fp ["user_id"];
		if ((int)(long)fp ["user_id"] == tc.user_id) {
			gc.role = 0;
		}

		var lp = (Dictionary<string,object>)json ["last_player"];
		names [1] = (string)lp ["name"];
		if ((int)(long)lp ["user_id"] == tc.user_id) {
			gc.role = 10;
			var b = GameObject.Find("Board");
			b.transform.Rotate(new Vector3(0,0,180));
			b.transform.position += new Vector3(0,0.05f,0);
		}
		if(gc.role<0) gc.player = (int)(long)fp ["user_id"];
		gc.fs = true;
		gc.p = true;
	}

	//盤面配置
	public void FieldData(){
		string url = "http://192.168.3.83:"+tc.port.ToString()+"/plays/"+tc.play_id.ToString()+"/pieces";
		WWW www = new WWW (url);
		StartCoroutine (GetField(www));
	}

	//盤面配置コルーチン
	private IEnumerator GetField(WWW www){
		yield return www;
		var Mt = this.GetComponent<MoveTypeSc> ();
		gc.CursorDestroy ();
		Mt.LoadDestroy ();
		GameObject Death = null;

		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);

			var json = MiniJSON.Json.Deserialize(www.text) as Dictionary<string,object>;

			for (int i=0; i<28; i++) {
				hand [i] = 0;
				if(i<9) gc.fu[i] = false;
			}

			for (int i=0; i<40; i++) {
				if(gc.clone[39]==null) yield return new WaitForSeconds(0.05f);
				int j = i+1,x = gc.actor[i,1], y = gc.actor[i,2];
				var piece = (Dictionary<string,object>)json [j.ToString()];
				// 0:王 1:飛車 3:角 5:金 6:銀 8:桂馬 10:香車 12:歩  (+1で成る・+14で敵サイド)
				//情報更新　駒種・座標・持ち主
				gc.actor[i,0] = 0;
				if((string)piece["name"]=="hisha"||(string)piece["name"]=="hisya") gc.actor[i,0] = 1;
				if((string)piece["name"]=="kaku") gc.actor[i,0] = 3;
				if((string)piece["name"]=="kin") gc.actor[i,0] = 5;
				if((string)piece["name"]=="gin") gc.actor[i,0] = 6;
				if((string)piece["name"]=="keima") gc.actor[i,0] = 8;
				if((string)piece["name"]=="kyosha"||(string)piece["name"]=="kyosya") gc.actor[i,0] = 10;
				if((string)piece["name"]=="fu") gc.actor[i,0] = 12;
				if((long)piece["owner"]!=tc.user_id&&(long)piece["owner"]!=gc.player) gc.actor[i,0] += 14;
				if((bool)piece["promote"]) gc.actor[i,0]++;
				if(gc.role>0){
					gc.actor[i,1] = 10-(int)(long)piece["posx"]; gc.actor[i,2] = 10-(int)(long)piece["posy"];
				}else{
					gc.actor[i,1] = (int)(long)piece["posx"]; gc.actor[i,2] = (int)(long)piece["posy"];
				}
				gc.actor[i,3] = (int)(long)piece["owner"];
				if(gc.actor[i,1]>0&&gc.actor[i,2]>0&&gc.actor[i,1]<10&&gc.actor[i,2]<10){
					gc.survival[i] = true;

					if(0<x&&x<10&&0<y&&y<10&&(gc.actor[i,1]!=x||gc.actor[i,2]!=y)){
						Vector3 after = gc.guide[(gc.actor[i,1]-1)+(gc.actor[i,2]-1)*9].transform.position;
						Vector3 before = gc.guide[(x-1)+(y-1)*9].transform.position;
						int times = 30;
						float mx = after.x - before.x;
						float my = after.y - before.y;
						Debug.Log (mx + "  " + my);
						mx /= times; my /= times;
						Debug.Log (mx + "  " + my);
						gc.clone[i].transform.position -= new Vector3(0, 0, 5);
						for (int jj=0; jj<times; jj++) {
							yield return new WaitForSeconds(0.01f);
							gc.clone[i].transform.position += new Vector3(mx, my, 0);
						}
						if(CM != null) Destroy(CM);
						CM = Instantiate(CursorMove,gc.guide[(gc.actor[i,1]-1)+(gc.actor[i,2]-1)*9].transform.position - new Vector3(0, 0, 5),Quaternion.identity) as GameObject;
					}

					if(gc.actor[i,0]==12)
						gc.fu[gc.actor[i,1]-1] = true;
					if(gc.stage[gc.actor[i,1]-1,gc.actor[i,2]-1]!=i){
						if(gc.stage[gc.actor[i,1]-1,gc.actor[i,2]-1]>=0)
							if(gc.clone[gc.stage[gc.actor[i,1]-1,gc.actor[i,2]-1]]!=null){
								Destroy(gc.clone[gc.stage[gc.actor[i,1]-1,gc.actor[i,2]-1]]);
								gc.clone[gc.stage[gc.actor[i,1]-1,gc.actor[i,2]-1]] = null;
								Debug.Log(gc.stage[gc.actor[i,1]-1,gc.actor[i,2]-1]+"はすでに死んでいたのだよ");
							}
						if(gc.clone[i]!=null)
							Destroy (gc.clone[i]);
						gc.clone [i] = Instantiate (gc.pieces [gc.actor [i, 0]], gc.guide [gc.actor [i, 1] - 1 + (gc.actor [i, 2] - 1) * 9].transform.position, Quaternion.identity) as GameObject;
						gc.stage [gc.actor [i, 1] - 1, gc.actor [i, 2] - 1] = i;
						Debug.Log("自己主張して "+i);
						PosBox posbox = gc.clone [i].GetComponent<PosBox> ();
						posbox.x = gc.actor [i, 1];
						posbox.y = gc.actor [i, 2];
						posbox.num = i;
					}
				}else{
					if(gc.actor[i,0]<14) putNum[gc.actor[i,0],hand[gc.actor[i,0]]] = i;
					hand[gc.actor[i,0]]++;
					gc.survival [i] = false;
					Death = gc.clone[i];
				}
				if(gc.survival[i]&&(x!=gc.actor[i,1]||y!=gc.actor[i,2])&&0<x&&x<10&&0<y&&y<10){
					Debug.Log("銀色を探せ");
					gc.stage[x-1,y-1] = -1;
					Debug.Log(i+"はここ(x:"+x+" y:"+y+") から消えました");
				}
			}
			Destroy(Death);
			for (int i=0; i<2; i++) {
				string s = "";
				if(hand[12+14*i]<10) s = " "; //持ち歩
				gc.hand[0+7*i].text = s + hand[12+14*i].ToString();

				if(hand[5+14*i]<10) s = " "; //持ち金
				gc.hand[1+7*i].text = s + hand[5+14*i].ToString();

				if(hand[8+14*i]<10) s = " "; //持ち桂馬
				gc.hand[2+7*i].text = s + hand[8+14*i].ToString();

				if(hand[1+14*i]<10) s = " "; //持ち飛車
				gc.hand[3+7*i].text = s + hand[1+14*i].ToString();

				if(hand[6+14*i]<10) s = " "; //持ち銀
				gc.hand[4+7*i].text = s + hand[6+14*i].ToString();

				if(hand[10+14*i]<10) s = " "; //持ち香車
				gc.hand[5+7*i].text = s + hand[10+14*i].ToString();

				if(hand[3+14*i]<10) s = " "; //持ち角
				gc.hand[6+7*i].text = s + hand[3+14*i].ToString();
			}
			if(gc.role>=0){
				var TG = Instantiate(turnguide) as GameObject;
				var TGSp = TG.GetComponent<SpriteRenderer>();
				var TGSc = TG.GetComponent<TurnGuide>();
				if(turn_p==tc.user_id){
					TGSp.sprite = TurnGuideSprite[0];
				}else{
					TGSp.sprite = TurnGuideSprite[1];
				}
				TGSc.p = true;
			}
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
		gc.p = true;
	}

	public void end(){
		string url = "http://192.168.3.83:"+tc.port.ToString()+"/plays/"+tc.play_id.ToString()+"/winner";
		WWW www = new WWW (url);
		StartCoroutine (WinnerCheck(www));
	}

	private IEnumerator WinnerCheck(WWW www) {
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}

		var json = MiniJSON.Json.Deserialize(www.text) as Dictionary<string,object>;
		if ((int)(long)json ["winner"] == first_n) {
			sugoi (0);
		} else {
			sugoi (1);
		}
	}

	void sugoi(int win){
		sugoihito [0].text = names [win];
		sugoihito [0].enabled = true;
		sugoihito [1].enabled = true;
	}

	public WWW GET(string url) {
		WWW www = new WWW (url);
		StartCoroutine (WaitForRequest (www));
		return www;
	}
	
	public WWW POST(string url, Dictionary<string,string> post) {
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<string,string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
		return www;
	}
	
	private IEnumerator WaitForRequest(WWW www) {
		yield return www;
		// check for errors
		if (www.error == null) {
			Debug.Log("WWW Ok!: " + www.text);
		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
		gc.p = true;
	}
}
