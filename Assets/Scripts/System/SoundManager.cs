using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public enum SE
	{
		GenerateBuilding,
		ShotPlayer
	}

	public enum BGM
	{
		RunTrolley
	}

	static List<AudioClip> se = new List<AudioClip>();
	static List<AudioClip> bgm = new List<AudioClip>();

	static AudioSource audioSource;

	// Use this for initialization
	void Awake () {
		audioSource = GetComponent<AudioSource>();

		foreach (SE key in SE.GetValues(typeof(SE)))
		{
			se.Add(Resources.Load("Audio/SE/" + key.ToString()) as AudioClip);
		}

		foreach (BGM key in BGM.GetValues(typeof(BGM)))
		{
			bgm.Add(Resources.Load("Audio/BGM/" + key.ToString()) as AudioClip);
		}

	}

	void Start(){
		// PlayBGM(BGM.RunTrolley);
	}

	public static void PlaySE(SE play){
		audioSource.PlayOneShot(se[(int)play], 1f);
	}

	public static void PlayBGM(BGM play){
		audioSource.clip = bgm[(int)play];
		audioSource.Play();
	}	
}
