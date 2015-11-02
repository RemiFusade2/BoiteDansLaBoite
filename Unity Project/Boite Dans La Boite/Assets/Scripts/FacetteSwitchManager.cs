using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacetteSwitchManager : MonoBehaviour {

	public bool P1isHunted = true;

	public Transform to;
	public float speed = 2.0F;

	private float degree;
	private float angle;

	public GameObject Player;

	public float timeBetweenTwoMoves;

	private float lastMoveTime;
	public bool isMoving;

	private int movingDirection;

	public List<GameObject> levels;

	public int currentLevelIndex;

	public Rigidbody rb;

	public float timer = 0.5f; // set duration time in seconds in the Inspector
	public bool launchTimer = false;
	
	public SoundEngineScript soundEngine;

	public RoundManager roundManager;

	// Use this for initialization
	void Start () 
	{
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isMoving)
		{
			if(P1isHunted)
			{
				if ((Input.GetKeyDown("[6]") || Input.GetButtonDown("Fire5")) && roundManager.IsSwitchAvailable())
				{
					degree -= 90f;
					currentLevelIndex = (currentLevelIndex + levels.Count - 1) % levels.Count;
					
					launchTimer = true;
					timer = 0.5f;

					roundManager.UseSwitch();
				}
				if ((Input.GetKeyDown("[4]")|| Input.GetButtonDown("Fire4")) && roundManager.IsSwitchAvailable())
				{
					degree += 90f;
					currentLevelIndex = (currentLevelIndex + 1) % levels.Count;
					
					launchTimer = true;
					timer = 0.5f;
					
					roundManager.UseSwitch();
				}
			}
			else if (!P1isHunted)
			{
				if ((Input.GetKeyDown("[6]") || Input.GetButtonDown("Fire5P1")) && roundManager.IsSwitchAvailable())
				{
					movingDirection = -1;
					degree -= 90f;
					currentLevelIndex = (currentLevelIndex + levels.Count - 1) % levels.Count;
					
					launchTimer = true;
					timer = 0.5f;

					roundManager.UseSwitch();
				}
				if ((Input.GetKeyDown("[4]")|| Input.GetButtonDown("Fire4P1")) && roundManager.IsSwitchAvailable())
				{
					movingDirection = 1;
					degree += 90f;
					currentLevelIndex = (currentLevelIndex + 1) % levels.Count;
					
					launchTimer = true;
					timer = 0.5f;
					
					roundManager.UseSwitch();
				}
			}

			if(launchTimer)
			{
				timer -= Time.deltaTime;
				Player.transform.parent = this.transform; // player is now child of Camera
				Player.GetComponent<Rigidbody>().useGravity = false;
				Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
				Player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

				if(timer <= 0.0f)
				{
					isMoving = true;
				}
			}
			if (isMoving)
			{
				// start movement
				Player.transform.parent = this.transform; // player is now child of Camera
				Player.GetComponent<Rigidbody>().useGravity = false;
				Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
				Player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				Player.GetComponent<PlayerControls>().DisableColliders();
				launchTimer = false;

				if (movingDirection == -1)
				{
					soundEngine.PlaySound("wooshLeft");
				} 
				else if (movingDirection == 1)
				{
					soundEngine.PlaySound("wooshRight");
				}

				// activate rails
				/*
				foreach (Transform child in levels[currentLevelIndex].transform)
				{
					if (child.tag.Equals("Rail"))
					{
						child.GetComponent<RailBehaviour>().SetActivated(true);
					}
				}
				*/
			}
		}

		if (isMoving)
		{
			if (rb != null)
			{
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero; 
			}

			//angle = Mathf.LerpAngle(transform.rotation.z, degree, Time.deltaTime);
			Quaternion targetOrientation = Quaternion.Euler (0, degree, 0);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, speed);

			float epsilon = 0.00001f;
			if (Mathf.Abs(transform.rotation.x - targetOrientation.x) < epsilon && 
			    Mathf.Abs(transform.rotation.y - targetOrientation.y) < epsilon && 
			    Mathf.Abs(transform.rotation.z - targetOrientation.z) < epsilon && 
			    Mathf.Abs(transform.rotation.w - targetOrientation.w) < epsilon )
			{
				lastMoveTime = Time.time;
				isMoving = false;
				
				// end movement
				Player.transform.parent = levels[currentLevelIndex].transform; // player is no longer a child of Camera
				Player.GetComponent<Rigidbody>().useGravity = true;
				Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
				Player.GetComponent<PlayerControls>().EnableColliders();

				// Beware of particular cases !
				// When levels are perpendicular, axes X and Z switched
				Player.transform.localPosition = new Vector3(Player.transform.localPosition.x, Player.transform.localPosition.y, 0);

				if (currentLevelIndex == 0 || currentLevelIndex == 2)
				{
					Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
				}
				else
				{
					Player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
				}

				if (currentLevelIndex == 0)
				{
					// haunted mansion
					soundEngine.PlayGhostBkgMusic();
				} 
				else if (currentLevelIndex == 1)
				{
					// forest
					soundEngine.PlayForestBkgMusic();
				}
				else if (currentLevelIndex == 2)
				{
					// blocks
					soundEngine.PlayBlocksBkgMusic();
				}
				else if (currentLevelIndex == 3)
				{
					// doll house
					soundEngine.PlayPrincessBkgMusic();
				}

			}
		}
	}
}
