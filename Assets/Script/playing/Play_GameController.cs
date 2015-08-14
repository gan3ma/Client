using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MiniJSON;

public class Play_GameController : MonoBehaviour {

	public GameObject[] pieces = new GameObject[28]; //駒のプレハブ 0:王 1:飛車 3:角 5:金 6:銀 8:桂馬 10:香車 12:歩  (+1で成る・+14で敵サイド)
	public Text[] hand = new Text[14];
	public GameObject[] guide = new GameObject[81]; //ガイド (x - 1) + (y - 1) * 9  x,y:盤面基準
	public bool[] fu = new bool[9]; //歩のいる列 
	public GameObject[] clone = new GameObject[40]; //インスタンス化した駒 IDで管理
	public bool[] survival = new bool[40]; //引数のIDの駒が盤上にある
	public int[,] actor = new int[40,4]; //駒の状態　第2引数　0:使用するテクスチャ 1:盤上でのx座標　2:盤上でのy座標 4:オーナー
	public int[,] stage = new int[9, 9]; //内部処理上での座標　[x,y]
	public bool p,fs,exit; //p:現在通信中でない　fs:試合が始まっている  exit:試合が終わっている
	public int role; //自分の立場　-1:観戦者　0:先手　10:後手
	public int player; //観戦用視点
	public GameObject back;
	public GameObject by;
	public GameObject nariButton;
	public GameObject Cursor;
	//
	private TitleController tc;
	private play_Wrapper W;
	private string url;
	private MoveTypeSc Mv;
	private float t; //タイマー
	private Fade fader;

	// Use this for initialization
	void Start () {
		W = this.GetComponent<play_Wrapper> ();
		Mv = this.GetComponent<MoveTypeSc> ();
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();

		//初期化処理
		p = true;

		for (int i=0; i<40; i++) {
			clone[i] = null;
			survival [i] = true;
			actor[i,2] = 0;
		}

		for (int i=0; i<9; i++)
			for (int j=0; j<9; j++)
				stage [i, j] = -1;

		fs = false;
		role = -1; //立場の初期値は観戦者
		t = 0f;
		player = -1;
		exit = false;
		fader = GameObject.Find ("FadeController").GetComponent<Fade> ();
		fader.Fade_p = true;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t > 0.1f) { //0.1秒ごとに処理
			if (fs) { //試合が始まっているとき
				if(p){
					p = false;
					W.CheckPlay();
				}
			} else { //試合が始まっていないとき
				if(p){
					p = false;
					W.CheckPlay(); //試合が始まったか確認
				}
			}
			t = 0f;
		}
		if (fs && exit) {
			fs = false;
			finish ();
		}
	}

	//現在のCursorを全削除
	public void CursorDestroy() {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Cursor");
		foreach(GameObject obs in obstacles) {
			Destroy(obs);
		}
	}

	void finish(){
		W.FieldData ();
		if (W.CM != null) Destroy (W.CM);
		var TC = GameObject.Find ("TurnChecker");
		Mv.LoadDestroy ();
		TC.transform.position = new Vector3(0,0,-5);
		var g = back.GetComponent<SpriteRenderer> ();
		g.enabled = true;
		CursorDestroy ();
		Destroy (by);

		W.end ();
	}

	public void GetIt(int i, int x, int y, bool p){
		int enemy;

		enemy = stage [x, y]+1;
		stage [x, y] = -1;
		Debug.Log ("敵は"+enemy);

		//stage[actor[i,1]-1,actor[i,2]-1] = -1;

		i++;
		if (role > 0) {
			x = 9 - x;
			y = 9 - y;
		} else {
			x++; y++;
		}
		var Mes = new Dictionary<string, string>{ {"play_id" , tc.play_id.ToString()} , {"user_id" , tc.user_id.ToString()} , {"move_id" , i.ToString()} , {"posx" , x.ToString()} , {"posy" , y.ToString()} , {"promote" , p.ToString()} , {"get_id" , enemy.ToString()}};
		url = "http://192.168.3.83:"+tc.port.ToString()+"/plays/update";
		p = false;
		W.POST (url, Mes);
		
		Mv.LoadDestroy();
	}

	public void MoveIt(int i, int x, int y, bool p){
		//stage[actor[i,1]-1,actor[i,2]-1] = -1;
/*		actor [i, 1] = x + 1;
		actor [i, 2] = y + 1;
		Destroy (clone [i]);
		clone [i] = null;

/*		clone[i] = Instantiate(pieces[actor[i,0]],guide[actor[i,1]-1+(actor[i,2]-1)*9].transform.position,Quaternion.identity) as GameObject;
		stage[actor[i,1]-1,actor[i,2]-1] = i;
		PosBox posbox = clone[i].GetComponent<PosBox>();
		posbox.x = actor[i,1]; posbox.y = actor[i,2]; posbox.num = i;
/*		
 		対戦ID(play_id),
		ユーザID(user_id)
		動かした駒{
			ID(move_id)
				X(posx)
					Y(posy)
					成(promote)
		}
		取得した駒{
			駒ID(get_id)
		}
*/
		i++;
		if (role > 0) {
			x = 9 - x;
			y = 9 - y;
		} else {
			x++; y++;
		}
		var Mes = new Dictionary<string, string>{ {"play_id" , tc.play_id.ToString()} , {"user_id" , tc.user_id.ToString()} , {"move_id" , i.ToString()} , {"posx" , x.ToString()} , {"posy" , y.ToString()} , {"promote" , p.ToString()} , {"get_id" , "-1"}};
		url = "http://192.168.3.83:"+tc.port.ToString()+"/plays/update";
		p = false;
		W.POST (url, Mes);

		Mv.LoadDestroy();
	}
}

/*
https://docs.google.com/document/d/13R6xRfpT4yVQOBRhWZw7n4v3BqaibvSsywdtSFepfRM/edit
 */
