using UnityEngine;
using System.Collections;

public class AlwaysUpPlatformBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.LookAt (this.transform.position + Vector3.forward);
	}
}
