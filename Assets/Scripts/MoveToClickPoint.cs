using System.Collections;
using UnityEngine;
using UnityEngine.AI;
    

// ** BUGS **


public class MoveToClickPoint : MonoBehaviour {
  //NavMeshAgent component.
  NavMeshAgent agent;

  //Player camera script.
  public PlayerCameraMovement playerCamera;

  //Player character template script.
  public CharacterTemplate characterTemplate;

  //Radius of the walkable distance around player character.
  public float radius;

  //Position of the player character.
  private Vector3 centerPosition;

  //Boolean that checks if the character this script is attached to is active.
  public bool isCharacterActive;

  //Boolean that checks if the character is moving.
  public bool isCharacterMoving;

  // Boolean to see if the character is attacking;
  public bool isAttacking;

  //Raycast hit variable
  private RaycastHit hit;

  // Line renderer script
  public MovementRadiusRenderer movementRadiusRenderer;

  //Line renderer variable
  public LineRenderer line;
  // Variables for line renderer material
  public Material full;
  public Material half;
  public Material none;

  // Animator variable
  public Animator animator;

  // Variable to initialize script.
  public bool initialized = false;

  // Attack target
  public GameObject attackTarget;

  // Attack range
  public float stopDistance;
  
  void Awake() {
    //Gets the NavMesh of the object this script is attached to.
    agent = GetComponent<NavMeshAgent>();
    
    //Finds the player camera script.
    playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();

    // Finds character template script.
    characterTemplate = gameObject.GetComponent<CharacterTemplate>();

    //Gets the line renderer component on game object.
    line = gameObject.GetComponent<LineRenderer>();

    // Gets the movement radius script.
    movementRadiusRenderer = gameObject.GetComponent<MovementRadiusRenderer>();

    //Sets the character as inactive by default.
    SetCharacterInactive();

    //Grabs animator component.
    animator = gameObject.GetComponent<Animator>();
  }
  
  void Update() 
  {
    if (!initialized)
    {
      radius = characterTemplate.movementRadius;
      stopDistance = characterTemplate.attackRange;
      movementRadiusRenderer.xradius = radius;
      movementRadiusRenderer.yradius = radius;
      movementRadiusRenderer.enabled = true;
      initialized = true;
    }

    if (isAttacking && isCharacterActive && isCharacterMoving && Vector3.Distance (transform.position, attackTarget.transform.position) <= agent.stoppingDistance)
    {
      playerCamera.UpdateOffsetPosition();
      isCharacterMoving = false;

      SetCircleColor();
      StartCoroutine(AttackEnemy());

      //Update camera variable
      playerCamera.isCharMoving = false;

      // Attack trigger
    }
    else if (isCharacterActive && isCharacterMoving && agent.remainingDistance == 0)
    {
      playerCamera.UpdateOffsetPosition();
      isCharacterMoving = false;

      SetCircleColor();

      //Update camera variable
      playerCamera.isCharMoving = false;
      animator.SetBool("running", false);
    }

    if (Input.GetMouseButtonDown(1) && characterTemplate.actionPoints > 0) 
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

    // Gets the line color
    SetCircleColor();

    //Sets the avoidance priority. 0 is most importand, 99 is least. Least important will move around most important agents.
    agent.avoidancePriority = 50;
  }

  //Sets the character to the inactive state.
  public void SetCharacterInactive()
  {
    isCharacterActive = false;

    isAttacking = false;

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

        //location of character
        centerPosition = transform.localPosition; 

        //distance from ray cast hit to character location
        float distance = Vector3.Distance(hit.point, centerPosition);

        if ((distance < radius && hit.collider.gameObject.tag == "EnemyCharacter") || (hit.collider.gameObject.tag == "EnemyCharacter" && distance <= stopDistance))
        {
          Debug.Log("Player attacking");
          characterTemplate.actionPoints = 0;
          agent.stoppingDistance = stopDistance;
          agent.destination = hit.point;

          attackTarget = hit.collider.gameObject;
          Debug.Log(attackTarget.name);

          playerCamera.MoveCameraToCurrentPC();
          isCharacterMoving = true;
          animator.SetBool("running", true);

          playerCamera.isCharMoving = true;

          isAttacking = true;
        }
      
        //If the distance is less than the radius, it is already within the circle.
        else if (distance < radius && hit.collider.gameObject.tag != "PlayableCharacter") 
        {
          Debug.Log("Player moving");
          characterTemplate.actionPoints -= 1;
          agent.destination = hit.point;
          playerCamera.MoveCameraToCurrentPC();
          isCharacterMoving = true;
          animator.SetBool("running", true);

          //Update camera variable
          playerCamera.isCharMoving = true;
        }
      }
    }
  }

  // Used to show movement radius color.
  void SetCircleColor()
  {
    if (characterTemplate.actionPoints == 2)
    {
      line.material = full;
    }
    else if (characterTemplate.actionPoints == 1)
    {
      line.material = half;
    }
    else
    {
      line.material = none;
    }
  }

  // Function to stop enemy unit from clipping into the PC. Also ends movement turn.
  public void StopUnit()
  {
    agent.isStopped = true;
    agent.ResetPath();
    agent.isStopped = false;
    animator.SetBool("running", false);
    agent.stoppingDistance = 0;
    Debug.Log("Move over");
  }

    // Function to trigger enemy attack. Enemy attack scripts will be triggered here.
  IEnumerator AttackEnemy()
  {
    isAttacking = false;
    agent.isStopped = true;
    agent.ResetPath();
    agent.isStopped = false;
    animator.SetBool("running", false);

    // yield return here to wait for rotate to finish before continuing with the function.
    yield return StartCoroutine(RotateTowardEnemy());

    // ** Add attack trigger here.
    StartCoroutine(characterTemplate.AttackTarget(attackTarget));

    // Adjust this wait time as needed for enemy attack animation to finish.
    yield return new WaitForSecondsRealtime(2);
    Debug.Log("Attack over");
    agent.stoppingDistance = 0;
  }

  // Function to make the enemy rotate towards the target player.
  IEnumerator RotateTowardEnemy()
  {
    float elaspedTime = 0f;
    Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    while (elaspedTime < 1f)
    {
      transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
      elaspedTime += Time.deltaTime;
      yield return null;
    }
  }
}