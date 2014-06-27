using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

	public class ScoreEntry
	{
		public string ID;
		public string Team;
		public float AttackQuantity;
		public float DamageQuantity;

		public float CON;
		public float Strength;
		public float Attack_Speed;
		public float Moving_Speed;
		public float Defensive;
		public float Critical;
		public float Range;
	}

	public static List<ScoreEntry> highScore =  new List<ScoreEntry>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
