using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRotationBehaviour : MonoBehaviour 
{
	public List<GameObject> Levels;

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
		if (angle != 0)
		{
			foreach ( GameObject level in Levels)
			{
				level.transform.RotateAround(level.transform.position, level.transform.forward, angle);
			}
		}
		angle = 0;
	}
}
