using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SoundDicoElement
{
	public string name;
	public AudioSource sound;
}

public class SoundEngineScript : MonoBehaviour {

	public AudioSource menuBkgMusic;
	
	public AudioSource blocksBkgMusicIntro;
	public AudioSource blocksBkgMusicLoop;
		
	public AudioSource forestBkgMusicIntro;
	public AudioSource forestBkgMusicLoop;
		
	public AudioSource ghostBkgMusicIntro;
	public AudioSource ghostBkgMusicLoop;
	
	public AudioSource princessBkgMusicIntro;
	public AudioSource princessBkgMusicLoop;

	public float fadingTimeIntro;
	public float fadingTimeBetweenMusics;

	private AudioSource currentBkgMusicIntro;
	private AudioSource currentBkgMusicLoop;

	private List<Coroutine> allCoroutines;
	
	public List<SoundDicoElement> availableSounds;
	private Dictionary<string, AudioSource> soundDico;

	// Use this for initialization
	void Start () 
	{
		allCoroutines = new List<Coroutine> ();

		soundDico = new Dictionary<string, AudioSource> ();
		foreach (SoundDicoElement elem in availableSounds)
		{
			soundDico.Add(elem.name, elem.sound);
		}

		currentBkgMusicIntro = menuBkgMusic;
		currentBkgMusicLoop = menuBkgMusic;

		FadeInMusic (menuBkgMusic, fadingTimeIntro);
	}

	private void FadeInMusic(AudioSource music, float time)
	{
		float minVolume = music.volume;
		music.Play ();
		int numberOfSteps = 10;
		for (int i = 1; i <= numberOfSteps ; i++)
		{
			allCoroutines.Add(StartCoroutine(WaitAndChangeMusicVolume(i*(time/numberOfSteps), music, minVolume + (i * (1.0f-minVolume) / numberOfSteps))));
		}
		currentBkgMusicIntro = music;
	}
	private void FadeOutMusic(AudioSource music, float time)
	{
		float maxVolume = music.volume;
		int numberOfSteps = 10;
		for (int i = 1; i <= numberOfSteps ; i++)
		{
			allCoroutines.Add(StartCoroutine(WaitAndChangeMusicVolume(i*(time/numberOfSteps), music, ((numberOfSteps-i) * maxVolume / numberOfSteps))));
		}
	}

	IEnumerator WaitAndChangeMusicVolume(float timer, AudioSource music, float volume)
	{
		yield return new WaitForSeconds (timer);
		music.volume = volume;
		if (volume <= 0.01f)
		{
			music.Stop();
		}
	}

	public void PlayBlocksBkgMusic()
	{
		foreach (Coroutine coroutine in allCoroutines)
		{
			StopCoroutine(coroutine);
		}
		FadeOutMusic (currentBkgMusicIntro, fadingTimeBetweenMusics);
		FadeOutMusic (currentBkgMusicLoop, fadingTimeBetweenMusics);
		FadeInMusic (blocksBkgMusicIntro, fadingTimeBetweenMusics);
		currentBkgMusicIntro = blocksBkgMusicIntro;
		currentBkgMusicLoop = blocksBkgMusicLoop;
		allCoroutines.Add (StartCoroutine (WaitAndPlaySound (blocksBkgMusicIntro.clip.length, blocksBkgMusicLoop)));
	}
	
	public void PlayForestBkgMusic()
	{
		foreach (Coroutine coroutine in allCoroutines)
		{
			StopCoroutine(coroutine);
		}
		FadeOutMusic (currentBkgMusicIntro, fadingTimeBetweenMusics);
		FadeOutMusic (currentBkgMusicLoop, fadingTimeBetweenMusics);
		FadeInMusic (forestBkgMusicIntro, fadingTimeBetweenMusics);
		currentBkgMusicIntro = forestBkgMusicIntro;
		currentBkgMusicLoop = forestBkgMusicLoop;
		allCoroutines.Add (StartCoroutine (WaitAndPlaySound (forestBkgMusicIntro.clip.length, forestBkgMusicLoop)));
	}
	
	public void PlayGhostBkgMusic()
	{
		foreach (Coroutine coroutine in allCoroutines)
		{
			StopCoroutine(coroutine);
		}
		FadeOutMusic (currentBkgMusicIntro, fadingTimeBetweenMusics);
		FadeOutMusic (currentBkgMusicLoop, fadingTimeBetweenMusics);
		FadeInMusic (ghostBkgMusicIntro, fadingTimeBetweenMusics);
		currentBkgMusicIntro = ghostBkgMusicIntro;
		currentBkgMusicLoop = ghostBkgMusicLoop;
		allCoroutines.Add (StartCoroutine (WaitAndPlaySound (ghostBkgMusicIntro.clip.length, ghostBkgMusicLoop)));
	}
	
	public void PlayPrincessBkgMusic()
	{
		foreach (Coroutine coroutine in allCoroutines)
		{
			StopCoroutine(coroutine);
		}
		FadeOutMusic (currentBkgMusicIntro, fadingTimeBetweenMusics);
		FadeOutMusic (currentBkgMusicLoop, fadingTimeBetweenMusics);
		FadeInMusic (princessBkgMusicIntro, fadingTimeBetweenMusics);
		currentBkgMusicIntro = princessBkgMusicIntro;
		currentBkgMusicLoop = princessBkgMusicLoop;
		allCoroutines.Add (StartCoroutine (WaitAndPlaySound (princessBkgMusicIntro.clip.length, princessBkgMusicLoop)));
	}
	
	public void PlayMenuBkgMusic()
	{
		foreach (Coroutine coroutine in allCoroutines)
		{
			StopCoroutine(coroutine);
		}
		FadeOutMusic (currentBkgMusicIntro, fadingTimeBetweenMusics);
		FadeOutMusic (currentBkgMusicLoop, fadingTimeBetweenMusics);
		FadeInMusic (menuBkgMusic, fadingTimeBetweenMusics);
		currentBkgMusicIntro = menuBkgMusic;
		currentBkgMusicLoop = menuBkgMusic;
	}

	IEnumerator WaitAndPlaySound(float timer, AudioSource sound)
	{
		yield return new WaitForSeconds (timer);
		sound.volume = 1;
		sound.Play ();
	}

	public void PlaySound(string name)
	{
		soundDico [name].Play ();
	}
}
