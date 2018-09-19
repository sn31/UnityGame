using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
{

    //NavMeshAgent component.
    NavMeshAgent agent;

    //The layer that the RayCast interacts with.
    int layer_mask;

    //Player camera script.
    public PlayerCameraMovement playerCamera;

    //The walkable distance circle.
    public GameObject distanceRadius;

    //Boolean that checks if the character this script is attached to is active.
    public bool isCharacterActive;

    //Boolean that checks if the character is moving.
    public bool isCharacterMoving;

    //Initialize the Animator.
    Animator animator;

    void Start()
    {
        //Gets the NavMesh of the object this script is attached to.
        agent = GetComponent<NavMeshAgent>();

        //Finds the player camera script.
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();

        //Get the MaxDistanceRadius on game object.
        distanceRadius = transform.Find("MoveDistanceRadius").gameObject;

        //Finds the script of the 'WalkableArea'.
        layer_mask = LayerMask.GetMask("WalkableArea");

        //Sets the character as inactive by default.
        SetCharacterInactive();

        //Get the Animator on the Player
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (isCharacterActive && isCharacterMoving && agent.remainingDistance == 0)
        {
            playerCamera.UpdateOffsetPosition();
            isCharacterMoving = false;
			animator.SetBool("IsWalking", false);

            //Update camera variable
            playerCamera.isCharMoving = false;

            //Activate distance radius again if the character still has a move left.
            //** NEED TO CHANGE ONCE WE HAVE MAX ACTIONS PER TURN STATS **
            distanceRadius.SetActive(true);
        }

        if (TurnManager.currentState == TurnManager.TurnState.Move)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Player now moving after a proper click");
                MoveToClick();
            }
        }
    }

    //Sets the character to the active state.
    public void SetCharacterActive()
    {
        isCharacterActive = true;
        distanceRadius.SetActive(true);
    }

    //Sets the character to the inactive state.
    public void SetCharacterInactive()
    {
        isCharacterActive = false;
        distanceRadius.SetActive(false);
    }

    //Move to click point.
    void MoveToClick()
    {
        if (isCharacterActive && isCharacterMoving == false)
        {
            RaycastHit hit;


            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, layer_mask))
            {
                agent.destination = hit.point;
                playerCamera.MoveCameraToCurrentPC();
                isCharacterMoving = true;
                animator.SetBool("IsWalking", true);

                //Update camera variable
                playerCamera.isCharMoving = true;

                //Disable distance radius while moving.
                distanceRadius.SetActive(false);
            }
        }
    }
}