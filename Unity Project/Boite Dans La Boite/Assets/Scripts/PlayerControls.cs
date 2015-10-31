using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public bool P1isHunted = true;

	public Camera mainCamera;

	public float maxSlopeAngle;

	public float initialJumpForce;
	public float initialMoveForce;

	public float currentJumpForce;
	public float currentMoveForce;

	public float strongJumpForce;
	public float strongMoveForce;

	public int airJumpsAuthorized;

	private int airJumpsExecuted;

	public GameObject leftFoot;
	public GameObject rightFoot;
	
	public GameObject leftMiddle;
	public GameObject rightMiddle;
	
	public GameObject leftHead;
	public GameObject rightHead;

	public float ignoreInputAfterHitTimer; 

	private float lastHitTime;

	public float inAirSlowing;

	public bool isOnGround;

	public float thrust;
	public float thrustInitialValue= 0.0f;
	public Rigidbody rb;
	public FacetteSwitchManager facetteScript;
	public bool switchingFacette = false;

	Vector3 initialVelocity = Vector3.zero;
	Vector3 initialAngularVelocity = Vector3.zero;

	bool hasSwitched = false;

	public GameObject playerSprite;
	private Vector3 initialPlayerSpriteHorizontalScale;

	public SoundEngineScript soundEngine;

	private bool invicible;
	public float invicibilityTimeAfterHit;

	// Use this for initialization
	void Start () {
		airJumpsExecuted = 0;
		lastHitTime = 0;
		rb = this.GetComponent<Rigidbody>();
		thrustInitialValue = thrust;

		invicible = false;
		
		if (playerSprite != null) 
		{
			initialPlayerSpriteHorizontalScale = playerSprite.transform.localScale;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		switchingFacette = facetteScript.isMoving;

		if (switchingFacette) {
			thrust = 0.0f;

			initialVelocity = rb.velocity;
			initialAngularVelocity = rb.angularVelocity;

			hasSwitched = true;

		} 

		else if(! switchingFacette && hasSwitched)
		{
			rb.velocity = initialVelocity;
			rb.angularVelocity = initialAngularVelocity;
			hasSwitched = false;
		}

		if (this.transform.parent != null && !this.transform.parent.tag.Equals ("MainCamera")) 
		{
			bool isInputToMove = false;

			float slopeAngle;
			isOnGround = IsOnGround (out slopeAngle);
			float slopeSlowDownRatioRight = (slopeAngle < -maxSlopeAngle) ? 0 : ((maxSlopeAngle + slopeAngle) / maxSlopeAngle);
			slopeSlowDownRatioRight = (slopeSlowDownRatioRight > 1.1f) ? 1.1f : slopeSlowDownRatioRight;
			float slopeSlowDownRatioLeft = (slopeAngle > maxSlopeAngle) ? 0 : ((maxSlopeAngle - slopeAngle) / maxSlopeAngle);
			slopeSlowDownRatioLeft = (slopeSlowDownRatioLeft > 1.1f) ? 1.1f : slopeSlowDownRatioLeft;

			float controlerInputLimit = 0.1f;
			float p1ControlerHorizontalInput = Input.GetAxis("Horizontal");
			float p2ControlerHorizontalInput = Input.GetAxis("Horizontal2");
			if ( (Input.GetKey (KeyCode.Q) || (P1isHunted && p1ControlerHorizontalInput < -controlerInputLimit) || (!P1isHunted && p2ControlerHorizontalInput < -controlerInputLimit))
			    && (Time.time - lastHitTime > ignoreInputAfterHitTimer) && !IsObstacleOnLeft ()) 
			{
				float inputRatio = Input.GetKey (KeyCode.Q) ? 1 : Mathf.Abs((P1isHunted ? p1ControlerHorizontalInput : p2ControlerHorizontalInput));
				Vector3 newVelocity = new Vector3 (0, this.GetComponent<Rigidbody> ().velocity.y, 0) - inputRatio * currentMoveForce * slopeSlowDownRatioLeft * this.transform.right;
				if (!float.IsNaN(newVelocity.x) && !float.IsNaN(newVelocity.y) && !float.IsNaN(newVelocity.z))
				{
					this.GetComponent<Rigidbody> ().velocity = newVelocity;
				}
				isInputToMove = true;
			}
			if ( (Input.GetKey (KeyCode.D) || (P1isHunted && p1ControlerHorizontalInput > controlerInputLimit) || (!P1isHunted && p2ControlerHorizontalInput > controlerInputLimit))
				&& (Time.time - lastHitTime > ignoreInputAfterHitTimer) && !IsObstacleOnRight ()) 
			{
				float inputRatio = Input.GetKey (KeyCode.D) ? 1 : (P1isHunted ? p1ControlerHorizontalInput : p2ControlerHorizontalInput);
				Vector3 newVelocity = new Vector3 (0, this.GetComponent<Rigidbody> ().velocity.y, 0) + inputRatio * currentMoveForce * slopeSlowDownRatioRight * this.transform.right;
				if (!float.IsNaN(newVelocity.x) && !float.IsNaN(newVelocity.y) && !float.IsNaN(newVelocity.z))
				{
					this.GetComponent<Rigidbody> ().velocity = newVelocity;
				}
				isInputToMove = true;
			}

			/*
			if ((Time.time - lastHitTime > ignoreInputAfterHitTimer) && !IsObstacleOnRight ()) 
			{
				//this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, this.GetComponent<Rigidbody> ().velocity.y, 0) * (currentMoveForce * Input.GetAxis("Horizontal"));

				if(P1isHunted)
					transform.Translate(Input.GetAxis("Horizontal")* Time.deltaTime * currentMoveForce,0,0);
				else if(!P1isHunted)
					transform.Translate(Input.GetAxis("Horizontal2")* Time.deltaTime * currentMoveForce,0,0);
				//isInputToMove = true;

			}
			*/

			if (isOnGround && !isInputToMove) 
			{
				this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, this.GetComponent<Rigidbody> ().velocity.y, 0);
			}
			if (!isOnGround) 
			{
				Vector3 newVelocitySlowedDown = new Vector3(this.GetComponent<Rigidbody> ().velocity.x * inAirSlowing, this.GetComponent<Rigidbody> ().velocity.y, this.GetComponent<Rigidbody> ().velocity.z * inAirSlowing);
				this.GetComponent<Rigidbody> ().velocity = newVelocitySlowedDown;
			}


			if (isOnGround && !Input.GetKeyDown (KeyCode.Space) ) 
			{
				thrust = thrustInitialValue;
				currentMoveForce = strongMoveForce;
				currentJumpForce = strongJumpForce;
			}

			if (!isOnGround) 
			{
				currentJumpForce = initialJumpForce;
				thrust = 0.0f;
				currentMoveForce = initialMoveForce;
			}

			if(P1isHunted)
			{
				if (Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("JumpP1")) 
				{
					thrust = 0.0f;
					Jump ();
				}
			}

			else if(!P1isHunted)
			{
				if (Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("Jump")) 
				{
					thrust = 0.0f;
					Jump ();
				}
			}


			if (this.GetComponent<Animator> () != null) 
			{
				Vector3 horizontalVelocity = new Vector3 (this.GetComponent<Rigidbody> ().velocity.x, 0, this.GetComponent<Rigidbody> ().velocity.z);
				this.GetComponent<Animator> ().SetInteger ("moveSpeed", Mathf.RoundToInt (horizontalVelocity.magnitude));
				this.GetComponent<Animator> ().SetBool ("onGround", isOnGround);
			}

			if (mainCamera != null) 
			{				
				float direction = Vector3.Dot (this.GetComponent<Rigidbody> ().velocity, mainCamera.transform.right);
				float epsilon = 0.01f;
				if (playerSprite != null && Mathf.Abs (direction) > epsilon) 
				{
					playerSprite.transform.localScale = new Vector3 ((direction < 0 ? -1 : 1) * initialPlayerSpriteHorizontalScale.x, initialPlayerSpriteHorizontalScale.y, initialPlayerSpriteHorizontalScale.z);
				}
			}
		}
	}

	private bool IsOnGround(out float slopeAngle)
	{
		float epsilon = 0.1f;
		Ray isLeftFootGroundedRayPlusEpsilon = new Ray (leftFoot.transform.position + epsilon * Vector3.up, -Vector3.up);
		Ray isLeftFootGroundedRayMinusEpsilon = new Ray (leftFoot.transform.position - epsilon * Vector3.up, -Vector3.up);
		Ray isRightFootGroundedRayPlusEpsilon = new Ray (rightFoot.transform.position + epsilon * Vector3.up, -Vector3.up);
		Ray isRightFootGroundedRayMinusEpsilon = new Ray (rightFoot.transform.position - epsilon * Vector3.up, -Vector3.up);
		RaycastHit outputHitLeftFoot, outputHitRightFoot;
		float maxDistance = 0.3f;
		slopeAngle = 0;

		// left foot
		if ((Physics.Raycast(isLeftFootGroundedRayPlusEpsilon, out outputHitLeftFoot, maxDistance - epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isLeftFootGroundedRayMinusEpsilon, out outputHitLeftFoot, maxDistance + epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) )
		{
			float angleSign = mainCamera.transform.right.x >= 0.8f ? Mathf.Sign ( outputHitLeftFoot.normal.x ) :
							  mainCamera.transform.right.x <= -0.8f ? Mathf.Sign ( -outputHitLeftFoot.normal.x ) :
							  mainCamera.transform.right.z >= 0.8f ? Mathf.Sign ( outputHitLeftFoot.normal.z ) : Mathf.Sign ( -outputHitLeftFoot.normal.z );
			float angle = angleSign * Mathf.Acos ( outputHitLeftFoot.normal.y ) * Mathf.Rad2Deg; //get angle
			slopeAngle = angle;
			if (Mathf.Abs(angle) < maxSlopeAngle)
			{
				airJumpsExecuted = 0;
				return true;
			}
		}

		// right foot
		if ((Physics.Raycast(isRightFootGroundedRayPlusEpsilon, out outputHitRightFoot, maxDistance - epsilon) && outputHitRightFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isRightFootGroundedRayMinusEpsilon, out outputHitRightFoot, maxDistance + epsilon) && outputHitRightFoot.collider.tag.Equals("Ground")) )
		{
			float angleSign = mainCamera.transform.right.x >= 0.8f ? Mathf.Sign ( outputHitRightFoot.normal.x ) :
							  mainCamera.transform.right.x <= -0.8f ? Mathf.Sign ( -outputHitRightFoot.normal.x ) :
							  mainCamera.transform.right.z >= 0.8f ? Mathf.Sign ( outputHitRightFoot.normal.z ) : Mathf.Sign ( -outputHitRightFoot.normal.z );
			float angle = angleSign * Mathf.Acos ( outputHitRightFoot.normal.y ) * Mathf.Rad2Deg; //get angle
			slopeAngle = angle;
			if (Mathf.Abs(angle) < maxSlopeAngle)
			{
				airJumpsExecuted = 0;
				return true;
			}
		}

		return false;
	}

	private bool IsObstacleOnRight()
	{
		float epsilon = 0.1f;
		Ray isRightHeadOnObstacleRayPlusEpsilon = new Ray (rightHead.transform.position + epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightHeadOnObstacleRayMinusEpsilon = new Ray (rightHead.transform.position - epsilon * this.transform.parent.right, this.transform.parent.right);
		/*Ray isRightFootOnObstacleRayPlusEpsilon = new Ray (rightFoot.transform.position + epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightFootOnObstacleRayMinusEpsilon = new Ray (rightFoot.transform.position - epsilon * this.transform.parent.right, this.transform.parent.right);*/
		Ray isRightMiddleOnObstacleRayPlusEpsilon = new Ray (rightMiddle.transform.position + epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightMiddleOnObstacleRayMinusEpsilon = new Ray (rightMiddle.transform.position - epsilon * this.transform.parent.right, this.transform.parent.right);
		RaycastHit outputHitLeftFoot, outputHitRightFoot;
		float maxDistance = 0.2f;
		if ((Physics.Raycast(isRightHeadOnObstacleRayPlusEpsilon, out outputHitLeftFoot, maxDistance - epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isRightHeadOnObstacleRayMinusEpsilon, out outputHitLeftFoot, maxDistance + epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    /*(Physics.Raycast(isRightFootOnObstacleRayPlusEpsilon, out outputHitRightFoot, maxDistance - epsilon) && outputHitRightFoot.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isRightFootOnObstacleRayMinusEpsilon, out outputHitRightFoot, maxDistance + epsilon) && outputHitRightFoot.collider.tag.Equals("Ground"))||*/
		    (Physics.Raycast(isRightMiddleOnObstacleRayPlusEpsilon, out outputHitRightFoot, maxDistance + epsilon) && outputHitRightFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isRightMiddleOnObstacleRayMinusEpsilon, out outputHitRightFoot, maxDistance + epsilon) && outputHitRightFoot.collider.tag.Equals("Ground"))  )
		{
			return true;
		}
		return false;
	}
	
	private bool IsObstacleOnLeft()
	{
		float epsilon = 0.1f;
		Ray isLeftHeadOnObstacleRayPlusEpsilon = new Ray (leftHead.transform.position + epsilon * this.transform.parent.right, -this.transform.parent.right);
		Ray isLeftHeadOnObstacleRayMinusEpsilon = new Ray (leftHead.transform.position - epsilon * this.transform.parent.right, -this.transform.parent.right);
		/*Ray isLeftFootOnObstacleRayPlusEpsilon = new Ray (leftFoot.transform.position + epsilon * this.transform.parent.right, -this.transform.parent.right);
		Ray isLeftFootOnObstacleRayMinusEpsilon = new Ray (leftFoot.transform.position - epsilon * this.transform.parent.right, -this.transform.parent.right);*/
		Ray isLeftMiddleOnObstacleRayPlusEpsilon = new Ray (leftMiddle.transform.position + epsilon * this.transform.parent.right, -this.transform.parent.right);
		Ray isLeftMiddleOnObstacleRayMinusEpsilon = new Ray (leftMiddle.transform.position - epsilon * this.transform.parent.right, -this.transform.parent.right);
		RaycastHit outputHit;
		float maxDistance = 0.2f;
		if ((Physics.Raycast(isLeftHeadOnObstacleRayPlusEpsilon, out outputHit, maxDistance - epsilon) && outputHit.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isLeftHeadOnObstacleRayMinusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground")) ||
		    /*(Physics.Raycast(isLeftFootOnObstacleRayPlusEpsilon, out outputHit, maxDistance - epsilon) && outputHit.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isLeftFootOnObstacleRayMinusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground"))||*/
		    (Physics.Raycast(isLeftMiddleOnObstacleRayPlusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isLeftMiddleOnObstacleRayMinusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground")) )
		{
			return true;
		}
		return false;
	}

	public void ResetPosition()
	{
		this.transform.localPosition = Vector3.zero;
		Hit (Vector3.zero);
	}

	private void Jump()
	{
		float slopeAngle;
		if ( IsOnGround (out slopeAngle) )
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			//this.GetComponent<Rigidbody>().AddForce(currentJumpForce * Vector3.up );
			this.GetComponent<Rigidbody>().velocity = new Vector3 (this.GetComponent<Rigidbody>().velocity.x, currentJumpForce / 200.0f, this.GetComponent<Rigidbody>().velocity.z);
		}
		else if (airJumpsExecuted < airJumpsAuthorized)
		{
			//this.GetComponent<Rigidbody>().AddForce(currentJumpForce * Vector3.up );
			this.GetComponent<Rigidbody>().velocity = new Vector3 (this.GetComponent<Rigidbody>().velocity.x, currentJumpForce / 200.0f, this.GetComponent<Rigidbody>().velocity.z);
			airJumpsExecuted++;
		}
	}

	public bool Hit(Vector3 hitterPosition)
	{
		if (!invicible)
		{
			float random = Random.Range (0.0f, 1.0f);
			if (random < 0.75f) {
				soundEngine.PlaySound("hit");
			} else {
				if (P1isHunted)
				{
					soundEngine.PlaySound("hitGirl");
				}
				else
				{
					soundEngine.PlaySound("hitDemon");
				}
			}
			lastHitTime = Time.time;
			invicible = true;
			StartCoroutine (WaitAndMakePlayerHitableAgain (invicibilityTimeAfterHit));
			float bumpForce = 10;
			this.GetComponent<Rigidbody> ().velocity = (this.transform.position - hitterPosition).normalized * bumpForce;
			this.GetComponent<Animator> ().SetTrigger ("hit");
			mainCamera.GetComponent<CameraEffectsBehaviour> ().Shake (0.2f);
			return true;
		}
		return false;
	}

	IEnumerator WaitAndMakePlayerHitableAgain(float timer)
	{
		yield return new WaitForSeconds (timer);
		invicible = false;
	}

	public void Bump(Vector3 bumperPosition, float bumpForce)
	{
		lastHitTime = Time.time;
		this.GetComponent<Rigidbody> ().velocity = (this.transform.position - bumperPosition).normalized * bumpForce;
	}

	public void DisableColliders()
	{
		foreach (Collider col in this.GetComponents<Collider>()) 
		{
			col.enabled = false;
		}
	}
	public void EnableColliders()
	{		
		foreach (Collider col in this.GetComponents<Collider>()) 
		{
			col.enabled = true;
		}
	}
}
