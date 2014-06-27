using UnityEngine;
using System.Collections;
using SIMONFramework;

public class ResultPageManager : MonoBehaviour {

	private GameObject ResultString;
	public GUIText[] RedValue = new GUIText[12];
	public GUIText[] BlueValue = new GUIText[12];
	public GUIText totalRed;
	public GUIText totalBlue;
	public Sprite Win_Red;
	public Sprite Win_Blue;

	public GameObject SoundObject_1;
	public GameObject SoundObject_2;

	void setRedValueOfObejct(int id)
	{
		if (SceneManager.SceneMode == 1) 
		{
			RedValue [0].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Damage").ToString ("0.0");
			RedValue[1].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_AttackedDamage").ToString("0.0");
			RedValue [2].text = " / " + ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("HP").ToString ("0.0");
			RedValue [3].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("CurHp").ToString ("0.0");
			RedValue [4].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Strength").ToString ("0.0");
			RedValue [5].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Attack_Speed").ToString ("0.0");
			RedValue [6].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Moving_Speed").ToString ("0.0");
			RedValue [7].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Defensive").ToString ("0.0");
			RedValue [8].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Critical").ToString ("0.0");
			RedValue [9].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Range").ToString ("0.0");
			//RedValue[9].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("").ToString();
			//RedValue[10].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Enemy_Defensive").ToString("0.0");
			RedValue [10].text = ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Defense").ToString ("0.0");
		}
		else if (SceneManager.SceneMode == 2) 
		{
			RedValue [0].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Damage").ToString ("0.0");
			RedValue[1].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_AttackedDamage").ToString("0.0");
			RedValue [2].text = " / " + ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("HP").ToString ("0.0");
			RedValue [3].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("CurHp").ToString ("0.0");
			RedValue [4].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Strength").ToString ("0.0");
			RedValue [5].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Attack_Speed").ToString ("0.0");
			RedValue [6].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Moving_Speed").ToString ("0.0");
			RedValue [7].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Defensive").ToString ("0.0");
			RedValue [8].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Critical").ToString ("0.0");
			RedValue [9].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Range").ToString ("0.0");
			//RedValue[9].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("").ToString("0.0");
			//RedValue[10].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Enemy_Defensive").ToString("0.0");
			RedValue [10].text = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Defense").ToString ("0.0");
		}
		else if (SceneManager.SceneMode == 3)
		{
			RedValue [0].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Damage").ToString ("0.0");
			RedValue[1].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_AttackedDamage").ToString("0.0");
			RedValue [2].text = " / " + ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("HP").ToString ("0.0");
			RedValue [3].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("CurHp").ToString ("0.0");
			RedValue [4].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Strength").ToString ("0.0");
			RedValue [5].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Attack_Speed").ToString ("0.0");
			RedValue [6].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Moving_Speed").ToString ("0.0");
			RedValue [7].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Defensive").ToString ("0.0");
			RedValue [8].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Critical").ToString ("0.0");
			RedValue [9].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Range").ToString ("0.0");
			//RedValue[9].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("").ToString("0.0");
			//RedValue[10].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Enemy_Defensive").ToString("0.0");
			RedValue [10].text = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Defense").ToString ("0.0");
		}
	}

