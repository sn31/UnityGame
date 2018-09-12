using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveDestination : MonoBehaviour
{

    //   public Transform goal;
    //// Use this for initialization
    //void Start () {
    //       NavMeshAgent agent = GetComponent<NavMeshAgent>();
    //       agent.destination = goal.position;
    //}



    public NavMeshAgent agent = new NavMeshAgent();

    Animator anim;
    public GameObject target;
    public float distance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("focusedUnit");


    }

    void Update()
    {
        //Check distance between target player and AI unit
        distance = Vector3.Distance(this.gameObject.transform.position, target.transform.position);
        if (target && distance < 7.0f)
        {
            MoveToLocation(target.transform.position);
            if (agent.remainingDistance < 1.2f)
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("IsIdle", true);

            }
            else
            {
                Debug.Log("yes");
                anim.SetBool("IsWalking", true);
                anim.SetBool("IsIdle", false);
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsIdle", true);
        }
    }

    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
    }

}

