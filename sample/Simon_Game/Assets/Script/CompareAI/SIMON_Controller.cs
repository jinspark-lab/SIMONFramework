using UnityEngine;
using System.Collections;
using SIMONFramework;
using System.Reflection;
public class SIMON_Controller : MonoBehaviour 
{
	public GameObject gObj_Monster;
	public GameObject gObj_Target_Attack;
	public GameObject gObj_Target_Move;
	public float ID_Target_Attack;
	public float ID_Target_Move;
	
	public int SimulationType;
	
	public Vector3 Vector_Target;
	public Animator animator;
	public string Target_Name;
	public int value;
	public SIMONObject sObject;
	
	public float curPositionX;
	public float curPositionY;
	public int curDirection;
	
	public GameObject Snowball;
	public Sprite SnowballSprite;
	public GameObject sb;
	public GameObject MovingTarget;
	public GameObject mt;
	
	public float Monster_ID;
	public float dis;
	
	public float coolTime_Attack_Start;
	
	public bool isSkill;
	
	public int Target_Attack_Index;
	public int Target_Move_Index;
	
	// health bar
	public float NowHealth;
	public float TotalHealth;
	public float realHealthBar;
	public Camera cam;
	
	private float left;
	private float top;
	private Vector2 pos; 
	
	public Texture2D emptyTex;
	public Texture2D fullTex;
	
	public void setTotalHP(float HP)
	{
		TotalHealth = HP;
		NowHealth = HP;
	}
	
	public bool getDamage(float damage)
	{
		NowHealth -= damage;
		if (NowHealth < 0) 
		{
			realHealthBar = 0;
			NowHealth = 0;
			return false;
		}
		else 
		{
			return true;
		}
	}
	
	public void getHealing(float heal)
	{
		if (NowHealth + heal > TotalHealth) 
		{
			NowHealth = TotalHealth;
		}
		else
		{
			NowHealth += heal;
		}
	}
	
	void Awake()
	{
		TotalHealth = 100;
		NowHealth = 100;
		
	}
	
