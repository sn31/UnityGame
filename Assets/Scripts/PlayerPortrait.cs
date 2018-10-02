using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour 
{
	private GameObject[] pcList;
	private int index;

	void Start () 
	{
		if (pcList == null)
		{
			UpdatePCList();
		}		
	}
	

	void Update () 
	{
		UpdatePlayerHealth();
	}


	void UpdatePCList()
	{
		pcList = GameObject.FindGameObjectsWithTag("PlayableCharacter");
	}

	void UpdatePortraitImg()
	{
		//   for(int i = 0; i < 4; i++)
		//   {
			
		//   }

		//  set img to prefab image based on index of photo

	}

	void UpdatePlayerHealth()
	{

	}

}
