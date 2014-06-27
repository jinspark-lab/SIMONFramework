
using System;
using System.Collections;
using SIMONFramework;
using UnityEngine;

public class SIMONUserFunction : SIMONFunctionInterface{
	public string[] GetFunctionList(){
		string[] arr = { "Move" };
		return arr;
	}
	public int GetFunctionCount(){
		return 1;
	}
	public object Move(SIMONObject[] obj, SIMONObject[] obj2){
		Debug.Log ("Move Call");
		return null;
	}
}
