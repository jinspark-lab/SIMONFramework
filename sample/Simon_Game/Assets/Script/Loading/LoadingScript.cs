using UnityEngine;
using System.Collections;

public class LoadingScript : MonoBehaviour {


	IEnumerator NowLoading()
	{
		yield return new WaitForSeconds(3.0f); 
		Application.LoadLevel (0);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (NowLoading ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
