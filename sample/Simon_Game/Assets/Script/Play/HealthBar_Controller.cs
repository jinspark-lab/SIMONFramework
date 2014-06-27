using UnityEngine;
using System.Collections;

public class HealthBar_Controller : MonoBehaviour {

	public static HealthBar_Controller playerHealthBar;
	public float NowHealth;
	public float TotalHealth;
	public float realHealthBar;
	public Camera cam;

	private float left;
	private float top;
	private Vector2 pos; 

	public Texture2D emptyTex;
	public Texture2D fullTex;

	void Awake()
	{
		playerHealthBar = this;
	}
	// Use this for initialization
	void Start () {
		TotalHealth = 100;
		NowHealth = 100;
			//Camera.main.WorldToScreenPoint (transform.position).y;
	}
	
	// Update is called once per frame
	void Update () {
		realHealthBar = 50 * NowHealth / TotalHealth;
		pos = cam.camera.WorldToScreenPoint (transform.position);
		left = pos.x;
		top = Screen.height - pos.y;
	}

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

	void OnGUI() {

		GUI.DrawTexture(new Rect(left + 49f, top - 85, realHealthBar , 10.0f), fullTex);
		GUI.DrawTexture(new Rect(left + 49f, top - 85, 50.0f , 10.0f), emptyTex);
	}
}
