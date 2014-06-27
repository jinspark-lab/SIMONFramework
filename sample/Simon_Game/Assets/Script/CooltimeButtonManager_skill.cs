using UnityEngine;
using System.Collections;

public class CooltimeButtonManager_skill : MonoBehaviour {

	public static CooltimeButtonManager_skill SkillAttackManager;

	public float CooltimeMax_skill = 5.0f;
	public float Cooltime_skill;

	public GameObject Q;
	public GameObject W;
	public GameObject E;

	public UISlider slider_Q;
	public UISlider slider_W;
	public UISlider slider_E;
	
	// Use this for initialization
	void Start () {
		SkillAttackManager = this;

		slider_Q = Q.GetComponent<UISlider> ();
		slider_Q.fillDirection = UISlider.FillDirection.BottomToTop;
		slider_W = W.GetComponent<UISlider> ();
		slider_W.fillDirection = UISlider.FillDirection.BottomToTop;
		slider_E = E.GetComponent<UISlider> ();
		slider_E.fillDirection = UISlider.FillDirection.BottomToTop;

		Cooltime_skill = CooltimeMax_skill;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Cooltime_skill > 0.0f) 
		{
			Cooltime_skill -= Time.deltaTime;
			float sliderPercent = 1.0f - Cooltime_skill/CooltimeMax_skill;
			slider_Q.value = sliderPercent;
			slider_W.value = sliderPercent;
			slider_E.value = sliderPercent;
		}
	}
	
	public bool IsUseSkill()
	{
		if (Cooltime_skill < 0.0f) 
		{
			return true;
		}
		return false;
	}

	public void CoolTime_Start()
	{
		Cooltime_skill = CooltimeMax_skill;
	}
}
