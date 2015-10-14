using UnityEngine;
using System.Collections;

public class FacetteSwitchManager : MonoBehaviour {

	public Transform to;
	public float speed = 2.0F;

	private float degree;
	private float angle;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("[6]"))
		{
			degree -= 90f;
		}
		if (Input.GetKeyDown("[4]"))
		{
			degree += 90f;
		}

		angle = Mathf.LerpAngle(transform.rotation.z, degree, Time.deltaTime);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, degree, 0), speed);
	}
}
