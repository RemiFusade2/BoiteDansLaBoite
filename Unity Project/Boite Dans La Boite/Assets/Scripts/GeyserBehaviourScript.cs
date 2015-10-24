using UnityEngine;
using System.Collections;

public class GeyserBehaviourScript : MonoBehaviour {

	Transform initialScale;
	Transform newScale;

	public float timer = 0.0f;
	float initialTimer = 0.0f;

	bool growUp = true;
	bool shrink = false;

	// Use this for initialization
	void Start () {

		initialScale = this.transform;
		initialTimer = timer;

	}
	
	// Update is called once per frame
	void Update () {
	
		timer -= Time.deltaTime;

		if (timer <= 0.0f && growUp) {

			Vector3 newScale = new Vector3 (initialScale.transform.localScale.x, initialScale.transform.localScale.y + 5.0f, initialScale.transform.localScale.z);
			Debug.Log ("new Scale: " + newScale);
		
			//scaling step
			/*float scaleStep += (Time.deltaTime/scalingDuration);
			Debug.Log ("player's scale: " + transform.localScale.x);*/

			transform.Translate (0, 2.0f, 0, Space.Self);

			transform.localScale = Vector3.Lerp (transform.localScale, newScale, 0.5f);

			timer = initialTimer;

			growUp = false;
			shrink = true;
		}

		if (timer <= 0.0f && shrink)
		{
			Vector3 newScale = new Vector3 (initialScale.transform.localScale.x, initialScale.transform.localScale.y - 5.0f, initialScale.transform.localScale.z);
			Debug.Log ("new Scale: " + newScale);
			
			//scaling step
			/*float scaleStep =  Time.deltaTime/1.0f;
			Debug.Log ("player's scale: " + transform.localScale.x);*/
			
			//alter player's scale

			transform.Translate (0, -2.0f, 0, Space.Self);


			transform.localScale = Vector3.Lerp (transform.localScale, newScale, 0.5f);

			timer = initialTimer;
			
			growUp = true;
			shrink = false;
		}



	}
}
