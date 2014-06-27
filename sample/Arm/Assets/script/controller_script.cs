using UnityEngine;
using System.Collections;
using SIMONFramework; 

public class controller_script : MonoBehaviour {
	enum State{
		Init,
		Phase1,
		Phase2,
		Shoot,
		Finish
	}

	private int tick;
	private State state = State.Init;
	private int StateNumber=5;

	/*arm1*/
	private Vector3 arm1_size;			//script1 v
	private Vector3 arm1_axis;
	private float arm1_halfsize;
	private Transform arm1;
	private Renderer arm1_render;

	public float arm1_acceleration = 0.0f;//0.01f;
	public float arm1_velocity = 0.0f;
	public float arm1_scale_y = 1.0f;
	public float arm1_scale_x = 1.0f;

	/*arm2*/
	private Vector3 arm2_size;			//script2 v
	private Transform arm2;
	private Renderer arm2_render;
	private float arm2_angle = 90.0f;

	public float arm2_acceleration = 0.0f;//0.01f;
	public float arm2_velocity = 0.0f;
	public float arm2_scale_y = 1.0f;
	public float arm2_scale_x = 1.0f;

	/*joint*/
	private Vector3 joint_position;

	/*ball*/
	private Transform ball;
	private Renderer ball_render;
	private Vector3 ball_size;
	public Vector3 ball_direction;
	public float ball_velocity;

	public float ball_scale_x = 0.3f;
	public float ball_scale_y = 0.3f;

	/*environment*/
	public float gravity = 1.0f;
	public float max_velocity = 2.0f;
	public float velocity_offset = 100.0f;
	public float angle_error = 1.0f;
	public float position_limit = -1.0f;
	private float a12;
	//private ArrayList ObjectList = new ArrayList ();
	public int ObjectSize = 10;
	private int CurrentIteration = 0;
	public static SIMONCollection ObjectList = SIMON.GlobalSIMON.CreateSIMONCollection();
	private bool IterationFlag = false;

	/* SIMON */
	public SIMONObject controller;



	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
		tick = 0;
		//init arm1
		arm1 = GameObject.Find ("Arm1").transform;
		arm1_render = GameObject.Find ("Arm1").renderer;
		//arm1.GetComponent<SpriteRenderer> ().transform.position = new Vector3(
		arm1_size = arm1_render.bounds.size;
		
		//arm1.localScale = new Vector3(1/arm1_size.x*arm1_scale_x,1/arm1_size.y*arm1_scale_y,1/arm1_size.z);
		arm1_halfsize = transform.localScale.y / 2.0f;
		arm1_axis = new Vector3 (arm1.position.x, -arm1_halfsize-arm1.position.y, arm1.position.z);
		//Debug.Log(-halfsize-transform.position.y);
		arm1.RotateAround (arm1_axis, new Vector3 (0, 0, 1), 90f);

		//init arm2
		arm2 = GameObject.Find ("Arm2").transform;
		arm2_render = GameObject.Find ("Arm2").renderer;
		arm2_size = arm2_render.bounds.size;
		//arm2.localScale = new Vector3(1/arm2_size.x*arm2_scale_x,1/arm2_size.y*arm2_scale_y,1/arm2_size.z);

