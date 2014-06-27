using UnityEngine;
using System.Collections;
using SIMONFramework;
using System.Reflection;

public class Player_Controller : MonoBehaviour {

	public GameObject gObj_Player;
	public GameObject gObj_ClickCheck;
	public GameObject Snowball;
	public Sprite SnowballSprite;
	public GameObject sb;
	public GameObject MovingTarget;
	public GameObject mt;
	public HealthBar_Controller HC;

	public Vector3 Vector_target;
	public Vector3 Vector_curPlayer;

	public Animator animator;
 
	public float tmp_speed;
	public float curPositionX;
	public float curPositionY;
	public float moveToLength;

	public int curDirection;
	public Camera cam;

	public int PlayerState ; 
	public float dis;
	public float Speed;
	public float Attack_Range;
	public int KeyCheck;

	public float Player_Strength;
	public float Player_Attack_Speed;
	public float Player_Moving_Speed;
	public float Player_Critical;
	public float Player_Defensive;
	public float Player_CON;
	public float Player_Range;
	public float Player_Skill_CoolTime;
	public bool isStrength;
	public bool isCritical;
	public int select_Skill_A;
	public int select_Skill_B;
	public int select_Skill_C;
	



	// Use this for initialization
	void Start () {
		gObj_Player = GameObject.Find ("Player");
		tmp_speed = 0.1f;
		curDirection = 2;
		animator = GetComponent<Animator>();
		PlayerState = 0;

		KeyCheck = 0;

		Player_CON = PlayerPrefs.GetFloat ("Player_CON");
		Player_Strength = PlayerPrefs.GetFloat ("Player_Strength");
		Player_Attack_Speed = PlayerPrefs.GetFloat ("Player_Attack_Speed");
		Player_Moving_Speed = PlayerPrefs.GetFloat ("Player_Moving_Speed");
		Player_Defensive = PlayerPrefs.GetFloat ("Player_Defensive");
		Player_Critical = PlayerPrefs.GetFloat ("Player_Critical");
		Player_Range = PlayerPrefs.GetFloat ("Player_Range");

		select_Skill_A = -1;
		select_Skill_B = -1;
		select_Skill_C = -1;

		if (Player_Strength > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 1;
		}
		if (Player_Attack_Speed > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 2;
			else if(select_Skill_B == -1)
				select_Skill_B = 2;
		}
		if (Player_Moving_Speed > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 3;
			else if(select_Skill_B == -1)
				select_Skill_B = 3;
			else if(select_Skill_C == -1)
				select_Skill_C = 3;
		}
		if (Player_Critical > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 4;
			else if(select_Skill_B == -1)
				select_Skill_B = 4;
			else if(select_Skill_C == -1)
				select_Skill_C = 4;
		}
		if (Player_Defensive > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 5;
			else if(select_Skill_B == -1)
				select_Skill_B = 5;
			else if(select_Skill_C == -1)
				select_Skill_C = 5;
		}
		if (Player_CON > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 6;
			else if(select_Skill_B == -1)
				select_Skill_B = 6;
			else if(select_Skill_C == -1)
				select_Skill_C = 6;
		}
		if (Player_Range > 20) 
		{
			if(select_Skill_A == -1)
				select_Skill_A = 7;
			else if(select_Skill_B == -1)
				select_Skill_B = 7;
			else if(select_Skill_C == -1)
				select_Skill_C = 7;
		}
		if (Player_CON == 0)
			Player_CON = 1;
		if (Player_Strength == 0)
			Player_Strength = 1;
		if (Player_Attack_Speed == 0)
			Player_Attack_Speed = 1;
		if (Player_Moving_Speed == 0)
			Player_Moving_Speed = 1;
		if (Player_Critical == 0)
			Player_Critical = 1;
		if (Player_Defensive == 0)
			Player_Defensive = 1;
		if (Player_Range == 0)
			Player_Range = 1;




		CooltimeButtonManager.NormalAttackManager.setCoolTime (4.0f - (float)Player_Attack_Speed/10.0f);
		HealthBar_Controller.playerHealthBar.setTotalHP (Player_CON*50);
		Attack_Range = 1.0f + (Player_Range/6.0f);
		
		Speed = Player_Moving_Speed/20;
		Player_Skill_CoolTime = 60.0f;
		isStrength = false;
		isCritical = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Use Skill
		if (GameTimeManager.isPause)
						return;

		if (PlayerState == -1)
						return;
		if (Input.GetKeyDown(KeyCode.R))
		{
			PlayerState = 3;
		}
		if (Input.GetKeyUp (KeyCode.R)) 
		{
			PlayerState = 0;
		}
		if (CooltimeButtonManager_skill.SkillAttackManager.IsUseSkill () && PlayerState != 3) 
		{
			if (Input.GetKeyDown (KeyCode.Q)) 
			{
				if (KeyCheck == select_Skill_A)
					KeyCheck = 0;
				else
				{
					KeyCheck = select_Skill_A;
					if(KeyCheck == 1)
						StartCoroutine(setSkill_Strength());
					else if(KeyCheck == 2)
						StartCoroutine(setSkill_Attack_Speed());
					else if(KeyCheck == 3)
						StartCoroutine(setSkill_Moving_Speed());
					else if(KeyCheck == 4)
						StartCoroutine(setSkill_Critical());
					else if(KeyCheck == 5)
						StartCoroutine(setSkill_Defensive());
					else if(KeyCheck == 6)
						StartCoroutine(setSkill_CON());
					else if(KeyCheck == 7)
						StartCoroutine(setSkill_Range());
				}
			}
			if (Input.GetKeyDown (KeyCode.W)) 
			{
				if (KeyCheck == select_Skill_B)
					KeyCheck = 0;
				else
				{
					KeyCheck = select_Skill_B;
					if(KeyCheck == 1)
						StartCoroutine(setSkill_Strength());
					else if(KeyCheck == 2)
						StartCoroutine(setSkill_Attack_Speed());
					else if(KeyCheck == 3)
						StartCoroutine(setSkill_Moving_Speed());
					else if(KeyCheck == 4)
						StartCoroutine(setSkill_Critical());
					else if(KeyCheck == 5)
						StartCoroutine(setSkill_Defensive());
					else if(KeyCheck == 6)
						StartCoroutine(setSkill_CON());
					else if(KeyCheck == 7)
						StartCoroutine(setSkill_Range());
				}
			}
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				if (KeyCheck == select_Skill_C)
					KeyCheck = 0;
				else
				{
					KeyCheck = select_Skill_C;
					if(KeyCheck == 1)
						StartCoroutine(setSkill_Strength());
					else if(KeyCheck == 2)
						StartCoroutine(setSkill_Attack_Speed());
					else if(KeyCheck == 3)
						StartCoroutine(setSkill_Moving_Speed());
					else if(KeyCheck == 4)
						StartCoroutine(setSkill_Critical());
					else if(KeyCheck == 5)
						StartCoroutine(setSkill_Defensive());
					else if(KeyCheck == 6)
						StartCoroutine(setSkill_CON());
					else if(KeyCheck == 7)
						StartCoroutine(setSkill_Range());
				}
			}
		}

