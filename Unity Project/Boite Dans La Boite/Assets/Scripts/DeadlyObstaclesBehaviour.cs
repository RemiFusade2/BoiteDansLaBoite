﻿using UnityEngine;
using System.Collections;

public class DeadlyObstaclesBehaviour : MonoBehaviour {

	public RoundManager roundScript;
	GameObject roundManager;

	// Use this for initialization
	void Start () {

		roundManager = GameObject.Find("RoundManager");
		roundScript = roundManager.GetComponent<RoundManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals("Player"))
		{
			HitPlayer(col.GetComponent<PlayerControls>());
		}
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag.Equals("Player"))
		{
			HitPlayer(col.collider.GetComponent<PlayerControls>());
		}
	}

	private void HitPlayer(PlayerControls player) 
	{
		player.Hit(this.transform.position);
		roundScript.upScore = true;

		Debug.Log ("hello");
	}
}
