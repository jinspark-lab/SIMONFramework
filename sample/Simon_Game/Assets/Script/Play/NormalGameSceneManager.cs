using UnityEngine;
using System.Collections;
using SIMONFramework;

public class NormalGameSceneManager : MonoBehaviour {

	public GameObject gettingObject;
	private bool isGameOver;
	public static int whoIsWin;
	public static bool isBoost = false;
	public static bool BoostAtResultPage = false;

	public GUIText play_watch;

	// Use this for initialization
	void Start () {
		isGameOver = false;
		NormalGameSceneManager.whoIsWin = 3;
		SceneManager.SceneMode = 1;
	}
	
	// Update is called once per frame
	void Update () {
		float RedHealth = 0;
		float BlueHealth = 0;


		play_watch.text = GameTimeManager.playTime.ToString ("0.00");


		for (int i=1; i<GameTimeManager.GroupA.Count; i++) 
		{
			gettingObject = GameObject.Find("Monster_A_"+i);
			RedHealth += gettingObject.GetComponent<Monster_Controller>().NowHealth;
		}

		for (int i=1; i<=GameTimeManager.GroupB.Count; i++) 
		{
			gettingObject = GameObject.Find("Monster_B_"+i);
			BlueHealth += gettingObject.GetComponent<Monster_Controller_B>().NowHealth;
		}

		if ((RedHealth == 0 && !isGameOver) || (GameTimeManager.playTime > 180.0f && RedHealth < BlueHealth)) 
		{
			NormalGameSceneManager.whoIsWin = 1;
			isGameOver = true;
			if(!NormalGameSceneManager.isBoost)
			{
				Time.timeScale = 1.0f;
				SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);
			}
		}
		else if ((BlueHealth == 0 && !isGameOver) || (GameTimeManager.playTime > 180.0f && RedHealth >= BlueHealth)) 
		{
			NormalGameSceneManager.whoIsWin = 2;
			isGameOver = true;
			if(!NormalGameSceneManager.isBoost)
			{
				Time.timeScale = 1.0f;
				SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);
			}
		}
	}
}
