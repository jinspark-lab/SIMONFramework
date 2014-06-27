using UnityEngine;
using System.Collections;

public class arm_script_1 : MonoBehaviour {

	private int tick;
	private Vector3 v;
	private Vector3 arm1_axis;
	public float halfsize;

	// Use this for initialization
	void Start () {
		tick = 0;
		v = renderer.bounds.size;

		transform.localScale = new Vector3(1/v.x*0.5f,1/v.y*1.5f,1/v.z);
		halfsize = transform.localScale.y / 2.0f;
		arm1_axis = new Vector3 (transform.position.x, -halfsize-transform.position.y, transform.position.z);
		//Debug.Log(-halfsize-transform.position.y);
		transform.RotateAround (arm1_axis, new Vector3 (0, 0, 1), 90f);
	}
	
	// Update is called once per frame
	void Update () {
		if ((tick %= 1) == 0) 
		{
			if(transform.rotation.eulerAngles.z<=90||transform.rotation.eulerAngles.z>=270){
				transform.RotateAround (arm1_axis, new Vector3 (0, 0, 1), -1f);
			}else{
				transform.RotateAround (arm1_axis, new Vector3 (0, 0, 1), -180f);
			}
			//Debug.Log(transform.rotation.x+" "+transform.rotation.y+" "+transform.rotation.z);
		}
		tick++;

	}
}
