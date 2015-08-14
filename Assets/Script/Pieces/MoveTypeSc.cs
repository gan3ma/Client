using UnityEngine;
using System.Collections;

public class MoveTypeSc : MonoBehaviour {

	//局所のx・yはいずれも処理内の座標
	public GameObject move;
	public GameObject get;
	public GameObject party;
	private Play_GameController gc;
	private TitleController tc;
	public int myposx,myposy;

	void Start(){
		gc = this.GetComponent<Play_GameController> ();
		tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
	}

	//敵味方判別
	private bool Beacon(int i){
		bool p;

		if (tc.user_id != gc.actor [i, 3])
			p = true;
		else
			p = false;

		return p;
	}

	//移動用loadを設置
	private void putMove(int num, int x, int y){
		var load = Instantiate(move,gc.guide[x+y*9].transform.position-new Vector3(0,0,1),Quaternion.identity) as GameObject;
		var posbox = load.GetComponent<PosBox>();
		posbox.num = num; posbox.x = x; posbox.y = y;
	}

	//攻撃用loadを設置
	private void putGet(int num, int x, int y){
		var load = Instantiate(get,gc.guide[x+y*9].transform.position-new Vector3(0,0,1),Quaternion.identity) as GameObject;
		var posbox = load.GetComponent<PosBox>();
		posbox.num = num; posbox.x = x; posbox.y = y;
	}

	//味方用loadを設置
	private void putParty(int num, int x, int y){
		var load = Instantiate(party,gc.guide[x+y*9].transform.position-new Vector3(0,0,1),Quaternion.identity) as GameObject;
	}

	//現在のloadを全削除
	public void LoadDestroy() {
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("load");
		foreach(GameObject obs in obstacles) {
			Destroy(obs);
		}
	}

	//盤面チェック
	public void StageCheck(int num, int x, int y){
		if(0<=x&&x<9&&0<=y&&y<9){
			if(gc.stage[x,y]<0){
				putMove(num, x, y);
			}else if(Beacon(gc.stage[x,y])){
				putGet(num, x, y);
			}else{
				putParty(num, x, y);
			}
		}
	}

	//直線チェック
	public void StraightCheck(int num, int x, int y,int type){
		//typeの値が方向　上:1　下:2　左:3　右:4　それ以外なら何もしない
		if (type == 1)
			y--;
		else if (type == 2)
			y++;
		else if (type == 3)
			x++;
		else if (type == 4)
			x--;
		else
			return;

		//盤面チェックが特殊なので関数は使わない
		if(0<=x&&x<9&&0<=y&&y<9){
			if(gc.stage[x,y]<0){
				putMove(num, x, y);
				StraightCheck(num,x,y,type);
			}else if(Beacon(gc.stage[x,y])){
				putGet(num, x, y);
			}else{
				putParty(num, x, y);
			}
		}
	}

	//斜めチェック
	public void DiagonallyCheck(int num, int x, int y,int type){
		//typeの値が方向　右上:1　右下:2　左下:3　左上:4　それ以外なら何もしない
		if (type == 1) {
			x--;
			y--;
		} else if (type == 2) {
			x--;
			y++;
		} else if (type == 3) {
			x++;
			y++;
		} else if (type == 4) {
			x++;
			y--;
		}else
			return;

		//盤面チェックが特殊なので関数は使わない
		if(0<=x&&x<9&&0<=y&&y<9){
			if(gc.stage[x,y]<0){
				putMove(num, x, y);
				DiagonallyCheck(num,x,y,type);
			}else if(Beacon(gc.stage[x,y])){
				putGet(num, x, y);
			}else{
				putParty(num, x, y);
			}
		}
	}

//***************************ここから駒の動き*********************************

	//王の動き
	public void king(int num,int posx, int posy){
		for(int y=posy-2; y<=posy; y++){
			for (int x=posx-2; x<=posx; x++) {
				if(y!=posy-1||x!=posx-1) StageCheck(num,x,y);
			}
		}
	}

