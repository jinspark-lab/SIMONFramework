using UnityEngine;
using System.Collections;
using SIMONFramework;
public class GameTimeManager_Compare : MonoBehaviour {

	public static SIMONCollection GroupA = SIMON.GlobalSIMON.CreateSIMONCollection();
	public static SIMONCollection GroupB = SIMON.GlobalSIMON.CreateSIMONCollection();

	public static float playTime;
	public static float prevTime;
	public static bool isPause = false;
	public GameObject blackScreen;

	// Use this for initialization
	void Start () {
		playTime = 0.0f;
		prevTime = Time.time;

	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown("p") && !isPause){
			Time.timeScale = 0.0f;
			isPause = true;
		}
		else if(Input.GetKeyDown("p") && isPause) 
		{
			Time.timeScale = 1.0f;
			isPause = false;
		}

		if (!isPause) 
		{
			playTime += Time.time - prevTime;
		}
		prevTime = Time.time;
	}

	void OnGUI()
	{

		GUI.BeginGroup(new Rect(Screen.width-250, 40, 700, 900));
		GUI.skin.button.fontSize = 25;
		if(!NormalGameSceneManager_Compare.BoostAtResultPage)
		{	
			if (GUI.Button (new Rect (0, 0, 60, 40), "End")) 
			{
				Time.timeScale = 1.0f;
				SceneManager.SM.changeAndMoveScene(SceneState.scene_menu);
			}

			if (GUI.Button (new Rect (80, 0, 60, 40), "II")) {
				if(!isPause)
				{
					Time.timeScale = 0.0f;
					isPause = true;
				}
				else if(isPause)
				{
					Time.timeScale = 1.0f;
					isPause = false;
				}
			}
			if (GUI.Button (new Rect (160, 0, 60, 40), ">>")) 
			{
				if(Time.timeScale <20.0f)
				{
					Time.timeScale += 1.0f;
				}
			}
		}
			GUI.EndGroup ();

	}
	public void PauseGame()
	{
		isPause = true;
	}

	public void PlayGame()
	{
		isPause = false;
	}
	
	public float getTime()
	{
		return playTime;
	}

}
