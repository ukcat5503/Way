using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

	public enum SE
	{
		select,
		push,
		cancel,
		move,
		clear,
		miss
	}

	public enum BGM
	{
		Blue_Ever
	}

	static List<AudioClip> se = new List<AudioClip>();
	static List<AudioClip> bgm = new List<AudioClip>();

	static AudioSource audioSource;

	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad(this);

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

	public static void PlaySE(SE play)
	{
		if(audioSource == null){
			("audioSourceが生成されていません。\nPlay: " + play.ToString()).LogWarning();
			return;
		}

		audioSource.PlayOneShot(se[(int)play], 1f);
	}

	public static void PlayBGM(BGM play)
	{
		if(audioSource == null){
			("audioSourceが生成されていません。\nPlay: " + play.ToString()).LogWarning();
			return;
		}
		
		audioSource.clip = bgm[(int)play];
		audioSource.Play();
	}
}
