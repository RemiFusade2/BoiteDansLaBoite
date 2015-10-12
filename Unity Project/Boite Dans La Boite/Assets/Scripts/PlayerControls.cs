using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public float jumpForce;
	public float moveForce;

	public int airJumpsAuthorized;

	private int airJumpsExecuted;

	public GameObject leftFoot;
	public GameObject rightFoot;

	public float ignoreInputAfterHitTimer; 

	private float lastHitTime;

	public float inAirSlowing;

	// Use this for initialization
	void Start () {
		airJumpsExecuted = 0;
		lastHitTime = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool isInputToMove = false;
		bool isOnGround = IsOnGround ();
		if (Input.GetKey(KeyCode.Q) && (Time.time - lastHitTime > ignoreInputAfterHitTimer) )
		{
			this.GetComponent<Rigidbody>().velocity = new Vector3(- moveForce, this.GetComponent<Rigidbody>().velocity.y, 0);
			isInputToMove = true;
		}
		if (Input.GetKey(KeyCode.D) && (Time.time - lastHitTime > ignoreInputAfterHitTimer) )
		{
			this.GetComponent<Rigidbody>().velocity = new Vector3(  moveForce, this.GetComponent<Rigidbody>().velocity.y, 0);
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
			Jump();
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

	private void Slide()
	{

	}

	private void Jump()
	{
		if ( IsOnGround () )
		{
			this.GetComponent<Rigidbody>().AddForce(jumpForce * Vector3.up);
		}
		else if (airJumpsExecuted < airJumpsAuthorized)
		{
			this.GetComponent<Rigidbody>().AddForce(jumpForce * Vector3.up);
			airJumpsExecuted++;
		}
	}

	public void Hit(Vector3 hitterPosition)
	{
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
