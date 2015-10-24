using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRotationBehaviour : MonoBehaviour 
{
	public List<GameObject> Levels;

	public float angleSpeed;

	private float angle;
	private float angle2;

	public  float speed = 0.5f;

	public float degree;

	public bool powerActivated = false;

	Quaternion target1;
	Quaternion target2;
	Quaternion target3;
	Quaternion target4;

	float oldZ;

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


		//P2 special power - Appuie sur le "1" du num pad pour faire faire un 180° au niveau
		if (Input.GetKeyDown ("[1]")) 
		{
			powerActivated = true;

			Debug.Log(Levels[0].transform.rotation.z*100 + 180.0f);

			target1 = Levels[0].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
			target2 = Levels[1].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
			target3 = Levels[2].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
			target4 = Levels[3].transform.rotation * Quaternion.Euler (0, 0, 180.0f);

		}
	
		if (powerActivated == true) {
			foreach (GameObject level in Levels) {
				if (level.name == "Level")
					level.transform.rotation = Quaternion.RotateTowards (level.transform.rotation, target1, speed);
				else if (level.name == "Level - Middle rigth")
					level.transform.rotation = Quaternion.RotateTowards (level.transform.rotation, target2, speed);
				else if (level.name == "Level - Middle Left")
					level.transform.rotation = Quaternion.RotateTowards (level.transform.rotation, target3, speed);
				else if (level.name == "Level-Middle Back")
					level.transform.rotation = Quaternion.RotateTowards (level.transform.rotation, target4, speed);

				Debug.Log(target1.eulerAngles);
			}

			if(Levels[3].transform.rotation ==  target4)
			{
				powerActivated = false;
				Debug.Log("I'm done :)");
			}

		}


	}
}
