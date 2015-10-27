using UnityEngine;
using System.Collections;

public class RailBehaviour : MonoBehaviour {

	public Vector3 defaultEulerAngles;

	public GameObject sphereToFollow;

	public GameObject onRailPrefabObject;

	private GameObject onRailGameObject;
	
	private bool perpendicularLevel;

	public void SetActivated(bool newValue)
	{
		InitObject ();
	}

	private void InitObject()
	{
		if (onRailGameObject == null)
		{
			Vector3 objectPosition = new Vector3 (perpendicularLevel ? 0 : sphereToFollow.transform.position.x, sphereToFollow.transform.position.y, perpendicularLevel ? sphereToFollow.transform.position.z : 0);
			Quaternion objectOrientation = Quaternion.Euler ( new Vector3 ( defaultEulerAngles.x, defaultEulerAngles.y + (perpendicularLevel ? 90.0f : 0.0f), defaultEulerAngles.z ) );
			onRailGameObject = (GameObject) Instantiate (onRailPrefabObject, objectPosition, objectOrientation);
			onRailGameObject.transform.parent = this.transform;
		}
	}

	// Use this for initialization
	void Start () 
	{
		float epsilon = 0.1f;
		perpendicularLevel = (Mathf.Abs (Mathf.Cos (Mathf.Deg2Rad * this.transform.parent.eulerAngles.y)) < epsilon);
		onRailGameObject = null;
		InitObject ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (onRailGameObject != null)
		{
			Vector3 newObjectPosition = new Vector3 (perpendicularLevel ? this.transform.position.x : sphereToFollow.transform.position.x, sphereToFollow.transform.position.y, perpendicularLevel ? sphereToFollow.transform.position.z : this.transform.position.z);
			onRailGameObject.transform.position = newObjectPosition;
		}
	}
}
