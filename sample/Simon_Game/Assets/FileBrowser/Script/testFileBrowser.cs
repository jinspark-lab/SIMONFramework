using UnityEngine;
using System.Collections;
using SIMONFramework;

public class testFileBrowser : MonoBehaviour {
	public GUISkin[] skins;
	public Texture2D file,folder,back,drive;
	
	string[] layoutTypes = {"Type 0","Type 1"};
	FileBrowser fb = new FileBrowser();
	
	// Use this for initialization
	void Start () {
		//setup file browser style
		//fb.setLayout (1);
		fb.guiSkin = skins[1]; //set the starting skin
		//set the various textures
		fb.fileTexture = file; 
		fb.directoryTexture = folder;
		fb.backTexture = back;
		fb.driveTexture = drive;
	}
	
	void OnGUI(){

		if (fb.draw ()) 
		{

				
		}

		GUI.BeginGroup(new Rect(Screen.width/2 + 450, Screen.height - 270, 700, 900));
		GUI.skin.button.fontSize = 25;
	
		if (GUI.Button (new Rect (0, 140, 110, 65), "BACK")) {
			SceneManager.SM.changeAndMoveScene(SceneState.scene_result_page);
		}
		
		if (GUI.Button (new Rect (120, 140, 110, 65), "SELECT")) {
			string fileName = fb.getCurrentFolder();
			
			if(SceneManager.SceneMode == 1)
			{
				for (int i = 0; i< GameTimeManager.GroupB.Count; i++) 
				{
					((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).LoadObjectDefinition(fileName,// + ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).ObjectID, 
					                                                                            ((SIMONObject)GameTimeManager.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).LoadObjectDefinition(fileName,// + ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).ObjectID, 
					                                                                            ((SIMONObject)GameTimeManager.GroupA.ValueOfIndex (i)).ObjectID);
				}
			}
			else if(SceneManager.SceneMode == 2)
			{
				for (int i = 0; i< GameTimeManager_Simulation.GroupB.Count; i++) 
				{
					((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (i)).LoadObjectDefinition(fileName, 
					                                                                                       ((SIMONObject)GameTimeManager_Simulation.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i)).LoadObjectDefinition(fileName, 
					                                                                                       ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i)).ObjectID);
				}
			}
			else if(SceneManager.SceneMode == 3)
			{
				for (int i = 0; i< GameTimeManager_Compare.GroupB.Count; i++)
				{
					((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).LoadObjectDefinition(fileName, 
					                                                                                    ((SIMONObject)GameTimeManager_Compare.GroupB.ValueOfIndex (i)).ObjectID);
					
					((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).LoadObjectDefinition(fileName, 
					                                                                                    ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).ObjectID);
				}
			}
			
			if(SceneManager.SceneMode == 1)
				SceneManager.SM.changeAndMoveScene(SceneState.scene_play_game);
			else if(SceneManager.SceneMode == 2)
				SceneManager.SM.changeAndMoveScene(SceneState.scene_play_simulation);
			else if(SceneManager.SceneMode == 3)
				SceneManager.SM.changeAndMoveScene(SceneState.scene_play_compare);
		}
		
		GUI.EndGroup ();

	}
}
