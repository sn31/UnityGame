using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurnManager : MonoBehaviour {

  public bool isCharacterTurn = false;
  public GameObject moveRadius;

	void Start () {
		
	}

  public void SetActive()
  {
    isCharacterTurn = true;

  }

  public void SetInactive()
  {
    isCharacterTurn = false;
  }


}
