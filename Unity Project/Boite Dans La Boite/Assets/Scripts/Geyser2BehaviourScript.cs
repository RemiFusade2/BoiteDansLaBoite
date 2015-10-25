using UnityEngine;
using System.Collections;

public class Geyser2BehaviourScript : MonoBehaviour {

	public float warningTime;

	private bool isAlreadyFiring;

	// Use this for initialization
	void Start () 
	{
		isAlreadyFiring = false;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		//P2 special power 2
		if (Input.GetKeyDown ("[2]") && !isAlreadyFiring) 
		{
			isAlreadyFiring = true;
			StartCountdown();
		}	
	}

	public void StartCountdown()
	{
		this.GetComponent<Animator> ().SetTrigger ("Warning"); // l'animator est une machine à états, il gère tout seul le passage d'un état à un autre
		StartCoroutine (WaitAndFire (warningTime)); // les coroutines permettent d'éviter les timers dans le code
	}

	IEnumerator WaitAndFire (float time)
	{
		yield return new WaitForSeconds (time);
		Fire ();
	}

	public void Fire()
	{
		this.GetComponent<Animator> ().SetTrigger ("Fire");
	}

	// Cette méthode est appelée par l'animation "GeyserFire"
	public void SetGeyserReady()
	{
		isAlreadyFiring = false;
	}
}
