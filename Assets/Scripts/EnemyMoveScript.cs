using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// ** NOTES
// Need to incorporate rotation for the enemy to face the player character.

public class EnemyMoveScript : MonoBehaviour 
{
  // Variable to hold NavMeshAgent
  public NavMeshAgent agent;

  // Boolean to hold if this enemy is alerted to the presence of the player.
  public bool isAlerted;

  // Boolean to see if enemy unit is in an action.
  public bool inAction;

  // Boolean to signal that the current unit turn is over.
  public bool unitTurnOver;

  // Boolean for if the enemy is dead.
  public bool isDead;

  // Boolean to see if attack script has triggered.
  public bool hasAttacked;

// Variables to find nearest PC.
  // Variable to hold nearest player character.
  public GameObject nearestPC;
  // Variable to hold the distance to nearest PC.
  public float nearestPCDistance;

  // Turn Manager script variable.
  public TurnManagerScript turnManager;

  // Character template script.
  public CharacterTemplate charTemplate;

  // Variable for list of PCs
  public GameObject[] pcList;

// Variables that hold the movement stats of this enemy unit.
  // Variable for move distance.
  public float movementRadius;

  // Player character character script.
  public CharacterTemplate playerCharTemp;

// Enum for unit state.
  public enum UnitState {None, Moving, Attacking};
  public UnitState currentState;

	// Use this for initialization
	void Start () 
  {
    //Grab TurnManagerScript
    turnManager = GameObject.Find("TurnManager").GetComponent<TurnManagerScript>();

    // Grabs the NavMeshAgent of object.
		agent = GetComponent<NavMeshAgent>();

    // Grabs the enemy CharacterTemplate script
    charTemplate = GetComponent<CharacterTemplate>();

    // Sets the default state of isAlerted bool to false;
    isAlerted = false;

    // Grabs PC list from Turn Manager Script.
    pcList = turnManager.GetPCList();

    // Sets inAction to false by default.
    inAction = false;

    // Sets unitTurnOver to false;
    unitTurnOver = true;

    // Sets the enemy unit to alive status;
    isDead = false;

    // Sets state to none.
    currentState = UnitState.None;

    // Sets state of attack to false;
    hasAttacked = false;

    // Sets the movementRadius to a default value from character stat.
    // ** NEED TO CHANGE THIS ONCE ENEMY STAT SCRIPT IS COMPLETE **
    movementRadius = 8;
	}
	
	// Update is called once per frame
	void Update () 
  {
    if (pcList == null)
    {
      pcList = turnManager.GetPCList();
    }
    // Checks if the unit who is attacking has reached the destination point.
    // ** Had to change condition from checking agent.remainingDistance to Vector3.distance because for some reason the remaining distance is slow to update. It would break my movement before the enemy even started moving.
		if (currentState == UnitState.Attacking && inAction == true && Vector3.Distance (transform.position, nearestPC.transform.position) <= agent.stoppingDistance)
    {

      if (currentState == UnitState.Attacking && hasAttacked == false)
      {
        StartCoroutine(AttackPC());
      }
    }
    
    // Checks if the unit who is moving has reached the destination point.
    if (currentState == UnitState.Moving && inAction == true && Vector3.Distance (transform.position, nearestPC.transform.position) <= agent.stoppingDistance)
    {
      StopUnit();
    }
	}

  // Sets the character to be alerted to the player
  public void SetAlerted()
  {
    isAlerted = true;
  }

  // Finds the nearest PC;
  public void FindNearestPC()
  {
    // Initially set distance to a high number.
    nearestPCDistance = Mathf.Infinity;
    
    foreach (GameObject PC in pcList)
    {
      playerCharTemp = PC.GetComponent<CharacterTemplate>();
      if (playerCharTemp.isDead == false)
      {
        // Grabs the distance between this enemy and a PC in the array.
      float distance = Vector3.Distance(transform.position, PC.transform.position);

        if (distance < nearestPCDistance)
        {
          nearestPC = PC;
          nearestPCDistance = distance;
        }
      }
    }
    MoveToNearestPC();
  }

  public void MoveToNearestPC()
  {
    // Sets bool inAction to true;
    inAction = true;
    unitTurnOver = false;

    // Checks if the nearest PC is within the circle of this enemy units movement radius. If it is, move to nearest point and attack. If not moves towards him.
    float distance = Vector3.Distance(nearestPC.transform.position, transform.position); 

    // if (distance < movementRadius)
    // {
    //   Debug.Log("Attacking");
    //   currentState = UnitState.Attacking;
    //   hasAttacked = false;

    //   // Adds stopping distance so the unit doesn't run into the PC.
    //   agent.stoppingDistance = 2.5f;

    //   agent.SetDestination(nearestPC.transform.position);
    // }
    // else if (distance >= movementRadius)
    // {
    //   Debug.Log("Moving");
    //   currentState = UnitState.Moving;

    //   // Adds a stopping distance equal to the PC position minus the radius.
    //   agent.stoppingDistance = distance - movementRadius;

    //   agent.destination = nearestPC.transform.position;
    //   Debug.Log("stopping distance = " + agent.stoppingDistance + " and remaining distance = " + agent.remainingDistance);
    // }



    // ********* BUG **********
    // ** Minor issue to fix here. Since we are using stoppingDistance, the unit has a 'reach' of 2.35. For example, if we use 8 as the amount of game units the enemy can move, they can attack units 10.35 units away.
    agent.stoppingDistance = distance - movementRadius;

    if (agent.stoppingDistance < 2.35)
    {
      currentState = UnitState.Attacking;
      hasAttacked = false;

      // Adds stopping distance so the unit doesn't run into the PC.
      agent.stoppingDistance = 2.35f;

      agent.SetDestination(nearestPC.transform.position);
    }
    else
    {
      currentState = UnitState.Moving;

      agent.destination = nearestPC.transform.position;
    }
  }

  // Function to trigger enemy attack. Enemy attack scripts will be triggered here.
  IEnumerator AttackPC()
  {
    hasAttacked = true;
    agent.isStopped = true;
    agent.ResetPath();
    agent.isStopped = false;

    // ** Add attack trigger here.
    charTemplate.AttackTarget(nearestPC);

    // Adjust this wait time as needed for enemy attack animation to finish.
    yield return new WaitForSecondsRealtime(2);
    Debug.Log("Attack over");
    currentState = UnitState.None;
    inAction = false;
    agent.stoppingDistance = 0;
    unitTurnOver = true;
    turnManager.currentEnemyDone = true;
  }

  // Function to stop enemy unit from clipping into the PC. Also ends movement turn.
  public void StopUnit()
  {
    agent.isStopped = true;
    agent.ResetPath();
    agent.isStopped = false;
    currentState = UnitState.None;
    inAction = false;
    agent.stoppingDistance = 0;
    unitTurnOver = true;
    turnManager.currentEnemyDone = true;
    Debug.Log("Move over");
  }
}
