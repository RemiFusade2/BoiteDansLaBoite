using UnityEngine;
using System.Collections;

public class BumperBehaviour : MonoBehaviour {

	public float bumpForce;

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
			BumpPlayer(col.GetComponent<PlayerControls>());
		}
		else
		{
			col.GetComponent<Collider>().GetComponent<Rigidbody> ().velocity = (this.transform.position - col.GetComponent<Collider>().transform.position).normalized * bumpForce;
		}
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag.Equals("Player"))
		{
			BumpPlayer(col.collider.GetComponent<PlayerControls>());
		}
		else
		{
			col.collider.GetComponent<Rigidbody> ().velocity = (this.transform.position - col.collider.transform.position).normalized * bumpForce;
		}
	}
	
	private void BumpPlayer(PlayerControls player) 
	{
		player.Bump(this.transform.position, bumpForce);
	}
}
