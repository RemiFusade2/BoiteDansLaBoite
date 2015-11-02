using UnityEngine;
using System.Collections;

public class Geyser2BehaviourScript : MonoBehaviour {

	public float warningTime;

	private bool isAlreadyFiring;

	public bool P1isHunted = true;

	public SoundEngineScript soundEngine;

	public RoundManager roundManager;

	// Use this for initialization
	void Start () 
	{
		isAlreadyFiring = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (roundManager.IsGeyserAvailable())
		{
			//P2 special power 2
			if (P1isHunted) 
			{
				if (Input.GetKeyDown ("[2]") && !isAlreadyFiring || Input.GetButtonDown ("Fire2P2") && !isAlreadyFiring) 
				{
					isAlreadyFiring = true;
					StartCountdown ();
					StartCoroutine (WaitAndLockGeyser (0.1f));
				}	
			}
			else if (!P1isHunted) 
			{
				if (Input.GetKeyDown ("[2]") && !isAlreadyFiring || Input.GetButtonDown ("Fire2") && !isAlreadyFiring) 
				{
					isAlreadyFiring = true;
					StartCountdown ();
					StartCoroutine (WaitAndLockGeyser (0.1f));
				}	
			}
		}
	}

	public void StartCountdown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Warning"); // l'animator est une machine à états, il gère tout seul le passage d'un état à un autre
		StartCoroutine (WaitAndFire (warningTime)); // les coroutines permettent d'éviter les timers dans le code
	}
	
	IEnumerator WaitAndLockGeyser (float time)
	{
		yield return new WaitForSeconds (time);
		roundManager.UseGeyser();
	}

	IEnumerator WaitAndFire (float time)
	{
		yield return new WaitForSeconds (time);
		Fire ();
	}

	public void Fire()
	{
		this.GetComponent<Animator> ().SetTrigger ("Fire");
		if (soundEngine != null)
		{
			soundEngine.PlaySound ("geyser");
		}
	}

	// Cette méthode est appelée par l'animation "GeyserFire"
	public void SetGeyserReady()
	{
		isAlreadyFiring = false;
	}
}
