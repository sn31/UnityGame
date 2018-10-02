using UnityEngine;
using System.Collections;

// ** IMPORTANT NOTES **
// NEED TO CHANGE XRADIUS AND YRADIUS TO USE MOVEMENT DISTANCE ON STAT SHEET FOR EACH CHARACTER.


// ** BUGS **
// - Doesn't resize even when entering new radius in script. Seems to only take variables at start up.

[RequireComponent(typeof(LineRenderer))]
public class MovementRadiusRenderer : MonoBehaviour {

	//number of lines in circle.
	public int segments = 50;

	//xradius of circle.
	public float xradius;

	//yradius of circle.
	public float yradius;

	//Variable to hold the line renderer component.
	LineRenderer line;

	void Start ()
	{
		//Grabs the line renderer component from game object.
		line = gameObject.GetComponent<LineRenderer>();
		
		line.positionCount = (segments + 1);
		line.useWorldSpace = false;
		CreatePoints ();
	}

	void CreatePoints ()
	{
		float x;
		// float y;
		float z;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

			line.SetPosition (i,new Vector3(x,0,z) );

			angle += (360f / segments);
		}
	}
}