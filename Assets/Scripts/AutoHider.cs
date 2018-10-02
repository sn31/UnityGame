using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHider : MonoBehaviour {
    public GameObject mapObj;
    public float desiredDistance = 8;
    public float maxDistance = 100;
    public bool doShow = true;
    public bool doScale = false;

    void Start()
    {
        Debug.Log("Here");
    }
    // Update is called once per frame
    void Update () {
        disappearCheck();
	}
    private void disappearCheck()
    {
        int counter = 0;
        foreach (Transform child in mapObj.transform)
        {
            counter++;
            float distance = Vector3.Distance(child.position, transform.position);
            if (distance < desiredDistance) //the child cloud is within the desirable distance
            {
                if (!doShow) //Show the object within the desired distance, if false = showing
                {
                    if (!child.gameObject.activeInHierarchy)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                else //Hide the child even though it is within the desired distance
                {
                    if (child.gameObject.activeInHierarchy && !doScale) // The child is inactive and we don't want to scale it
                    {
                        child.gameObject.SetActive(false);
                    }
                    else if (child.gameObject.activeInHierarchy && doScale) //The child is active but we do want to scale it
                    {
                        child.GetComponent<Scaler>().ToggleScale(false);
                    }
                }
            }
            else //what to do when the object is outside of the desired distance. If the user already visited that point of the map, let's keep this unhidden?
            {
                if (!doShow) //if false = hiding
                {
                    if (child.gameObject.activeInHierarchy)
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                else //child is outside of the desired distance but we still want to show it
                {
                    if (distance <maxDistance) //Don't want to have a bunch of objects outside of the max range b/c that would slow things down
                    {
                        if (!child.gameObject.activeInHierarchy && !doScale)
                        {
                            child.gameObject.SetActive(true);
                        }
                        else if(!child.gameObject.activeInHierarchy && doScale)
                        {
                            child.gameObject.SetActive(true);
                            child.GetComponent<Scaler>().ToggleScale(true);
                        }
                    }
                    else //if child is outside of the max range, hide them all!
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
        Debug.Log(counter);
    }
}
