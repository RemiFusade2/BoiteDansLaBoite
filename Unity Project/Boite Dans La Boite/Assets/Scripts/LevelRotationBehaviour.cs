using UnityEngine;
using System.Collections;

public class LevelRotationBehaviour : MonoBehaviour 
{
	public GameObject Level;

	public float angleSpeed;

	private float angle;

	// Use this for initialization
	void Start () 
	{
		angle = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			angle = angleSpeed;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			angle = -angleSpeed;
		}
		Level.transform.RotateAround(Vector3.zero, Vector3.forward, angle);
		angle = 0;
	}
}