		//set arm
		joint_position = new Vector3(arm1.position.x-1.0f/2.0f*arm1.localScale.y*Mathf.Sin(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.y+1.0f/2.0f*arm1.localScale.y*Mathf.Cos(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.z);
		arm2.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,arm1.rotation.eulerAngles.z+arm2_angle));
		Vector3 arm2_normal = new Vector3(Mathf.Sin(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,Mathf.Cos(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,0);
		arm2.position = new Vector3((joint_position.x - arm2_normal.x),(joint_position.y + arm2_normal.y),arm2.position.z);

		//init ball
		ball = GameObject.Find ("Ball").transform;
		ball_render = GameObject.Find ("Ball").renderer;
		ball_size = ball_render.bounds.size;
		ball.localScale = new Vector3(1/ball_size.x*ball_scale_x,1/ball_size.y*ball_scale_y,1/ball_size.z);


		//SIMON
		float a1 = Random.Range (-87, 87);
		float a2 = Random.Range (-87, 87);
		if (a1 < 0) {
			a1 = 360+a1;
		}
		if(a2<0){
			a2 = 360+a2;
		}
		controller = new SIMONObject ();
		controller.ObjectID = "Controller"+CurrentIteration;
		controller.Properties.Add (new SIMONProperty("Angle1", a1, true));
		controller.Properties.Add (new SIMONProperty("Angle2", a2, true));
		controller.Properties.Add (new SIMONProperty("State", 0.0, false));
		controller.Properties.Add (new SIMONProperty("State1", 0.0, true));
		controller.Properties.Add (new SIMONProperty("State2", 0.0, true));
		controller.Properties.Add (new SIMONProperty("State3", 0.0, true));
		controller.Properties.Add (new SIMONProperty("Result", 0.0, false));
		controller.ObjectFitnessFunctionName = "FitnessProperty";
		controller.UpdatePropertyDNA ();

		//dummy gene
		System.Collections.Generic.List<SIMONGene> armGene = new System.Collections.Generic.List<SIMONGene> ();
		controller.Actions.Add (new SIMONAction("MoveStart", "ActionStart", "ExecutionStart", "FitnessStart", armGene));
		controller.Actions.Add (new SIMONAction("MoveArm1", "ActionArm1", "ExecutionArm1", "FitnessArm1", armGene));
		controller.Actions.Add (new SIMONAction("MoveArm2", "ActionArm2", "ExecutionArm2", "FitnessArm2", armGene));
		controller.Actions.Add (new SIMONAction("MoveShoot", "ActionShoot", "ExecutionShoot", "FitnessShoot", armGene));
		controller.Actions.Add (new SIMONAction("MoveFinish", "ActionFinish", "ExecutionFinish", "FitnessFinish", armGene));

		SIMONFunction fitnessDel = FitnessProperty;
		SIMONFunction actionStart = ActionStart;
		SIMONFunction actionArm1 = ActionArm1;
		SIMONFunction actionArm2 = ActionArm2;
		SIMONFunction actionShoot = ActionShoot;
		SIMONFunction actionFinish = ActionFinish;
		SIMONFunction executionStart = ExecutionStart;
		SIMONFunction executionArm1 = ExecutionArm1;
		SIMONFunction executionArm2 = ExecutionArm2;
		SIMONFunction executionShoot = ExecutionShoot;
		SIMONFunction executionFinish = ExecutionFinish;
		SIMONFunction fitnessStart = FitnessStart;
		SIMONFunction fitnessArm1 = FitnessArm1;
		SIMONFunction fitnessArm2 = FitnessArm2;
		SIMONFunction fitnessShoot = FitnessShoot;
		SIMONFunction fitnessFinish = FitnessFinish;


		SIMON.GlobalSIMON.InsertSIMONMethod ("FitnessProperty", fitnessDel);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ActionStart", actionStart);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ActionArm1", actionArm1);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ActionArm2", actionArm2);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ActionShoot", actionShoot);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ActionFinish", actionFinish);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ExecutionStart", executionStart);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ExecutionArm1", executionArm1);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ExecutionArm2", executionArm2);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ExecutionShoot", executionShoot);
		SIMON.GlobalSIMON.InsertSIMONMethod ("ExecutionFinish", executionFinish);
		SIMON.GlobalSIMON.InsertSIMONMethod ("FitnessStart", fitnessStart);
		SIMON.GlobalSIMON.InsertSIMONMethod ("FitnessArm1", fitnessArm1);
		SIMON.GlobalSIMON.InsertSIMONMethod ("FitnessArm2", fitnessArm2);
		SIMON.GlobalSIMON.InsertSIMONMethod ("FitnessShoot", fitnessShoot);
		SIMON.GlobalSIMON.InsertSIMONMethod ("FitnessFinish", fitnessFinish);

		SIMON.GlobalSIMON.InsertSIMONObject (controller);

		a12 = (float)controller.GetPropertyElement("Angle2")+(float)controller.GetPropertyElement("Angle1");
		a12 %= 360;
	}

    public object FitnessProperty(SIMONObject s, SIMONObject[] o){
		return s.GetPropertyElement ("Result");
	}

	public object ActionStart(SIMONObject s, SIMONObject[] o){
		if((int)s.GetPropertyElement("State")==(int)State.Init)
			return 1.0;
		else
			return 0.0;
	}

	public object ActionArm1(SIMONObject s, SIMONObject[] o){
		if((int)s.GetPropertyElement("State")==(int)State.Phase1)
			return 1.0;
		else
			return 0.0;
	}

	public object ActionArm2(SIMONObject s, SIMONObject[] o){
		if((int)s.GetPropertyElement("State")==(int)State.Phase2)
			return 1.0;
		else
			return 0.0;
	}

	public object ActionShoot(SIMONObject s, SIMONObject[] o){
		if((int)s.GetPropertyElement("State")==(int)State.Shoot)
			return 1.0;
		else
			return 0.0;
	}

	public object ActionFinish(SIMONObject s, SIMONObject[] b){
		if((int)s.GetPropertyElement("State")==(int)State.Finish)
			return 1.0;
		else
			return 0.0;
	}

	public object ExecutionStart(SIMONObject s, SIMONObject[] o){
		state = (State)(((int)state + 1) % StateNumber);
		s.SetPropertyElement ("State", (double)State.Phase1);
		return 1.0;
	}

	public object ExecutionArm1(SIMONObject s, SIMONObject[] o){
		//Debug.Log((double)s.GetPropertyElement("Angle1")+" "+arm1.rotation.eulerAngles.z+" "+(arm1.rotation.eulerAngles.z-(float)s.GetPropertyElement("Angle1")));
		if(Mathf.Abs( arm1.rotation.eulerAngles.z-(float)s.GetPropertyElement("Angle1")) <= angle_error )
		{
			state = (State)(((int)state + 1) % StateNumber);
			s.SetPropertyElement ("State", (double)State.Phase2);
		}
		return 1.0;
	}

	public object ExecutionArm2(SIMONObject s, SIMONObject[] o){
		//Debug.Log((double)s.GetPropertyElement("Angle2")+" "+arm2.rotation.eulerAngles.z+" "+(arm2.rotation.eulerAngles.z-(float)s.GetPropertyElement("Angle2")));

		if(Mathf.Abs( arm2.rotation.eulerAngles.z-a12) <= angle_error )
		{
			//Debug.Log((double)s.GetPropertyElement("Angle2")+" "+arm2.rotation.eulerAngles.z+" "+(arm2.rotation.eulerAngles.z-(float)s.GetPropertyElement("Angle2")));
			state = (State)(((int)state + 1) % StateNumber);
			s.SetPropertyElement ("State", (double)State.Shoot);
		}
		return 1.0;
	}

	public object ExecutionShoot(SIMONObject s, SIMONObject[] o){
		if(ball.position.y < position_limit)
		{
			state = (State)(((int)state + 1) % StateNumber);
			s.SetPropertyElement ("State", (double)State.Finish);
		}
		return 1.0;
	}

	public object ExecutionFinish(SIMONObject s, SIMONObject[] o){
		if(CurrentIteration<ObjectSize)
		{
			Debug.Log(CurrentIteration+" "+s.GetPropertyElement("Angle1")+" "+s.GetPropertyElement("Angle2")+" "+ball.position.x);
			save (s);
			init ();
			state = (State)(((int)state + 1) % StateNumber);
			s.SetPropertyElement ("State", (double)State.Init);
		}
		else //save & learn
		{
			SIMON.GlobalSIMON.SIMONLearn(ObjectList);

			IterationFlag = true;
			save (s);
			init ();
			state = (State)(((int)state + 1) % StateNumber);
			s.SetPropertyElement ("State", (double)State.Init);
		}

		return 1.0;
	}

	/*public object FitnessProperty(SIMONObject s, SIMONObject[] o){
		return 1;
	}*/

	public object FitnessStart(SIMONObject s, SIMONObject[] o){
		return 1.0;
	}

	public object FitnessArm1(SIMONObject s, SIMONObject[] o){
		return 1.0;
	}

	public object FitnessArm2(SIMONObject s, SIMONObject[] o){
		return 1.0;
	}

	public object FitnessShoot(SIMONObject s, SIMONObject[] o){
		return 1.0;
	}

	public object FitnessFinish(SIMONObject s, SIMONObject[] o){
		return (double)1;
	}

	void init()
	{
		SIMON.GlobalSIMON.DeleteSIMONObject ("Controller" + (CurrentIteration-1));
		if(CurrentIteration == ObjectSize+1) CurrentIteration = 0;

		arm1.RotateAround (arm1_axis, new Vector3 (0, 0, 1), (360.0f-arm1.rotation.eulerAngles.z)+90.0f);
		arm2_angle = 90.0f;
		//arm2.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,0.0f));
		//set arm
		joint_position = new Vector3(arm1.position.x-1.0f/2.0f*arm1.localScale.y*Mathf.Sin(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.y+1.0f/2.0f*arm1.localScale.y*Mathf.Cos(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.z);
		arm2.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,arm1.rotation.eulerAngles.z+arm2_angle));
		Vector3 arm2_normal = new Vector3(Mathf.Sin(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,Mathf.Cos(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,0);
		arm2.position = new Vector3((joint_position.x - arm2_normal.x),(joint_position.y + arm2_normal.y),arm2.position.z);

		//SIMON
		if(!IterationFlag){
			float a1 = Random.Range (-87, 87);
			float a2 = Random.Range (-87, 87);
			if (a1 < 0) {
				a1 = 360+a1;
			}
			if(a2<0){
				a2 = 360+a2;
			}
			controller = new SIMONObject ();
			controller.ObjectID = "Controller"+CurrentIteration;
			controller.Properties.Add (new SIMONProperty("Angle1", a1, true));
			controller.Properties.Add (new SIMONProperty("Angle2", a2, true));
			controller.Properties.Add (new SIMONProperty("State", 0.0, true));
			controller.Properties.Add (new SIMONProperty("Result", 0.0, false));
			controller.ObjectFitnessFunctionName = "FitnessProperty";
			controller.UpdatePropertyDNA ();
			
			//dummy gene
			System.Collections.Generic.List<SIMONGene> armGene = new System.Collections.Generic.List<SIMONGene> ();
			controller.Actions.Add (new SIMONAction("MoveStart", "ActionStart", "ExecutionStart", "FitnessStart", armGene));
			controller.Actions.Add (new SIMONAction("MoveArm1", "ActionArm1", "ExecutionArm1", "FitnessArm1", armGene));
			controller.Actions.Add (new SIMONAction("MoveArm2", "ActionArm2", "ExecutionArm2", "FitnessArm2", armGene));
			controller.Actions.Add (new SIMONAction("MoveShoot", "ActionShoot", "ExecutionShoot", "FitnessShoot", armGene));
			controller.Actions.Add (new SIMONAction("MoveFinish", "ActionFinish", "ExecutionFinish", "FitnessFinish", armGene));	
			

		}
		else{
			//CurrentIteration = 0;
			controller = (SIMONObject)ObjectList.ValueOfIndex(CurrentIteration);
		}
		SIMON.GlobalSIMON.InsertSIMONObject (controller);
		a12 = (float)controller.GetPropertyElement("Angle2")+(float)controller.GetPropertyElement("Angle1");
		a12 %= 360;
	}

	void init2()
	{

	}

	void save(SIMONObject s)
	{
		s.SetPropertyElement("Result", ball.position.x);
		ObjectList.Add (s.ObjectID, s);
		CurrentIteration++;
	}

	void save2(SIMONObject s)
	{
		s.SetPropertyElement("Result", ball.position.x);
		//ObjectList.Add (s.ObjectID, s);
		CurrentIteration++;
	}

	// Update is called once per frame
	void Update () {
		Time.timeScale = 1.0f;
		//Debug.Log (state);
		//Debug.Log(state+" "+(double)controller.GetPropertyElement("Angle1")+" "+arm1.rotation.eulerAngles.z+" "+(arm1.rotation.eulerAngles.z-(float)controller.GetPropertyElement("Angle1"))+" "+(double)controller.GetPropertyElement("Angle2")+" "+arm2.rotation.eulerAngles.z+" "+(arm2.rotation.eulerAngles.z-(float)controller.GetPropertyElement("Angle2")));
		if ((tick %= 1) == 0) 
		{
			GetInput ();
			MoveArm ();
			Shoot ();

		}
		tick++;
	}

	void MoveArm()
	{

			if(state == State.Phase1)
			{
				MoveArm1();
			}
			else if(state == State.Phase2)
			{
				MoveArm2();
				SetVelocity();
				SetDirection();
			}
	}

	void GetInput()
	{
		if (Input.GetKeyDown ("space")){}
			//state = (State)(((int)state + 1) % StateNumber);
		//Debug.Log (state);
	}

	void MoveArm1()
	{
		//Debug.Log (arm1_acceleration + " " + arm1_velocity);
		if(Mathf.Abs((float)controller.GetPropertyElement("Angle1")-arm1.rotation.eulerAngles.z)>angle_error){
			arm1.RotateAround (arm1_axis, new Vector3 (0, 0, 1), -arm1_velocity);
			//arm2_angle-= 0.5f;
		}else{
			//arm1.RotateAround (arm1_axis, new Vector3 (0, 0, 1), -180f);
			//arm2_angle = 90.0f;
			//state = (State)(((int)state + 1) % StateNumber);
		}
		joint_position = new Vector3(arm1.position.x-1.0f/2.0f*arm1.localScale.y*Mathf.Sin(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.y+1.0f/2.0f*arm1.localScale.y*Mathf.Cos(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.z);
		arm2.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,arm1.rotation.eulerAngles.z+arm2_angle));
		Vector3 arm2_normal = new Vector3(Mathf.Sin(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,Mathf.Cos(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,0);
		arm2.position = new Vector3((joint_position.x - arm2_normal.x),(joint_position.y + arm2_normal.y),arm2.position.z);
		if(arm1_velocity<max_velocity)
			arm1_velocity += arm1_acceleration;
	}

	void MoveArm2()
	{

		if(Mathf.Abs(a12-arm2.rotation.eulerAngles.z)>angle_error){
			arm2_angle-= arm2_velocity;
			joint_position = new Vector3(arm1.position.x-1.0f/2.0f*arm1.localScale.y*Mathf.Sin(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.y+1.0f/2.0f*arm1.localScale.y*Mathf.Cos(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad), arm1.position.z);
			arm2.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,arm1.rotation.eulerAngles.z+arm2_angle));
			Vector3 arm2_normal = new Vector3(Mathf.Sin(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,Mathf.Cos(arm2.rotation.eulerAngles.z*Mathf.Deg2Rad)/2.0f*arm2.localScale.y,0);
			arm2.position = new Vector3((joint_position.x - arm2_normal.x),(joint_position.y + arm2_normal.y),arm2.position.z);
		}else{
			//arm1.RotateAround (arm1_axis, new Vector3 (0, 0, 1), -180f);
			//arm2_angle = 90.0f;
			SetVelocity();
			SetDirection();
			//state = (State)(((int)state + 1) % StateNumber);
		}
		if(arm2_velocity<max_velocity)
			arm2_velocity+=arm2_acceleration;
	}

	void SetDirection()
	{
		float angle = arm2.rotation.eulerAngles.z - 90;
		ball_direction = new Vector3 (-ball_velocity * Mathf.Sin (angle * Mathf.Deg2Rad), ball_velocity * Mathf.Cos (angle * Mathf.Deg2Rad), 0);
	}

	void SetVelocity()
	{
		float circumference = ball.localScale.x * 2.0f * Mathf.PI;
		float anglevelocity = arm1_velocity / 360.0f;
		ball_velocity = circumference * anglevelocity * velocity_offset;
		//Debug.Log (anglevelocity+" "+ball.localScale.x+" "+ball_velocity);
	}

	void Shoot()
	{
		if(state == State.Shoot)
		{
			if(ball.position.y > position_limit)
			{
				ball.position = new Vector3(ball.position.x + ball_direction.x, ball.position.y+ball_direction.y, ball.position.z);
				ball_direction.y-=gravity;
			}
			else
			{
				//state = (State)(((int)state + 1) % StateNumber);
			}
		}
		else if(state == State.Finish)
		{

		}
		else
		{
			float angle = arm2.rotation.eulerAngles.z;
			ball.position = new Vector3(arm2.position.x-Mathf.Sin (angle * Mathf.Deg2Rad)*arm2_size.y/3.0f,arm2.position.y+Mathf.Cos (angle * Mathf.Deg2Rad)*arm2_size.y/3.0f,arm2.position.z);
		}
	}

	void Finish()
	{
		if(state == State.Finish)
		{

		}
	}
}