using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour {

	public float initialTimer = 250.0f;
	public float timer = 0.0f;
	public int currentRound = 0;

	public bool upScore = false;

	public int playerOneScore = 0;
	public int playerTwoScore = 0;

	// Use this for initialization
	void Start () {

		timer = initialTimer;	
	}
	
	// Update is called once per frame
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0.0f) 
		{
			currentRound++;
			timer = initialTimer;
		}

		if (currentRound == 0 && upScore == true || currentRound == 2 && upScore == true) 
		{
			playerOneScore += 10;
			upScore = false;
		}

		if (currentRound == 1 && upScore == true || currentRound == 3 && upScore == true) 
		{
			playerTwoScore += 10;
			upScore = false;
		}
	}
}
