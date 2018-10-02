using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guy : MonoBehaviour {

    public CloudGen cloud;

	// Use this for initialization
	void Start () {
        Debug.Log("I'm the guy");
        Debug.Log(transform.GetInstanceID());
	}
	
	// Update is called once per frame
	void Update () {
        cloud.mapObj = this.gameObject;
	}
}
