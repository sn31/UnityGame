using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePath : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public IEnumerator TargetLocation(GameObject target)
	{
		while (transform.position != target.transform.position)
		{
			transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * 10);
		}
		
		Destroy(gameObject);
		yield return null;
	}
}
