using UnityEngine;
using System.Collections;
using SIMONFramework;
using System.Reflection;
public class Monster_Controller_Simulation : MonoBehaviour 
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
	public int[] PropertyValue;
	
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

        if (!NormalGameSceneManager_Simulation.BoostAtResultPage)
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
		if (GameTimeManager_Simulation.GroupA.Count == NormalGameSceneManager_Simulation.Total_Object) 
		{
			for(int i= 0 ; i < GameTimeManager_Simulation.GroupA.Count ; i++)
			{
				if(((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i)).ObjectID.Equals(gObj_Monster.name))
				{
					sObject = ((SIMONObject)GameTimeManager_Simulation.GroupA.ValueOfIndex (i));
					reStart();
					return;
				}
			}
		}
		SimulationType = SceneManager.SimulationType;
		if(SimulationType == 1)
			init ();
		else
			init ((int)Monster_ID);

	
		
	

		sObject = new SIMONObject ();
		sObject.ObjectID = gObj_Monster.name;
		GameTimeManager_Simulation.GroupA.Add (sObject.ObjectID, sObject);

		GameTimeManager_Simulation.GroupC.Add (sObject.ObjectID, sObject);
		



		SIMONProperty Strength = new SIMONProperty ();
		Strength.PropertyName = "Strength";
		Strength.PropertyValue = (double)PropertyValue [0];
	//	Strength.Inherit = true;
		
		SIMONProperty Attack_Speed = new SIMONProperty ();
		Attack_Speed.PropertyName = "Attack_Speed";
		Attack_Speed.PropertyValue = (double)PropertyValue [1];
	//	Attack_Speed.Inherit = true;
		
		
		SIMONProperty Moving_Speed = new SIMONProperty ();
		Moving_Speed.PropertyName = "Moving_Speed";
		Moving_Speed.PropertyValue =(double)PropertyValue [2];
	//	Moving_Speed.Inherit = true;
		
		
		SIMONProperty Critical = new SIMONProperty ();
		Critical.PropertyName = "Critical";
		Critical.PropertyValue = (double)PropertyValue [3];
		//Critical.Inherit = true;
		
		SIMONProperty Defensive = new SIMONProperty ();
		Defensive.PropertyName = "Defensive";
		Defensive.PropertyValue = (double)PropertyValue [4];
		//Defensive.Inherit = true;
		
		SIMONProperty CON = new SIMONProperty ();
		CON.PropertyName = "CON";
		CON.PropertyValue = (double)PropertyValue [5];
	//	CON.Inherit = true;
		
		SIMONProperty Range = new SIMONProperty ();
		Range.PropertyName = "Range";
		Range.PropertyValue =(double)PropertyValue [6];
	//	Range.Inherit = true;

		SIMONProperty HP = new SIMONProperty ();
		HP.PropertyName = "HP";
		HP.PropertyValue = CON.PropertyValue*50.0d;
		
		SIMONProperty CurHp = new SIMONProperty ();
		CurHp.PropertyName = "CurHp";
		CurHp.PropertyValue = CON.PropertyValue*50.0d;
		
		
		SIMONProperty PositionX = new SIMONProperty ();
		PositionX.PropertyName = "PositionX";
		
		SIMONProperty PositionY = new SIMONProperty ();
		PositionY.PropertyName = "PositionY";
		
		SIMONProperty Type = new SIMONProperty ();
		Type.PropertyName = "Type";
		if(gObj_Monster.tag.ToString().Equals("Home"))
			Type.PropertyValue = 2;
		else
			Type.PropertyValue = 1;
		
		SIMONProperty State = new SIMONProperty ();
		State.PropertyName = "State";
		State.PropertyValue = -1.0f;
		
		SIMONProperty Target = new SIMONProperty ();
		Target.PropertyName = "Target";
		Target.PropertyValue = -1.0f;
		
		SIMONProperty ID = new SIMONProperty ();
		ID.PropertyName = "ID";
		ID.PropertyValue = (double)Monster_ID;
		
		SIMONProperty AttackedCount = new SIMONProperty ();
		AttackedCount.PropertyName = "AttackedCount";
		AttackedCount.PropertyValue = 0.0d;
		
		
		SIMONProperty Move_Target = new SIMONProperty ();
		Move_Target.PropertyName = "Move_Target";
		Move_Target.PropertyValue = 0.0d;
		
		SIMONProperty Attack_Target = new SIMONProperty ();
		Attack_Target.PropertyName = "Attack_Target";
		Attack_Target.PropertyValue = 0.0d;
		
		SIMONProperty isCheck = new SIMONProperty ();
		isCheck.PropertyName = "isCheck";
		isCheck.PropertyValue = 0.0d;
		
		SIMONProperty coolTime_Attack_Start = new SIMONProperty ();
		coolTime_Attack_Start.PropertyName = "coolTime_Attack_Start";
		coolTime_Attack_Start.PropertyValue = 0.0d;

		SIMONProperty Monster_Skill_CoolTime = new SIMONProperty ();
		Monster_Skill_CoolTime.PropertyName = "Monster_Skill_CoolTime";
		Monster_Skill_CoolTime.PropertyValue = 60.0d;
		
		SIMONProperty Skill_number = new SIMONProperty ();
		Skill_number.PropertyName = "Skill_number";
		Skill_number.PropertyValue = 0.0d;
		
		SIMONProperty End = new SIMONProperty ();
		End.PropertyName = "End";
		End.PropertyValue = 0.0d;
		
		SIMONProperty Monster_Damage = new SIMONProperty ();
		Monster_Damage.PropertyName = "Monster_Damage";
		Monster_Damage.PropertyValue = Strength.PropertyValue;

		SIMONProperty Monster_Attack_Speed = new SIMONProperty ();
		Monster_Attack_Speed.PropertyName = "Monster_Attack_Speed";
		Monster_Attack_Speed.PropertyValue = 4.0f -(float) Attack_Speed.PropertyValue / 10;

		SIMONProperty Monster_Moving_Speed = new SIMONProperty ();
		Monster_Moving_Speed.PropertyName = "Monster_Moving_Speed";
		Monster_Moving_Speed.PropertyValue =(float)(Moving_Speed.PropertyValue / 20);
		
		SIMONProperty Monster_Range = new SIMONProperty ();
		Monster_Range.PropertyName = "Monster_Range";
		Monster_Range.PropertyValue = 1.0f + (Range.PropertyValue/6);
		
		SIMONProperty Monster_Defensive = new SIMONProperty ();
		Monster_Defensive.PropertyName = "Monster_Defensive";
		Monster_Defensive.PropertyValue = (float)Defensive.PropertyValue;
		
		SIMONProperty Monster_Critical = new SIMONProperty ();
		Monster_Critical.PropertyName = "Monster_Critical";
		Monster_Critical.PropertyValue = (float)Critical.PropertyValue;
		
		SIMONProperty Monster_Sight = new SIMONProperty ();
		Monster_Sight.PropertyName = "Monster_Sight";
		Monster_Sight.PropertyValue = (float)Monster_Range.PropertyValue;
		
		SIMONProperty Monster_Enemy_1 = new SIMONProperty ();
		Monster_Enemy_1.PropertyName = "Monster_Enemy_1";
		Monster_Enemy_1.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Enemy_2 = new SIMONProperty ();
		Monster_Enemy_2.PropertyName = "Monster_Enemy_2";
		Monster_Enemy_2.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Enemy_3 = new SIMONProperty ();
		Monster_Enemy_3.PropertyName = "Monster_Enemy_3";
		Monster_Enemy_3.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Enemy_4 = new SIMONProperty ();
		Monster_Enemy_4.PropertyName = "Monster_Enemy_4";
		Monster_Enemy_4.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Home_1 = new SIMONProperty ();
		Monster_Home_1.PropertyName = "Monster_Home_1";
		Monster_Home_1.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Home_2 = new SIMONProperty ();
		Monster_Home_2.PropertyName = "Monster_Home_2";
		Monster_Home_2.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Home_3 = new SIMONProperty ();
		Monster_Home_3.PropertyName = "Monster_Home_3";
		Monster_Home_3.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Home_4 = new SIMONProperty ();
		Monster_Home_4.PropertyName = "Monster_Home_4";
		Monster_Home_4.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Avoid_State = new SIMONProperty ();
		Monster_Avoid_State.PropertyName = "Monster_Avoid_State";
		Monster_Avoid_State.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Home_Total = new SIMONProperty ();
		Monster_Home_Total.PropertyName = "Monster_Home_Total";
		Monster_Home_Total.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Enemy_Total = new SIMONProperty ();
		Monster_Enemy_Total.PropertyName = "Monster_Enemy_Total";
		Monster_Enemy_Total.PropertyValue = (float)0.0d;
		
		SIMONProperty Monster_Avoid_Time = new SIMONProperty();
		Monster_Avoid_Time.PropertyName = "Monster_Avoid_Time";
		Monster_Avoid_Time.PropertyValue = 0.0d;
		
		SIMONProperty Monster_Total_Damage = new SIMONProperty ();
		Monster_Total_Damage.PropertyName = "Monster_Total_Damage";
		Monster_Total_Damage.PropertyValue = 0.0d;

		SIMONProperty Monster_Total_AttackedDamage = new SIMONProperty ();
		Monster_Total_AttackedDamage.PropertyName = "Monster_Total_AttackedDamage";
		Monster_Total_AttackedDamage.PropertyValue = 0.0d;

		SIMONProperty Monster_Total_AttackedCount = new SIMONProperty ();
		Monster_Total_AttackedCount.PropertyName = "Monster_Total_AttackedCount";
		Monster_Total_AttackedCount.PropertyValue = 0.0d;

		SIMONProperty Monster_Total_AttackCount = new SIMONProperty ();
		Monster_Total_AttackCount.PropertyName = "Monster_Total_AttackCount";
		Monster_Total_AttackCount.PropertyValue = 0.0d;

		SIMONProperty Monster_Total_Defense = new SIMONProperty ();
		Monster_Total_Defense.PropertyName = "Monster_Total_Defense";
		Monster_Total_Defense.PropertyValue = 0.0d;
		
		SIMONProperty Monster_Enemy_Defensive = new SIMONProperty ();
		Monster_Enemy_Defensive.PropertyName = "Monster_Enemy_Defensive";
		Monster_Enemy_Defensive.PropertyValue = 0.0d;

		SIMONProperty Monster_Enemy_CurHp = new SIMONProperty ();
		Monster_Enemy_CurHp.PropertyName = "Monster_Enemy_CurHp";
		Monster_Enemy_CurHp.PropertyValue = 0.0d;

		SIMONProperty Monster_Avoid_Value = new SIMONProperty ();
		Monster_Avoid_Value.PropertyName = "Monster_Avoid_Value";
		Monster_Avoid_Value.PropertyValue = 0.0d;

		SIMONProperty Monster_Strength_Value = new SIMONProperty ();
		Monster_Strength_Value.PropertyName = "Monster_Strength_Value";
		Monster_Strength_Value.PropertyValue = 0.0d;
		
		SIMONProperty Monster_Attack_Speed_Value = new SIMONProperty ();
		Monster_Attack_Speed_Value.PropertyName = "Monster_Attack_Speed_Value";
		Monster_Attack_Speed_Value.PropertyValue = 0.0d;

		SIMONProperty Monster_Moving_Speed_Value = new SIMONProperty ();
		Monster_Moving_Speed_Value.PropertyName = "Monster_Moving_Speed_Value";
		Monster_Moving_Speed_Value.PropertyValue = 0.0d;

		SIMONProperty Monster_Critical_Value = new SIMONProperty ();
		Monster_Critical_Value.PropertyName = "Monster_Critical_Value";
		Monster_Critical_Value.PropertyValue = 0.0d;

		SIMONProperty Monster_Defensive_Value = new SIMONProperty ();
		Monster_Defensive_Value.PropertyName = "Monster_Defensive_Value";
		Monster_Defensive_Value.PropertyValue = 0.0d;

		SIMONProperty Monster_CON_Value = new SIMONProperty ();
		Monster_CON_Value.PropertyName = "Monster_CON_Value";
		Monster_CON_Value.PropertyValue = 0.0d;
		
		SIMONProperty Monster_Range_Value = new SIMONProperty ();
		Monster_Range_Value.PropertyName = "Monster_Range_Value";
		Monster_Range_Value.PropertyValue = 0.0d;

		if (SimulationType == 1) 
		{
			Strength.Inherit = true;
			Moving_Speed.Inherit = true;
			Attack_Speed.Inherit = true;
			Critical.Inherit = true;
			CON.Inherit = true;
			Range.Inherit = true;
			Defensive.Inherit = true;
		}


		sObject.Properties.Add (Strength);
		sObject.Properties.Add (Attack_Speed);
		sObject.Properties.Add (Moving_Speed);
		sObject.Properties.Add (Critical);
		sObject.Properties.Add (Defensive);
		sObject.Properties.Add (CON);
		sObject.Properties.Add (Range);
		sObject.Properties.Add (HP);
		sObject.Properties.Add (CurHp);
		sObject.Properties.Add (Type);
		sObject.Properties.Add (PositionX);
		sObject.Properties.Add (PositionY);
		sObject.Properties.Add (Target);
		sObject.Properties.Add (State);
		sObject.Properties.Add (ID);
		sObject.Properties.Add (AttackedCount);
		sObject.Properties.Add (Move_Target);
		sObject.Properties.Add (Attack_Target);
		sObject.Properties.Add (isCheck);
		sObject.Properties.Add (coolTime_Attack_Start);
		sObject.Properties.Add (Monster_Attack_Speed);
		sObject.Properties.Add (Monster_Skill_CoolTime);
		sObject.Properties.Add (Skill_number);
		sObject.Properties.Add (End);
		sObject.Properties.Add (Monster_Damage);
		sObject.Properties.Add (Monster_Moving_Speed);
		sObject.Properties.Add (Monster_Range);
		sObject.Properties.Add (Monster_Defensive);
		sObject.Properties.Add (Monster_Critical);
		sObject.Properties.Add (Monster_Sight);
		sObject.Properties.Add (Monster_Enemy_1);
		sObject.Properties.Add (Monster_Enemy_2);
		sObject.Properties.Add (Monster_Enemy_3);
		sObject.Properties.Add (Monster_Enemy_4);
		sObject.Properties.Add (Monster_Home_1);
		sObject.Properties.Add (Monster_Home_2);
		sObject.Properties.Add (Monster_Home_3);
		sObject.Properties.Add (Monster_Home_4);
		sObject.Properties.Add (Monster_Avoid_State);
		sObject.Properties.Add (Monster_Enemy_Total);
		sObject.Properties.Add (Monster_Home_Total);
		sObject.Properties.Add (Monster_Avoid_Time);
		sObject.Properties.Add (Monster_Total_Damage);
		sObject.Properties.Add (Monster_Enemy_Defensive);
		sObject.Properties.Add (Monster_Total_AttackedDamage);
		sObject.Properties.Add (Monster_Total_Defense);
		sObject.Properties.Add (Monster_Enemy_CurHp);
		sObject.Properties.Add (Monster_Total_AttackedCount);
		sObject.Properties.Add (Monster_Total_AttackCount);
		sObject.Properties.Add (Monster_Avoid_Value);
		sObject.Properties.Add (Monster_Strength_Value);
		sObject.Properties.Add (Monster_Attack_Speed_Value);
		sObject.Properties.Add (Monster_Moving_Speed_Value);
		sObject.Properties.Add (Monster_Critical_Value);
		sObject.Properties.Add (Monster_Defensive_Value);
		sObject.Properties.Add (Monster_CON_Value);
		sObject.Properties.Add (Monster_Range_Value);

		sObject.ObjectFitnessFunctionName = "ObjectFitness";
		sObject.UpdatePropertyDNA ();

		SIMONAction propertyUpdate_Function = new SIMONAction ();
		propertyUpdate_Function.ActionFunctionName = "propertyUpdate" ;
		propertyUpdate_Function.ExecutionFunctionName = "Execution_propertyUpdate" ;
		propertyUpdate_Function.FitnessFunctionName = "FitnessFunction_propertyUpdate"  ;
		propertyUpdate_Function.ActionName = "Update" ;

		SIMONAction Monster_Move = new SIMONAction ();
		Monster_Move.ActionFunctionName = "ActionFunction_Move" ;
		Monster_Move.ExecutionFunctionName = "Monster_Move" ;
		Monster_Move.FitnessFunctionName = "FitnessFunction_Move"  ;
		Monster_Move.ActionName = "Move" ;

		SIMONAction Monster_Attack = new SIMONAction ();
		Monster_Attack.ActionFunctionName = "ActionFunction_Attack";
		Monster_Attack.ExecutionFunctionName = "Monster_Attack" ;
		Monster_Attack.FitnessFunctionName = "FitnessFunction_Attack"  ;
		Monster_Attack.ActionName = "Attack" ;
	
		SIMONAction Monster_Defense = new SIMONAction ();
		Monster_Defense.ActionFunctionName = "ActionFunction_Defense" ;
		Monster_Defense.ExecutionFunctionName = "Monster_Defense" ;
		Monster_Defense.FitnessFunctionName = "FitnessFunction_Defense"  ;
		Monster_Defense.ActionName = "Defense" ;

		SIMONAction Monster_Avoid = new SIMONAction ();
		Monster_Avoid.ActionFunctionName = "ActionFunction_Avoid";
		Monster_Avoid.ExecutionFunctionName = "Monster_Avoid";
		Monster_Avoid.FitnessFunctionName = "FitnessFunction_Avoid";
		Monster_Avoid.ActionName = "Avoid";
		SIMONAction Monster_Skill_Strength = new SIMONAction ();
		Monster_Skill_Strength.ActionFunctionName = "ActionFunction_Skill_Strength"  ;
		Monster_Skill_Strength.ExecutionFunctionName = "Monster_Skill_Strength"  ;
		Monster_Skill_Strength.FitnessFunctionName = "FitnessFunction_Skill_Strength"  ;
		Monster_Skill_Strength.ActionName = "Skill_Strength"  ;
		
		SIMONProperty coolTime_Skill_Start_1 = new SIMONProperty ();
		coolTime_Skill_Start_1.PropertyName = "coolTime_Skill_Start_1";
		coolTime_Skill_Start_1.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_1);
		
		SIMONAction Monster_Skill_Attack_Speed = new SIMONAction ();
		Monster_Skill_Attack_Speed.ActionFunctionName = "ActionFunction_Skill_Attack_Speed"  ;
		Monster_Skill_Attack_Speed.ExecutionFunctionName = "Monster_Skill_Attack_Speed"  ;
		Monster_Skill_Attack_Speed.FitnessFunctionName = "FitnessFunction_Skill_Attack_Speed"  ;
		Monster_Skill_Attack_Speed.ActionName = "Skill_Attack_Speed"  ;
		
		SIMONProperty coolTime_Skill_Start_2 = new SIMONProperty ();
		coolTime_Skill_Start_2.PropertyName = "coolTime_Skill_Start_2";
		coolTime_Skill_Start_2.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_2);
		
		SIMONAction Monster_Skill_Moving_Speed = new SIMONAction ();
		Monster_Skill_Moving_Speed.ActionFunctionName = "ActionFunction_Skill_Moving_Speed"  ;
		Monster_Skill_Moving_Speed.ExecutionFunctionName = "Monster_Skill_Moving_Speed"  ;
		Monster_Skill_Moving_Speed.FitnessFunctionName = "FitnessFunction_Skill_Moving_Speed"  ;
		Monster_Skill_Moving_Speed.ActionName = "Skill_Moving_Speed"  ;
		
		SIMONProperty coolTime_Skill_Start_3 = new SIMONProperty ();
		coolTime_Skill_Start_3.PropertyName = "coolTime_Skill_Start_3";
		coolTime_Skill_Start_3.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_3);
		
		SIMONAction Monster_Skill_Critical = new SIMONAction ();
		Monster_Skill_Critical.ActionFunctionName = "ActionFunction_Skill_Critical"  ;
		Monster_Skill_Critical.ExecutionFunctionName = "Monster_Skill_Critical"  ;
		Monster_Skill_Critical.FitnessFunctionName = "FitnessFunction_Skill_Critical"  ;
		Monster_Skill_Critical.ActionName = "Skill_Critical"  ;
		
		SIMONProperty coolTime_Skill_Start_4 = new SIMONProperty ();
		coolTime_Skill_Start_4.PropertyName = "coolTime_Skill_Start_4";
		coolTime_Skill_Start_4.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_4);
		
		SIMONAction Monster_Skill_Defensive = new SIMONAction ();
		Monster_Skill_Defensive.ActionFunctionName = "ActionFunction_Skill_Defensive"  ;
		Monster_Skill_Defensive.ExecutionFunctionName = "Monster_Skill_Defensive"  ;
		Monster_Skill_Defensive.FitnessFunctionName = "FitnessFunction_Skill_Defensive"  ;
		Monster_Skill_Defensive.ActionName = "Skill_Defensive"  ;
		
		SIMONProperty coolTime_Skill_Start_5 = new SIMONProperty ();
		coolTime_Skill_Start_5.PropertyName = "coolTime_Skill_Start_5";
		coolTime_Skill_Start_5.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_5);
		
		SIMONAction Monster_Skill_CON = new SIMONAction ();
		Monster_Skill_CON.ActionFunctionName = "ActionFunction_Skill_CON"  ;
		Monster_Skill_CON.ExecutionFunctionName = "Monster_Skill_CON"  ;
		Monster_Skill_CON.FitnessFunctionName = "FitnessFunction_Skill_CON"  ;
		Monster_Skill_CON.ActionName = "Skill_CON"  ;
		
		SIMONProperty coolTime_Skill_Start_6 = new SIMONProperty ();
		coolTime_Skill_Start_6.PropertyName = "coolTime_Skill_Start_6";
		coolTime_Skill_Start_6.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_6);
		
		SIMONAction Monster_Skill_Range = new SIMONAction ();
		Monster_Skill_Range.ActionFunctionName = "ActionFunction_Skill_Range"  ;
		Monster_Skill_Range.ExecutionFunctionName = "Monster_Skill_Range"  ;
		Monster_Skill_Range.FitnessFunctionName = "FitnessFunction_Skill_Range"  ;
		Monster_Skill_Range.ActionName = "Skill_Range"  ;
		
		SIMONProperty coolTime_Skill_Start_7 = new SIMONProperty ();
		coolTime_Skill_Start_7.PropertyName = "coolTime_Skill_Start_7";
		coolTime_Skill_Start_7.PropertyValue =(double)Time.time;
		sObject.Properties.Add (coolTime_Skill_Start_7);

		if (SimulationType == 2) 
		{
			SIMONGene Update_Gene_A = new SIMONGene ("Update_Gene_A",(double)Random.Range(-100.0f,100.0f));
			propertyUpdate_Function.InsertDNA(Update_Gene_A);
			
			SIMONGene Update_Gene_B = new SIMONGene ("Update_Gene_B",(double)Random.Range(-100.0f,100.0f));
			propertyUpdate_Function.InsertDNA(Update_Gene_B);
			
			SIMONGene Update_Gene_C = new SIMONGene ("Update_Gene_C",(double)Random.Range(-100.0f,100.0f));
			propertyUpdate_Function.InsertDNA(Update_Gene_C);
			
			
			
			SIMONGene Move_Gene_A = new SIMONGene ("Move_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Move.InsertDNA(Move_Gene_A);
			
			SIMONGene Move_Gene_C = new SIMONGene ("Move_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Move.InsertDNA(Move_Gene_C);
			
			SIMONGene Move_Gene_D = new SIMONGene ("Move_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Move.InsertDNA(Move_Gene_D);
			
			
			//
			SIMONGene Attack_Gene_A = new SIMONGene ("Attack_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Attack.InsertDNA(Attack_Gene_A);
			
			SIMONGene Attack_Gene_C = new SIMONGene ("Attack_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Attack.InsertDNA(Attack_Gene_C);
			
			SIMONGene Attack_Gene_D = new SIMONGene ("Attack_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Attack.InsertDNA(Attack_Gene_D);
			
			
			SIMONGene Defense_Gene_A = new SIMONGene ("Defense_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Defense.InsertDNA(Defense_Gene_A);
			
			SIMONGene Defense_Gene_B = new SIMONGene ("Defense_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Defense.InsertDNA(Defense_Gene_B);
			
			SIMONGene Defense_Gene_D = new SIMONGene ("Defense_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Defense.InsertDNA(Defense_Gene_D);
			
			
			SIMONGene Avoid_Gene_A = new SIMONGene ("Avoid_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Avoid.InsertDNA(Avoid_Gene_A);
			
			SIMONGene Avoid_Gene_B = new SIMONGene ("Avoid_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Avoid.InsertDNA(Avoid_Gene_B);
			
			SIMONGene Avoid_Gene_D = new SIMONGene ("Avoid_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Avoid.InsertDNA(Avoid_Gene_D);
			
			
			SIMONGene Strength_Gene_A = new SIMONGene ("Strength_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Strength.InsertDNA(Strength_Gene_A);
			SIMONGene Strength_Gene_B = new SIMONGene ("Strength_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Strength.InsertDNA(Strength_Gene_B);
			
			SIMONGene Strength_Gene_C = new SIMONGene ("Strength_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Strength.InsertDNA(Strength_Gene_C);
			
			SIMONGene Strength_Gene_D = new SIMONGene ("Strength_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Strength.InsertDNA(Strength_Gene_D);
			
			
			
			SIMONGene Attack_Speed_Gene_A = new SIMONGene ("Attack_Speed_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Attack_Speed.InsertDNA(Attack_Speed_Gene_A);
			
			SIMONGene Attack_Speed_Gene_B = new SIMONGene ("Attack_Speed_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Attack_Speed.InsertDNA(Attack_Speed_Gene_B);
			
			SIMONGene Attack_Speed_Gene_C = new SIMONGene ("Attack_Speed_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Attack_Speed.InsertDNA(Attack_Speed_Gene_C);
			
			SIMONGene Attack_Speed_Gene_D = new SIMONGene ("Attack_Speed_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Attack_Speed.InsertDNA(Attack_Speed_Gene_D);
			
			
			SIMONGene Moving_Speed_Gene_A = new SIMONGene ("Moving_Speed_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Moving_Speed.InsertDNA(Moving_Speed_Gene_A);
			
			SIMONGene Moving_Speed_Gene_B = new SIMONGene ("Moving_Speed_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Moving_Speed.InsertDNA(Moving_Speed_Gene_B);
			
			SIMONGene Moving_Speed_Gene_C = new SIMONGene ("Moving_Speed_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Moving_Speed.InsertDNA(Moving_Speed_Gene_C);
			
			SIMONGene Moving_Speed_Gene_D = new SIMONGene ("Moving_Speed_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Moving_Speed.InsertDNA(Moving_Speed_Gene_D);
			
			SIMONGene Critical_Gene_A = new SIMONGene ("Critical_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Critical.InsertDNA(Critical_Gene_A);
			
			SIMONGene Critical_Gene_B = new SIMONGene ("Critical_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Critical.InsertDNA(Critical_Gene_B);
			
			SIMONGene Critical_Gene_C = new SIMONGene ("Critical_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Critical.InsertDNA(Critical_Gene_C);
			
			SIMONGene Critical_Gene_D = new SIMONGene ("Critical_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Critical.InsertDNA(Critical_Gene_D);
			
			SIMONGene Defensive_Gene_A = new SIMONGene ("Defensive_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Defensive.InsertDNA(Defensive_Gene_A);
			
			SIMONGene Defensive_Gene_B = new SIMONGene ("Defensive_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Defensive.InsertDNA(Defensive_Gene_B);
			
			SIMONGene Defensive_Gene_C = new SIMONGene ("Defensive_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Defensive.InsertDNA(Defensive_Gene_C);
			
			SIMONGene Defensive_Gene_D = new SIMONGene ("Defensive_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Defensive.InsertDNA(Defensive_Gene_D);
			
			SIMONGene CON_Gene_A = new SIMONGene ("CON_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_CON.InsertDNA(CON_Gene_A);
			
			SIMONGene CON_Gene_B = new SIMONGene ("CON_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_CON.InsertDNA(CON_Gene_B);
			
			SIMONGene CON_Gene_C = new SIMONGene ("CON_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_CON.InsertDNA(CON_Gene_C);
			
			SIMONGene CON_Gene_D = new SIMONGene ("CON_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_CON.InsertDNA(CON_Gene_D);
			
			SIMONGene Range_Gene_A = new SIMONGene ("Range_Gene_A",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Range.InsertDNA(Range_Gene_A);
			
			SIMONGene Range_Gene_B = new SIMONGene ("Range_Gene_B",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Range.InsertDNA(Range_Gene_B);
			
			SIMONGene Range_Gene_C = new SIMONGene ("Range_Gene_C",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Range.InsertDNA(Range_Gene_C);
			
			SIMONGene Range_Gene_D = new SIMONGene ("Range_Gene_D",(double)Random.Range(-100.0f,100.0f));
			Monster_Skill_Range.InsertDNA(Range_Gene_D);
			
		}
		//
		//
		sObject.Actions.Add(propertyUpdate_Function);
		sObject.Actions.Add(Monster_Move);
		sObject.Actions.Add(Monster_Attack);
		sObject.Actions.Add(Monster_Defense);
		sObject.Actions.Add(Monster_Avoid);
		sObject.Actions.Add(Monster_Skill_Strength);
		sObject.Actions.Add(Monster_Skill_Attack_Speed);
		sObject.Actions.Add(Monster_Skill_Moving_Speed);
		sObject.Actions.Add(Monster_Skill_Critical);
		sObject.Actions.Add(Monster_Skill_Defensive);
		sObject.Actions.Add(Monster_Skill_CON);
		sObject.Actions.Add(Monster_Skill_Range);


		//

		
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
		value = (double)Random.Range(10.0f,100.0f);
		return value;
	}
	
	public System.Object PropertyUpdate_FitnessFunction(SIMONObject a, SIMONObject[] b)
	{
		double value ;
		value =0.0001d;
		return value;
	}
	public System.Object ObjectFitness(SIMONObject a, SIMONObject[] b)
	{
		double value = 0.0001d;
		if(NormalGameSceneManager_Simulation.whoIsWin ==(int)a.GetPropertyElement("Type"))
		{
			value = a.GetPropertyElement("Monster_Total_Damage");
			if (value > 9999.0d)
				value = 9000.0d;
			if (value < -9000.0d)
				value = -9000.0d;
		}
		return value;
	}
	
	public System.Object FitnessFunction_Move(SIMONObject a, SIMONObject[] b)
	{
		double value = 0.0001d;
		if(NormalGameSceneManager_Simulation.whoIsWin ==(int)a.GetPropertyElement("Type"))
			value = a.GetPropertyElement("Monster_Total_Damage");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	
	public System.Object FitnessFunction_Attack(SIMONObject a, SIMONObject[] b)
	{
		double value = 0.0001d;
		if(NormalGameSceneManager_Simulation.whoIsWin ==(int)a.GetPropertyElement("Type"))
			value = a.GetPropertyElement("Monster_Total_Damage");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Defense(SIMONObject a, SIMONObject[] b)
	{
		double value = 0.0001d;
		if(NormalGameSceneManager_Simulation.whoIsWin ==(int)a.GetPropertyElement("Type"))
			value = a.GetPropertyElement("Monster_Total_Defense");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Avoid(SIMONObject a, SIMONObject[] b)
	{		
		double value = 0.0001d;
		if(NormalGameSceneManager_Simulation.whoIsWin ==(int)a.GetPropertyElement("Type"))
			value = a.GetPropertyElement("Monster_Avoid_Value");
		if (value > 9999.9999d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}

	public System.Object FitnessFunction_Skill_Strength(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Strength_Value");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Skill_Attack_Speed(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Attack_Speed_Value");
		if (value > 9999.9999d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Skill_Moving_Speed(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Total_Damage");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Skill_Critical(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Critical_Value");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Skill_Defensive(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Defensive_Value");
		if (value > 9999.0d)
			value = 9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Skill_CON(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_CON_Value");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	public System.Object FitnessFunction_Skill_Range(SIMONObject a, SIMONObject[] b)
	{
		double value;
		value = a.GetPropertyElement("Monster_Range_Value");
		if (value > 9999.0d)
			value = 9000.0d;
		if (value < -9000.0d)
			value = -9000.0d;
		return value;
	}
	// Update is called once per frame
	
	void reStart()
	{
		PropertyValue = new int[7];
		int total;
		int value;
		int i = 0;

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
		sObject.SetPropertyElement ("Monster_Total_Defense", 0.0d);
		sObject.SetPropertyElement ("Monster_Avoid_Value",0.0d);

		sObject.SetPropertyElement ("Monster_Strength_Value",0.0d);
		sObject.SetPropertyElement ("Monster_Attack_Speed_Value",0.0d);
		sObject.SetPropertyElement ("Monster_Total_Damage",0.0d);
		sObject.SetPropertyElement ("Monster_Critical_Value",0.0d);
		sObject.SetPropertyElement ("Monster_Defensive_Value",0.0d);
		sObject.SetPropertyElement ("Monster_CON_Value",0.0d);
		sObject.SetPropertyElement ("Monster_Range_Value",0.0d);

		sObject.SetPropertyElement ("coolTime_Skill_Start_1", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_2", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_3", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_4", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_5", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_6", (double)Time.time);
		sObject.SetPropertyElement ("coolTime_Skill_Start_7", (double)Time.time);
		sObject.SetPropertyElement ("Monster_Avoid_Time", (double)Time.time);

		PropertyValue [0] = (int)sObject.GetPropertyElement ("Strength");
		PropertyValue [1] = (int)sObject.GetPropertyElement ("Moving_Speed");
		PropertyValue [2] = (int)sObject.GetPropertyElement ("Attack_Speed");
		PropertyValue [3] = (int)sObject.GetPropertyElement ("Critical");
		PropertyValue [4] = (int)sObject.GetPropertyElement ("Range");
		PropertyValue [5] = (int)sObject.GetPropertyElement ("CON");
		PropertyValue [6] = (int)sObject.GetPropertyElement ("Defensive");
		for(int j = 0; j < 7 ; j++)
		{
			if(PropertyValue[j] < 1)
			{
				PropertyValue[j] =1;
			}
		}
		
		total = PropertyValue [0] + PropertyValue [1] + PropertyValue [2] + PropertyValue [3] + PropertyValue [4] + PropertyValue [5] + PropertyValue [6];
		if (total> 100) 
		{
			
			while(true)
			{
				if(PropertyValue [0] + PropertyValue [1] + PropertyValue [2] + PropertyValue [3] + PropertyValue [4] + PropertyValue [5] + PropertyValue [6] == 100)
					break;
				value = (int)Random.Range(0,2);
				if(value > 0 )
				{
					if(PropertyValue[i] > 2)
						PropertyValue[i] = PropertyValue[i]-1;
				}
				i++;
				if(i > 6)
					i=0;
			}
		}
		else
		{
			int max = PropertyValue[0];
			int maxi=0;
			while(true)
			{
				if(max < PropertyValue[i])
				{
					max = PropertyValue[i];
					maxi = i;
				}
				i++;
				if(i == 7)
					break;
			}
			PropertyValue[maxi] = PropertyValue[maxi] + 100- total;
		}


		sObject.SetPropertyElement ("Strength",PropertyValue [0]);
		sObject.SetPropertyElement ("Moving_Speed",PropertyValue[1]);
		sObject.SetPropertyElement ("Attack_Speed",PropertyValue[2]);
		sObject.SetPropertyElement ("Critical",PropertyValue[3]);
		sObject.SetPropertyElement ("Range",PropertyValue[4]);
		sObject.SetPropertyElement ("CON", PropertyValue [5]);
		sObject.SetPropertyElement ("Defensive", PropertyValue [6]);

		sObject.SetPropertyElement ("CurHp", sObject.GetPropertyElement ("CON")*50);
		sObject.SetPropertyElement ("Monster_Damage",sObject.GetPropertyElement("Strength"));
		sObject.SetPropertyElement ("Monster_Moving_Speed", sObject.GetPropertyElement ("Moving_Speed") / 20);
		sObject.SetPropertyElement ("Monster_Attack_Speed", 4.0d - (sObject.GetPropertyElement ("Attack_Speed") / 10));
		sObject.SetPropertyElement ("Monster_Defensive",sObject.GetPropertyElement("Defensive"));
		sObject.SetPropertyElement ("Monster_Critical",sObject.GetPropertyElement("Critical"));
		sObject.SetPropertyElement ("Monster_Range",1.0d + (sObject.GetPropertyElement("Range")/6));
		sObject.SetPropertyElement ("Monster_Sight",sObject.GetPropertyElement("Monster_Range"));
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
			if (myObject.GetComponent<Monster_Controller_Simulation>().animator.GetCurrentAnimatorStateInfo (0).IsTag ("Stay")) 
			{
				a.SetPropertyElement("isCheck",1);
			}
		}
		else
		{
			if (myObject.GetComponent<Monster_Controller_B_Simulation>().animator.GetCurrentAnimatorStateInfo (0).IsTag ("Stay")) 
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
			
			if(distance < 50.0f)
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

	public void init(int id)
	{
		id = id % 7;
		PropertyValue = new int[7];
		PropertyValue [id] = 28;
		for (int i = 0; i < 7; i++) 
		{
			if(PropertyValue[i] == 0)
				PropertyValue[i] =12;
		}
	}


	public void init()
	{
		int count = 0 , i = 0;
		int value;
		
		PropertyValue = new int[7];
		
		while (true) 
		{
			value = (int)Random.Range(0,2);
			if(count > 30)
				break;
			if(value > 0 )
			{
				PropertyValue[i] = PropertyValue[i]+3;
				count++;
			}
			
			i++;
			if(i > 6)
				i=0;
		}
		for(int j = 0 ; j < 7 ;j++)
		{
			if(PropertyValue[j] == 0)
				PropertyValue[j] = 1;
		}
		while (true) 
		{
			if(PropertyValue[0]+PropertyValue[1]+PropertyValue[2]+PropertyValue[3]+PropertyValue[4]+PropertyValue[5]+PropertyValue[6] == 100)
				break;
			value = (int)Random.Range(0,2);
			if(value > 0 )
			{
				PropertyValue[i] = PropertyValue[i]+1;
			}
		}
		
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
		if (a.GetPropertyElement ("Strength") < 20)
			return MaxValue;
		
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
		
		if (a.GetPropertyElement ("Attack_Speed") < 20)
			return MaxValue;
		
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
		
		if (a.GetPropertyElement ("Moving_Speed") < 20)
			return MaxValue;
		
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
		
		if (a.GetPropertyElement ("Critical") < 20)
			return MaxValue;
		
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
		
		if (a.GetPropertyElement ("Defensive") < 20)
			return MaxValue;
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
		
		if (a.GetPropertyElement ("CON") < 20)
			return MaxValue;
		
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
		
		double pointX = a.GetPropertyElement ("PositionX");
		double pointY = a.GetPropertyElement ("PositionY");
		Vector2 a_v;
		
		GameObject Target_Temp;
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
		SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "Attack_Speed");
		isSkill = true;
		sObject.SetPropertyElement("Monster_Attack_Speed",sObject.GetPropertyElement("Monster_Attack_Speed")/2);
		yield return new WaitForSeconds (10.0f); 
		sObject.SetPropertyElement("Monster_Attack_Speed",sObject.GetPropertyElement("Monster_Attack_Speed")*2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill = false;
		
	}
	
	IEnumerator setSkill_Moving_Speed()
	{
		SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "Moving_Speed");
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
		SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "Defensive");
		isSkill= true;
		sObject.SetPropertyElement("Monster_Defensive",sObject.GetPropertyElement("Monster_Defensive")*2);
		yield return new WaitForSeconds(10.0f); 
		sObject.SetPropertyElement("Monster_Defensive",sObject.GetPropertyElement("Monster_Defensive")/2);
		sObject.SetPropertyElement("Skill_number",0.0d);
		isSkill= false;
		
	}
	
	IEnumerator setSkill_CON()
	{
		SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "CON");
		isSkill= true;
		sObject.SetPropertyElement("Skill_number",0.0d);
		if(sObject.GetPropertyElement("CurHp")+(sObject.GetPropertyElement("HP")/10) > sObject.GetPropertyElement("HP"))
		{
			sObject.SetPropertyElement("Monster_CON_Value",sObject.GetPropertyElement("Monster_CON_Value")+
			                           sObject.GetPropertyElement("HP")-sObject.GetPropertyElement("CurHp"));
			
			sObject.SetPropertyElement("CurHp",sObject.GetPropertyElement ("HP"));
			NowHealth = (float)sObject.GetPropertyElement ("HP");
		}
		else
		{
			sObject.SetPropertyElement("Monster_CON_Value",sObject.GetPropertyElement("Monster_CON_Value")+
			                           (sObject.GetPropertyElement ("HP")/10));
			
			sObject.SetPropertyElement("CurHp",sObject.GetPropertyElement("CurHp") + (sObject.GetPropertyElement ("HP")/10));
			NowHealth =(float)sObject.GetPropertyElement("CurHp") + (float)(sObject.GetPropertyElement ("HP")/10);
		}
		
		yield return new WaitForSeconds(1.0f); 
		isSkill= false;
	}
	
	IEnumerator setSkill_Range()
	{	
		SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "Range");
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
			sObject.SetPropertyElement("coolTime_Attack_Start",Time.time) ;
			bool check  = false;

			gObj_Target_Attack = GameObject.Find ("Monster_B_"+sObject.GetPropertyElement("Target").ToString());
			if(sObject.GetPropertyElement("Skill_number") == 1)
			{
				sObject.SetPropertyElement("Monster_Strength_Value",sObject.GetPropertyElement("Monster_Strength_Value")+
				                           (sObject.GetPropertyElement("Monster_Damage")-
				 (sObject.GetPropertyElement("Monster_Damage")*(sObject.GetPropertyElement("Monster_Enemy_Defensive")/100))));

				SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "Strength");
			}
			else if(sObject.GetPropertyElement("Skill_number")==4)
			{
				sObject.SetPropertyElement("Monster_Critical_Value",sObject.GetPropertyElement("Monster_Critical_Value")+
				                           (sObject.GetPropertyElement("Monster_Damage")-
				 (sObject.GetPropertyElement("Monster_Damage")*(sObject.GetPropertyElement("Monster_Enemy_Defensive")/100))));

				SkillManager_Simulation.skillManagerVal.startSkill (this.gameObject, "Critical");
			}
			else if(sObject.GetPropertyElement("Skill_number") == 2)
			{
				sObject.SetPropertyElement("Monster_Attack_Speed_Value",sObject.GetPropertyElement("Monster_Attack_Speed_Value")+
				                           (sObject.GetPropertyElement("Monster_Damage")-
				 (sObject.GetPropertyElement("Monster_Damage")*(sObject.GetPropertyElement("Monster_Enemy_Defensive")/100))));

			}
			else if(sObject.GetPropertyElement("Skill_number") == 7)
			{
				sObject.SetPropertyElement("Monster_Range_Value",sObject.GetPropertyElement("Monster_Range_Value")+
				                           (sObject.GetPropertyElement("Monster_Damage")-
				 (sObject.GetPropertyElement("Monster_Damage")*(sObject.GetPropertyElement("Monster_Enemy_Defensive")/100))));

			}
			if(Random.Range(0,100) + sObject.GetPropertyElement("Monster_Critical") > 100)
			{
				check = true;
				SkillManager_Simulation.skillManagerVal.getCritical(this.gameObject);
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
			if(sObject.GetPropertyElement("CurHp") != sObject.GetPropertyElement("HP"))
			{
				sObject.SetPropertyElement("Monster_Total_AttackCount",sObject.GetPropertyElement("Monster_Total_AttackCount")+1) ;
				sObject.SetPropertyElement("Monster_Total_Damage",sObject.GetPropertyElement("Monster_Total_Damage")+
				                           (((sObject.GetPropertyElement("Monster_Damage")-
				   (sObject.GetPropertyElement("Monster_Damage")*(sObject.GetPropertyElement("Monster_Enemy_Defensive")/100)))/
				  sObject.GetPropertyElement("Monster_Enemy_CurHp"))*100));
			}
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
			sObject.SetPropertyElement("Monster_Total_AttackedDamage",sObject.GetPropertyElement("Monster_Total_AttackedDamage") + damage - 
			                           ((sObject.GetPropertyElement("Defensive")/100)*damage));

			if(sObject.GetPropertyElement("Skill_number") == 5)
			{
				sObject.SetPropertyElement("Monster_Defensive_Value",sObject.GetPropertyElement("Monster_Defensive_Value")+
				                           ((sObject.GetPropertyElement("Defensive")/100)*damage)
				                           );
			}

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
