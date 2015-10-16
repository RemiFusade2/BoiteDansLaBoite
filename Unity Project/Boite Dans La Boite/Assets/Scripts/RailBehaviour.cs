using UnityEngine;
using System.Collections;

public class RailBehaviour : MonoBehaviour {

	public Vector3 defaultEulerAngles;

	public GameObject sphereToFollow;

	public GameObject onRailPrefabObject;

	private GameObject onRailGameObject;

	public bool activated;

	public void SetActivated(bool newValue)
	{
		activated = newValue;
		InitObject ();
	}

	private void InitObject()
	{
		if (activated && onRailGameObject == null)
		{
			Vector3 objectPosition = new Vector3 (sphereToFollow.transform.position.x, sphereToFollow.transform.position.y, 0);
			Quaternion objectOrientation = Quaternion.Euler ( defaultEulerAngles );
			onRailGameObject = (GameObject) Instantiate (onRailPrefabObject, objectPosition, objectOrientation);
			onRailGameObject.transform.parent = this.transform;
		}
	}

	// Use this for initialization
	void Start () 
	{
		onRailGameObject = null;
		InitObject ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (activated && onRailGameObject != null)
		{
			Vector3 newObjectPosition = new Vector3 (sphereToFollow.transform.position.x, sphereToFollow.transform.position.y, this.transform.position.z);
			onRailGameObject.transform.position = newObjectPosition;
		}
	}
}
