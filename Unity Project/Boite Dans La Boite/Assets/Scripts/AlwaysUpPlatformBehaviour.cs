using UnityEngine;
using System.Collections;

public class AlwaysUpPlatformBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.transform.parent != null)
		{
			this.transform.LookAt (this.transform.position + this.transform.parent.forward);
		}
	}
}
