using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

	public GUIStyle myGui;

	public float initialTimer = 250.0f;
	public float timer = 0.0f;
	public float interRoundTimer = 3.0f;
	public int currentRound = 0;

	public bool interRound = false;
	public bool upScore = false;
	bool gameOver = false;

	bool showTimerInterRound = false;
	bool startTimer = true;

	public int playerOneScore = 0;
	public int playerTwoScore = 0;

	public GameObject playerScript;
	public GameObject rotationScript;
	public GameObject[] geyserScript;
	public GameObject facetteScript;

	public int counter = 0;

	public Text player1ScoreUI;
	public Text player2ScoreUI;

	public GameObject inGameUI;

	public GameObject pouvoirsP1Panel;
	public GameObject pouvoirsP2Panel;
	public Image pouvoirRotationP1;
	public Image pouvoirRotationP2;
	public Sprite pouvoirRotationAvailable;
	public Sprite pouvoirRotationNotAvailable;
	public Image pouvoirSwitchP1;
	public Image pouvoirSwitchP2;
	public Sprite pouvoirSwitchAvailable;
	public Sprite pouvoirSwitchNotAvailable;
	public Image pouvoirGeyserP1;
	public Image pouvoirGeyserP2;
	public Sprite pouvoirGeyserAvailable;
	public Sprite pouvoirGeyserNotAvailable;

	public float pouvoirRotationTimer;
	private float lastRotationTime;
	
	public float pouvoirSwitchTimer;
	private float lastSwitchTime;
	
	public float pouvoirGeyserTimer;
	private float lastGeyserTime;

	private bool gameIsRunning;

	public bool IsGameRunning()
	{
		return gameIsRunning;
	}

	public bool IsRotationAvailable()
	{
		return (Time.time - lastRotationTime) > pouvoirRotationTimer;
	}
	public bool IsSwitchAvailable()
	{
		return (Time.time - lastSwitchTime) > pouvoirSwitchTimer;
	}
	public bool IsGeyserAvailable()
	{
		return (Time.time - lastGeyserTime) > pouvoirGeyserTimer;
	}

	private void UpdatePouvoirsAvailability()
	{
		pouvoirRotationP1.sprite = IsRotationAvailable() ? pouvoirRotationAvailable : pouvoirRotationNotAvailable;
		pouvoirRotationP2.sprite = IsRotationAvailable() ? pouvoirRotationAvailable : pouvoirRotationNotAvailable;
		pouvoirSwitchP1.sprite = IsSwitchAvailable() ? pouvoirSwitchAvailable : pouvoirSwitchNotAvailable;
		pouvoirSwitchP2.sprite = IsSwitchAvailable() ? pouvoirSwitchAvailable : pouvoirSwitchNotAvailable;
		pouvoirGeyserP1.sprite = IsGeyserAvailable() ? pouvoirGeyserAvailable : pouvoirGeyserNotAvailable;
		pouvoirGeyserP2.sprite = IsGeyserAvailable() ? pouvoirGeyserAvailable : pouvoirGeyserNotAvailable;
	}

	public void UseRotation()
	{
		lastRotationTime = Time.time;
	}
	public void UseSwitch()
	{
		lastSwitchTime = Time.time;
	}
	public void UseGeyser()
	{
		lastGeyserTime = Time.time;
	}

	private void ResetPowers()
	{
		UseRotation ();
		UseSwitch ();
		UseGeyser ();
	}

	// Use this for initialization
	void Start () 
	{
		gameIsRunning = false;
	}

	public void StartGame()
	{
		timer = initialTimer;
		playerOneScore = 0;
		playerTwoScore = 0;
		currentRound = 0;
		SetRound ();
		StartCoroutine (WaitAndChangeRound (initialTimer));
		playerScript.GetComponent<PlayerControls> ().StartGame ();
		inGameUI.SetActive (true);
		gameOver = false;
		gameIsRunning = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameIsRunning && !gameOver) 
		{
			if (!interRound)
			{
				timer -= Time.deltaTime;
			}
			
			if (interRound || startTimer) 
			{
				interRoundTimer -= Time.deltaTime;
				showTimerInterRound = true;
			}

			if (interRoundTimer <= 0.0f) 
			{
				showTimerInterRound = false;
				interRoundTimer = 3.0f;
				startTimer = false;
				interRound = false;
			}
			
			/*
			if (timer <= 0.0f) 
			{
				currentRound++;
				timer = initialTimer;
				interRound = true;
			}

			if(currentRound == 0 || currentRound == 2)
			{				
				playerScript.GetComponent<PlayerControls>().P1isHunted = true;
				rotationScript.GetComponent<LevelRotationBehaviour>().P1isHunted = true;
				facetteScript.GetComponent<FacetteSwitchManager>().P1isHunted = true;

				UpdatePouvoirsVisibility(true);

				ResetPowers();

				while(counter < geyserScript.Length)
				{
					geyserScript[counter].GetComponent<Geyser2BehaviourScript>().P1isHunted = true;
					counter++;
				}
				
				counter = 0;
			}

			else if(currentRound == 1 || currentRound == 3)
			{					
				playerScript.GetComponent<PlayerControls>().P1isHunted = false;
				rotationScript.GetComponent<LevelRotationBehaviour>().P1isHunted = false;
				facetteScript.GetComponent<FacetteSwitchManager>().P1isHunted = false;
				
				UpdatePouvoirsVisibility(false);

				ResetPowers();

				while(counter < geyserScript.Length)
				{
					geyserScript[counter].GetComponent<Geyser2BehaviourScript>().P1isHunted = false;
					counter++;
				}

				counter = 0;
			}
			*/


			if (currentRound == 0 && upScore == true && ! startTimer || currentRound == 2 && upScore == true && ! startTimer) 
			{
				playerTwoScore += 10;
				player2ScoreUI.text = playerTwoScore.ToString();
				upScore = false;
			}
			
			if (currentRound == 1 && upScore == true && ! startTimer || currentRound == 3 && upScore == true && ! startTimer) 
			{
				playerOneScore += 10;
				player1ScoreUI.text = playerOneScore.ToString();
				upScore = false;
			}

			UpdatePouvoirsAvailability();
		} 
	}

	public MenuEngine menuEngine;

	IEnumerator WaitAndChangeRound(float timer)
	{
		yield return new WaitForSeconds (timer);

		currentRound++;
		timer = initialTimer;
		interRound = true;

		SetRound ();
		
		if (currentRound == 4)
		{
			gameOver = true;
			menuEngine.ShowGameOver(playerOneScore, playerTwoScore);
		}
		else
		{
			StartCoroutine (WaitAndChangeRound (initialTimer));
		}
	}

	private void SetRound()
	{
		if(currentRound == 0 || currentRound == 2)
		{				
			playerScript.GetComponent<PlayerControls>().SetP1IsHunted(true);
			rotationScript.GetComponent<LevelRotationBehaviour>().P1isHunted = true;
			facetteScript.GetComponent<FacetteSwitchManager>().P1isHunted = true;
			
			UpdatePouvoirsVisibility(true);
			
			ResetPowers();
			
			while(counter < geyserScript.Length)
			{
				geyserScript[counter].GetComponent<Geyser2BehaviourScript>().P1isHunted = true;
				counter++;
			}
			
			counter = 0;
		}
		
		else if(currentRound == 1 || currentRound == 3)
		{					
			playerScript.GetComponent<PlayerControls>().SetP1IsHunted(false);
			rotationScript.GetComponent<LevelRotationBehaviour>().P1isHunted = false;
			facetteScript.GetComponent<FacetteSwitchManager>().P1isHunted = false;
			
			UpdatePouvoirsVisibility(false);
			
			ResetPowers();
			
			while(counter < geyserScript.Length)
			{
				geyserScript[counter].GetComponent<Geyser2BehaviourScript>().P1isHunted = false;
				counter++;
			}
			
			counter = 0;
		}
	}

	private void UpdatePouvoirsVisibility (bool P1Hunted)
	{
		pouvoirsP1Panel.SetActive (!P1Hunted);
		pouvoirsP2Panel.SetActive (P1Hunted);
	}

}
