using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Enumeration of state that the game mode can be in
    public enum GameState
    {
        IntroSequence,
        LoadGame,
        PauseBeforeStart,
        Playing,
        Credits
    }

    // The current game state.
    public static GameState currentState;

    // Time the last state change occured.
    float lastStateChange = 0.0f;

    void Start()
    {
        SetCurrentState(GameState.IntroSequence);
    }

    // Set to inputted gamestate
    void SetCurrentState(GameState state)
    {
        Debug.Log("GameState:" + currentState + " changed to " + state + " state.");
        currentState = state;
        lastStateChange = Time.time;
    }

    //Get the elapsed time spend in the current state
    float GetStateElapsed()
    {
        return Time.time - lastStateChange;
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.IntroSequence:
                //todo: insert menu options and title screen etc 
                if (GetStateElapsed() > 2.5f)
                    SetCurrentState(GameState.Playing);
                break;
            case GameState.LoadGame:
                //todo: insert game loading scripts/functions here
                if (GetStateElapsed() > 2.5f)
                    SetCurrentState(GameState.Playing);
                break;
            case GameState.PauseBeforeStart:
                if (GetStateElapsed() > 2.5f)
                    SetCurrentState(GameState.Playing);
                break;
            case GameState.Playing:
                //todo: insert game loading scripts/functions here
                break;
            case GameState.Credits:
                //todo: insert credit scripts/functions here
                break;
        }
    }
}