	void setBlueValueOfObejct(int id)
	{
		if(SceneManager.SceneMode == 1)
		{
			BlueValue[0].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Damage").ToString("0.0");
			BlueValue[1].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_AttackedDamage").ToString("0.0");
			BlueValue[2].text = " / " +((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("HP").ToString("0.0");
			BlueValue[3].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("CurHp").ToString("0.0");
			BlueValue[4].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Strength").ToString("0.0");
			BlueValue[5].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Attack_Speed").ToString("0.0");
			BlueValue[6].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Moving_Speed").ToString("0.0");
			BlueValue[7].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Defensive").ToString("0.0");
			BlueValue[8].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Critical").ToString("0.0");
			BlueValue[9].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Range").ToString("0.0");
			//BlueValue[9].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("").ToString("0.0");
			//BlueValue[10].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Enemy_Defensive").ToString("0.0");
			BlueValue[10].text = ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Defense").ToString("0.0");
		}
		else if(SceneManager.SceneMode == 2)
		{
			BlueValue[0].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Damage").ToString("0.0");
			BlueValue[1].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_AttackedDamage").ToString("0.0");
			BlueValue[2].text = " / " +((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("HP").ToString("0.0");
			BlueValue[3].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("CurHp").ToString("0.0");
			BlueValue[4].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Strength").ToString("0.0");
			BlueValue[5].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Attack_Speed").ToString("0.0");
			BlueValue[6].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Moving_Speed").ToString("0.0");
			BlueValue[7].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Defensive").ToString("0.0");
			BlueValue[8].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Critical").ToString("0.0");
			BlueValue[9].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Range").ToString("0.0");
			//BlueValue[9].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("").ToString("0.0");
			//BlueValue[10].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Enemy_Defensive").ToString("0.0");
			BlueValue[10].text = ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Defense").ToString("0.0");
		}
		else if (SceneManager.SceneMode ==3)
		{
			BlueValue[0].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Damage").ToString("0.0");
			BlueValue[1].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_AttackedDamage").ToString("0.0");
			BlueValue[2].text = " / " +((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("HP").ToString("0.0");
			BlueValue[3].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("CurHp").ToString("0.0");
			BlueValue[4].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Strength").ToString("0.0");
			BlueValue[5].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Attack_Speed").ToString("0.0");
			BlueValue[6].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Moving_Speed").ToString("0.0");
			BlueValue[7].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Defensive").ToString("0.0");
			BlueValue[8].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Critical").ToString("0.0");
			BlueValue[9].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Range").ToString("0.0");
			//BlueValue[9].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("").ToString("0.0");
			//BlueValue[10].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Enemy_Defensive").ToString("0.0");
			BlueValue[10].text = ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (id)).GetPropertyElement ("Monster_Total_Defense").ToString("0.0");
		}
	}


	// Use this for initialization
	void Start () {

		ResultString = GameObject.Find ("ResultPage");
		if (NormalGameSceneManager.whoIsWin == 1 || NormalGameSceneManager_Simulation.whoIsWin == 1 || NormalGameSceneManager_Compare.whoIsWin == 1)
			ResultString.GetComponent<SpriteRenderer> ().sprite = Win_Blue;
		else if (NormalGameSceneManager.whoIsWin == 2 || NormalGameSceneManager_Simulation.whoIsWin == 2 || NormalGameSceneManager_Compare.whoIsWin == 2)
			ResultString.GetComponent<SpriteRenderer> ().sprite = Win_Red;

		setRedValueOfObejct (0);
		setBlueValueOfObejct (0);

		double totalR = 0;
		double totalB = 0;

		for (int i =0; i <7; i++) 
		{
			if(SceneManager.SceneMode == 1)
			{
				totalR += ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).GetPropertyElement ("Monster_Total_AttackedDamage");
				totalB += ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).GetPropertyElement ("Monster_Total_AttackedDamage");
			}
			else if(SceneManager.SceneMode == 2)
			{
				totalR += ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (i)).GetPropertyElement ("Monster_Total_AttackedDamage");
				totalB += ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i)).GetPropertyElement ("Monster_Total_AttackedDamage");
			}
			else if (SceneManager.SceneMode ==3 )
			{
				totalR += ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).GetPropertyElement ("Monster_Total_AttackedDamage");
				totalB += ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).GetPropertyElement ("Monster_Total_AttackedDamage");
			}

		}
		totalRed.text = totalR.ToString ("0.0");
		totalBlue.text = totalB.ToString ("0.0");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI()
	{
		// red
		GUI.BeginGroup(new Rect(Screen.width/2 - 520, Screen.height/2 - 160, 700, 900));
		GUI.skin.button.fontSize = 25;
		if (GUI.Button (new Rect (0, 0, 30, 30), "1")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(0);
		}

		if (GUI.Button (new Rect (40, 0, 30, 30), "2")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(1);
		}

		if (GUI.Button (new Rect (80, 0, 30, 30), "3")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(2);
		}

		if (GUI.Button (new Rect (120, 0, 30, 30), "4")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(3);
		}

		if (GUI.Button (new Rect (160, 0, 30, 30), "5")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(4);
		}

		if (GUI.Button (new Rect (200, 0, 30, 30), "6")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(5);
		}

		if (GUI.Button (new Rect (240, 0, 30, 30), "7")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setRedValueOfObejct(6);
		}

		GUI.EndGroup ();

		// blue
		GUI.BeginGroup(new Rect(Screen.width/2 + 40, Screen.height/2 - 160, 700, 900));
		GUI.skin.button.fontSize = 25;

		if (GUI.Button (new Rect (0, 0, 30, 30), "1")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(0);
		}
		
		if (GUI.Button (new Rect (40, 0, 30, 30), "2")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(1);
		}
		
		if (GUI.Button (new Rect (80, 0, 30, 30), "3")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(2);
		}
		
		if (GUI.Button (new Rect (120, 0, 30, 30), "4")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(3);
		}
		
		if (GUI.Button (new Rect (160, 0, 30, 30), "5")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(4);
		}
		
		if (GUI.Button (new Rect (200, 0, 30, 30), "6")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(5);
		}
		
		if (GUI.Button (new Rect (240, 0, 30, 30), "7")) {
			SoundObject_1.GetComponent<AudioSource> ().Play ();
			setBlueValueOfObejct(6);
		}


		GUI.EndGroup ();

		GUI.BeginGroup(new Rect(Screen.width/2 + 450, Screen.height - 270, 700, 900));
		GUI.skin.button.fontSize = 30;

		if(SceneManager.SceneMode == 2)
		{
			if(GUI.Button (new Rect (0, 0, 230, 65), "Boost Evolution")) {
				StartCoroutine(toPlaySound_BoostEvolution());
			}
			GUI.skin.button.fontSize = 35;
			if (GUI.Button (new Rect (0, 70, 110, 65), "LOAD")) {
				StartCoroutine(toPlaySound_load());
			}
	
			if (GUI.Button (new Rect (120, 70, 110, 65), "SAVE")) {
				StartCoroutine(toPlaySound_save());
			}
		}

		if (GUI.Button (new Rect (0, 140, 110, 65), "PLAY")) {
			StartCoroutine(toPlaySound_accept());
		}
		
		if (GUI.Button (new Rect (120, 140, 110, 65), "END")) {
			StartCoroutine(toPlaySound_cancel());
		}

		GUI.EndGroup ();
	}

	IEnumerator toPlaySound_BoostEvolution()
	{

		SoundObject_2.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.7f); 
		if(SceneManager.SceneMode == 1)
			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_game);
		else if(SceneManager.SceneMode == 2)
		{

			SIMON.GlobalSIMON.SIMONLearn(GameTimeManager_Simulation.GroupC);
			NormalGameSceneManager_Simulation.BoostAtResultPage = true;
			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_simulation);
		}
		else if(SceneManager.SceneMode == 3)
		{
			NormalGameSceneManager_Compare.BoostAtResultPage = true;
			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_compare);
		}


	}

	
	IEnumerator toPlaySound_load()
	{
		SoundObject_2.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.7f); 
		SceneManager.SM.changeAndMoveScene(SceneState.scene_loadDefinition);
	}

	IEnumerator toPlaySound_save()
	{
		SoundObject_2.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.1f); 
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
		else if (SceneManager.SceneMode == 3)
		{
			for(int i=0; i<GameTimeManager_Compare.GroupA.Count; i++)
				SIMON.GlobalSIMON.PublishDefinition("\\" + nowDay + "\\" + ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).ObjectID
				                                    , (SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex(i));
			for(int i=0; i<GameTimeManager_Compare.GroupB.Count; i++)
				SIMON.GlobalSIMON.PublishDefinition("\\" + nowDay + "\\" + ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).ObjectID
				                                    , (SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex(i));
		}
	}

	IEnumerator toPlaySound_cancel()
	{
		SoundObject_2.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.7f); 
		SceneManager.SM.changeAndMoveScene(SceneState.scene_menu);
	}
	
	IEnumerator toPlaySound_accept()
	{
		SoundObject_2.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.7f); 

		if(SceneManager.SceneMode == 1)
		{
			SceneManager.PlayGame_State++;
			if(SceneManager.PlayGame_State > 5)
				SceneManager.SM.changeAndMoveScene(SceneState.scene_menu);
			for (int i = 0; i< GameTimeManager.GroupB.Count; i++) 
			{
				if(SceneManager.SimulationType == 1)
				{
					((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_property_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_property_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).ObjectID);
				}
				else
				{
					((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_action_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_action_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).ObjectID);
				}
			}

			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_game);
		}
		else if(SceneManager.SceneMode == 2)
		{
			SIMON.GlobalSIMON.SIMONLearn(GameTimeManager_Simulation.GroupC);
			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_simulation);
		}
		else if(SceneManager.SceneMode == 3)
		{
			SceneManager.PlayGame_State++;
			if(SceneManager.PlayGame_State > 5)
				SceneManager.SM.changeAndMoveScene(SceneState.scene_menu);
			for (int i = 0; i< GameTimeManager_Compare.GroupB.Count; i++) 
			{
				if(SceneManager.SimulationType == 1)
				{
					((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_property_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_property_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).ObjectID);
				}
				else
				{
					((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_action_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).LoadObjectDefinition("PlayGame\\State_action_"+SceneManager.PlayGame_State.ToString(),((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).ObjectID);
				}
			}

			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_compare);
		}
	}
	

}
