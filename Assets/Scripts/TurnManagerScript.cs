using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// **NOTES**
// *NEED TO CHANGE StartPlayerTurn() TO CHECK FOR ALIVE CHARACTERS FOR INITIAL INDEX ONCE STAT SHEET IS DONE.
// Need to incorporate enemy camera for enemy turn.

public class TurnManagerScript : MonoBehaviour 
{
  // List of all PCs on the level.
	private GameObject[] pcList;
	// Current snapped PC.
	public GameObject currentPC;
	// Current index of snapped PC.
	public int currentPCIndex;
  // Get the player camera script attached to player camera object.
  public PlayerCameraMovement playerCamera;
  // Move to click script.
  public MoveToClickPoint moveScript;

  // Enum to determine who's turn it is.
  public enum TurnOwner {None, Player, Enemy};
  // Object to track TurnOwner.
  public TurnOwner currentTurn;
  // Boolean to signal that the current turn is over.
  public bool turnOver;

  // List of all enemy characters on the level.
  private GameObject[] enemyList;
  // Current enemy unit.
  public GameObject currentEnemy;
  // Bool for signals if the current enemy is done moving.
  public bool currentEnemyDone;
  // Current index of snapped PC.
  public int currentEnemyIndex;
  // Variable to hold enemy move script.
  public EnemyMoveScript enemyMoveScript;

	// Use this for initialization.
	void Start () 
  {
		playerCamera = GameObject.Find("PlayerCamera").GetComponent<PlayerCameraMovement>();
    currentTurn = TurnOwner.Player;

    //Get list of playable characters.
    if (pcList == null)
		{
			UpdatePCList();
		}
		currentPCIndex = 0;
		currentPC = pcList[0];

    //Sets the first PC to active state.
    moveScript = currentPC.GetComponent<MoveToClickPoint>();
    moveScript.SetCharacterActive();

    //Grabs all the enemy units on the level.
    if (enemyList == null)
    {
      UpdateEnemyList();
    }
    currentEnemyIndex = 0;
    currentEnemy = enemyList[currentEnemyIndex];

    // Set booleans to default values.
    currentEnemyDone = true;
    turnOver = false;
	}
	
	// Update is called once per frame.
	void Update () 
  {
    if (currentTurn == TurnOwner.Player && turnOver == true)
    {
      // Put a condition in here to start enemy turn on condition true.
      // Not really needed since we have an 'End Turn' button but useful for automatic end of turn.
    }

    if (currentTurn == TurnOwner.Enemy && currentEnemyDone == true)
    {
      currentEnemyDone = false;
      StartCoroutine(MoveCurrentEnemy());
    }
	}

  // Public function to update PC list. Just in case we drop in more units mid-game.
  public void UpdatePCList()
  {
    pcList = GameObject.FindGameObjectsWithTag("PlayableCharacter");
  }

  // Public function to update enemy list. Use this to update list for mid-game spawns.
  public void UpdateEnemyList()
  {
    enemyList = GameObject.FindGameObjectsWithTag("EnemyCharacter");
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

  //Function to end player turn.
  public void EndPlayerTurn()
  {
    //Sets current character to inactive.
    moveScript.SetCharacterInactive();

    //Updates bool in Player Camera to false.
    playerCamera.IsPlayerTurn = false;

    //Sets current turn to enemy.
    currentTurn = TurnOwner.Enemy;
    turnOver = false;

    //Starts the enemy turn. This is currently a test function.
    StartEnemyTurn();
  }

  //Function to switch to player turn.
  public void StartPlayerTurn()
  {
    //Sets current turn to player
    currentTurn = TurnOwner.Player;

    //Updates bool in Player Camera to true.
    playerCamera.IsPlayerTurn = true;

    //Sets the current character to first character.
    // ** NEED TO CHANGE THIS TO CHECK FOR ALIVE CHARACTERS ONCE STAT SHEET IS DONE.
    currentPCIndex = 0;
		currentPC = pcList[0];

    //Sets the first PC to active state.
    moveScript = currentPC.GetComponent<MoveToClickPoint>();
    moveScript.SetCharacterActive();

    //Function in player camera to snap to first index character.
    playerCamera.StartNewTurn();
  }

  //Test function for enemy turn.
  public void StartEnemyTurn()
  {
    currentEnemyIndex = 0;
    currentEnemyDone = false;
    StartCoroutine(MoveCurrentEnemy());
  }

  // Function to move current enemy character.
  IEnumerator MoveCurrentEnemy()
  {
    // Wait period before the start of next unit turn. Adjust as needed.
    yield return new WaitForSecondsRealtime(1);
    if (currentEnemyIndex >= enemyList.Length)
    {
      StartPlayerTurn();
      Debug.Log("Player Turn Starting");
      yield break;
    }
    currentEnemy = enemyList[currentEnemyIndex];
    enemyMoveScript = currentEnemy.GetComponent<EnemyMoveScript>();
    if (enemyMoveScript.isDead == false)
    {
      enemyMoveScript.FindNearestPC();
    }
    currentEnemyIndex++;
  }
}
