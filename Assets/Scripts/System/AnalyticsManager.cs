using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour {

	[SerializeField]
	GameObject GAv4Prefab;
	
	static GoogleAnalyticsV4 googleAnalytics;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);

		googleAnalytics = (Instantiate(GAv4Prefab) as GameObject).GetComponent<GoogleAnalyticsV4>();
	}

	public static void StartSession(){
		if(googleAnalytics != null){
			googleAnalytics.StartSession();
		}else{
			"googleAnalyticsがnullです".LogWarning();
		}
		
	}

	public static void StopSession(){
		if(googleAnalytics != null){
			googleAnalytics.StopSession();
		}else{
			"googleAnalyticsがnullです".LogWarning();
		}
	}
	
	public static void LogScreen(string eventName){
		if(googleAnalytics != null){
			googleAnalytics.LogScreen(eventName);
		}else{
			"googleAnalyticsがnullです".LogWarning();
		}
	}

	public static void LogEvent(string eventCategory, string eventAction, string eventLabel, long value){
		if(googleAnalytics != null){
			googleAnalytics.LogEvent(eventCategory, eventAction, eventLabel, value);
		}else{
			"googleAnalyticsがnullです".LogWarning();
		}
	}
}
