using UnityEngine;
using System.Collections;

public class CameraEffectsBehaviour : MonoBehaviour {

	public float maxShake;

	private Vector3 actualPosition;

	private bool isShaking;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isShaking)
		{
			float deltaShakeRight = Random.Range(-maxShake,maxShake);
			float deltaShakeUp = Random.Range(-maxShake,maxShake);
			this.transform.localPosition = actualPosition + deltaShakeRight * this.transform.right + deltaShakeUp * this.transform.up;
		}
	}

	public void Shake(float time)
	{		
		isShaking = true;
		actualPosition = this.transform.localPosition;
		StartCoroutine (WaitAndStopShake (time));
	}


	IEnumerator WaitAndStopShake(float timer)
	{
		yield return new WaitForSeconds(timer);
		isShaking = false;
		this.transform.localPosition = actualPosition;
	}
}
