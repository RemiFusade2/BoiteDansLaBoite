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
				timer -= Time.deltaTime;
			
			if (interRound) {
				interRoundTimer -= Time.deltaTime;
			}
			
			if (interRoundTimer <= 0.0f) {
				interRoundTimer = 3.0f;
				interRound = false;
			}
			
			if (timer <= 0.0f) {
				currentRound++;
				timer = initialTimer;
				interRound = true;
			}
			
			if (currentRound == 0 && upScore == true || currentRound == 2 && upScore == true) {
				playerOneScore += 10;
				upScore = false;
			}
			
			if (currentRound == 1 && upScore == true || currentRound == 3 && upScore == true) {
				playerTwoScore += 10;
				upScore = false;
			}

			if (currentRound == 4)
				gameOver = true;
		} 

		else 
		{

		}

	}

	void OnGUI()
	{

		GUI.Label (new Rect (10, 10, 100, 20), "Player One Score : ", myGui); 
		GUI.Label (new Rect (10, 35, 100, 20), playerOneScore.ToString(), myGui); 

		GUI.Label (new Rect (Screen.width - 180, 10, 100, 20), "Player Two Score : ", myGui); 
		GUI.Label (new Rect (Screen.width - 25, 35, 100, 20), playerTwoScore.ToString(), myGui); 


	}
}
