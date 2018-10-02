using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePath : MonoBehaviour {

	private GameObject targetEnemy;
	private bool isTraveling = false;
	public float speed;

	public float maxTime;
	public float currentTime;

	public Vector3 enemyLocation;


	// Use this for initialization
	void Start () 
	{
		currentTime = 0;
		maxTime = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (isTraveling)
		{
			// transform.position = Vector3.MoveTowards(transform.position, targetEnemy.transform.position, Time.deltaTime * speed);
			transform.position = Vector3.MoveTowards(transform.position, enemyLocation, Time.deltaTime * speed);
		}

		currentTime += Time.deltaTime;
		if (currentTime >= maxTime)
		{
			Destroy(gameObject);
		}
	}

	public IEnumerator TargetLocation(GameObject target)
	{
		targetEnemy = target;
		enemyLocation = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 1f, targetEnemy.transform.position.z);
		float distance = Vector3.Distance(target.transform.position, transform.position);
		speed = distance / .5f;
		isTraveling = true;
		yield return new WaitForSecondsRealtime(.5f);
	}

	// void OnTriggerEnter(Collider other)
	// {
	// 	if (other.gameObject == targetEnemy)
	// 	{
	// 		Destroy(gameObject);
	// 	}
	// }
}
