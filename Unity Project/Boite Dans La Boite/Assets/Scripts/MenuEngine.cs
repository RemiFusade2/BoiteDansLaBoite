using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuEngine : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject creditsPanel;
	public GameObject gameOverPanel;
	public Text gameOverScoreP1;
	public Text gameOverScoreP2;
	public Text gameOverText;

	public GameObject inGameUI;

	public GameObject inGameHands;

	public SoundEngineScript soundEngine;

	public void ShowGameOver(int scoreP1, int scoreP2)
	{
		soundEngine.PlayMenuBkgMusic ();
		inGameHands.SetActive (false);
		inGameUI.SetActive (false);

		gameOverPanel.SetActive(true);
		gameOverScoreP1.text = scoreP1.ToString();
		gameOverScoreP2.text = scoreP2.ToString();
		if (scoreP1 > scoreP2)
		{
			gameOverText.text = "Le joueur 1 gagne !";
		} 
		else if (scoreP1 < scoreP2)
		{
			gameOverText.text = "Le joueur 2 gagne !";
		}
		else if (scoreP1 == scoreP2)
		{
			gameOverText.text = "Match nul !";
		}
	}

	public void ShowMainMenu()
	{
		creditsPanel.SetActive (false);
		gameOverPanel.SetActive (false);
		mainMenuPanel.SetActive (true);
	}
	
	public void ShowCredits()
	{
		creditsPanel.SetActive (true);
		gameOverPanel.SetActive (false);
		mainMenuPanel.SetActive (false);
	}

	public void HideMainMenu()
	{
		mainMenuPanel.SetActive (false);
		inGameHands.SetActive (true);
		inGameHands.GetComponent<Animator> ().SetTrigger ("Start");
	}

	public void ExitGame()
	{
		StartCoroutine (WaitAndQuitApplication (0.4f));
	}

	IEnumerator WaitAndQuitApplication(float timer)
	{
		yield return new WaitForSeconds(timer);
		Application.Quit();
	}
}
