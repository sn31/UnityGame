using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TurnManager : MonoBehaviour
{

    //Enumeration of state that the game mode can be in
    public enum TurnState
    {
        PreMove,
        Move,
        Combat,
        WinningConditions,
        PostMove
    }

    // The current game state.
    public static TurnState currentState;
    // Time the last state change occured.
    float lastStateChange = 0.0f;
    // Set to inputted TurnState

    public bool isPlayerTurn;
    public bool isEnemyTurn;

    //List of all PCs on the level.
    private GameObject[] pcList;
    //Current snapped PC
    public GameObject currentPC;
    //Current index of snapped PC
    public int currentPCIndex;

    public PlayerCameraMovement playerCamera;

    //Move to click script.
    public MoveToClickPoint moveScript;

    //Used to get where the camera should be at the end of a command.
    private GameObject cameraDummy;

    public MoveToClickPoint MoveToClickScript;

    // Use this for initialization.
    void Start()
    {
        //start new turn with first state
        SetCurrentState(TurnState.PreMove);

        playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();
        isPlayerTurn = true;
        isEnemyTurn = false;
        //Find camera dummy
        cameraDummy = GameObject.Find("PlayerCameraDummy");

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
    void Update()
    {
        switch (currentState)
        {
            case TurnState.PreMove:
                //todo: add characters to arrays and anything else that needs to be done
                if (GetStateElapsed() > 5.5f)
                    SetCurrentState(TurnState.Move);
                break;
            case TurnState.Move:
                //todo: Look for mouseclick and move the character with mouse click animations
                //if (player turn is over...... need a link from movetoclick once movement is complete)
                if (GetStateElapsed() > 10.0f)
                {
                SetCurrentState(TurnState.PreMove);
                }
                break; 
            case TurnState.Combat:
                //todo: Run combat encounter
                if (GetStateElapsed() > 2.5f)
                    SetCurrentState(TurnState.WinningConditions);
                break;
            case TurnState.WinningConditions:
                //todo: check for winning conditions
                break;
            case TurnState.PostMove:
                //todo: Any postmove cleanup or game management that needs to be done before next move
                break;
        }

    }

    public bool MoveComplete(bool cond)
    {
        return cond;
    }

    void SetCurrentState(TurnState state)
    {
        Debug.Log("TurnState:" + currentState + " changed to " + state + " state.");

        currentState = state;
        lastStateChange = Time.time;
    }

    //Get the elapsed time spend in the current state
    float GetStateElapsed()
    {
        return Time.time - lastStateChange;
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

        cameraDummy.GetComponent<CinemachineVirtualCamera>().m_Follow = currentPC.transform;
        cameraDummy.GetComponent<CinemachineVirtualCamera>().m_LookAt = currentPC.transform;
    }

    //Function for other scripts to get PCList.
    public GameObject[] GetPCList()
    {
        return pcList;
    }
}
