using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {

	public GameObject logoSound;

	IEnumerator NowStarting()
	{
		logoSound.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds(4.0f); 
		Application.LoadLevel (5);
	}
	
	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
		StartCoroutine (NowStarting ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
