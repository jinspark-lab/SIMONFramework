using UnityEngine;
using System.Collections;

public class SnowBall_Controller : MonoBehaviour {
	public GameObject Target;
	public GameObject SnowBall;
	public GameObject MovingTarget;
	public string TargetName;
	public float Attack_Speed;
	public bool ischeck;
	public float damage;
	// Use this for initialization
	void Start () {
		SnowBall = this.gameObject;
		ischeck = false;
		Attack_Speed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		MovingTarget.transform.localPosition = Vector3.MoveTowards (
			MovingTarget.transform.position,
			Target.transform.position,
			Time.deltaTime * 5.0f);
		
		SnowBall.transform.localPosition = Vector3.MoveTowards (
			SnowBall.transform.position, 
			MovingTarget.transform.position, 
			Time.deltaTime * Attack_Speed);
	}

	public void setTarget(string name,float d)
	{
		TargetName = name;
		damage = d;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name.Equals(Target.name)) 
		{
//			Debug.Log (other.gameObject.name + "  -> SnowBall!!!");
			Destroy (MovingTarget);
			Destroy (SnowBall);

		}


		//DestroyObject(other.gameObject);
	}



	void Attack(GameObject target ){
		Target = target;
		
	}
	
	void NormalAttackSetting(GameObject movingTarget)
	{
		MovingTarget = movingTarget;
	}
}
