using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDetection : MonoBehaviour 
{
	// Enemy movement script.
	public EnemyMoveScript moveScript;
	// Mesh collider trigger.
	public SphereCollider detectionCollider;

	// Use this for initialization
	void Start () 
	{
		// moveScript = gameObject.GetComponent<EnemyMoveScript>();

		detectionCollider = GetComponent<SphereCollider>();

		detectionCollider.radius = moveScript.sightRadius;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PlayableCharacter"))
		{
			moveScript.SetAlerted();
			Debug.Log("ALERTED");
		}
	}
}
