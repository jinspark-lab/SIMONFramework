using UnityEngine;
using System.Collections;
using SIMONFramework;

public class NormalGameSceneManager_Compare : MonoBehaviour {

	public GameObject gettingObject;
	public Sprite barrierSprite;
	public Sprite NoneSprite;
	private bool isGameOver;
	public static int whoIsWin;
	public static bool isBoost = false;
	public static bool BoostAtResultPage = false;
	public static int BoostCount = 0;

	public Texture2D emptyTex;
	public Texture2D fullTex;
	public GUIText play_watch;

	float Lining;

	// Use this for initialization
	void Start () {
		isGameOver = false;
		NormalGameSceneManager_Compare.whoIsWin = 3;
		SceneManager.SceneMode = 3;
		GameObject barrier = GameObject.Find("BoostBarrier");
		barrier.GetComponent<SpriteRenderer> ().sprite = NoneSprite;

		// in Boosting
		if(NormalGameSceneManager_Compare.BoostAtResultPage)
		{
			Time.timeScale = 20.0f;
			NormalGameSceneManager_Compare.BoostCount++;
			Lining = 0.0f;
			AudioListener.volume = 0;
			barrier.GetComponent<SpriteRenderer> ().sprite = barrierSprite;
		}
	}
	
	void OnGUI()
	{

		if (NormalGameSceneManager_Compare.BoostAtResultPage)
		{
			Lining += 1;

			if(Lining > 500)
				Lining = 500;

			GUI.DrawTexture(new Rect(250.0f, Screen.height/2 + 200.0f, Lining + (1 * (((float)NormalGameSceneManager_Compare.BoostCount)-1)), 100.0f), fullTex);
			GUI.DrawTexture(new Rect(250.0f, Screen.height/2 + 200.0f, 1500.0f, 100.0f), emptyTex);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float RedHealth = 0;
		float BlueHealth = 0;

		if(!NormalGameSceneManager_Compare.BoostAtResultPage)
		{	
			play_watch.text = GameTimeManager_Compare.playTime.ToString ("0.00");
		}


		for (int i=1; i<=GameTimeManager_Compare.GroupB.Count; i++) 
		{
			gettingObject = GameObject.Find("Monster_A_"+i);
			RedHealth += gettingObject.GetComponent<SIMON_Controller>().NowHealth;
		}

		for (int i=1; i<=GameTimeManager_Compare.GroupB.Count; i++) 
		{
			gettingObject = GameObject.Find("Monster_B_"+i);
			BlueHealth += gettingObject.GetComponent<UsualAI_Controller>().NowHealth;
		}

		if ((RedHealth == 0 && !isGameOver) || (GameTimeManager_Compare.playTime > 280.0f && RedHealth < BlueHealth)) 
		{
	       

			NormalGameSceneManager_Compare.whoIsWin = 1;
			isGameOver = true;

			Time.timeScale = 1.0f;
			SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);

		


		}
		else if ((BlueHealth == 0 && !isGameOver) || (GameTimeManager_Compare.playTime > 280.0f && RedHealth >= BlueHealth)) 
		{
			NormalGameSceneManager_Compare.whoIsWin = 2;
			isGameOver = true;

			Time.timeScale = 1.0f;
			SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);

		}
	}
}
