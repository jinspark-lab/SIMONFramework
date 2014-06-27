using UnityEngine;
using System.Collections;
using SIMONFramework;
public class ChoiceSceneTypeScript : MonoBehaviour {


	public GameObject Sound_Btn_Click;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator MoveSimulationScene()
	{
		Sound_Btn_Click.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(0.7f); 

		if(SceneManager.SceneMode == 1)
			SceneManager.SM.changeAndMoveScene(SceneState.scene_setting_property);
		else if(SceneManager.SceneMode == 2)
			SceneManager.SM.changeAndMoveScene(SceneState.scene_play_simulation);
		else if(SceneManager.SceneMode == 3)
		{
			SIMONObject sObject;
			SIMONObject sObjectB;
			for(int i = 1; i < 8 ; i++)
			{
				sObject = new SIMONObject ();
				sObjectB = new SIMONObject();
				sObject.ObjectID = "Monster_A_"+i.ToString();
				GameTimeManager_Compare.GroupA.Add (sObject.ObjectID, sObject);
				sObjectB.ObjectID = "Monster_B_"+i.ToString();
				GameTimeManager_Compare.GroupB.Add (sObject.ObjectID, sObjectB);
			}
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

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(Screen.width/2 + 320, Screen.height/2 + 200, 1500, 900));
		GUI.skin.button.fontSize = 20;
		GUI.color = Color.black;
		if (GUI.Button (new Rect (0, 0, 150, 50), "PROPERTY")) {
			SceneManager.SimulationType = 1;
			StartCoroutine(MoveSimulationScene());

		}
		if (GUI.Button (new Rect (180, 0, 150, 50), "ACTION")) {
			SceneManager.SimulationType = 2;
			StartCoroutine(MoveSimulationScene());
		}
		if (GUI.Button (new Rect (90, 70, 150, 50), "Back")) {
			SceneManager.SM.changeAndMoveScene(SceneState.scene_menu);
		}


		GUI.EndGroup ();
	}
}
