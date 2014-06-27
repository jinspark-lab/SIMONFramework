using UnityEngine;
using System.Collections;

public class arm_script_2 : MonoBehaviour {

	private Transform arm1;
	private GameObject arm1o;
	private Vector3 v;
	private float arm2_offset_y;
	private float arm2_scale_y = 1.0f;
	private float arm2_scale_x = 0.5f;
	private float arm1_halfsize = 0.0f;
	// Use this for initialization
	void Start () {
		arm1 = GameObject.Find ("Arm1").transform;
		arm1o = GameObject.Find ("Arm1");

		v = renderer.bounds.size;
		arm2_offset_y = 0.25f;//1 / v.y * (1 - arm2_scale_y)/2.0f ;
		transform.localScale = new Vector3(1/v.x*arm2_scale_x,1/v.y*arm2_scale_y,1/v.z);
		//transform.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,arm1.rotation.eulerAngles.z-90));
	}
	
	// Update is called once per frame
	void Update () {
		arm1_halfsize = 1-arm1.localScale.y / 2.0f;
		Debug.Log (-Mathf.Sin( arm1.rotation.eulerAngles.z*Mathf.Deg2Rad)*arm1.localScale.y+" "+ (Mathf.Cos(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad)*arm1.localScale.y-arm2_offset_y));
		transform.position = new Vector3(-Mathf.Sin( arm1.rotation.eulerAngles.z*Mathf.Deg2Rad)*arm1.localScale.y, Mathf.Cos(arm1.rotation.eulerAngles.z*Mathf.Deg2Rad)*arm1.localScale.y-arm1_halfsize, arm1.position.z);
		transform.rotation = Quaternion.Euler (new Vector3(arm1.rotation.eulerAngles.x,arm1.rotation.eulerAngles.y,arm1.rotation.eulerAngles.z-90));
	}
}
