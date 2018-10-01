using UnityEngine;
using UnityEngine.AI;
    

// ** BUGS **


public class MoveToClickPoint : MonoBehaviour {
  //NavMeshAgent component.
  NavMeshAgent agent;

  //Player camera script.
  public PlayerCameraMovement playerCamera;

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

  //Line renderer variable
  public LineRenderer line;
  
  void Awake() {
    //Gets the NavMesh of the object this script is attached to.
    agent = GetComponent<NavMeshAgent>();
    
    //Finds the player camera script.
    playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();

    //Gets the line renderer component on game object.
    line = gameObject.GetComponent<LineRenderer>();

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
    }

    if (Input.GetMouseButtonDown(1)) 
    {
      MoveToClick();
    }
  }

  //Sets the character to the active state.
  public void SetCharacterActive()
  {
    //Bool to show which character is active on player turn.
    isCharacterActive = true;

    //Enables the line renderer to show walkable distance.
    line.enabled = true;

    //Sets the avoidance priority. 0 is most importand, 99 is least. Least important will move around most important agents.
    agent.avoidancePriority = 50;
  }

  //Sets the character to the inactive state.
  public void SetCharacterInactive()
  {
    isCharacterActive = false;

    //Disables line renderer that shows walkable distance.
    line.enabled = false;

    //Sets the avoidance priority. 0 is most importand, 99 is least. Least important will move around most important agents.
    agent.avoidancePriority = 49;
  }

  //Move to click point.
  void MoveToClick()
  {
    if (isCharacterActive && isCharacterMoving == false) 
    { 
      if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {

        //radius of walkable area
        radius = 8f; 
        //location of character
        centerPosition = transform.localPosition; 

        //distance from ray cast hit to character location
        float distance = Vector3.Distance(hit.point, centerPosition); 
      
        //If the distance is less than the radius, it is already within the circle.
        if (distance < radius && hit.collider.gameObject.tag != "PlayableCharacter") 
        {
          agent.destination = hit.point;
          playerCamera.MoveCameraToCurrentPC();
          isCharacterMoving = true;

          //Update camera variable
          playerCamera.isCharMoving = true;
        }
      }
    }
  }
}