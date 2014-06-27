using UnityEngine;
using System.Collections;

public class CooltimeButtonManager : MonoBehaviour {

	public static CooltimeButtonManager NormalAttackManager;

	public float CooltimeMax ;
	public float Cooltime;

	UIProgressBar slider = null;

	// Use this for initialization
	void Start () {
		NormalAttackManager = this;

		slider = GetComponent<UISlider> ();
		slider.fillDirection = UIProgressBar.FillDirection.BottomToTop;
		CooltimeMax = 1.0f;
		Cooltime = CooltimeMax;
	}
	
	// Update is called once per frame
	void Update () {
		if (Cooltime > 0.0f) 
		{
			Cooltime -= Time.deltaTime;
			float sliderPercent = 1.0f - Cooltime/CooltimeMax;
			slider.value = sliderPercent;
		}
	}
	public void setCoolTime(float value)
	{
		CooltimeMax = value;
	}

	public bool IsUseNormal()
	{
		if (Cooltime < 0.0f) 
		{
			Cooltime = CooltimeMax;
			return true;
		}
		return false;
	}
}
