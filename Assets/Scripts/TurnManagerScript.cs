using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour {

  public bool isPlayerTurn;
  public bool isEnemyTurn;

  //List of all PCs on the level.
	private GameObject[] pcList;
	//Current snapped PC
	public GameObject currentPC;
	//Current index of snapped PC
	public int currentPCIndex;
  //Get the player camera script attached to player camera object.
  public PlayerCameraMovement playerCamera;
  //Move to click script.
  public MoveToClickPoint moveScript;

	// Use this for initialization.
	void Start () {
		playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();
    isPlayerTurn = true;
    isEnemyTurn = false;

    //Get list of playable characters.
    if (pcList == null)
		{
			pcList = GameObject.FindGameObjectsWithTag("PlayableCharacter");
		}
		currentPCIndex = 0;
		currentPC = pcList[0];

    //Sets the first PC to active state.
    moveScript = currentPC.GetComponent<MoveToClickPoint>();
    moveScript.SetCharacterActive();
	}
	
	// Update is called once per frame.
	void Update () {

	}

  //Function to select next PC.
  public void SelectNextPC()
  {
    moveScript = currentPC.GetComponent<MoveToClickPoint>();
    moveScript.SetCharacterInactive();

    currentPCIndex++;
    if (currentPCIndex == pcList.Length)
    {
      currentPCIndex = 0;
    }
    currentPC = pcList[currentPCIndex];

    moveScript = currentPC.GetComponent<MoveToClickPoint>();
    moveScript.SetCharacterActive();

    playerCamera.currentPCIndex = currentPCIndex;
    playerCamera.currentPC = currentPC;
  }

  //Function for other scripts to get PCList.
  public GameObject[] GetPCList()
  {
    return pcList;
  }
}