	void OnGUI() {
		
		if (!NormalGameSceneManager_Compare.BoostAtResultPage)
		{
			GUI.DrawTexture(new Rect(left - 20, top - 75, realHealthBar, 10.0f), fullTex);
			GUI.DrawTexture(new Rect(left - 20, top - 75, 50.0f, 10.0f), emptyTex);
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		gObj_Monster = this.gameObject;
		animator = GetComponent<Animator> ();
		SimulationType = SceneManager.SimulationType;

		sObject = new SIMONObject ();
		
		sObject.ObjectID = gObj_Monster.name;
		
		if (GameTimeManager_Compare.GroupA.Count == 7) 
		{
			for(int i= 0 ; i < GameTimeManager_Compare.GroupA.Count ; i++)
			{
				if(((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i)).ObjectID.Equals(sObject.ObjectID))
				{
					sObject = ((SIMONObject)GameTimeManager_Compare.GroupA.ValueOfIndex (i));
					reStart();
					if(SceneManager.PlayGame_State >1)
						return;
					break;
					
				}
			}
		}
		
		SIMON.GlobalSIMON.InsertSIMONObject(sObject);
		
		
		SIMONFunction propertyUpdate_Name = PropertyUpdate;
		SIMONFunction ExecutionFunction_propertyUpdate_Name = PropertyUpdate_Exection;
		SIMONFunction FitnessFunction_propertyUpdate_Name = PropertyUpdate_FitnessFunction;
		
		SIMONFunction ActionFunction_Move_Name = ActionFunction_Move;
		SIMONFunction ExecutionFunction_Move_Name = Monster_move;
		SIMONFunction FitnessFunction_Move_Name = FitnessFunction_Move;
		
		SIMONFunction ActionFunction_Attack_Name = ActionFunction_Attack;
		SIMONFunction ExecutionFunction_Attack_Name = Monster_attack;
		SIMONFunction FitnessFunction_Attack_Name = FitnessFunction_Attack;
		
		SIMONFunction ActionFunction_Defense_Name = ActionFunction_Defense;
		SIMONFunction ExecutionFunction_Defense_Name = Monster_defense;
		SIMONFunction FitnessFunction_Defense_Name = FitnessFunction_Defense;
		
		SIMONFunction ActionFunction_Avoid_Name = ActionFunction_Avoid;
		SIMONFunction ExecutionFunction_Avoid_Name = Monster_avoid;
		SIMONFunction FitnessFunction_Avoid_Name = FitnessFunction_Avoid;
		//
		
		SIMONFunction ActionFunction_Skill_Strength_Name = ActionFunction_Skill_Strength;
		SIMONFunction ExecutionFunction_Skill_Strength_Name = monster_Skill_Strength;
		SIMONFunction FitnessFunction_Skill_Strength_Name = FitnessFunction_Skill_Strength;
		
		SIMONFunction ActionFunction_Skill_Attack_Speed_Name = ActionFunction_Skill_Attack_Speed;
		SIMONFunction ExecutionFunction_Skill_Skill_Attack_Speed_Name = monster_Skill_Attack_Speed;
		SIMONFunction FitnessFunction_Skill_Attack_Speed_Name = FitnessFunction_Skill_Attack_Speed;
		
		SIMONFunction ActionFunction_Skill_Moving_Speed_Name = ActionFunction_Skill_Moving_Speed;
		SIMONFunction ExecutionFunction_Skill_Skill_Moving_Speed_Name = monster_Skill_Moving_Speed;
		SIMONFunction FitnessFunction_Skill_Moving_Speed_Name = FitnessFunction_Skill_Moving_Speed;
		
		SIMONFunction ActionFunction_Skill_Critical_Name = ActionFunction_Skill_Critical;
		SIMONFunction ExecutionFunction_Skill_Skill_Critical_Name = monster_Skill_Critical;
		SIMONFunction FitnessFunction_Skill_Critical_Name = FitnessFunction_Skill_Critical;
		
		SIMONFunction ActionFunction_Skill_Defensive_Name = ActionFunction_Skill_Defensive;
		SIMONFunction ExecutionFunction_Skill_Skill_Defensive_Name = monster_Skill_Defensive;
		SIMONFunction FitnessFunction_Skill_Defensive_Name = FitnessFunction_Skill_Defensive;
		
		SIMONFunction ActionFunction_Skill_CON_Name = ActionFunction_Skill_CON;
		SIMONFunction ExecutionFunction_Skill_Skill_CON_Name = monster_Skill_CON;
		SIMONFunction FitnessFunction_Skill_CON_Name = FitnessFunction_Skill_CON;
		
		SIMONFunction ActionFunction_Skill_Range_Name = ActionFunction_Skill_Range;
		SIMONFunction ExecutionFunction_Skill_Skill_Range_Name = monster_Skill_Range;
		SIMONFunction FitnessFunction_Skill_Range_Name = FitnessFunction_Skill_Range;
		
		//
		
		SIMON.GlobalSIMON.InsertSIMONMethod("propertyUpdate", propertyUpdate_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Execution_propertyUpdate" , ExecutionFunction_propertyUpdate_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_propertyUpdate" , FitnessFunction_propertyUpdate_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Move" , ActionFunction_Move_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Move" , ExecutionFunction_Move_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Move" , FitnessFunction_Move_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Attack" , ActionFunction_Attack_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Attack" , ExecutionFunction_Attack_Name);		
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Attack" , FitnessFunction_Attack_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Defense" , ActionFunction_Defense_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Defense" , ExecutionFunction_Defense_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Defense" , FitnessFunction_Defense_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Avoid" , ActionFunction_Avoid_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Avoid" , ExecutionFunction_Avoid_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Avoid" , FitnessFunction_Avoid_Name);
		//
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_Strength" , ActionFunction_Skill_Strength_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_Strength" , ExecutionFunction_Skill_Strength_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_Strength" , FitnessFunction_Skill_Strength_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_Attack_Speed" , ActionFunction_Skill_Attack_Speed_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_Attack_Speed" , ExecutionFunction_Skill_Skill_Attack_Speed_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_Attack_Speed" , FitnessFunction_Skill_Attack_Speed_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_Moving_Speed" , ActionFunction_Skill_Moving_Speed_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_Moving_Speed" , ExecutionFunction_Skill_Skill_Moving_Speed_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_Moving_Speed" , FitnessFunction_Skill_Moving_Speed_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_Critical" , ActionFunction_Skill_Critical_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_Critical" , ExecutionFunction_Skill_Skill_Critical_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_Critical" , FitnessFunction_Skill_Critical_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_Defensive" , ActionFunction_Skill_Defensive_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_Defensive" , ExecutionFunction_Skill_Skill_Defensive_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_Defensive" , FitnessFunction_Skill_Defensive_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_CON" , ActionFunction_Skill_CON_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_CON" , ExecutionFunction_Skill_Skill_CON_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_CON" , FitnessFunction_Skill_CON_Name);
		
		SIMON.GlobalSIMON.InsertSIMONMethod("ActionFunction_Skill_Range" , ActionFunction_Skill_Range_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("Monster_Skill_Range" , ExecutionFunction_Skill_Skill_Range_Name);
		SIMON.GlobalSIMON.InsertSIMONMethod("FitnessFunction_Skill_Range" , FitnessFunction_Skill_Range_Name);
		
		//
		
		
		
		setTotalHP((float)sObject.GetPropertyElement("HP"));
		curDirection = 3;
		isSkill = false;
		
	}
	
	public System.Object PropertyUpdate_Exection (SIMONObject a, SIMONObject[] b)
	{
		double value ;
		value =-10000000.0d;
		return value;
	}
	
	public System.Object PropertyUpdate_FitnessFunction(SIMONObject a, SIMONObject[] b)
	{
		double value ;
		value =-10000000.0d;
		return value;
	}
	public System.Object ObjectFitness(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage")/a.GetPropertyElement("Monster_Total_AttackCount");
		return value;
	}
	
	public System.Object FitnessFunction_Move(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage")/a.GetPropertyElement("Monster_Total_AttackCount");
		return value;
	}
	
	public System.Object FitnessFunction_Attack(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage")/a.GetPropertyElement("Monster_Total_AttackCount");
		return value;
	}
	public System.Object FitnessFunction_Defense(SIMONObject a, SIMONObject[] b)
	{
		double value=100.0d;
		value = a.GetPropertyElement("Monster_Total_Defense")/a.GetPropertyElement("Monster_Total_AttackedCount");
		return value;
	}
	public System.Object FitnessFunction_Avoid(SIMONObject a, SIMONObject[] b)
	{		
		double value=100.0d;
		value = -1.0d * a.GetPropertyElement("Monster_Total_AttackedCount");
		return value;
	}
	
	public System.Object FitnessFunction_Skill_Strength(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	public System.Object FitnessFunction_Skill_Attack_Speed(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	public System.Object FitnessFunction_Skill_Moving_Speed(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	public System.Object FitnessFunction_Skill_Critical(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	public System.Object FitnessFunction_Skill_Defensive(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	public System.Object FitnessFunction_Skill_CON(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	public System.Object FitnessFunction_Skill_Range(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		return value;
	}
	// Update is called once per frame
	
	void reStart()
	{
		sObject.SetPropertyElement ("End", 0.0d);
		sObject.SetPropertyElement ("State", -1.0d);
		sObject.SetPropertyElement ("Attack_Target", -1.0d);
		sObject.SetPropertyElement ("Move_Target", -1.0d);
		sObject.SetPropertyElement ("Target", -1.0d);
		sObject.SetPropertyElement ("AttackedCount", 0.0d);
		sObject.SetPropertyElement ("isCheck",0.0d);
		sObject.SetPropertyElement ("coolTime_Attack_Start",0.0d);
		sObject.SetPropertyElement ("Skill_number",0.0d);
		sObject.SetPropertyElement ("Monster_Total_Damage",0.0d);
		sObject.SetPropertyElement ("Monster_Total_AttackedDamage",0.0d);
		sObject.SetPropertyElement ("Monster_Total_AttackedCount",0.0d);
		sObject.SetPropertyElement ("Monster_Total_AttackCount",0.0d);
		sObject.SetPropertyElement ("Monster_Total_Defense",0.0d);

		sObject.SetPropertyElement ("coolTime_Skill_Start_1", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_2", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_3", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_4", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_5", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_6", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_7", (double)Time.time);
		sObject.SetPropertyElement ("Monster_Avoid_Time", (double)Time.time);

		sObject.SetPropertyElement ("CurHp", sObject.GetPropertyElement ("CON")*50);
		sObject.SetPropertyElement ("Monster_Damage",sObject.GetPropertyElement("Strength"));
		sObject.SetPropertyElement ("Monster_Moving_Speed", sObject.GetPropertyElement ("Moving_Speed") / 10);
		sObject.SetPropertyElement ("Monster_Attack_Speed", 4.0d - (sObject.GetPropertyElement ("Attack_Speed") / 10));
		sObject.SetPropertyElement ("Monster_Defensive",sObject.GetPropertyElement("Defensive"));
		sObject.SetPropertyElement ("Monster_Critical",sObject.GetPropertyElement("Critical"));
		sObject.SetPropertyElement ("Monster_Range",3.0d + (sObject.GetPropertyElement("Range")/6));
		sObject.SetPropertyElement ("Monster_Sight",sObject.GetPropertyElement("Monster_Range")*2);
		sObject.SetPropertyElement ("HP",sObject.GetPropertyElement("CON")*50);
		
		setTotalHP((float)sObject.GetPropertyElement("HP"));
		curDirection = 3;
		isSkill = false;
		
	}
	
	void Update () 
	{
		if (sObject.GetPropertyElement ("End") == 100)
			return;
		
		realHealthBar = 50 * NowHealth / TotalHealth;
		pos = cam.camera.WorldToScreenPoint (transform.position);
		left = pos.x;
		top = Screen.height - pos.y;
		
		
		if (sObject.GetPropertyElement ("State") == 3)
			Monster_defense ();
		else if (sObject.GetPropertyElement ("State") == 1)
			Monster_move ();
		else if(sObject.GetPropertyElement ("State") == 2)
			Monster_attack();
		else if(sObject.GetPropertyElement ("State") == 4)
			Monster_avoid();
		if (sObject.GetPropertyElement ("Skill_number") == 1 && !isSkill) 
			StartCoroutine(setSkill_Strength());
		else if(sObject.GetPropertyElement ("Skill_number") == 2 && !isSkill) 
			StartCoroutine(setSkill_Attack_Speed());
		else if(sObject.GetPropertyElement ("Skill_number") == 3 && !isSkill) 
			StartCoroutine(setSkill_Moving_Speed());
		else if(sObject.GetPropertyElement ("Skill_number") == 4 && !isSkill) 
			StartCoroutine(setSkill_Critical());
		else if(sObject.GetPropertyElement ("Skill_number") == 5 && !isSkill) 
			StartCoroutine(setSkill_Defensive());
		else if(sObject.GetPropertyElement ("Skill_number") == 6 && !isSkill) 
			StartCoroutine(setSkill_CON());
		else if(sObject.GetPropertyElement ("Skill_number") == 7 && !isSkill) 
			StartCoroutine(setSkill_Range());
		
	}
	
	public System.Object PropertyUpdate(SIMONObject a, SIMONObject[] b)
	{
		GameObject myObject;
		myObject = GameObject.Find (a.ObjectID);
		int count = 0, count_H1 = 0, count_H2 = 0, count_H3=0,count_H4=0;
		int count_E1 = 0, count_E2 = 0, count_E3=0,count_E4=0;
		double positionX = 0.0d;
		double positionY = 0.0d;
		double myPositionX = myObject.transform.position.x;
		double myPositionY = myObject.transform.position.y;
		double disX, disY;
		Vector2 p;
		float distance;
		if(a.GetPropertyElement("Type") == 2)
		{
			if (myObject.GetComponent<SIMON_Controller>().animator.GetCurrentAnimatorStateInfo (0).IsTag ("Stay")) 
			{
				a.SetPropertyElement("isCheck",1);
			}
		}
		else
		{
			if (myObject.GetComponent<UsualAI_Controller>().animator.GetCurrentAnimatorStateInfo (0).IsTag ("Stay")) 
			{
				a.SetPropertyElement("isCheck",1);
			}
		}
		a.SetPropertyElement ("PositionX", (double)myObject.transform.position.x);
		a.SetPropertyElement ("PositionY", (double)myObject.transform.position.y);
		
		for(int i = 0; i < b.Length; i++)
		{
			if(b[i].GetPropertyElement ("CurHp") <= 0)
			{
				continue;
			}
			
			if(b[i].GetPropertyElement ("Attack_Target")== a.GetPropertyElement("ID"))
			{
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
					count++;
			}
			positionX = b[i].GetPropertyElement ("PositionX");
			positionY = b[i].GetPropertyElement ("PositionY");
			p = new Vector2((float)positionX,(float)positionY);
			
			distance = Vector2.Distance(p,myObject.transform.position);
			
			if(distance < a.GetPropertyElement("Monster_Sight"))
			{
				disX = positionX - myPositionX;
				disY = positionY - myPositionY;
				if(disX > 0 && disY > 0)
				{
					if(disY - disX > 0)
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H1++;
						else
							count_E1++;
					}
					else
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H4++;
						else
							count_E4++;
					}
					
				}
				else if(disX < 0 && disY > 0)
				{
					if(disY - (disX * -1.0d) > 0)
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H1++;
						else
							count_E1++;
					}
					else
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H2++;
						else
							count_E2++;
					}
				}
				else if(disX < 0 && disY < 0)
				{
					if((disY * -1.0d) - (disX * -1.0d) > 0)
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H3++;
						else
							count_E3++;
					}
					else
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H2++;
						else
							count_E2++;
					}
				}
				else if(disX > 0 && disY < 0)
				{
					if((disY * -1.0d) - disX > 0)
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H3++;
						else
							count_E3++;
					}
					else
					{
						if(a.GetPropertyElement("Type") == b[i].GetPropertyElement("Type"))
							count_H4++;
						else
							count_E4++;
					}
				}
			}
		}
		
		a.SetPropertyElement ("AttackedCount", (double)count);
		a.SetPropertyElement ("Monster_Enemy_1", (double)count_E1);
		a.SetPropertyElement ("Monster_Enemy_2", (double)count_E2);
		a.SetPropertyElement ("Monster_Enemy_3", (double)count_E3);
		a.SetPropertyElement ("Monster_Enemy_4", (double)count_E4);
		a.SetPropertyElement ("Monster_Home_1", (double)count_H1);
		a.SetPropertyElement ("Monster_Home_2", (double)count_H2);
		a.SetPropertyElement ("Monster_Home_3", (double)count_H3);
		a.SetPropertyElement ("Monster_Home_4", (double)count_H4);
		
		
		
		return -1000000.0d;
	}

	
	public System.Object ActionFunction_Skill_Strength(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double hp =a.GetPropertyElement("CurHp");
		double damage = a.GetPropertyElement ("Monster_Damage");
		double attack_Speed = a.GetPropertyElement ("Monster_Attack_Speed");
		double tempValue = 0.0d;
		double geneA, geneB, geneC , geneD;
		double pointX = a.GetPropertyElement ("PositionX");
		double pointY = a.GetPropertyElement ("PositionY");
		Vector2 a_v;
		
		GameObject Target_Temp;
		if (a.GetPropertyElement ("Strength") < 20)
			return MaxValue;
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_1")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_1",(double)Time.time);
				MaxValue = 1000000.0d;
				
			}
			return MaxValue;
		}
		
		if(SimulationType == 1)
		{
			geneA = -29.0d;
			geneB = 18.0d;
			geneC = 23.0d;
			geneD = 192.0d;
		}
		else
		{
			geneA = a.GetActionObject ("Skill_Strength").FindWeight ("Strength_Gene_A");
			geneB = a.GetActionObject ("Skill_Strength").FindWeight ("Strength_Gene_B");
			geneC = a.GetActionObject ("Skill_Strength").FindWeight ("Strength_Gene_C");
			geneD = a.GetActionObject ("Skill_Strength").FindWeight ("Strength_Gene_D");
		}
		
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_1")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_1",(double)Time.time);
			
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					
					Target_Temp = GameObject.Find (b[i].ObjectID);
					a_v= new Vector2((float)pointX,(float)pointY);
					dis = Vector2.Distance (a_v, Target_Temp.transform.position);
					tempValue =((scoreConversion(-1.0d * b[i].GetPropertyElement("HP"),b[i].GetPropertyElement("CurHp"))*geneA)+
					            (scoreConversion(25.0d,b[i].GetPropertyElement("AttackedCount"))*geneB)+
					            (scoreConversion(-25.0d,a.GetPropertyElement("AttackedCount"))*geneC)+
					            (scoreConversion(-19.0d,dis)*geneD)
					            )*2.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Skill_Attack_Speed(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double hp =a.GetPropertyElement("CurHp");
		double damage = a.GetPropertyElement ("Monster_Damage");
		double attack_Speed = a.GetPropertyElement ("Monster_Attack_Speed");
		double tempValue = 0.0d;
		double geneA, geneB, geneC, geneD;
		double pointX = a.GetPropertyElement ("PositionX");
		double pointY = a.GetPropertyElement ("PositionY");
		Vector2 a_v;
		
		GameObject Target_Temp;
		
		if (a.GetPropertyElement ("Attack_Speed") < 20)
			return MaxValue;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_2")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_2",(double)Time.time);
				MaxValue = 1000000.0d;
				
			}
			return MaxValue;
		}
		
		if(SimulationType == 1)
		{
			geneA = 3.0d;
			geneB = -0.05d;
			geneC = -4.0d;
			geneD = -6.0d;
		}
		else
		{
			geneA = a.GetActionObject ("Skill_Attack_Speed"  ).FindWeight ("Attack_Speed_Gene_A");
			geneB = a.GetActionObject ("Skill_Attack_Speed"  ).FindWeight ("Attack_Speed_Gene_B");
			geneC = a.GetActionObject ("Skill_Attack_Speed"  ).FindWeight ("Attack_Speed_Gene_C");
			geneD = a.GetActionObject ("Skill_Attack_Speed"  ).FindWeight ("Attack_Speed_Gene_D");
		}

		
		
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_2")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_2",(double)Time.time);
			
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					Target_Temp = GameObject.Find (b[i].ObjectID);
					a_v= new Vector2((float)pointX,(float)pointY);
					dis = Vector2.Distance (a_v, Target_Temp.transform.position);
					tempValue =((scoreConversion(-1.0d * b[i].GetPropertyElement("HP"),b[i].GetPropertyElement("CurHp"))*geneA)+
					            (scoreConversion(25.0d,b[i].GetPropertyElement("AttackedCount"))*geneB)+
					            (scoreConversion(-25.0d,a.GetPropertyElement("AttackedCount"))*geneC)+
					            (scoreConversion(-19.0d,dis)*geneD)
					            )*2.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Skill_Moving_Speed(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double hp =a.GetPropertyElement("CurHp");
		double total_hp = a.GetPropertyElement ("HP");
		double attackedCount = a.GetPropertyElement ("AttackedCount");
		double Total_Home = a.GetPropertyElement("Monster_Home_1")+ a.GetPropertyElement("Monster_Home_2") +
			a.GetPropertyElement("Monster_Home_3")+a.GetPropertyElement("Monster_Home_4");
		double Total_Enemy = a.GetPropertyElement("Monster_Enemy_1")+ a.GetPropertyElement("Monster_Enemy_2") +
			a.GetPropertyElement("Monster_Enemy_3")+a.GetPropertyElement("Monster_Enemy_4");
		
		double tempValue = 0.0d;
		double geneA, geneB, geneC , geneD;
		
		if (a.GetPropertyElement ("Moving_Speed") < 20)
			return MaxValue;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_3")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_3",(double)Time.time);
				MaxValue = 1000000.0d;
			}
			return MaxValue;
		}
		
		
		if(SimulationType == 1)
		{
			geneA = 0.08d;
			geneB = -41.0d;
			geneC = 0.66d;
			geneD = -105.0d;
		}
		else
		{
			geneA = a.GetActionObject("Skill_Moving_Speed"  ).FindWeight ("Moving_Speed_Gene_A");
			geneB = a.GetActionObject("Skill_Moving_Speed"  ).FindWeight ("Moving_Speed_Gene_B");
			geneC = a.GetActionObject("Skill_Moving_Speed"  ).FindWeight ("Moving_Speed_Gene_C");
			geneD = a.GetActionObject("Skill_Moving_Speed"  ).FindWeight ("Moving_Speed_Gene_D");
		}
		
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_3")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_3",(double)Time.time);
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					tempValue =((scoreConversion(-1.0d * total_hp,hp)*geneA)+
					            (scoreConversion(-25.0d,Total_Home)*geneB)+
					            (scoreConversion(25.0d,Total_Enemy)*geneD)+
					            (scoreConversion(25.0d,attackedCount)*geneC)
					            )*2.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Skill_Critical(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double hp =a.GetPropertyElement("CurHp");
		double damage = a.GetPropertyElement ("Monster_Damage");
		double attack_Speed = a.GetPropertyElement ("Monster_Attack_Speed");
		double tempValue = 0.0d;
		double geneA, geneB, geneC ,geneD;
		double pointX = a.GetPropertyElement ("PositionX");
		double pointY = a.GetPropertyElement ("PositionY");
		Vector2 a_v;
		
		GameObject Target_Temp;
		
		if (a.GetPropertyElement ("Critical") < 20)
			return MaxValue;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_4")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_4",(double)Time.time);
				MaxValue = 1000000.0d;
			}
			return MaxValue;
		}
		if(SimulationType == 1)
		{
			geneA = -2.0d;
			geneB = 195.0d;
			geneC = 43.0d;
			geneD = -3.0d;
		}
		else
		{
			geneA =	a.GetActionObject("Skill_Critical"  ).FindWeight ("Critical_Gene_A");
			geneB =	a.GetActionObject("Skill_Critical"  ).FindWeight ("Critical_Gene_B");
			geneC =	a.GetActionObject("Skill_Critical"  ).FindWeight ("Critical_Gene_C");
			geneD = a.GetActionObject("Skill_Critical"  ).FindWeight ("Critical_Gene_D");
		}

		
		
	
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_4")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_4",(double)Time.time);
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					
					Target_Temp = GameObject.Find (b[i].ObjectID);
					a_v= new Vector2((float)pointX,(float)pointY);
					dis = Vector2.Distance (a_v, Target_Temp.transform.position);
					tempValue =((scoreConversion(-1.0d * b[i].GetPropertyElement("HP"),b[i].GetPropertyElement("CurHp"))*geneA)+
					            (scoreConversion(25.0d,b[i].GetPropertyElement("AttackedCount"))*geneB)+
					            (scoreConversion(-25.0d,a.GetPropertyElement("AttackedCount"))*geneC)+
					            (scoreConversion(-19.0d,dis)*geneD)
					            )*2.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Skill_Defensive(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double tempValue = 0.0d;	
		double hp =a.GetPropertyElement("CurHp");
		double total_hp = a.GetPropertyElement ("HP");
		double attackedCount = a.GetPropertyElement ("AttackedCount");
		
		double Total_Home = a.GetPropertyElement("Monster_Home_1")+ a.GetPropertyElement("Monster_Home_2") +
			a.GetPropertyElement("Monster_Home_3")+a.GetPropertyElement("Monster_Home_4");
		
		double Total_Enemy = a.GetPropertyElement("Monster_Enemy_1")+ a.GetPropertyElement("Monster_Enemy_2") +
			a.GetPropertyElement("Monster_Enemy_3")+a.GetPropertyElement("Monster_Enemy_4");
		
		double geneA, geneB, geneC , geneD;


		if (a.GetPropertyElement ("Defensive") < 20)
			return MaxValue;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_5")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_5",(double)Time.time);
				MaxValue = 1000000.0d;
			}
			return MaxValue;
		}
		
		
		
		if(SimulationType == 1)
		{
			geneA = 400.0d;
			geneB = -1.0d;
			geneC = 6.0d;
			geneD = 5.0d;
		}
		else
		{
			geneA =	a.GetActionObject("Skill_Defensive"  ).FindWeight ("Defensive_Gene_A");
			geneB =	a.GetActionObject("Skill_Defensive"  ).FindWeight ("Defensive_Gene_B");
			geneC =	a.GetActionObject("Skill_Defensive"  ).FindWeight ("Defensive_Gene_C");
			geneD =	a.GetActionObject("Skill_Defensive"  ).FindWeight ("Defensive_Gene_D");
		}
		
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_5")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_5",(double)Time.time);
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					tempValue =((scoreConversion(-1.0d * total_hp,hp)*geneA)+
					            (scoreConversion(25.0d,Total_Home)*geneB)+
					            (scoreConversion(25.0d,Total_Enemy)*geneD)+
					            (scoreConversion(25.0d,attackedCount)*geneC)
					            )*2.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Skill_CON(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double tempValue = 0.0d;
		double hp =a.GetPropertyElement("CurHp");
		double total_hp = a.GetPropertyElement ("HP");
		double attackedCount = a.GetPropertyElement ("AttackedCount");

		double geneA, geneB, geneC ,geneD;
		
		if (a.GetPropertyElement ("CON") < 20)
			return MaxValue;
		
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_6")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_6",(double)Time.time);
				MaxValue = 1000000.0d;
			}
			return MaxValue;
		}
		
		
		if(SimulationType == 1)
		{
			geneA = 44.0d;
			geneB = 100.0d;
			geneC = 100.0d;
			geneD = 1.0d;
		}
		else
		{
			geneA = a.GetActionObject("Skill_CON"  ).FindWeight ("CON_Gene_A");
			geneB =	a.GetActionObject("Skill_CON"  ).FindWeight ("CON_Gene_B");
			geneC =	a.GetActionObject("Skill_CON"  ).FindWeight ("CON_Gene_C");
			geneD =	a.GetActionObject("Skill_CON"  ).FindWeight ("CON_Gene_D");
		}
		
		
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_6")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_6",(double)Time.time);
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					tempValue =((scoreConversion(-1.0d * total_hp,hp)*geneA)
					            )*4.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Skill_Range(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double Total_Enemy = a.GetPropertyElement("Monster_Enemy_1")+ a.GetPropertyElement("Monster_Enemy_2") +
			a.GetPropertyElement("Monster_Enemy_3")+a.GetPropertyElement("Monster_Enemy_4");
		double tempValue = 0.0d;
		
		double geneA, geneB, geneC , geneD;
		
		if (a.GetPropertyElement ("Range") < 20)
			return MaxValue;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_7")) && !isSkill) 
			{
				a.SetPropertyElement("coolTime_Skill_Start_7",(double)Time.time);
				MaxValue = 1000000.0d;
			}
			return MaxValue;
		}
		
		
		if(SimulationType == 1)
		{
			geneA = -48.0d;
			geneB = 22.0d;
			geneC = 24.0d;
			geneD = 176.0d;
		}
		else
		{
			geneA = a.GetActionObject("Skill_Range"  ).FindWeight ("Range_Gene_A");
			geneB =	a.GetActionObject("Skill_Range"  ).FindWeight ("Range_Gene_B");
			geneC = a.GetActionObject("Skill_Range"  ).FindWeight ("Range_Gene_C");
			geneD = a.GetActionObject("Skill_Range"  ).FindWeight ("Range_Gene_D");
		}
		
		if (a.GetPropertyElement ("Range") < 20)
			return MaxValue;
		
		if (isSkillCoolTime((float)a.GetPropertyElement("coolTime_Skill_Start_7")) && !isSkill) 
		{
			a.SetPropertyElement("coolTime_Skill_Start_7",(double)Time.time);
			for(int i = 0 ; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					tempValue =((scoreConversion(-25.0d,Total_Enemy)*geneA)
					            )*4.0d;
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
					}
				}
			}
		}
		return MaxValue;
	}
	
	
	
	public System.Object ActionFunction_Avoid(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double tempValue = 0.0d;	
		double hp =a.GetPropertyElement("CurHp");
		double total_hp = a.GetPropertyElement ("HP");
		double attackedCount = a.GetPropertyElement ("AttackedCount");
		
		double Total_Home = a.GetPropertyElement("Monster_Home_1")+ a.GetPropertyElement("Monster_Home_2") +
			a.GetPropertyElement("Monster_Home_3")+a.GetPropertyElement("Monster_Home_4");
		
		double Total_Enemy = a.GetPropertyElement("Monster_Enemy_1")+ a.GetPropertyElement("Monster_Enemy_2") +
			a.GetPropertyElement("Monster_Enemy_3")+a.GetPropertyElement("Monster_Enemy_4");
		
		double geneA, geneB, geneD;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (a.GetPropertyElement ("HP") / 2 > a.GetPropertyElement ("CurHp"))
			{
				MaxValue = Random.Range(100,200);
			}
			
			return MaxValue;
		}
		
		if(SimulationType == 1)
		{
			geneA = -35.0d;
			geneB = -0.4d;
			geneD = -15.0d;
		}
		else
		{
			geneA =	a.GetActionObject("Avoid").FindWeight ("Avoid_Gene_A");
			geneB = 	a.GetActionObject("Avoid").FindWeight ("Avoid_Gene_B");
			geneD = a.GetActionObject("Avoid").FindWeight ("Avoid_Gene_D");
		}

		
		if(a.GetPropertyElement("AttackedCount")==0)
		{
			return MaxValue;
		}
		if (a.GetPropertyElement ("HP") == hp) 
		{
			return MaxValue;
		}
		for (int i = 1; i < b.Length; i++) 
		{
			if(b[i].GetPropertyElement("CurHp") <=0)
				continue;
			if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
			{
				tempValue =((scoreConversion(-1.0d * total_hp,hp)*geneA)+
				            (scoreConversion(-25.0d,Total_Home)*geneB)+
				            (scoreConversion(25.0d,Total_Enemy)*geneD)
				            );
				if(MaxValue < tempValue)
				{
					MaxValue = tempValue;
				}
			}
		}
		return MaxValue;
	}
	
	public System.Object ActionFunction_Defense(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue= -1000000.0d;
		double tempValue = 0.0d;	
		double hp =a.GetPropertyElement("CurHp");
		double total_hp = a.GetPropertyElement ("HP");
		double attackedCount = a.GetPropertyElement ("AttackedCount");
		
		double Total_Home = a.GetPropertyElement("Monster_Home_1")+ a.GetPropertyElement("Monster_Home_2") +
			a.GetPropertyElement("Monster_Home_3")+a.GetPropertyElement("Monster_Home_4");
		
		double Total_Enemy = a.GetPropertyElement("Monster_Enemy_1")+ a.GetPropertyElement("Monster_Enemy_2") +
			a.GetPropertyElement("Monster_Enemy_3")+a.GetPropertyElement("Monster_Enemy_4");
		
		double geneA, geneB, geneD;
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			if (a.GetPropertyElement ("HP") / 2 > a.GetPropertyElement ("CurHp"))
			{
				MaxValue = Random.Range(100,200);
			}
			
			return MaxValue;
		}
		
		
		if(SimulationType == 1)
		{
			geneA = 80.0d;
			geneB = -17.0d;
			geneD = -9.0d;
		}
		else
		{
			geneA =	a.GetActionObject("Defense").FindWeight ("Defense_Gene_A");
			geneB = a.GetActionObject("Defense").FindWeight ("Defense_Gene_B");
			geneD = a.GetActionObject("Defense").FindWeight ("Defense_Gene_D");
		}
		
		if (a.GetPropertyElement ("AttackedCount") == 0) 
		{
			return MaxValue;
		}
		if (a.GetPropertyElement ("HP") == hp) 
		{
			return MaxValue;
		}
		for(int i = 0 ; i < b.Length ; i++)
		{
			if(b[i].GetPropertyElement("CurHp") <=0)
				continue;
			if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
			{
				tempValue =((scoreConversion(-1.0d * total_hp,hp)*geneA)+
				            (scoreConversion(25.0d,Total_Home)*geneB)+
				            (scoreConversion(25.0d,Total_Enemy)*geneD)
				            );
				if(MaxValue < tempValue)
				{
					MaxValue = tempValue;
				}
			}
		}
		return MaxValue;
	}
	
	
	
	public System.Object ActionFunction_Move(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue = -1000000.0d;
		double tempValue = 0.0d;
		double hp =a.GetPropertyElement("CurHp");
		double damage = a.GetPropertyElement ("Monster_Damage");
		double attack_Speed = a.GetPropertyElement ("Monster_Attack_Speed");
		double attackedCount = a.GetPropertyElement ("AttackedCount");
		double defensive = a.GetPropertyElement("Defensive");
		double total_hp = a.GetPropertyElement ("HP");
		
		double geneA, geneC, geneD;
		
		
		if (a.GetPropertyElement ("Type") == 1) 
		{
			for(int i = 0; i < b.Length ; i++)
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					tempValue = 100000.0d - b[i].GetPropertyElement("CurHp");
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
						a.SetPropertyElement("Move_Target",b[i].GetPropertyElement("ID"));
					}
				}
			}
			
			return MaxValue;
		}

		if(SimulationType == 1)
		{
			geneA = -0.5d;
			geneC = 0.0d;
			geneD = 4.0d;
		}
		else
		{
			geneA =	a.GetActionObject("Move").FindWeight ("Move_Gene_A");
			geneC =	a.GetActionObject("Move").FindWeight ("Move_Gene_C");
			geneD = a.GetActionObject("Move").FindWeight ("Move_Gene_D");
		}
		
		double pointX = a.GetPropertyElement ("PositionX");
		double pointY = a.GetPropertyElement ("PositionY");
		Vector2 a_v;
		
		GameObject Target_Temp;
		
		for(int i = 0; i < b.Length ; i++)
		{
			if(b[i].GetPropertyElement("CurHp") <=0)
				continue;
			if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
			{
				Target_Temp = GameObject.Find (b[i].ObjectID);
				a_v= new Vector2((float)pointX,(float)pointY);
				dis = Vector2.Distance (a_v, Target_Temp.transform.position);
				tempValue =((scoreConversion(-1.0d * b[i].GetPropertyElement("HP"),b[i].GetPropertyElement("CurHp"))*geneA)+
				            (scoreConversion(-25.0d,a.GetPropertyElement("AttackedCount"))*geneC)+
				            (scoreConversion(19.0d,dis)*geneD)
				            );
				if(MaxValue < tempValue)
				{
					MaxValue = tempValue;
					a.SetPropertyElement("Move_Target",b[i].GetPropertyElement("ID"));
				}
			}
		}
		
		return MaxValue;
	}
	public System.Object ActionFunction_Attack(SIMONObject a, SIMONObject[] b)
	{
		double MaxValue = -1000000.0d;
		double tempValue = 0.0d;
		double hp =a.GetPropertyElement("CurHp");
		double damage = a.GetPropertyElement ("Monster_Damage");
		double attack_Speed = a.GetPropertyElement ("Monster_Attack_Speed");
		double attackedCount = a.GetPropertyElement ("AttackedCount");
		double defensive = a.GetPropertyElement("Defensive");
		double total_hp = a.GetPropertyElement ("HP");
		
		double geneA, geneC, geneD;
		double pointX = a.GetPropertyElement ("PositionX");
		double pointY = a.GetPropertyElement ("PositionY");
		Vector2 a_v;
		
		GameObject Target_Temp;
		if (a.GetPropertyElement ("Type") == 1) 
		{
			for (int i = 0; i < b.Length; i++) 
			{
				if(b[i].GetPropertyElement("CurHp") <=0)
					continue;
				if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
				{
					Target_Temp = GameObject.Find (b[i].ObjectID);
					a_v= new Vector2((float)pointX,(float)pointY);
					dis = Vector2.Distance (a_v, Target_Temp.transform.position);
					if (dis < a.GetPropertyElement("Monster_Range")) 
					{
						tempValue = 100000.0d - b[i].GetPropertyElement("CurHp")+100000;
						if(MaxValue < tempValue)
						{
							MaxValue = tempValue;
							a.SetPropertyElement("Attack_Target",b[i].GetPropertyElement("ID"));
							a.SetPropertyElement("Monster_Enemy_Defensive",b[i].GetPropertyElement("Monster_Defensive"));
							a.SetPropertyElement("Monster_Enemy_CurHp",b[i].GetPropertyElement("CurHp"));
						}       
					}
				}
			}
			
			return MaxValue;
		}
		
		if(SimulationType == 1)
		{
			geneA = 192.0d;
			geneC = 20.0d;
			geneD = 2.4d;
		}
		else
		{
			geneA = a.GetActionObject("Attack").FindWeight ("Attack_Gene_A");
			geneC = a.GetActionObject("Attack").FindWeight ("Attack_Gene_C");
			geneD = a.GetActionObject("Attack").FindWeight ("Attack_Gene_D");
		}
		
	
		for (int i = 0; i < b.Length; i++) 
		{
			if(b[i].GetPropertyElement("CurHp") <=0)
				continue;
			if(a.GetPropertyElement("Type") != b[i].GetPropertyElement("Type"))
			{
				Target_Temp = GameObject.Find (b[i].ObjectID);
				a_v= new Vector2((float)pointX,(float)pointY);
				dis = Vector2.Distance (a_v, Target_Temp.transform.position);
				if (dis < a.GetPropertyElement("Monster_Range")) 
				{
					tempValue =((scoreConversion(-1.0d * b[i].GetPropertyElement("HP"),b[i].GetPropertyElement("CurHp"))*geneA)+
					            (scoreConversion(-25.0d,a.GetPropertyElement("AttackedCount"))*geneC)+
					            (scoreConversion(-19.0d,dis)*geneD)
					            );
					if(MaxValue < tempValue)
					{
						MaxValue = tempValue;
						a.SetPropertyElement("Attack_Target",b[i].GetPropertyElement("ID"));
						a.SetPropertyElement("Monster_Enemy_Defensive",b[i].GetPropertyElement("Monster_Defensive"));
						a.SetPropertyElement("Monster_Enemy_CurHp",b[i].GetPropertyElement("CurHp"));
					}       
				}
			}
		}
		
		return MaxValue;
		
		
	}
	//-->
	IEnumerator setSkill_Strength()
	{
		isSkill = true;
		sObject.SetPropertyElement("Monster_Damage",sObject.GetPropertyElement("Monster_Damage")*2);
		yield return new WaitForSeconds (10.0f); 
		sObject.SetPropertyElement("Monster_Damage",sObject.GetPropertyElement("Monster_Damage")/2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill = false;
	}
	
	IEnumerator setSkill_Attack_Speed()
	{
		SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "Attack_Speed");
		isSkill = true;
		sObject.SetPropertyElement("Monster_Attack_Speed",sObject.GetPropertyElement("Monster_Attack_Speed")/2);
		yield return new WaitForSeconds (10.0f); 
		sObject.SetPropertyElement("Monster_Attack_Speed",sObject.GetPropertyElement("Monster_Attack_Speed")*2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill = false;
		
	}
	
	IEnumerator setSkill_Moving_Speed()
	{
		SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "Moving_Speed");
		isSkill = true;
		sObject.SetPropertyElement("Monster_Moving_Speed",sObject.GetPropertyElement("Monster_Moving_Speed")*2);
		yield return new WaitForSeconds (10.0f); 
		sObject.SetPropertyElement("Monster_Moving_Speed",sObject.GetPropertyElement("Monster_Moving_Speed")/2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill = false;
		
	}
	
	IEnumerator setSkill_Critical()
	{
		isSkill= true;
		sObject.SetPropertyElement("Monster_Critical",sObject.GetPropertyElement("Monster_Critical")*2);
		yield return new WaitForSeconds(10.0f); 
		sObject.SetPropertyElement("Monster_Critical",sObject.GetPropertyElement("Monster_Critical")/2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill= false;
		
	}
	
	IEnumerator setSkill_Defensive()
	{
		SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "Defensive");
		isSkill= true;
		sObject.SetPropertyElement("Monster_Defensive",sObject.GetPropertyElement("Monster_Defensive")*2);
		yield return new WaitForSeconds(10.0f); 
		sObject.SetPropertyElement("Monster_Defensive",sObject.GetPropertyElement("Monster_Defensive")/2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill= false;
		
	}
	
	IEnumerator setSkill_CON()
	{
		SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "CON");
		isSkill= true;
		sObject.SetPropertyElement("Skill_number",0.0d);
		if(sObject.GetPropertyElement("CurHp")+(sObject.GetPropertyElement("HP")/10) > sObject.GetPropertyElement("HP"))
		{
			sObject.SetPropertyElement("CurHp",sObject.GetPropertyElement ("HP"));
			NowHealth = (float)sObject.GetPropertyElement ("HP");
		}
		else
		{
			sObject.SetPropertyElement("CurHp",sObject.GetPropertyElement("CurHp") + (sObject.GetPropertyElement ("HP")/10));
			NowHealth =(float)sObject.GetPropertyElement("CurHp") + (float)(sObject.GetPropertyElement ("HP")/10);
		}
		
		yield return new WaitForSeconds(1.0f); 
		isSkill= false;
		
	}
	
	IEnumerator setSkill_Range()
	{	
		SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "Range");
		isSkill= true;
		sObject.SetPropertyElement("Monster_Range",sObject.GetPropertyElement("Monster_Range")*2);
		yield return new WaitForSeconds(10.0f); 
		sObject.SetPropertyElement("Monster_Range",sObject.GetPropertyElement("Monster_Range")/2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill= false;
	}

	IEnumerator set_Avoid()
	{	
		sObject.SetPropertyElement("Monster_Moving_Speed",sObject.GetPropertyElement("Monster_Moving_Speed")*2.0d);
		yield return new WaitForSeconds(1.0f); 
		sObject.SetPropertyElement("Monster_Moving_Speed",sObject.GetPropertyElement("Monster_Moving_Speed")/2.0d);
	}

	
	public Object monster_Skill_Strength(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 1);
		return null;
	}
	
	public Object monster_Skill_Attack_Speed(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 2);
		return null;
	}
	
	public Object monster_Skill_Moving_Speed(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 3);
		return null;
	}
	
	public Object monster_Skill_Critical(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 4);
		return null;
	}
	
	public Object monster_Skill_Defensive(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 5);
		return null;
	}
	
	public Object monster_Skill_CON(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 6);
		return null;
	}
	
	public Object monster_Skill_Range(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("Skill_number", 7);
		return null;
	}
	
	//
	//
	//
	//
	public void Monster_avoid()
	{
		if ((sObject.GetPropertyElement("isCheck") == 1 || sObject.GetPropertyElement("State") == 4 ))
		{
			
			if(Time.time - sObject.GetPropertyElement("Monster_Avoid_Time") > 1.0f)
			{
				StartCoroutine(set_Avoid());
				sObject.SetPropertyElement("Monster_Avoid_Value",sObject.GetPropertyElement("Monster_Avoid_Value") +
				                           scoreConversion(-1.0d * sObject.GetPropertyElement("HP"),sObject.GetPropertyElement("CurHp")));
				int curState = (int)sObject.GetPropertyElement("Monster_Avoid_State");
				sObject.SetPropertyElement("Monster_Avoid_Time",Time.time);
				int value;
				int min= -10000;
				for(int i = 1 ; i < 5 ;i++)
				{
					if(curState == i)
						continue;
					value = (int)sObject.GetPropertyElement("Monster_Enemy_"+i.ToString());
					if(min < value)
					{
						min = value;
						sObject.SetPropertyElement("Monster_Avoid_State",i);
					}
					else if (min == value)
					{
						if(Random.Range(0,2) > 1)
						{
							min = value;
							sObject.SetPropertyElement("Monster_Avoid_State",i);
						}
					}		
				}
			}
			float x = gObj_Monster.transform.position.x;
			float y = gObj_Monster.transform.position.y;
			if(sObject.GetPropertyElement("Monster_Avoid_State") == 1)
			{
				if(y > -4 )
					y-=1;
				else
				{
					if(x < 8 )
						x+=1;
					else
						x-=1;
				}
			}
			else if(sObject.GetPropertyElement("Monster_Avoid_State") == 2)
			{
				if(x < 8 )
					x+=1;
				else
				{
					if(y > -4 )
						y-=1;
					else
						y+=1;
				}
			}
			else if(sObject.GetPropertyElement("Monster_Avoid_State") == 3)
			{
				if(y <3)
					y+=1;
				else
				{
					if(x > -8.0)
						x-=1;
					else
						x+=1;
				}
				
			}
			else if(sObject.GetPropertyElement("Monster_Avoid_State") == 4)
			{
				if(x > -8.0)
					x-=1;
				else
				{
					if(y <3)
						y+=1;
					else
						y-=1;
				}
				
			}
			Monster_Avoid_Ani (x,y);
			Vector_Target = new Vector3 (x,y,0.0f);
			gObj_Monster.transform.localPosition = Vector3.MoveTowards (
				gObj_Monster.transform.position, 
				Vector_Target, 
				Time.deltaTime * (float)sObject.GetPropertyElement("Monster_Moving_Speed"));
			sObject.SetPropertyElement("isCheck",0);
		}
	}

	
	public Object Monster_avoid(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("State", (double)4.0d);
		a.SetPropertyElement ("Attack_Target", -1.0d);
		a.SetPropertyElement ("Move_Target", -1.0d);
		a.SetPropertyElement ("Target",-1.0d);
		return null;
	}
	
	
	
	
	public void Monster_defense ()
	{
		switch(curDirection)
		{
		case 1:
			animator.Play ("Ani_Defense_Up");
			
			break;
		case 2:
			animator.Play("Ani_Defense_Right");
			break;
		case 3:
			animator.Play ("Ani_Defense_Down");
			break;
		case 4:
			animator.Play ("Ani_Defense_Left");
			break;
		}
	}
	
	
	public Object Monster_defense(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement ("State", (double)3.0d);
		a.SetPropertyElement ("Attack_Target", -1.0d);
		a.SetPropertyElement ("Move_Target", -1.0d);
		a.SetPropertyElement ("Target",-1.0d);
		return null;
	}
	
	public void Monster_move()
	{
		if (sObject.GetPropertyElement("isCheck") == 1 || sObject.GetPropertyElement("State") == 1) 
		{
			gObj_Target_Move = GameObject.Find ("Monster_B_"+sObject.GetPropertyElement("Target").ToString());
			Monster_Move_Ani ();
			Vector_Target = new Vector3 (gObj_Target_Move.transform.position.x,gObj_Target_Move.transform.position.y,0.0f);
			gObj_Monster.transform.localPosition = Vector3.MoveTowards (
				gObj_Monster.transform.position, 
				Vector_Target, 
				Time.deltaTime * (float)sObject.GetPropertyElement("Monster_Moving_Speed"));
			
			sObject.SetPropertyElement("isCheck",0);
		}
	}
	
	public Object Monster_move(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement("State",1.0d);
		a.SetPropertyElement ("Attack_Target", -1.0d);
		a.SetPropertyElement ("Target",a.GetPropertyElement("Move_Target"));
		return null;
	}
	
	public void Monster_attack()
	{
		
		if(Time.time - sObject.GetPropertyElement("coolTime_Attack_Start") > sObject.GetPropertyElement("Monster_Attack_Speed") && 
		   (sObject.GetPropertyElement("isCheck") == 1  || sObject.GetPropertyElement("State") == 1))
		{
			sObject.SetPropertyElement("Monster_Total_AttackCount",sObject.GetPropertyElement("Monster_Total_AttackCount")+1) ;
			sObject.SetPropertyElement("coolTime_Attack_Start",Time.time) ;
			bool check  = false;
			gObj_Target_Attack = GameObject.Find ("Monster_B_"+sObject.GetPropertyElement("Target").ToString());
			if(sObject.GetPropertyElement("Skill_number") == 1)
			{
				SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "Strength");
			}
			else if(sObject.GetPropertyElement("Skill_number")==4)
			{
				SkillManager_Compare.skillManagerVal.startSkill (this.gameObject, "Critical");
			}
			if(Random.Range(0,100) + sObject.GetPropertyElement("Monster_Critical") > 100)
			{
				check = true;
				SkillManager_Compare.skillManagerVal.getCritical(this.gameObject);
				sObject.SetPropertyElement("Monster_Damage",sObject.GetPropertyElement("Monster_Damage")*3);
			}
			
			coolTime_Attack_Start = Time.time;
			curDirection = AnimationControll(gObj_Target_Attack); // Change Player Direction
			switch(curDirection)
			{
			case 1:
				animator.Play ("Ani_Attack_Up");
				
				break;
			case 2:
				animator.Play("Ani_Attack_Right");
				break;
			case 3:
				animator.Play ("Ani_Attack_Down");
				break;
			case 4:
				animator.Play ("Ani_Attack_Left");
				break;
			}
			float distance = Vector3.Distance (gObj_Monster.transform.position, gObj_Target_Attack.transform.position) * 0.4f;
			Vector3 tmpVector = new Vector3(gObj_Target_Attack.transform.position.x,gObj_Target_Attack.transform.position.y + distance, gObj_Target_Attack.transform.position.z);
			
			MovingTarget = (GameObject)Instantiate (mt, tmpVector, gObj_Target_Attack.transform.rotation) as GameObject;
			
			Snowball = (GameObject)Instantiate (sb,gObj_Monster.transform.position,gObj_Monster.transform.rotation) as GameObject;
			Snowball.GetComponent<SpriteRenderer> ().sprite = SnowballSprite;
			
			Snowball.GetComponent<SnowBall_Controller>().setTarget(gObj_Target_Attack.name,(float)sObject.GetPropertyElement("Monster_Damage"));
			Snowball.GetComponent<AudioSource>().Play ();
			Snowball.SendMessage ("Attack",gObj_Target_Attack);
			Snowball.SendMessage ("NormalAttackSetting", MovingTarget);
			
			Destroy(Snowball,3.0f);
			Destroy(MovingTarget,3.0f);
			if (check) 
			{
				sObject.SetPropertyElement("Monster_Damage",sObject.GetPropertyElement("Monster_Damage")/3);
			}
			sObject.SetPropertyElement("Monster_Total_Damage",sObject.GetPropertyElement("Monster_Total_Damage")+
			                           (((sObject.GetPropertyElement("Monster_Damage")-
			   (sObject.GetPropertyElement("Monster_Damage")*(sObject.GetPropertyElement("Monster_Enemy_Defensive")/100)))/
			  sObject.GetPropertyElement("Monster_Enemy_CurHp"))*100));
		}
	}
	
	
	public Object Monster_attack(SIMONObject a, SIMONObject[] b)
	{
		a.SetPropertyElement("State",2.0d);
		a.SetPropertyElement ("Move_Target", -1.0d);
		a.SetPropertyElement("Target",a.GetPropertyElement("Attack_Target"));
		return null;
	}
	
	public void Monster_Avoid_Ani(float x,float y)
	{
		
		curDirection = AnimationControll(x,y); // Change Player Direction
		switch (curDirection) 
		{
		case 1:
			animator.Play ("Ani_Move_Up");
			break;
		case 2:
			animator.Play ("Ani_Move_Right");
			break;
		case 3:
			animator.Play ("Ani_Move_Down");
			break;
		case 4:
			animator.Play ("Ani_Move_Left");
			break;
		}
	}
	
	public void Monster_Move_Ani()
	{
		curDirection = AnimationControll(gObj_Target_Move); // Change Player Direction
		switch (curDirection) 
		{
		case 1:
			animator.Play ("Ani_Move_Up");
			break;
		case 2:
			animator.Play ("Ani_Move_Right");
			break;
		case 3:
			animator.Play ("Ani_Move_Down");
			break;
		case 4:
			animator.Play ("Ani_Move_Left");
			break;
		}
	}
	
	private int AnimationControll(GameObject Target)
	{
		curPositionX = gObj_Monster.transform.position.x;
		curPositionY = gObj_Monster.transform.position.y;
		
		float betweenX = Target.transform.position.x - curPositionX;
		float betweenY = Target.transform.position.y - curPositionY;
		if (betweenX == 0 && betweenY == 0)
			return curDirection;
		// 1사분면 
		if (betweenX >= 0 && betweenY >= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 1;
			else
				return 2;
		}
		// 2사분면 
		else if (betweenX <= 0 && betweenY >= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 1;
			else
				return 4;
		}
		// 3사분면 
		else if (betweenX <= 0 && betweenY <= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 3;
			else
				return 4;
		}
		// 4사분면 
		else if (betweenX >= 0 && betweenY <= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 3;
			else
				return 2;
		}
		return 0;
	}
	
	private int AnimationControll(float x,float y)
	{
		curPositionX = gObj_Monster.transform.position.x;
		curPositionY = gObj_Monster.transform.position.y;
		
		float betweenX = x - curPositionX;
		float betweenY = y - curPositionY;
		if (betweenX == 0 && betweenY == 0)
			return curDirection;
		// 1사분면 
		if (betweenX >= 0 && betweenY >= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 1;
			else
				return 2;
		}
		// 2사분면 
		else if (betweenX <= 0 && betweenY >= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 1;
			else
				return 4;
		}
		// 3사분면 
		else if (betweenX <= 0 && betweenY <= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 3;
			else
				return 4;
		}
		// 4사분면 
		else if (betweenX >= 0 && betweenY <= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
				return 3;
			else
				return 2;
		}
		return 0;
	}
	
	IEnumerator setTwinkleDelay()
	{
		this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
		yield return new WaitForSeconds(0.1f); 
		this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
	}
	
	
	
	
	
	void OnTriggerEnter2D (Collider2D other)
	{
		float damage = other.gameObject.GetComponent<SnowBall_Controller> ().damage;
		if (other.gameObject.GetComponent<SnowBall_Controller>().TargetName.Equals(this.name))
		{
			if(sObject.GetPropertyElement("State") == 3)
			{
				damage /=2;
			}
			if(!getDamage(damage - (damage * (float)(sObject.GetPropertyElement("Defensive")/100))))
			{
				gObj_Monster.transform.position = new Vector3(gObj_Monster.transform.position.x, gObj_Monster.transform.position.y, 2.0f);
				sObject.SetPropertyElement("End",100.0d);
				sObject.SetPropertyElement("CurHp",0.0d);
				animator.Play ("Ani_Tomb_Red");
				Destroy (Snowball);
			}
			else
			{
				StartCoroutine(setTwinkleDelay());
			}
			
			if(sObject.GetPropertyElement("CurHp") - damage+ ((sObject.GetPropertyElement("Defensive")/100)*damage) > 0 )
				sObject.SetPropertyElement("CurHp", sObject.GetPropertyElement("CurHp") - damage+
				                           ((sObject.GetPropertyElement("Defensive")/100)*damage));
			else
			{
				NowHealth = 0.0f;
				sObject.SetPropertyElement("CurHp",0.0d);
			}
			sObject.SetPropertyElement("Monster_Total_AttackedDamage",sObject.GetPropertyElement("Monster_Total_AttackedDamage") + damage - 
			                           ((sObject.GetPropertyElement("Defensive")/100)*damage));
			
			sObject.SetPropertyElement("Monster_Total_Defense",sObject.GetPropertyElement("Monster_Total_Defense")+ ((sObject.GetPropertyElement("Defensive")/100)*damage));

			sObject.SetPropertyElement("Monster_Total_AttackedCount",sObject.GetPropertyElement("Monster_Total_AttackedCount")+1);
		}
	}
	
	double scoreConversion(double max,double score)
	{	
		double resultScore;
		if (max > 0)
			resultScore = 100 - (((max - score) / max) * 100);
		else 
			resultScore = (((max*-1) - score)/(max*-1))*100;
		return resultScore;
	}
	
	
	bool isSkillCoolTime(float coolTime)
	{
		if (Time.time - coolTime > sObject.GetPropertyElement("Monster_Skill_CoolTime")) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
		
	}
}
