using UnityEngine;
using System.Collections;

public class FacetteSwitchManager : MonoBehaviour {

	public Transform to;
	public float speed = 0.1F;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("[6]"))
		{
			this.transform.RotateAround(transform.position, transform.up, -90);
			//this.transform.Rotate (0,-90,0, Space.World);
		}
		if (Input.GetKeyDown("[4]"))
		{
			this.transform.RotateAround(transform.position, transform.up, 90);
			//this.transform.Rotate (0,90,0, Space.World);
		}

		/*if (Input.GetKeyDown("[8]"))
		{
		    this.transform.RotateAround(transform.position, -transform.right, -90);
			//this.transform.Rotate (90,0,0,);
		}
		if (Input.GetKeyDown("[2]"))
		{
			this.transform.RotateAround(transform.position, -transform.right, 90);
			//this.transform.Rotate (-90,0,0, Space.World);
		}*/
	}
}
