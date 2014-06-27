using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	public static SkillManager skillManagerVal;
	public Sprite Snowball_Skill;
	public Sprite Snowball_Blue;
	public Sprite Snowball_Normal;

	public GameObject Critical_PreFab;
	public GameObject Critical;

	public GameObject particle_CON_preFab;
	public GameObject particle_CON;

	public GameObject particle_Strength_preFab;
	public GameObject particle_Strength;

	public GameObject particle_Attack_Speed_preFab;
	public GameObject particle_Attack_Speed;

	public GameObject particle_Moving_preFab;
	public GameObject particle_Moving;

	public GameObject particle_Defensive_preFab;
	public GameObject particle_Defensive;

	public GameObject particle_Critical_preFab;
	public GameObject particle_Critical;

	public GameObject particle_Range_preFab;
	public GameObject particle_Range;

	// Use this for initialization
	void Start () {
		skillManagerVal = this;
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	/* 
	 *  change snowball 
	 					*/

	private void clearSnowball(GameObject obj, string kindOfSkill)
	{
		if (obj.tag.ToString ().Equals ("Enemy")) 
			obj.GetComponent<Monster_Controller_B>().SnowballSprite = Snowball_Blue;
		else if(obj.tag.ToString ().Equals ("Home"))
			obj.GetComponent<Monster_Controller>().SnowballSprite = Snowball_Normal;
		else
			obj.GetComponent<Player_Controller>().SnowballSprite = Snowball_Normal;
	}

	private void setSnowball(GameObject obj, string kindOfSkill)
	{
		if (obj.tag.ToString ().Equals ("Enemy")) 
			obj.GetComponent<Monster_Controller_B>().SnowballSprite = Snowball_Skill;
		else if(obj.tag.ToString ().Equals ("Home"))
			obj.GetComponent<Monster_Controller>().SnowballSprite = Snowball_Skill;
		else
			obj.GetComponent<Player_Controller>().SnowballSprite = Snowball_Skill;
	}

	/* 
	 *     add Effect 
	 					*/
	private void showingParticle(GameObject obj, string kindOfSkill)
	{
		if (kindOfSkill.Equals ("CON")) 
		{
			particle_CON = (GameObject)Instantiate (particle_CON_preFab, obj.transform.position, particle_CON_preFab.transform.rotation) as GameObject;
			particle_CON.transform.parent = obj.transform;
			particle_CON.GetComponent<ParticleSystem> ().Play();
			particle_CON.GetComponent<AudioSource>().Play ();
			Destroy(particle_CON,3.0f);
		}
		else if(kindOfSkill.Equals("Strength"))
		{
			particle_Strength = (GameObject)Instantiate (particle_Strength_preFab, obj.transform.position, obj.transform.rotation) as GameObject;
			particle_Strength.GetComponent<ParticleSystem> ().Play();
			particle_Strength.GetComponent<AudioSource>().Play ();
			Destroy(particle_Strength,3.0f);
		}
		else if(kindOfSkill.Equals("Attack_Speed"))
		{
			particle_Attack_Speed = (GameObject)Instantiate (particle_Attack_Speed_preFab, new Vector3(obj.transform.position.x, obj.transform.position.y - 0.7f,obj.transform.position.z), particle_Attack_Speed_preFab.transform.rotation) as GameObject;
			particle_Attack_Speed.transform.parent = obj.transform;
			particle_Attack_Speed.GetComponent<ParticleSystem> ().Play();
			particle_Attack_Speed.GetComponent<AudioSource>().Play ();
			Destroy(particle_Attack_Speed,3.0f);
		}
		else if(kindOfSkill.Equals("Moving_Speed"))
		{
			particle_Moving = (GameObject)Instantiate (particle_Moving_preFab, new Vector3(obj.transform.position.x,obj.transform.position.y - 0.7f,obj.transform.position.z), particle_Moving_preFab.transform.rotation) as GameObject;
			particle_Moving.transform.parent = obj.transform;
			particle_Moving.GetComponent<ParticleSystem> ().Play();
			particle_Moving.GetComponent<AudioSource>().Play ();
			Destroy(particle_Moving,3.0f);
		}
		else if(kindOfSkill.Equals("Defensive"))
		{
			particle_Defensive = (GameObject)Instantiate (particle_Defensive_preFab, obj.transform.position, obj.transform.rotation) as GameObject;
			particle_Defensive.transform.parent = obj.transform;
			particle_Defensive.GetComponent<ParticleSystem> ().Play();
			particle_Defensive.GetComponent<AudioSource>().Play ();
			Destroy(particle_Defensive,3.0f);
		}
		else if(kindOfSkill.Equals("Critical"))
		{
			particle_Critical = (GameObject)Instantiate (particle_Critical_preFab, obj.transform.position, obj.transform.rotation) as GameObject;
			particle_Critical.GetComponent<ParticleSystem> ().Play();
			particle_Critical.GetComponent<AudioSource>().Play ();
			Destroy(particle_Critical,3.0f);
		}
		else if(kindOfSkill.Equals("Range"))
		{
			particle_Range = (GameObject)Instantiate (particle_Range_preFab, obj.transform.position, obj.transform.rotation) as GameObject;
			particle_Range.transform.parent = obj.transform;
			particle_Range.GetComponent<ParticleSystem> ().Play();
			particle_Range.GetComponent<AudioSource>().Play ();
			Destroy(particle_Range,3.0f);
		}
	}
	
	IEnumerator attackSnowball(GameObject obj, string kindOfSkill)
	{
		setSnowball (obj, kindOfSkill);
		yield return new WaitForSeconds(3.0f); 
		clearSnowball (obj, kindOfSkill);
	}

	public void startSkill(GameObject obj, string kindOfSkill)
	{
		StartCoroutine(attackSnowball(obj, kindOfSkill));
		switch (kindOfSkill) 
		{
		case "CON" :
		{
			showingParticle(obj, kindOfSkill);
			break;
		}
		case "Strength" :
		{
			showingParticle(obj, kindOfSkill);
			break;
		}
		case "Attack_Speed" :
		{
			showingParticle(obj, kindOfSkill);
			break;
		}
		case "Moving_Speed" :
		{
			showingParticle(obj, kindOfSkill);
			break;
		}
		case "Defensive" :
		{
			showingParticle(obj, kindOfSkill);

			break;
		}
		case "Critical" :
		{
			showingParticle(obj, kindOfSkill);
			break;
		}
		case "Range" :
		{
			showingParticle(obj, kindOfSkill);
			break;
		}
			
		}
	}

	public void getCritical(GameObject obj)
	{
		Critical = (GameObject)Instantiate (Critical_PreFab, new Vector3(obj.transform.position.x + 0.6f,obj.transform.position.y +0.7f, -4.0f), obj.transform.rotation) as GameObject;
		Critical.transform.parent = obj.transform;
		Destroy (Critical, 1.0f);
	}

}
