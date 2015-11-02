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

	public  float speed = 1.0f;

	public float degree;

	public bool powerActivated = false;

	Quaternion target1;
	Quaternion target2;
	Quaternion target3;
	Quaternion target4;

	public float timer = 0.0f;
	float initialTimer = 0.0f;

	float oldZ;

	public SoundEngineScript soundEngine;

	public Animator hands;

	public RoundManager roundManager;

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
		if (P1isHunted && !powerActivated) 
		{
			// P2 turns counterClockwise
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal2") > 0.0f )
			{
				hands.SetBool("isRotatingCounterClockwise", true);
				angle = angleSpeed;
			}
			else
			{				
				hands.SetBool("isRotatingCounterClockwise", false);
			}
			
			// P2 turns clockwise
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal2") < 0.0f )
			{
				hands.SetBool("isRotatingClockwise", true);
				angle = -angleSpeed;
			}
			else
			{				
				hands.SetBool("isRotatingClockwise", false);
			}
		}
		else if (!P1isHunted && !powerActivated) 
		{
			// P1 turns counterClockwise
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0.0f )
			{
				hands.SetBool("isRotatingCounterClockwise", true);
				angle = angleSpeed;
			}
			else
			{				
				hands.SetBool("isRotatingCounterClockwise", false);
			}

			// P1 turns clockwise
			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0.0f )
			{
				hands.SetBool("isRotatingClockwise", true);
				angle = -angleSpeed;
			}
			else
			{				
				hands.SetBool("isRotatingClockwise", false);
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
		if (roundManager.IsRotationAvailable())
		{
			if (P1isHunted) 
			{
				if (Input.GetKeyDown ("[1]") && timer <= 0.0f || Input.GetButtonDown("Fire3P2") && timer <= 0.0f) 
				{
					powerActivated = true;
					
					Debug.Log(Levels[0].transform.rotation.z*100 + 180.0f);
					
					target1 = Levels[0].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					target2 = Levels[1].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					target3 = Levels[2].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					target4 = Levels[3].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					
					timer += initialTimer;

					roundManager.UseRotation();
					StartCoroutine(WaitAndUnlockLevelRotation(1.5f));
				}
			}
			else if (!P1isHunted) 
			{
				if (Input.GetKeyDown ("[1]") && timer <= 0.0f || Input.GetButtonDown("Fire3") && timer <= 0.0f) 
				{
					powerActivated = true;
					
					Debug.Log(Levels[0].transform.rotation.z*100 + 180.0f);
					
					target1 = Levels[0].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					target2 = Levels[1].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					target3 = Levels[2].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					target4 = Levels[3].transform.rotation * Quaternion.Euler (0, 0, 180.0f);
					
					timer += initialTimer;
					
					roundManager.UseRotation();
					StartCoroutine(WaitAndUnlockLevelRotation(1.5f));
				}
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
			}
		}
	}
	
	
	IEnumerator WaitAndUnlockLevelRotation(float timer)
	{
		yield return new WaitForSeconds(timer);
		powerActivated = false;
	}
}
