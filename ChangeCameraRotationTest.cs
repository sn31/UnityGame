using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraRotationTest : MonoBehaviour {

	public float speed = 5f;

	private GameObject startRotation;
	private GameObject targetRotation;
	private bool isRotating;

	void Start()
	{
		startRotation = new GameObject("startCamRotation");
		targetRotation = new GameObject("targetCamRotation");
		isRotating = false;
	}

// Update is called once per frame
	void Update () 
	{
	//Need to change mouse click to rotate button
		if (Input.GetMouseButtonDown(1) && isRotating == false)
		{
			isRotating = true;
		//Get current values of camera rotation.
			startRotation.transform.rotation = transform.rotation;
			targetRotation.transform.rotation = transform.rotation;

		//This adds -90 degrees to Y axis of target rotation.
			targetRotation.transform.Rotate(0, -90, 0, Space.World);
		}
		
		if (transform.rotation != targetRotation.transform.rotation && isRotating == true)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation.transform.rotation, Time.deltaTime * speed);
			if (transform.rotation == targetRotation.transform.rotation)
			{
				isRotating = false;
			}
		}
	}
}
