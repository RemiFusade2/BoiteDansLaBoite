using UnityEngine;
using System.Collections;

public class GeyserBehaviourScript : MonoBehaviour {

	Transform initialScale;

	public float size = 5.0f;

	public float timer = 0.0f;
	float initialTimer = 0.0f;

	float timerToShrink = 0.0f;
	public float initialTimerToShrink = 0.0f;

	public float speed = 1.0f;

	bool growUp = true;
	bool shrink = false;

	bool launchGrowUp = false;
	bool launchShrink = false;
	bool launchTimer = false;

	Vector3 newScale;
	Vector3 oldPosition;

	// Use this for initialization
	void Start () {

		initialScale = this.transform;
		initialTimer = timer;

	}
	
	// Update is called once per frame
	void Update () {

		//P2 special power 2
		if (Input.GetKeyDown ("[2]")) 
		{
			timer = initialTimer;
			launchTimer = true;

		}

		if(launchTimer)
			timer -= Time.deltaTime;

		if (timer <= 0.0f && growUp) {

			newScale = new Vector3 (initialScale.transform.localScale.x, initialScale.transform.localScale.y + size, initialScale.transform.localScale.z);
			oldPosition = initialScale.transform.position;

			launchGrowUp = true;
			launchTimer = false;
					
		}

		if (launchGrowUp) 
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, oldPosition.y + size/2.0f, transform.position.z), Time.deltaTime * speed);
			transform.localScale = Vector3.Lerp (transform.localScale, newScale, Time.deltaTime * speed);

			growUp = false;

			if(this.transform.localScale.y >= newScale.y - 0.1f)
			{
				shrink = true;
				launchGrowUp = false;
				timerToShrink = initialTimerToShrink;
			}
		}

		if(shrink)
			timerToShrink -= Time.deltaTime;

		if (timerToShrink <= 0.0f && shrink)
		{
			newScale = new Vector3 (initialScale.transform.localScale.x, initialScale.transform.localScale.y - size, initialScale.transform.localScale.z);
			oldPosition = initialScale.transform.position;

			launchShrink = true;

		}

		if (launchShrink) 
		{
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, oldPosition.y - size/2.0f, transform.position.z), Time.deltaTime * speed);
			transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * speed);
			
			shrink = false;
			
			if(this.transform.localScale.y <= newScale.y + 0.1f)
			{
				growUp = true;
				launchShrink = false;
				timer = initialTimer;
			}
		}
	}
}
