using UnityEngine;
using System.Collections;

public class OutofSpaceBehaviour : MonoBehaviour {
	
	public RoundManager roundScript;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals("Player"))
		{
			PutPlayerAtTheCenterOfLevel(col.GetComponent<PlayerControls>());
		}
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag.Equals("Player"))
		{
			PutPlayerAtTheCenterOfLevel(col.collider.GetComponent<PlayerControls>());
		}
	}
	
	private void PutPlayerAtTheCenterOfLevel(PlayerControls player) 
	{
		player.ResetPosition ();
		if (roundScript != null)
		{
			roundScript.upScore = true;
		}
	}
}
