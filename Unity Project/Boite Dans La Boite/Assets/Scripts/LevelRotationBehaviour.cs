using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelRotationBehaviour : MonoBehaviour 
{
	public bool P1isHunted = true;

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

	public float timer = 0.0f;
	float initialTimer = 0.0f;

	float oldZ;

	// Use this for initialization
	void Start () 
	{
		angle = 0;
		initialTimer = timer;
		timer = 0.0f;			
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (P1isHunted) 
		{
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal2") > 0.0f )
			{
				angle = angleSpeed;
			}
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal2") < 0.0f )
			{
				angle = -angleSpeed;
			}
		}

		else if (!P1isHunted) 
		{
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0.5f )
			{
				angle = angleSpeed;
			}
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0.5f )
			{
				angle = -angleSpeed;
			}
		}


		if (angle != 0)
		{
			foreach ( GameObject level in Levels)
			{
				level.transform.RotateAround(level.transform.position, level.transform.forward, angle);
			}
		}
		angle = 0;


		timer -= Time.deltaTime;

		//P2 special power - Appuie sur le "1" du num pad pour faire faire un 180° au niveau
		if (P1isHunted) 
		{
			if (Input.GetKeyDown ("[1]") && timer <= 0.0f || Input.GetButtonDown("Fire1P2") && timer <= 0.0f) 
			{
				powerActivated = true;
				
				Debug.Log(Levels[0].transform.rotation.z*100 + 180.0f);
				
				target1 = Levels[0].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				target2 = Levels[1].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				target3 = Levels[2].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				target4 = Levels[3].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				
				timer += initialTimer;
				
			}
		}

		else if (!P1isHunted) 
		{
			if (Input.GetKeyDown ("[1]") && timer <= 0.0f || Input.GetButtonDown("Fire1") && timer <= 0.0f) 
			{
				powerActivated = true;
				
				Debug.Log(Levels[0].transform.rotation.z*100 + 180.0f);
				
				target1 = Levels[0].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				target2 = Levels[1].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				target3 = Levels[2].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				target4 = Levels[3].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
				
				timer += initialTimer;
				
			}
		}


        if (powerActivated == true)
        {
            foreach (GameObject level in Levels)
            {
                if (level.name == "Level - Haunted Mansion")
                    level.transform.rotation = Quaternion.RotateTowards(level.transform.rotation, target1, speed);
                else if (level.name == "Level - Forest")
                    level.transform.rotation = Quaternion.RotateTowards(level.transform.rotation, target2, speed);
                else if (level.name == "Level - Blocs")
                    level.transform.rotation = Quaternion.RotateTowards(level.transform.rotation, target3, speed);
                else if (level.name == "Level - Doll house")
                    level.transform.rotation = Quaternion.RotateTowards(level.transform.rotation, target4, speed);

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
