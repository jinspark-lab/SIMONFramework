
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SIMONFramework;

public static class SIMON{
	public static SIMONUnity GlobalSIMON = new SIMONUnity();
}

public sealed class SIMONUnity : MonoBehaviour {
	private static SIMONManager SimonManager = new SIMONManager();
	public SIMONUserFunction SimonFunctionManager = new SIMONUserFunction();

	// Use this for initialization
	void Start () {
		InitSIMONEnvironment ();
	}
	// Update is called once per frame
	void Update () {
		SimonManager.RunRoutine (10);
	}

	private void InitSIMONEnvironment(){
		SimonManager.LoadWorkSpace (null);
	}
	public void InitSIMONEnvironment(string projectName){
		SimonManager.LoadWorkSpace (projectName);
	}
	public void CleanSIMONEnvironment(){
		SimonManager.CleanWorkSpace ();
	}
	public void CleanSIMONGroup(SIMONCollection Group){
		SimonManager.CleanGroup (Group);
	}
	public void CleanSIMONDefinedEnvironment(){
		SimonManager.CleanDefinitionWorkSpace();
	}
	public SIMONObject LoadSIMONDefinition(string objectID){
		return SimonManager.CopyDefinitionObject (objectID);
	}
	public SIMONObject LoadSIMONDefinition(string definedPath, string objectID){
		SimonManager.LoadWorkSpace (definedPath);
		return SimonManager.CopyDefinitionObject (objectID);
	}
	public SIMONCollection CreateSIMONCollection(){
		return SimonManager.CreateSIMONGroup();
	}
	public SIMONCollection LoadSIMONCollection(){
		return SimonManager.SimonObjectCollections;
	}
	public void PublishDefinition(string fileName, SIMONObject source){
		SimonManager.PublishObject (fileName, source);
	}
	public void SIMONLearn(SIMONCollection Group){
		SimonManager.LearnRoutine(Group);
	}
	public void SIMONLearnSimulate(SIMONCollection Group, double learningRate){
		SimonManager.LearnSimulate(Group, learningRate);
	}
	public void InsertSIMONObject(SIMONObject sObject){
		SimonManager.RegisterSIMONObject (sObject);
	}
	public void InsertSIMONObject(string key, SIMONObject sObject){
		SimonManager.RegisterSIMONObject (key, sObject);
	}
	public void DeleteSIMONObject(SIMONObject sObject){
		SimonManager.UnregisterSIMONObject (sObject);
	}
	public void DeleteSIMONObject(string key){
		SimonManager.UnregisterSIMONObject (key);
	}

	public void InsertSIMONMethod(string methodName){
		SimonManager.AddMethod (methodName);
	}
	public void InsertSIMONMethod(string methodName, SIMONFunction methodPointer){
		SimonManager.InsertMethod (methodName, methodPointer);
	}
	public void InsertSIMONMethod(string methodName, Object classType){
		SimonManager.InsertMethod (methodName, classType);
	}
	public void DeleteSIMONMethod(string methodName){
		SimonManager.RemoveMethod (methodName);
	}
}
