using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	
	public Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.


	Vector3 offset;                     // The initial offset from the target.


	void Start()
	{
			try
			{
					target = GameObject.FindGameObjectWithTag("focusedUnit").transform; // this is goint to find a certain tagged object from hirarchey and assing it to target.
			}
			catch (NullReferenceException ex)
			{
					Debug.Log("target gameObjects is not present in hierarchy ");
			}

			// Calculate the initial offset.
			offset = transform.position - target.position;
	}


	void FixedUpdate()
	{

			// Create a postion the camera is aiming for based on the offset from the target.
			Vector3 targetCamPos = target.position + offset;

			//**This changes the X value of camera to the X value of Unit.
			//**Need to find out how to integrate this into one function.
			targetCamPos.x = target.position.x;

			// Smoothly interpolate between the camera's current position and it's target position.
			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
