using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour {
    NavMeshAgent agent;
    Animator animator;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) 
            {
                    agent.destination = hit.point;
                    animator.SetBool("Walking", true);
            }
            if (agent.remainingDistance < 1.2f)
            {
                animator.SetBool("Walking", false);
            }
        }
	}
}
