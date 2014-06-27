using UnityEngine;
using System.Collections;

public class Monster_Information_Controller_Simulation : MonoBehaviour {
	public Camera cam;
	public GameObject Click_gObj;
	public Sprite RedSprite;
	public Sprite BlueSprite;
	public GUIText[] Property_Text;
	public Vector3 out_Vector;
	public bool check;
	private bool isOverRight;
	// Use this for initialization
	void Start () {
		out_Vector = new Vector3 (100.0f, 0.0f, 0.0f);
		Monster_Information_out ();
		check = false;
		isOverRight = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (GameTimeManager_Simulation.isPause) 
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				Click_gObj = GetClickedObject ();  
				
				if (!Click_gObj.name.Equals ("BG")) 
				{
					Vector3 pos = Input.mousePosition;
					pos.z = 10;
					this.gameObject.transform.position = cam.camera.ScreenToWorldPoint (pos);
					
					if(this.gameObject.transform.position.x > 5.0f)
					{
						isOverRight = true;
						this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x-2.3f, this.gameObject.transform.position.y, -4.0f);
					}
					else
					{
						isOverRight = false;
						this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x+2.3f, this.gameObject.transform.position.y, -4.0f);
					}
					
					if(Click_gObj.tag.Equals("Home"))
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = RedSprite;
						//Property_Text[0].text = ((int)(Click_gObj.GetComponent<Monster_Controller>().NowHealth)).ToString()+" / "+
						//	((int)(Click_gObj.GetComponent<Monster_Controller>().TotalHealth)).ToString();
						Property_Text[0].text = ((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("CurHp"))).ToString("0.")+" / "+
								((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("CON"))).ToString("0.0");
						Property_Text[1].text = (Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Monster_Damage")).ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Strength"))).ToString("0.0");
						Property_Text[2].text = Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Monster_Attack_Speed").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Attack_Speed"))).ToString("0.0");
						Property_Text[3].text = Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Monster_Moving_Speed").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Moving_Speed"))).ToString("0.0");
						Property_Text[4].text = Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Monster_Defensive").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Defensive"))).ToString("0.0");
						Property_Text[5].text = Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Monster_Critical").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Critical"))).ToString("0.0");
						Property_Text[6].text = Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Monster_Range").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Range"))).ToString("0.0");
						Property_Text[7].text =  Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("ID").ToString("0.0");
						
						Property_Text[8].text =  Click_gObj.GetComponent<Monster_Controller_Simulation>().sObject.GetPropertyElement("Target").ToString("0.0");
						
						
					}
					else
					{
						this.gameObject.GetComponent<SpriteRenderer>().sprite = BlueSprite;
						//Property_Text[0].text = ((int)(Click_gObj.GetComponent<Monster_Controller_B>().NowHealth)).ToString()+" / "+
						//	((int)(Click_gObj.GetComponent<Monster_Controller_B>().TotalHealth)).ToString();
						Property_Text[0].text = ((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("CurHp"))).ToString("0.")+" / "+
								((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("CON"))).ToString("0.0");
						Property_Text[1].text = (Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Monster_Damage")).ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Strength"))).ToString("0.0");
						Property_Text[2].text = Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Monster_Attack_Speed").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Attack_Speed"))).ToString("0.0");
						Property_Text[3].text = Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Monster_Moving_Speed").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Moving_Speed"))).ToString("0.0");
						Property_Text[4].text = Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Monster_Defensive").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Defensive"))).ToString("0.0");
						Property_Text[5].text = Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Monster_Critical").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Critical"))).ToString("0.0");
						Property_Text[6].text = Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Monster_Range").ToString("0.0") + " / " + 
							((float)(Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Range"))).ToString("0.0");
						Property_Text[7].text =  Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("ID").ToString("0.0");
						
						Property_Text[8].text =  Click_gObj.GetComponent<Monster_Controller_B_Simulation>().sObject.GetPropertyElement("Target").ToString("0.0");
					}
					
					
					if(isOverRight)
					{
						pos = new Vector3 (pos.x - 150.0f, pos.y + 139.0f, 10.0f);
						for (int i =0; i < 7; i++) 
						{
							pos = new Vector3 (pos.x, pos.y - 32.0f, 10.0f);
							Property_Text [i].transform.position = cam.camera.ScreenToViewportPoint (pos);
						}
						pos = new Vector3 (pos.x - 140.0f, pos.y + (32.0f * 2) + 2.0f, 10.0f);
						Property_Text [7].transform.position = cam.camera.ScreenToViewportPoint (pos);
						pos = new Vector3 (pos.x, pos.y - 30.0f, 10.0f);
						Property_Text [8].transform.position = cam.camera.ScreenToViewportPoint (pos);
					}
					else if(!isOverRight)
					{
						pos = new Vector3 (pos.x+260.0f, pos.y + 139.0f, 10.0f);
						for (int i =0; i < 7; i++) 
						{
							pos = new Vector3 (pos.x, pos.y - 32.0f, 10.0f);
							Property_Text [i].transform.position = cam.camera.ScreenToViewportPoint (pos);
						}
						pos = new Vector3 (pos.x - 165.0f, pos.y + (32.0f * 2), 10.0f);
						Property_Text [7].transform.position = cam.camera.ScreenToViewportPoint (pos);
						pos = new Vector3 (pos.x, pos.y - 28.0f, 10.0f);
						Property_Text [8].transform.position = cam.camera.ScreenToViewportPoint (pos);
					}
					
				}
			}
		} 
		else 
		{
			Monster_Information_out();
		}
	}
	public void Monster_Information_out()
	{
		this.gameObject.transform.position = out_Vector;
		for (int i = 0; i<7; i++) 
		{
			Property_Text [i].transform.position = cam.camera.ScreenToViewportPoint (out_Vector);
		}
		
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
}
