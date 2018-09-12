using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviour {

	//   public Transform goal;
	//// Use this for initialization
	//void Start () {
	//       NavMeshAgent agent = GetComponent<NavMeshAgent>();
	//       agent.destination = goal.position;
	//}

    public NavMeshAgent agent = new NavMeshAgent();

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
			{
				agent.destination = hit.point;
			}
		}
	}
}

