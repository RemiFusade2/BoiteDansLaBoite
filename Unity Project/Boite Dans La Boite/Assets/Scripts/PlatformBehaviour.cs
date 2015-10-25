using UnityEngine;
using System.Collections;

public class PlatformBehaviour : MonoBehaviour 
{	
	public SpriteRenderer sprite;
	
	public Color standardColor;
	public Color bumperColor;
	public Color deadlyColor;

	public bool deadly;

	public bool alwaysUp;

	public bool bumping;
	public float bumpForce;
	
	public RoundManager roundScript;

	// Use this for initialization
	void Start () 
	{
		if (deadly)
		{
			sprite.color = deadlyColor;
		}
		else if (bumping)
		{
			sprite.color = bumperColor;
		}
		else
		{
			sprite.color = standardColor;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (alwaysUp && this.transform.parent != null)
		{
			this.transform.LookAt (this.transform.position + this.transform.parent.forward);
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.tag.Equals("Player"))
		{
			if (deadly)
			{
				HitPlayer(col.GetComponent<PlayerControls>());
			}
			if (bumping)
			{
				BumpPlayer(col.GetComponent<PlayerControls>());
			}
		}
		else
		{
			if (bumping)
			{
				col.GetComponent<Collider>().GetComponent<Rigidbody> ().velocity = (this.transform.position - col.GetComponent<Collider>().transform.position).normalized * bumpForce;
			}
		}
	}
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag.Equals("Player"))
		{
			if (deadly)
			{
				HitPlayer(col.collider.GetComponent<PlayerControls>());
			}
			if (bumping)
			{
				BumpPlayer(col.collider.GetComponent<PlayerControls>());
			}
		}
		else
		{
			if (bumping)
			{
				col.collider.GetComponent<Collider>().GetComponent<Rigidbody> ().velocity = (this.transform.position - col.collider.GetComponent<Collider>().transform.position).normalized * bumpForce;
			}
		}
	}
	
	private void HitPlayer(PlayerControls player) 
	{
		player.Hit(this.transform.position);
		if (roundScript != null)
		{
			roundScript.upScore = true;
		}
	}
	
	private void BumpPlayer(PlayerControls player) 
	{
		player.Bump(this.transform.position, bumpForce);

		if (this.gameObject.GetComponent<Animator>() != null)
		{
			this.gameObject.GetComponent<Animator>().SetTrigger("Bump");
		}
	}
}
