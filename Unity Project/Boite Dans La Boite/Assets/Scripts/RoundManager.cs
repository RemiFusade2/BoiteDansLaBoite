using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {

		timer = initialTimer;	
	}
	
	// Update is called once per frame
	void Update () {

		if (!gameOver) {
			if (!interRound)
			{
				timer -= Time.deltaTime;
			}
			
			if (interRound || startTimer) {
				interRoundTimer -= Time.deltaTime;
				showTimerInterRound = true;
			}

			if (interRoundTimer <= 0.0f) {
				showTimerInterRound = false;
				interRoundTimer = 3.0f;
				startTimer = false;
				interRound = false;
			}
			
			if (timer <= 0.0f) {
				currentRound++;
				timer = initialTimer;
				interRound = true;
			}
			
			if (currentRound == 0 && upScore == true && ! startTimer || currentRound == 2 && upScore == true && ! startTimer) {
				playerOneScore += 10;
				upScore = false;
			}
			
			if (currentRound == 1 && upScore == true && ! startTimer || currentRound == 3 && upScore == true && ! startTimer) {
				playerTwoScore += 10;
				upScore = false;
			}

			if (currentRound == 4)
				gameOver = true;
		} 
	}

	void OnGUI()
	{

		GUI.Label (new Rect (10, 10, 100, 20), "Player One Score : ", myGui); 
		GUI.Label (new Rect (10, 35, 100, 20), playerOneScore.ToString(), myGui); 

		GUI.Label (new Rect (Screen.width - 180, 10, 100, 20), "Player Two Score : ", myGui); 
		GUI.Label (new Rect (Screen.width - 25, 35, 100, 20), playerTwoScore.ToString(), myGui); 

		if (showTimerInterRound == true && interRoundTimer > 0) 
		{
			int timer = (int) interRoundTimer +1 ;
			GUI.Label (new Rect (Screen.width / 2, 10, 100, 20), timer.ToString (), myGui); 
		}

		if (gameOver) 
		{
			GUI.Box(new Rect(Screen.width/2 - 125, Screen.height/2 - 125,250, 250), "Game Over"); 

			if(GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 95 ,200, 30), "Retry"))
				Application.LoadLevel(0);
		}

	}
}