	//飛車の動き
	public void Flyer(int num, int x, int y){
		x--; y--;
		StraightCheck (num,x,y,1);
		StraightCheck (num,x,y,2);
		StraightCheck (num,x,y,3);
		StraightCheck (num,x,y,4);
	}

	//龍王の動き(成飛車)
	public void P_Flyer(int num, int x, int y){
		x--; y--;
		StageCheck(num,x-1,y-1);
		StageCheck(num,x-1,y+1);
		StageCheck(num,x+1,y+1);
		StageCheck(num,x+1,y-1);
		StraightCheck (num,x,y,1);
		StraightCheck (num,x,y,2);
		StraightCheck (num,x,y,3);
		StraightCheck (num,x,y,4);
	}

	//角の動き
	public void Assault(int num, int x, int y){
		x--; y--;
		DiagonallyCheck (num,x,y,1);
		DiagonallyCheck (num,x,y,2);
		DiagonallyCheck (num,x,y,3);
		DiagonallyCheck (num,x,y,4);
	}

	//馬の動き(成角)
	public void P_Assault(int num, int x, int y){
		x--; y--;
		StageCheck(num,x+1,y);
		StageCheck(num,x-1,y);
		StageCheck(num,x,y+1);
		StageCheck(num,x,y-1);
		DiagonallyCheck (num,x,y,1);
		DiagonallyCheck (num,x,y,2);
		DiagonallyCheck (num,x,y,3);
		DiagonallyCheck (num,x,y,4);
	}

	//金の動き
	public void Gold(int num, int posx, int posy){
		for(int y=posy-2; y<=posy; y++){
			for (int x=posx-2; x<=posx; x++) {
				if(!(y==posy&&x!=posx-1))
					if(y!=posy-1||x!=posx-1) StageCheck(num,x,y);
			}
		}
	}

	//銀の動き
	public void Silver(int num, int posx, int posy){
		for(int y=posy-2; y<=posy; y++){
			for (int x=posx-2; x<=posx; x++) {
				if(y==posy-2||(y==posy&&x!=posx-1))
					StageCheck(num,x,y);
			}
		}
	}

	//桂馬の動き
	public void Knight(int num, int posx, int posy){
		int[] t = {1,-1};

		for (int i=0; i<2; i++) {
			int x = posx - 1 + t[i];
			int y = posy - 3;
			StageCheck(num,x,y);
		}
	}

	//香車の動き
	public void Tank(int num, int x, int y){
		x--; y--;
		StraightCheck (num,x,y,1);
	}

	//歩の動き
	public void Soldier(int num, int posx, int posy){
		int x = posx - 1;
		int y = posy - 2;
		StageCheck(num,x,y);
	}

	//持ち駒を置く
	public void PutAthers(int num){
		LoadDestroy ();
		myposx = gc.actor [num, 1];
		myposy = gc.actor [num, 2];

		if (0 <= num && num < 18) //歩
			for (int x=0; x<9; x++) {
				if (!gc.fu [x])
					for (int y=1; y<9; y++)
						if (gc.stage [x, y] < 0)
							putMove (num, x, y);
			}
		else if (18 <= num && num <= 21) { //香車
			for (int x=0; x<9; x++) {
				for (int y=1; y<9; y++)
					if (gc.stage [x, y] < 0)
						putMove (num, x, y);
			}
		} else if (22 <= num && num <= 25) { //桂馬
			for (int x=0; x<9; x++) {
				for (int y=2; y<9; y++)
					if (gc.stage [x, y] < 0)
						putMove (num, x, y);
			}
		} else { //金・銀・飛車・角
			for (int x=0; x<9; x++)
				for (int y=0; y<9; y++)
					if (gc.stage [x, y] < 0)
						putMove (num,x,y);
		}
	}
}
