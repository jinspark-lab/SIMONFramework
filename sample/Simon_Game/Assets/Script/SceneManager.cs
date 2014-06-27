using UnityEngine;
using System.Collections;


public enum SceneState {
	scene_menu = 0,
	scene_setting_property,
	scene_play_game,
	scene_result_page,
	scene_play_simulation,
	scene_loading,
	scene_start,
	scene_dummy,
	scene_choice_simulation,
	scene_play_compare,
	scene_loadDefinition
}
public class SceneManager : MonoBehaviour {

	public static int SceneMode = 0;
	public static int SimulationType = 0;
	public static int PlayGame_State = 1;

	public static SceneManager SM;
	
	public Texture2D[] btnTexture = new Texture2D[5];
	public SceneState sceneState;
	public Camera cam;

	public GameObject btnStartGame;
	public GameObject btnSimulation;
	public GameObject btnCompareMon;
	public GameObject btnExit;

	public GameObject Sound_Btn_Click;

	// Use this for initialization
	void Awake() 
	{
		//SIMON.GlobalSIMON.CleanSIMONEnvironment ();
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager_Simulation.GroupC);
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager_Simulation.GroupA);
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager_Simulation.GroupB);
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager.GroupA);
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager.GroupB);
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager_Compare.GroupB);
		SIMON.GlobalSIMON.CleanSIMONGroup (GameTimeManager_Compare.GroupA);

		SIMON.GlobalSIMON.CleanSIMONEnvironment ();
		PlayGame_State = 1;
		SM = this;
		sceneState = SceneState.scene_menu;
	//	btnStartGame.GetComponent<Animator> ().
	}

	public void moveScene()
	{
		Application.LoadLevel ((int)SM.sceneState);
		//Application.LoadLevelAdditive
	}

	public SceneState checkState()
	{
		return SM.sceneState;
	}

	public int changeAndMoveScene(SceneState stateNum)
	{
		if (SM.sceneState != stateNum) 
		{
			SM.sceneState = stateNum;
			moveScene();
			return 1;
		} 
		else
			return 0;
	}

	// Click Check
	private GameObject GetClickedObject() 
	{
		//충돌이 감지된 영역
		GameObject target = null; 
		
		Vector3 pos = Input.mousePosition;
		pos.z = 10;
		
		RaycastHit2D hit1 = Physics2D.Raycast(cam.camera.ScreenToWorldPoint(pos), Vector2.zero);
		
		if(hit1.collider != null)
		{
			target = hit1.collider.gameObject;
		}
		return target; 
	} 

	IEnumerator StartGameRootine()
	{
		Sound_Btn_Click.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(0.7f); 


		SceneMode = 1;
		changeAndMoveScene(SceneState.scene_choice_simulation);
	}

	IEnumerator SimulationRootine()
	{
		Sound_Btn_Click.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(0.7f); 
		SceneMode = 2;
		changeAndMoveScene(SceneState.scene_choice_simulation);
	}

	IEnumerator CompareMonRootine()
	{
		Sound_Btn_Click.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(0.7f); 
		SceneMode = 3;
		changeAndMoveScene (SceneState.scene_choice_simulation);
	}

	IEnumerator QuitApplication()
	{
		Sound_Btn_Click.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(0.7f); 
		Application.Quit();
	}



	// Update is called once per frame
	void Update() 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			GameObject BtnObject = GetClickedObject();

			if(BtnObject.name.Equals("Btn_StartGame"))
			{
				BtnObject.GetComponent<Animator>().Play ("Ani_StartGame");
				StartCoroutine(StartGameRootine());
			}
			else if(BtnObject.name.Equals("Btn_Simulation"))
			{
				BtnObject.GetComponent<Animator>().Play ("Ani_Simulation");
				StartCoroutine(SimulationRootine());
			}
			else if(BtnObject.name.Equals("Btn_CompareMon"))
			{
				BtnObject.GetComponent<Animator>().Play ("Ani_Compare");
				StartCoroutine(CompareMonRootine());
			}
			else if(BtnObject.name.Equals("Btn_Exit"))
			{
				BtnObject.GetComponent<Animator>().Play ("Ani_Exit");
				StartCoroutine(QuitApplication());
			}
		}


	}
}