		// Do Normal Attack
		if (Input.GetMouseButtonDown (0)  && PlayerState != 3) 
		{
			gObj_ClickCheck = GetClickedObject();
			
			Vector_curPlayer = gObj_Player.transform.position;
			Vector_target = Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x,
			            Input.mousePosition.y,
			            -Camera.main.transform.position.z)
				);
			dis = Vector3.Distance (gObj_Player.transform.position, Vector_target);

			// If Enemy else Our Team
			if(gObj_ClickCheck.tag.Equals("Enemy"))
			{
				if(KeyCheck == 0)
				{
					PlayerState = 2;
				}
			}
			else if(gObj_ClickCheck.tag.Equals("Home"))
			{
				if(KeyCheck == 0)
				{
					PlayerState = 0;
				}
			}
			else
			{
				if(KeyCheck == 0)
				{
					PlayerState = 1;
				}
			}
		}
		
		curDirection = AnimationControll(); // Change Player Direction
	
		if (PlayerState == 2 && dis < Attack_Range )
		{
			if(CooltimeButtonManager.NormalAttackManager.IsUseNormal())
			{
				Attack(gObj_ClickCheck);
			}
		}
		else if (PlayerState == 1 || PlayerState == 2) 
		{
			Move ();
		} 
		else if( PlayerState == 0)
		{
			Stay();
		}
		else if( PlayerState == 3)
		{
			Defense();
		}
	}

	public void Stay()
	{
		switch (curDirection) 
		{
		case 1:
			animator.Play ("Ani_Stay_Up");
			break;
		case 2:
			animator.Play ("Ani_Stay_Right");
			break;
		case 3:
			animator.Play ("Ani_Stay_Down");
			break;
		case 4:
			animator.Play ("Ani_Stay_Left");
			break;
		}
	}
	IEnumerator setSkill_Strength()
	{
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		Player_Strength *= 2;
		isStrength = true;
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
		isStrength = false;
		Player_Strength /= 2;
	}

	IEnumerator setSkill_Attack_Speed()
	{
		SkillManager.skillManagerVal.startSkill (this.gameObject, "Attack_Speed");
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		CooltimeButtonManager.NormalAttackManager.setCoolTime ((4.0f - (float)Player_Attack_Speed/10.0f)/2.0f);
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
		CooltimeButtonManager.NormalAttackManager.setCoolTime (4.0f - (float)Player_Attack_Speed/10.0f);
	}

	IEnumerator setSkill_Moving_Speed()
	{
		SkillManager.skillManagerVal.startSkill (this.gameObject, "Moving_Speed");
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		Speed *= 2;
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
		Speed /= 2;
	}

	IEnumerator setSkill_Critical()
	{
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		isCritical = true;
		Player_Critical *= 2;
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
		Player_Critical /= 2;
		isCritical = false;
	}
	IEnumerator setSkill_Defensive()
	{
		SkillManager.skillManagerVal.startSkill (this.gameObject, "Defensive");
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		Player_Defensive *= 2;
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
		Player_Defensive /= 2;
	}

	IEnumerator setSkill_CON()
	{
		SkillManager.skillManagerVal.startSkill (this.gameObject, "CON");
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		HC.getHealing((Player_CON * 10));
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
	}
	IEnumerator setSkill_Range()
	{
		SkillManager.skillManagerVal.startSkill (this.gameObject, "Range");
		CooltimeButtonManager_skill.SkillAttackManager.CoolTime_Start ();
		Attack_Range *= 2;
		KeyCheck = 0;
		yield return new WaitForSeconds(3.0f); 
		Attack_Range /= 2;
	}

	
	public void Defense()
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

	public void Move()
	{
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
		gObj_Player.transform.localPosition = Vector3.MoveTowards (gObj_Player.transform.position, Vector_target, Time.deltaTime * Speed);
		dis = Vector3.Distance (gObj_Player.transform.position, Vector_target);
		if (dis < 0.01f) 
		{
			PlayerState = 0;
			gObj_Player.transform.localPosition = Vector3.MoveTowards (gObj_Player.transform.position, Vector_target, 1.0f);
		}
	}
	void Attack(GameObject target)
	{
		PlayerState = 100;
		int power = 1;
		float damage = 0;
		if (isStrength) 
		{
			SkillManager.skillManagerVal.startSkill (this.gameObject, "Strength");
			power = 2;
		}
		if (isCritical) 
		{
			SkillManager.skillManagerVal.startSkill (this.gameObject, "Critical");
		}
		damage = Player_Strength * power;
		if (Random.Range (0, 100) + Player_Critical > 100) 
		{
			SkillManager.skillManagerVal.getCritical(this.gameObject);
			damage *= 2;
		}

		switch(curDirection)
		{
		case 1:
			animator.Play ("Ani_Attack_Up");
			break;
		case 2:
			animator.Play ("Ani_Attack_Right");
			break;
		case 3:
			animator.Play ("Ani_Attack_Down");
			break;
		case 4:
			animator.Play ("Ani_Attack_Left");
			break;
		}
		float distance = Vector3.Distance (gObj_Player.transform.position, target.transform.position) * 0.4f;
		Vector3 tmpVector = new Vector3(target.transform.position.x,target.transform.position.y + distance, target.transform.position.z);
		
		MovingTarget = (GameObject)Instantiate (mt, tmpVector, target.transform.rotation) as GameObject;
		
		Snowball = (GameObject)Instantiate (sb,gObj_Player.transform.position,gObj_Player.transform.rotation) as GameObject;
		Snowball.GetComponent<SpriteRenderer> ().sprite = SnowballSprite;
		
		Snowball.GetComponent<SnowBall_Controller>().setTarget(target.name,(damage)-
		                                                       ((float)(target.GetComponent<Monster_Controller_B>().sObject.GetPropertyElement("Defensive")/100)*
		 (float)(damage)));
		Snowball.GetComponent<AudioSource> ().Play ();
		Snowball.SendMessage ("Attack",target);
		Snowball.SendMessage ("NormalAttackSetting", MovingTarget);

		Destroy(Snowball,3.0f);
		Destroy(MovingTarget,3.0f);


	}
	// Change Moving Animation
	private int AnimationControll()
	{
		curPositionX = gObj_Player.transform.position.x;
		curPositionY = gObj_Player.transform.position.y;

		float betweenX = Vector_target.x - curPositionX;
		float betweenY = Vector_target.y - curPositionY;
		if (betweenX == 0 && betweenY == 0)
			return curDirection;
		// 1사분면 
		if (betweenX >= 0 && betweenY >= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
			{
				return 1;
			}
			else
			{
				return 2;
			}
		}
		// 2사분면 
		else if (betweenX <= 0 && betweenY >= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
			{
				return 1;
			}
			else
			{
				return 4;
			}
		}
		// 3사분면 
		else if (betweenX <= 0 && betweenY <= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
			{
				return 3;
			}
			else
			{
				return 4;
			}
		}
		// 4사분면 
		else if (betweenX >= 0 && betweenY <= 0) 
		{
			if(System.Math.Abs(betweenY) - System.Math.Abs(betweenX) > 0 )
			{
				return 3;
			}
			else
			{
				return 2;
			}
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

			if(PlayerState == 3)
			{
				damage /=2;
			}
			if(!HealthBar_Controller.playerHealthBar.getDamage(damage - (damage * (float)(Player_Defensive/100))))
			{
				PlayerState = -1;
				gObj_Player.transform.position = new Vector3(gObj_Player.transform.position.x, gObj_Player.transform.position.y, 2.0f);
				animator.Play ("Ani_Tomb_Red");

				Destroy (Snowball);
				//Destroy (MovingTarget);
			}
			else
			{
				StartCoroutine(setTwinkleDelay());
			}
		}
		
		
		//DestroyObject(other.gameObject);
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
