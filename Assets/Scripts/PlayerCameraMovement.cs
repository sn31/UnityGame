using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// ** BUGS **

public class PlayerCameraMovement : MonoBehaviour {
//Dummy camera to show where the camera should be at end of a command.
	//Used to get where the camera should be at the end of a command.
	private GameObject cameraDummy;

//To check if current PC is moving	**Manipulated by the MoveToClickPoint script.
	public bool isCharMoving = false;

//Variables for camera rotation.
	//Check if camera is currently in rotation right.
	private bool isRotatingRight;
	//Check if camera is currently in rotation left.
	private bool isRotatingLeft;
	//Total degrees camera has rotated so far once it starts rotation.
	private float totalDegree;
	//Used to smoothly rotate camera.
	private float increments;
	//camera offset position variable
	private Vector3 cameraOffset;

//Variables to snap camera to next PC
	//List of all PCs on the level.
	private GameObject[] pcList;
	//Current snapped PC
	public GameObject currentPC;
	//Current index of snapped PC
	public int currentPCIndex;
	//Reference to Cinemachine Script
	private CinemachineVirtualCamera cvc;
	//Timer to prevent spam
	private float snapTimerCooldown = 1.0f;
	//Time to measure
	private float snapTimer;
	//Current snap target
	private GameObject snapTarget;
	//Distance between current target and snap target.
	private Vector3 snapTargetDistance;
	//Increments for smooth snap targeting
	private Vector3 targetPosDestination;
	//Boolean to track whether camera is currently snapping.
	private bool isSnapping;
	
//Variable for camera panning when mouse is near edge of screen.
	//Boolean to check if camera is currently panning.
	public bool isCamMoving;
	//Panning speed of camera.
	private float camMoveSpeed;
	//variable to hold screen width.
	public int screenWidth;
	//variable to hold screen height.
	public int screenHeight;
	//variable to hold the movement increments
	private Vector3 camPos;

//WASD Camera pan variables
	//Move speed of camera
	public float keyCamMoveSpeed = 20;
	//Camera position increments
	private Vector3 keyCamPos;

//Variable to fix camera offset at game start. For some reason, using cameraOffset = ... in Start() gives wrong Vector3. If this isn't here, using SnapToNextPC at start without rotating camera first gives wrong end point destination.
	private bool initialSnapToPC = false;

//TurnManager script
// **** NEED TO CHANGE ONCE WE START MERGING SCRIPTS **
  public TurnManagerScript turnManager;

	void Start()
	{
    //Grab TurnManagerScript
    turnManager = GameObject.Find("TurnManager").GetComponent<TurnManagerScript>();
    
		//Camera Rotation startup
		cameraDummy = GameObject.Find("PlayerCameraDummy");
		isRotatingRight = false;
		isRotatingLeft = false;

		//Find the first PC on startup
		cvc = gameObject.GetComponent<CinemachineVirtualCamera>();
		isSnapping = false;
		if (pcList == null)
		{
			UpdatePCList();
		}

    //Get currently selected character from TurnManagerScript.
		currentPCIndex = turnManager.currentPCIndex;
		currentPC = turnManager.currentPC;

		//Snap camera to first index of pcList.
		cvc.m_Follow = currentPC.transform;
		cvc.m_LookAt = currentPC.transform;

		//Snaps camera dummy to same character
		cameraDummy.GetComponent<CinemachineVirtualCamera>().m_Follow = cvc.m_Follow;
		cameraDummy.GetComponent<CinemachineVirtualCamera>().m_LookAt = cvc.m_LookAt;

		//Give values to camera pan variables
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		//Camera pan speed
		camMoveSpeed = 20f;
	}

// Update is called once per frame
	void Update () 
	{
		//Used to correct wrong camera destination at game start if using this function before rotating the camera.
		if (initialSnapToPC == false)
		{
			UpdateOffsetPosition();
			initialSnapToPC = true;
		}

		//Rotate camera right 90 degrees
		if (Input.GetKeyDown(KeyCode.E))
		{
			RotateCameraRight();
		}

		//Loop for rotating camera right
		if (isRotatingRight == true)
		{
			RotateCameraRightLoop();
		}

		//Rotate camera left 90 degrees
		if (Input.GetKeyDown(KeyCode.Q))
		{
			RotateCameraLeft();
		}

		//Loop for rotating camera left
		if (isRotatingLeft == true)
		{
			RotateCameraLeftLoop();
		}

		//Timer so snapping is not spammable.
		snapTimer += Time.deltaTime;

		//Press 'TAB' to snap camera to different character in pcList
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			CamToNextPC();
		}

