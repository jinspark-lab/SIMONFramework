using UnityEngine;
using System.Collections;
using SIMONFramework;

public class PlayerView : MonoBehaviour {

	public bool isOver;
	public int TotalValue;
	public GUIText TotalStatValue;
	public GUIText[] StatName = new GUIText[7];
	public GUIText[] StatValue = new GUIText[7];
	public GameObject[] StatusBar = new GameObject[7];
	public UIScrollBar[] StatusScrollBar = new UIScrollBar[7];

	public GameObject Sound_Btn_1;
	public GameObject Sound_Btn_2;

	// Use this for initialization
	void Start () {
		SceneManager.SceneMode = 1;
		isOver = false;
		TotalValue = 0;

		for(int i=0; i<7; i++)
		{
			StatusScrollBar[i] = StatusBar[i].GetComponent<UIScrollBar>();
			StatusScrollBar[i].value = 0.01f;
			StatValue[i].text = "1";
		}
		StatName [0].text = "　  체력";
		StatName [1].text = "　  　힘";
		StatName [2].text = "공격속도";
		StatName [3].text = "이동속도";
		StatName [4].text = "  방어력";
		StatName [5].text = "치명타율";
		StatName [6].text = "사정거리";
	}

	void SetStatus(int i, string text)
	{
		Sound_Btn_2.GetComponent<AudioSource>().Play ();

		int controlNumber = int.Parse (StatValue [i].text);
		if (text.Equals ("-") && TotalValue != 1 && controlNumber != 1)
		{
			controlNumber--;
		}
		else if(text.Equals("+") && TotalValue != 100 && controlNumber != 100)
		{
			controlNumber++;
		}
		StatValue [i].text = "" + controlNumber;
		StatusScrollBar [i].value = float.Parse (StatValue [i].text) / 100.0f;
	}

	void applyStatToPlayer()
	{
		PlayerPrefs.SetFloat ("Player_CON", float.Parse (StatValue [0].text));
		PlayerPrefs.SetFloat ("Player_Strength", float.Parse (StatValue [1].text));
		PlayerPrefs.SetFloat ("Player_Attack_Speed", float.Parse (StatValue [2].text));
		PlayerPrefs.SetFloat ("Player_Moving_Speed", float.Parse (StatValue [3].text));
		PlayerPrefs.SetFloat ("Player_Defensive", float.Parse (StatValue [4].text));
		PlayerPrefs.SetFloat ("Player_Critical", float.Parse (StatValue [5].text));
		PlayerPrefs.SetFloat ("Player_Range", float.Parse (StatValue [6].text));
	}

	void OnGUI()
	{
		GUI.BeginGroup(new Rect(Screen.width/2 + 280, Screen.height/2 - 385, 700, 900));
		GUI.skin.button.fontSize = 25;
		// 1
		if (GUI.Button (new Rect (0, 0, 30, 30), "-")) {
			SetStatus(0, "-");
		}
		if (GUI.Button (new Rect (40, 0, 30, 30), "+")) {
			SetStatus(0, "+");
		}

		// 2
		if (GUI.Button (new Rect (0, 115, 30, 30), "-")) {
			SetStatus(1, "-");
		}
		if (GUI.Button (new Rect (40, 115, 30, 30), "+")) {
			SetStatus(1, "+");
		}

		// 3
		if (GUI.Button (new Rect (0, 232, 30, 30), "-")) {
			SetStatus(2, "-");
		}
		if (GUI.Button (new Rect (40, 232, 30, 30), "+")) {
			SetStatus(2, "+");
		}

		// 4
		if (GUI.Button (new Rect (0, 342, 30, 30), "-")) {
			SetStatus(3, "-");
		}
		if (GUI.Button (new Rect (40, 342, 30, 30), "+")) {
			SetStatus(3, "+");
		}

		// 5
		if (GUI.Button (new Rect (0, 457, 30, 30), "-")) {
			SetStatus(4, "-");
		}
		if (GUI.Button (new Rect (40, 457, 30, 30), "+")) {
			SetStatus(4, "+");
		}

		// 6
		if (GUI.Button (new Rect (0, 575, 30, 30), "-")) {
			SetStatus(5, "-");
		}
		if (GUI.Button (new Rect (40, 575, 30, 30), "+")) {
			SetStatus(5, "+");
		}

		// 7
		if (GUI.Button (new Rect (0, 688, 30, 30), "-")) {
			SetStatus(6, "-");
		}
		if (GUI.Button (new Rect (40, 688, 30, 30), "+")) {
			SetStatus(6, "+");
		}
		if (GUI.Button (new Rect (180, 680, 100, 90), "Cancel")) {
			StartCoroutine(toPlaySound_cancel());
		}
		if (GUI.Button (new Rect (300, 680, 100, 90), "Accept")) {
			StartCoroutine(toPlaySound_accept());
		}
		GUI.EndGroup ();
	}

	IEnumerator toPlaySound_cancel()
	{
		Sound_Btn_1.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.7f); 
		SceneManager.SM.changeAndMoveScene(SceneState.scene_menu);
	}

	IEnumerator toPlaySound_accept()
	{
		Sound_Btn_1.GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds(0.7f); 
		SceneManager.SceneMode = 2;
		SIMONObject sObject;
		SIMONObject sObjectB;
		for(int i = 1; i < 8 ; i++)
		{
			sObject = new SIMONObject ();
			sObjectB = new SIMONObject();
			sObject.ObjectID = "Monster_A_"+i.ToString();
			GameTimeManager.GroupA.Add (sObject.ObjectID, sObject);
			sObjectB.ObjectID = "Monster_B_"+i.ToString();
			GameTimeManager.GroupB.Add (sObject.ObjectID, sObjectB);
		}
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

		applyStatToPlayer();
		SceneManager.SM.changeAndMoveScene(SceneState.scene_play_game);
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int i=0; i<7; i++)
		{
			if(StatValue [i].text.Equals("0") || StatusScrollBar[i].value == 0.01f)
			{
				StatValue [i].text = "1";
				StatusScrollBar[i].value = 0.01f;
			}
		}


		int tmpValue;
		int tmpTotalValue = 0;
		for(int i=0; i<7; i++)
		{
			tmpValue = Mathf.FloorToInt(StatusScrollBar[i].value * 100);
			StatusScrollBar[i].value = (float)tmpValue/100;
			tmpTotalValue += tmpValue;
		}

		if(tmpTotalValue > 100)
		{
			for (int i=0; i<7; i++) 
			{
				StatusScrollBar[i].value = (float.Parse(StatValue[i].text)) / 100.0f;
			}
		}

		if (tmpTotalValue <= 100) 
		{
			TotalValue = 0;
			for(int i=0; i<7; i++)
			{
				tmpValue = Mathf.FloorToInt(StatusScrollBar[i].value * 100);
				TotalValue += tmpValue;
				//StatValue[i].text = ""+Mathf.CeilToInt(StatusScrollBar[i].value * 100);
				StatValue[i].text = ""+Mathf.FloorToInt(StatusScrollBar[i].value * 100);
			}
			TotalStatValue.text = "Total : " + TotalValue;
		}
	}
}
