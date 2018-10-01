using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour {
    public float scaleSpeed = 5;
    public float maxSize = 5;
    private bool desiredState; //Do we want this spot to have a cloud or not.
    private bool doScaling = false;
	
	
	void Update () {
        if (doScaling)
        {
            if (!desiredState) //scaling the cloud out of existance.
            {

                Vector3 desiredSize = new Vector3(transform.localScale.x - Time.deltaTime * scaleSpeed, transform.localScale.y - Time.deltaTime * scaleSpeed, transform.localScale.z - Time.deltaTime * scaleSpeed);
                transform.localScale = desiredSize;
                if (transform.localScale.x <= 0.1)
                {
                    gameObject.SetActive(false);
                    doScaling = false;
                }
            }
            else if (desiredState) //scalling the cloud into existence 
            {
                Vector3 desiredSize = new Vector3(transform.localScale.x + Time.deltaTime * scaleSpeed, transform.localScale.y + Time.deltaTime * scaleSpeed, transform.localScale.z + Time.deltaTime * scaleSpeed);
                transform.localScale = desiredSize;
                if (transform.localScale.x >= maxSize)
                {
                    transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                    gameObject.SetActive(true);
                    doScaling = false;
                }
            }
        }
	}
    public void ToggleScale(bool setType)
    {
        desiredState = setType;
        doScaling = true;

    }
}
