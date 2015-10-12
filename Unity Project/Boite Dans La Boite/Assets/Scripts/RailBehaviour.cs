using UnityEngine;
using System.Collections;

public class RailBehaviour : MonoBehaviour {

	public Vector3 defaultEulerAngles;
	public GameObject onRailPrefabObject;

	private GameObject onRailGameObject;

	// Use this for initialization
	void Start () 
	{
		Vector3 objectPosition = new Vector3 (this.transform.position.x, this.transform.position.y, -0.175f);
		Quaternion objectOrientation = Quaternion.Euler ( defaultEulerAngles );
		onRailGameObject = (GameObject) Instantiate (onRailPrefabObject, this.transform.position, objectOrientation);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (onRailGameObject != null)
		{
			Vector3 newObjectPosition = new Vector3 (this.transform.position.x, this.transform.position.y, -0.175f);
			onRailGameObject.transform.localPosition = newObjectPosition;
		}
	}
}
