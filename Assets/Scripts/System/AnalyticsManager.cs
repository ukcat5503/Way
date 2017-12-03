using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour {

	[SerializeField]
	GameObject GAv4Prefab;
	static GoogleAnalyticsV4 googleAnalytics;

	// Use this for initialization
	void Start () {
		googleAnalytics = (Instantiate(GAv4Prefab) as GameObject).GetComponent<GoogleAnalyticsV4>();
	}

	public static void StartSession(){
		googleAnalytics.StartSession();
	}

	public static void StopSession(){
		googleAnalytics.StopSession();
	}
	
	public static void LogScreen(string eventName){
		googleAnalytics.LogScreen(eventName);
	}
	
	public static void LogEvent(string eventCategory, string eventAction, string eventLabel, long value){
		googleAnalytics.LogEvent(eventCategory, eventAction, eventLabel, value);
	}
}
