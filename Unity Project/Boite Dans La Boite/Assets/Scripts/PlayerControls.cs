using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		airJumpsExecuted = 0;
		lastHitTime = 0;
		rb = this.GetComponent<Rigidbody>();
		thrustInitialValue = thrust;
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

		if (this.transform.parent != null && !this.transform.parent.tag.Equals("MainCamera"))
		{
			bool isInputToMove = false;
			isOnGround = IsOnGround ();
			if (Input.GetKey(KeyCode.Q) && (Time.time - lastHitTime > ignoreInputAfterHitTimer) && !IsObstacleOnLeft())
			{
				this.GetComponent<Rigidbody>().velocity = new Vector3( 0, this.GetComponent<Rigidbody>().velocity.y, 0) - currentMoveForce * this.transform.right;
				isInputToMove = true;
			}
			if (Input.GetKey(KeyCode.D) && (Time.time - lastHitTime > ignoreInputAfterHitTimer) && !IsObstacleOnRight())
			{
				this.GetComponent<Rigidbody>().velocity = new Vector3( 0, this.GetComponent<Rigidbody>().velocity.y, 0) + currentMoveForce * this.transform.right;
				isInputToMove = true;
			}
			if (isOnGround && !isInputToMove )
			{
				this.GetComponent<Rigidbody>().velocity = new Vector3(0, this.GetComponent<Rigidbody>().velocity.y, 0);
			}
			if (!isOnGround)
			{
				this.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity * inAirSlowing;
			}
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				thrust = 0.0f;
				Jump();
			}

			if(isOnGround && !Input.GetKeyDown(KeyCode.Space) && !switchingFacette)
			{
				thrust = thrustInitialValue;
				rb.AddForce(-transform.up * thrust); 
				currentMoveForce = strongMoveForce;
				currentJumpForce = strongJumpForce;
			}

			if(!isOnGround)
			{
				currentJumpForce = initialJumpForce;
				thrust = 0.0f;
				currentMoveForce = initialMoveForce;
			}
		}
	}

	private bool IsOnGround()
	{
		float epsilon = 0.1f;
		Ray isLeftFootGroundedRayPlusEpsilon = new Ray (leftFoot.transform.position + epsilon * Vector3.up, -Vector3.up);
		Ray isLeftFootGroundedRayMinusEpsilon = new Ray (leftFoot.transform.position - epsilon * Vector3.up, -Vector3.up);
		Ray isRightFootGroundedRayPlusEpsilon = new Ray (rightFoot.transform.position + epsilon * Vector3.up, -Vector3.up);
		Ray isRightFootGroundedRayMinusEpsilon = new Ray (rightFoot.transform.position - epsilon * Vector3.up, -Vector3.up);
		RaycastHit outputHitLeftFoot, outputHitRightFoot;
		float maxDistance = 0.2f;
		if ((Physics.Raycast(isLeftFootGroundedRayPlusEpsilon, out outputHitLeftFoot, maxDistance - epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isLeftFootGroundedRayMinusEpsilon, out outputHitLeftFoot, maxDistance + epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isRightFootGroundedRayPlusEpsilon, out outputHitRightFoot, maxDistance - epsilon) && outputHitRightFoot.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isRightFootGroundedRayMinusEpsilon, out outputHitRightFoot, maxDistance + epsilon) && outputHitRightFoot.collider.tag.Equals("Ground")) )
		{
			airJumpsExecuted = 0;
			return true;
		}
		return false;
	}

	private bool IsObstacleOnRight()
	{
		float epsilon = 0.1f;
		Ray isRightHeadOnObstacleRayPlusEpsilon = new Ray (rightHead.transform.position + epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightHeadOnObstacleRayMinusEpsilon = new Ray (rightHead.transform.position - epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightFootOnObstacleRayPlusEpsilon = new Ray (rightFoot.transform.position + epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightFootOnObstacleRayMinusEpsilon = new Ray (rightFoot.transform.position - epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightMiddleOnObstacleRayPlusEpsilon = new Ray (rightMiddle.transform.position + epsilon * this.transform.parent.right, this.transform.parent.right);
		Ray isRightMiddleOnObstacleRayMinusEpsilon = new Ray (rightMiddle.transform.position - epsilon * this.transform.parent.right, this.transform.parent.right);
		RaycastHit outputHitLeftFoot, outputHitRightFoot;
		float maxDistance = 0.2f;
		if ((Physics.Raycast(isRightHeadOnObstacleRayPlusEpsilon, out outputHitLeftFoot, maxDistance - epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isRightHeadOnObstacleRayMinusEpsilon, out outputHitLeftFoot, maxDistance + epsilon) && outputHitLeftFoot.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isRightFootOnObstacleRayPlusEpsilon, out outputHitRightFoot, maxDistance - epsilon) && outputHitRightFoot.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isRightFootOnObstacleRayMinusEpsilon, out outputHitRightFoot, maxDistance + epsilon) && outputHitRightFoot.collider.tag.Equals("Ground"))||
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
		Ray isLeftFootOnObstacleRayPlusEpsilon = new Ray (leftFoot.transform.position + epsilon * this.transform.parent.right, -this.transform.parent.right);
		Ray isLeftFootOnObstacleRayMinusEpsilon = new Ray (leftFoot.transform.position - epsilon * this.transform.parent.right, -this.transform.parent.right);
		Ray isLeftMiddleOnObstacleRayPlusEpsilon = new Ray (leftMiddle.transform.position + epsilon * this.transform.parent.right, -this.transform.parent.right);
		Ray isLeftMiddleOnObstacleRayMinusEpsilon = new Ray (leftMiddle.transform.position - epsilon * this.transform.parent.right, -this.transform.parent.right);
		RaycastHit outputHit;
		float maxDistance = 0.2f;
		if ((Physics.Raycast(isLeftHeadOnObstacleRayPlusEpsilon, out outputHit, maxDistance - epsilon) && outputHit.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isLeftHeadOnObstacleRayMinusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground")) ||
		    (Physics.Raycast(isLeftFootOnObstacleRayPlusEpsilon, out outputHit, maxDistance - epsilon) && outputHit.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isLeftFootOnObstacleRayMinusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isLeftMiddleOnObstacleRayPlusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground"))||
		    (Physics.Raycast(isLeftMiddleOnObstacleRayMinusEpsilon, out outputHit, maxDistance + epsilon) && outputHit.collider.tag.Equals("Ground")) )
		{
			return true;
		}
		return false;
	}

	private void Slide()
	{

	}

	private void Jump()
	{
		if ( IsOnGround () )
		{
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero; 
			this.GetComponent<Rigidbody>().AddForce(currentJumpForce * Vector3.up );
		}
		else if (airJumpsExecuted < airJumpsAuthorized)
		{
			this.GetComponent<Rigidbody>().AddForce(currentJumpForce * Vector3.up );
			airJumpsExecuted++;
		}
	}

	public void Hit(Vector3 hitterPosition)
	{
		this.GetComponent<AudioSource> ().Play ();
		lastHitTime = Time.time;
		float bumpForce = 10;
		this.GetComponent<Rigidbody> ().velocity = (this.transform.position - hitterPosition).normalized * bumpForce;
	}

	public void Bump(Vector3 bumperPosition, float bumpForce)
	{
		lastHitTime = Time.time;
		this.GetComponent<Rigidbody> ().velocity = (this.transform.position - bumperPosition).normalized * bumpForce;
	}
}
