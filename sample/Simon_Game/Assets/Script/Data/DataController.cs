using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	public static DataController dataController;

	public struct DataSet {
		public float AttackQuantity;
		public float DamageQuantity;
		public float DefenseQuantity;

		public float SkillCount;
	}

	public DataSet[] RedDataSet = new DataSet[7];
	public DataSet[] BlueDataSet = new DataSet[7];

	// Use this for initialization
	void Start () {
		dataController = this;
	}
	
	// Update is called once per frame
	void Update () {
		float tmpDamageQuan = 0.0f;
		for(int i=0; i<7; i++)
		{
			tmpDamageQuan += RedDataSet[i].DamageQuantity;
		}
		Debug.Log (tmpDamageQuan);
	}
}
