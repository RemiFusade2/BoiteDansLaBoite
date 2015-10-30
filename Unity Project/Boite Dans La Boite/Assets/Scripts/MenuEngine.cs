using UnityEngine;
using System.Collections;

public class MenuEngine : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject creditsPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowMainMenu()
	{
		creditsPanel.SetActive (false);
		mainMenuPanel.SetActive (true);
	}
	
	public void ShowCredits()
	{
		creditsPanel.SetActive (true);
		mainMenuPanel.SetActive (false);
	}

	public void HideMainMenu()
	{
		mainMenuPanel.SetActive (false);
	}
}
