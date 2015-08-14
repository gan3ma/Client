using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
	
	public Texture fadeTexture; //使用するテクスチャ
	public float fadeSpeed = 0.3f; //フェードする速さ
	
	private int drawDepth = -1000; //前面に表示する
	private int fadeDir = 1; //インなら-1,アウトなら1
	private float alpha = 0.0f; //透過率

	public bool Fade_p;

	
	void Start () {
		var tc = GameObject.Find ("TitleController").GetComponent<TitleController> ();
		if(!tc.p) alpha = 255f;
	}
	
	
	void Update () {
		if(!Fade_p){ //Zキー フェードアウト
			FadeOut();
		}
		if(Fade_p){ //Xキー フェードイン
			FadeIn();
		}
	}
	
	void OnGUI(){
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		GUI.color = new Color(1,1,1,alpha);
		GUI.depth = drawDepth; //GUIを前面に表示する
		GUI.DrawTexture(new Rect(0, 0,Screen.width, Screen.height),fadeTexture);//画面全体にテクスチャを表示
	}
	
	void FadeIn(){
		fadeDir = -1;
	}
	void FadeOut(){
		fadeDir = 1;
	}
}