using UnityEngine;
using UnityEngine.AI;
    

// ** BUGS **
//When tabbing, deactivating NavMeshAgent/Obstacle and enabling the counterpart component causes the character to teleport a shor distance.

public class MoveToClickPoint : MonoBehaviour {
  //NavMeshAgent component.
  NavMeshAgent agent;

  //The layer that the RayCast interacts with.
  // int layer_mask;

  //Player camera script.
  public PlayerCameraMovement playerCamera;

  //The walkable distance circle.
  public GameObject distanceRadius;

  //Radius of the walkable distance around player character.
  public float radius;

  //Position of the player character.
  private Vector3 centerPosition;

  //Boolean that checks if the character this script is attached to is active.
  public bool isCharacterActive;

  //Boolean that checks if the character is moving.
  public bool isCharacterMoving;

  //Raycast hit variable
  private RaycastHit hit;

  //Nav Mesh obstacle component variable.
  private NavMeshObstacle obstacle;
  
  void Start() {
    //Gets the NavMesh of the object this script is attached to.
    agent = GetComponent<NavMeshAgent>();
    
    //Finds the player camera script.
    playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();

    //Get the MaxDistanceRadius on game object.
    distanceRadius = transform.Find("MoveDistanceRadius").gameObject;

    //Get the Nav Mesh obstacle component.
    obstacle = GetComponent<NavMeshObstacle>();

    //Finds the script of the 'WalkableArea'.
    // layer_mask = LayerMask.GetMask("WalkableArea");

    //Sets the character as inactive by default.
    SetCharacterInactive();
  }
  
  void Update() 
  {
    if (isCharacterActive && isCharacterMoving && agent.remainingDistance == 0)
    {
      playerCamera.UpdateOffsetPosition();
      isCharacterMoving = false;

      //Update camera variable
      playerCamera.isCharMoving = false;

      //Activate distance radius again if the character still has a move left.
      //** NEED TO CHANGE ONCE WE HAVE MAX ACTIONS PER TURN STATS **
      // distanceRadius.SetActive(true);
    }

    if (Input.GetMouseButtonDown(1)) 
    {
      MoveToClick();
    }
  }

  //Sets the character to the active state.
  public void SetCharacterActive()
  {
    obstacle.enabled = false;
    
    isCharacterActive = true;

    //Sets the avoidance priority. 0 is most importand, 99 is least. Least important will move around most important agents.

    agent.enabled = true;

    // distanceRadius.SetActive(true);
  }

  //Sets the character to the inactive state.
  public void SetCharacterInactive()
  {
    agent.enabled = false;
    isCharacterActive = false;

    //Sets the avoidance priority. 0 is most importand, 99 is least. Least important will move around most important agents.

    
    obstacle.enabled = true;
    
    // distanceRadius.SetActive(false);
  }

  //Move to click point.
  void MoveToClick()
  {
    if (isCharacterActive && isCharacterMoving == false) 
    { 
      if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {

        radius = 8f; //radius of walkable area
        centerPosition = transform.localPosition; //location of character

        float distance = Vector3.Distance(hit.point, centerPosition); //distance from ray cast hit to character location
      
        if (distance < radius) //If the distance is less than the radius, it is already within the circle.
        {
          agent.destination = hit.point;
          playerCamera.MoveCameraToCurrentPC();
          isCharacterMoving = true;

          //Update camera variable
          playerCamera.isCharMoving = true;

          //Disable distance radius while moving.
          distanceRadius.SetActive(false);
        }
      }
    }
  }

  //Function to stop movement when this character collides with another character.
  //This prevents the bug of this agent trying to navigate to a position already occupied by another agent.
  // void OnTriggerStay(Collider other)
  // {
  //   if (other.tag == "PlayableCharacter" && isCharacterActive && isCharacterMoving)
  //   {
  //     float currentDistance = Vector3.Distance(other.transform.position, hit.point);
  //     Debug.Log("TRIGGERED");
  //     if (currentDistance <= 1.5f) // This variable should be set to the size of the collider.
  //     {
  //       Debug.Log("STOPPED");
  //       //Stops the agent.
  //       // agent.isStopped = true;

  //       // agent.destination = transform.position;
  //       agent.destination = other.ClosestPoint(hit.point);


  //       // isCharacterMoving = false;

  //       // //Update camera variable
  //       // playerCamera.isCharMoving = false;

  //       // //Resumes the agent.
  //       // agent.isStopped = false;
  //     }
  //   }
  // }
}