		//Press 'HOME' to snap camera back to current target
		if (Input.GetKeyDown(KeyCode.Home))
		{
			MoveCameraToCurrentPC();
		}

		//Loop to keep moving camera to target position if isSnapping is true.
		if (isSnapping)
		{
			SnapToNextPC();
		}

		//Used to pan camera if mouse is at edge of screen.
		if (isSnapping == false && isRotatingLeft == false && isRotatingRight == false && isCharMoving == false)
		{
			MoveCamWithMouse();
		}

		//WASD keys to move camera around.
		if (Input.GetKey(KeyCode.D))
		{
			DCameraRight();
		}
		if (Input.GetKey(KeyCode.A))
		{
			ACameraLeft();
		}
		if (Input.GetKey(KeyCode.S))
		{
			SCameraDown();
		}
		if (Input.GetKey(KeyCode.W))
		{
			WCameraUp();
		}
	}

// Prepares camera to rotate right 90 degrees
	void RotateCameraRight()
	{
		if (isRotatingLeft == false && isRotatingRight == false && isSnapping == false && isCamMoving == false && isCharMoving == false)
		{
			increments = -135 * Time.deltaTime;
			totalDegree = 0;

			isRotatingRight = true;

		//Get current values of camera rotation.
			cameraDummy.transform.rotation = transform.rotation;

		//This adds -90 degrees to Y axis of target rotation.
			cameraDummy.transform.Rotate(0, -90, 0, Space.World);
		}
	}

	//This function loops in Update() until the camera reaches the destination point.
	void RotateCameraRightLoop()
	{
		if (totalDegree >= -90)
		{
			transform.Rotate(0, increments, 0, Space.World);
			totalDegree += increments;

			if (totalDegree <= -90)
			{
				//Need to use = cameraDummy. If this isn't here, transform.rotation will be off a little bit everytime it rotates, eventually adding up and throwing off game camera alignment.
				transform.rotation = cameraDummy.transform.rotation;

				UpdateOffsetPosition();
				isRotatingRight = false;
			}
		}
	}

	// Prepares camera to rotate left 90 degrees
	void RotateCameraLeft()
	{
		if (isRotatingLeft == false && isRotatingRight == false && isSnapping == false && isCamMoving == false && isCharMoving == false)
		{
			increments = 135 * Time.deltaTime;
			totalDegree = 0;

			isRotatingLeft = true;

		//Get current values of camera rotation.
			cameraDummy.transform.rotation = transform.rotation;

		//This adds 90 degrees to Y axis of target rotation.
			cameraDummy.transform.Rotate(0, 90, 0, Space.World);
		}
	}
			
	//This function loops in Update() until the camera reaches the destination point.
	void RotateCameraLeftLoop()
	{
		if (totalDegree <= 90)
		{
			transform.Rotate(0, increments, 0, Space.World);
			totalDegree += increments;

			if (totalDegree >= 90)
			{
				//Need to use = cameraDummy. If this isn't here, transform.rotation will be off a little bit everytime it rotates, eventually adding up and throwing off game camera alignment.
				transform.rotation = cameraDummy.transform.rotation;

				UpdateOffsetPosition();
				isRotatingLeft = false;
			}
		}
	}

	void FindIndexOfPC()
	{
		for (int i=0; i < pcList.Length; i++)
 		{
     if (pcList[i] == currentPC)
     {
        currentPCIndex = i;
        break;
     }
 		}
	}

	//Function to get snap target
	void CamToNextPC()
	{
		if (snapTimer >= snapTimerCooldown && isSnapping == false && isRotatingRight == false && isRotatingLeft == false && isCamMoving == false && isCharMoving == false)
		{
			snapTimer = 0f;
			ClearFollowTarget();

      //Select next character in turn manager.
			turnManager.SelectNextPC();

			targetPosDestination = currentPC.transform.position + cameraOffset;
			isSnapping = true;
		}
	}

	//Function to snap to next PC target
	void SnapToNextPC()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPosDestination, Time.deltaTime * 20);

		if (transform.position == targetPosDestination)
		{
			transform.position = targetPosDestination;
			cvc.m_Follow = currentPC.transform;
		  cvc.m_LookAt = currentPC.transform;

			cameraDummy.GetComponent<CinemachineVirtualCamera>().m_Follow = cvc.m_Follow;
			cameraDummy.GetComponent<CinemachineVirtualCamera>().m_LookAt = cvc.m_LookAt;

			UpdateOffsetPosition();
			isSnapping = false;
		}
	}

	//Move camera when mouse is near the edge of screen.
	void MoveCamWithMouse()
	{	
		camPos = transform.position;
		if (Input.mousePosition.x > screenWidth - 30)
		{
			isCamMoving = true;
			ClearFollowTarget();

			//Desired direction. Y = 0 because we don't want to change Y. We use the local X and Z axis to normalize into the right direction without changing height
			camPos = Vector3.Normalize(new Vector3(transform.right.x, 0, transform.right.z));

			transform.position += camPos * Time.deltaTime * camMoveSpeed;
		}
		else if (Input.mousePosition.x < 30)
		{
			isCamMoving = true;
			ClearFollowTarget();
			
			camPos = Vector3.Normalize(new Vector3(-transform.right.x, 0, -transform.right.z));

			transform.position += camPos * Time.deltaTime * camMoveSpeed;
		}
			
		else if (Input.mousePosition.y > screenHeight - 30)
		{
			isCamMoving = true;
			ClearFollowTarget();

			camPos = Vector3.Normalize(new Vector3(transform.forward.x, 0, transform.forward.z));

			transform.position += camPos * Time.deltaTime * camMoveSpeed;
		}
		else if (Input.mousePosition.y < 30)
		{
			isCamMoving = true;
			ClearFollowTarget();
			
			camPos = Vector3.Normalize(new Vector3(-transform.forward.x, 0, -transform.forward.z));

			transform.position += camPos * Time.deltaTime * camMoveSpeed;
		}
		else
		{
			isCamMoving = false;
		}
	}

	//Snaps camera to currentPC
	public void MoveCameraToCurrentPC()
	{
		if (snapTimer >= snapTimerCooldown && isSnapping == false && isRotatingRight == false && isRotatingLeft == false && isCamMoving == false && cvc.m_Follow == null && cvc.m_LookAt == null && isCharMoving == false)
		{
			snapTimer = 0f;
			ClearFollowTarget();
			currentPC = pcList[currentPCIndex];
			targetPosDestination = currentPC.transform.position + cameraOffset;
			isSnapping = true;
		}
	}

	//W key to pan camera up
	void WCameraUp()
	{
		if (isSnapping == false && isCharMoving == false)
		{
			ClearFollowTarget();
			keyCamPos = Vector3.Normalize(new Vector3(transform.forward.x, 0, transform.forward.z));
			transform.position += keyCamPos * Time.deltaTime * camMoveSpeed;
		}
	}

	void SCameraDown()
	{
		if (isSnapping == false && isCharMoving == false)
		{
			ClearFollowTarget();
			keyCamPos = Vector3.Normalize(new Vector3(-transform.forward.x, 0, -transform.forward.z));
			transform.position += keyCamPos * Time.deltaTime * camMoveSpeed;
		}
	}

	void ACameraLeft()
	{
		if (isSnapping == false && isCharMoving == false)
		{
			ClearFollowTarget();
			keyCamPos = Vector3.Normalize(new Vector3(-transform.right.x, 0, -transform.right.z));
			transform.position += keyCamPos * Time.deltaTime * camMoveSpeed;
		}
	}

	void DCameraRight()
	{
		if (isSnapping == false && isCharMoving == false)
		{
			ClearFollowTarget();
			keyCamPos = Vector3.Normalize(new Vector3(transform.right.x, 0, transform.right.z));
			transform.position += keyCamPos * Time.deltaTime * camMoveSpeed;
		}
	}

	void ClearFollowTarget()
	{
		cvc.m_Follow = null;
		cvc.m_LookAt = null;
	}

	//To update offset position. Used to fix jittery camera bug when moving a character after panning with mouse.
	public void UpdateOffsetPosition()
	{
		if (cvc.m_Follow != null && cvc.m_LookAt != null)
		{
			cameraOffset = transform.position - currentPC.transform.position;
		}
		else
		{
			cameraOffset = cameraDummy.transform.position - currentPC.transform.position;
		}
	}

  //To get and update PCList.
  public void UpdatePCList()
  {
    pcList = turnManager.GetPCList();
  }
}
