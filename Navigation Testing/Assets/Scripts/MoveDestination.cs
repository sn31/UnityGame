using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveDestination : MonoBehaviour
{

	//   public Transform goal;
	//// Use this for initialization
	//void Start () {
	//       NavMeshAgent agent = GetComponent<NavMeshAgent>();
	//       agent.destination = goal.position;
	//}



	public NavMeshAgent agent = new NavMeshAgent();
    public bool Walking = false;

	Animator anim;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
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

		if (agent.remainingDistance < 1.2f)
		{
			anim.SetBool("IsWalking", false);
			anim.SetBool("IsIdle", true);
            Walking = false;


		}
		else
		{
			anim.SetBool("IsWalking", true);
			anim.SetBool("IsIdle", false);
            Walking = true;

		}
	}
}