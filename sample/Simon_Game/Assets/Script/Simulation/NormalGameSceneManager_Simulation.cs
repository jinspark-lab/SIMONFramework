using UnityEngine;
using System.Collections;
using SIMONFramework;

public class NormalGameSceneManager_Simulation : MonoBehaviour {

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
	public static int Total_Object = 20;

	// Use this for initialization
	void Start () {
		isGameOver = false;
		NormalGameSceneManager_Simulation.whoIsWin = 3;
		SceneManager.SceneMode = 2;
		GameObject barrier = GameObject.Find("BoostBarrier");
		barrier.GetComponent<SpriteRenderer> ().sprite = NoneSprite;
		Total_Object = 20;
		// in Boosting
		if(NormalGameSceneManager_Simulation.BoostAtResultPage)
		{
			Time.timeScale = 20.0f;
			NormalGameSceneManager_Simulation.BoostCount++;
			AudioListener.volume = 0;
			barrier.GetComponent<SpriteRenderer> ().sprite = barrierSprite;
			Debug.Log("Boosting Now..-> " +NormalGameSceneManager_Simulation.BoostCount);
		}
	}
	
	void OnGUI()
	{

		if (NormalGameSceneManager_Simulation.BoostAtResultPage)
		{

			GUI.DrawTexture(new Rect(250.0f, Screen.height/2 + 200.0f, ((float)NormalGameSceneManager_Simulation.BoostCount) * 10.0f, 70.0f), fullTex);
			GUI.DrawTexture(new Rect(250.0f, Screen.height/2 + 200.0f, 1000.0f, 70.0f), emptyTex);
		}
	}
	
	// Update is called once per frame
	void Update () {
		float RedHealth = 0;
		float BlueHealth = 0;
		if(!NormalGameSceneManager_Simulation.BoostAtResultPage)
		{	
			play_watch.text = GameTimeManager_Simulation.playTime.ToString ("0.00");
		}

		for (int i=1; i<=GameTimeManager_Simulation.GroupA.Count; i++) 
		{
			gettingObject = GameObject.Find("Monster_A_"+i);
			RedHealth += gettingObject.GetComponent<Monster_Controller_Simulation>().NowHealth;
		}

		for (int i=1; i<=GameTimeManager_Simulation.GroupB.Count; i++) 
		{
			gettingObject = GameObject.Find("Monster_B_"+i);
			BlueHealth += gettingObject.GetComponent<Monster_Controller_B_Simulation>().NowHealth;
		}

		if ((RedHealth == 0 && !isGameOver) || (GameTimeManager_Simulation.playTime > 180.0f && RedHealth < BlueHealth)) 
		{
			NormalGameSceneManager_Simulation.whoIsWin = 1;
			isGameOver = true;


			/// 나중에 삭제!!!! bggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg
			if(!NormalGameSceneManager_Simulation.isBoost && !NormalGameSceneManager_Simulation.BoostAtResultPage)
			{
				Time.timeScale = 1.0f;
				SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);
			}
			else if(NormalGameSceneManager_Simulation.isBoost && !NormalGameSceneManager_Simulation.BoostAtResultPage)
			{
				Application.LoadLevel(7);
			}


			if(NormalGameSceneManager_Simulation.BoostCount != 100 && NormalGameSceneManager_Simulation.BoostAtResultPage)
			{
				if(NormalGameSceneManager_Simulation.BoostCount%10 ==0 )
				{
					string nowDay = System.DateTime.Now.ToString ("MMMM_HH_mm_dd");
					if(SceneManager.SceneMode == 2)
					{
						for(int i=0; i<GameTimeManager_Simulation.GroupA.Count; i++)
							SIMON.GlobalSIMON.PublishDefinition("\\" + nowDay + "\\" + ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i)).ObjectID
							                                    , (SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex(i));
						for(int i=0; i<GameTimeManager_Simulation.GroupB.Count; i++)
							SIMON.GlobalSIMON.PublishDefinition("\\" + nowDay + "\\" + ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (i)).ObjectID
							                                    , (SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex(i));
					}
				}
				SIMON.GlobalSIMON.SIMONLearn(GameTimeManager_Simulation.GroupC);
				Application.LoadLevel(7);
			}
			else if(NormalGameSceneManager_Simulation.BoostCount == 100)
			{
				GameObject barrier = GameObject.Find("BoostBarrier");
				barrier.GetComponent<SpriteRenderer> ().sprite = NoneSprite;
				Time.timeScale = 1.0f;
				AudioListener.volume = 1;
				NormalGameSceneManager_Simulation.BoostCount = 0;
				NormalGameSceneManager_Simulation.BoostAtResultPage = false;
				Application.LoadLevel(7);
			}

		}
		else if ((BlueHealth == 0 && !isGameOver) || (GameTimeManager_Simulation.playTime > 180.0f && RedHealth >= BlueHealth)) 
		{
			string nowDay = System.DateTime.Now.ToString ("MMMM_HH_mm_dd");

			NormalGameSceneManager_Simulation.whoIsWin = 2;

			isGameOver = true;


			if(!NormalGameSceneManager_Simulation.isBoost  && !NormalGameSceneManager_Simulation.BoostAtResultPage)
			{
				Time.timeScale = 1.0f;
				SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);
			}
			else if(NormalGameSceneManager_Simulation.isBoost && !NormalGameSceneManager_Simulation.BoostAtResultPage)
			{
				Application.LoadLevel(7);
			}


			if(NormalGameSceneManager_Simulation.BoostCount != 100 && NormalGameSceneManager_Simulation.BoostAtResultPage)
			{
				if(NormalGameSceneManager_Simulation.BoostCount % 10 ==0 )
				{
					string nowDay1 = System.DateTime.Now.ToString ("MMMM_HH_mm_dd");
					if(SceneManager.SceneMode == 2)
					{
						for(int i=0; i<GameTimeManager_Simulation.GroupA.Count; i++)
							SIMON.GlobalSIMON.PublishDefinition("\\" + nowDay1 + "\\" + ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i)).ObjectID
							                                    , (SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex(i));
						for(int i=0; i<GameTimeManager_Simulation.GroupB.Count; i++)
							SIMON.GlobalSIMON.PublishDefinition("\\" + nowDay1 + "\\" + ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (i)).ObjectID
							                                    , (SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex(i));
					}
				}
				SIMON.GlobalSIMON.SIMONLearn(GameTimeManager_Simulation.GroupC);
				Application.LoadLevel(7);
			}
			else if(NormalGameSceneManager_Simulation.BoostCount == 100)
			{
				GameObject barrier = GameObject.Find("BoostBarrier");
				barrier.GetComponent<SpriteRenderer> ().sprite = NoneSprite;
				Time.timeScale = 1.0f;
				AudioListener.volume = 1;
				NormalGameSceneManager_Simulation.BoostCount = 0;
				NormalGameSceneManager_Simulation.BoostAtResultPage = false;
				Application.LoadLevel(7);
			}
		}
	}
}
