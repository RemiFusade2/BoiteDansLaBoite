using UnityEngine;
using System.Collections;

public class DeadlyObstaclesBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
	}
}
