using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour 
{
	public GameObject[] pcList;
	public List<GameObject> portraits = new List<GameObject>();
	private RawImage img;
	private CharacterTemplate target;

	private TurnManagerScript turnManager;
     
	void Start () 
	{

		turnManager = GameObject.Find("TurnManager").GetComponent<TurnManagerScript>();

		foreach(GameObject portrait in GameObject.FindGameObjectsWithTag("Portrait")) 
		{
				portraits.Add(portrait);
		}			
	}	

	void Update () 
	{
		UpdatePlayerHealth();
		if (pcList.Length == 0)
		{
			pcList = turnManager.GetPCList();
			Debug.Log("Grab List");

			if (pcList.Length > 0)
			{
				for(int i = 3; i >= 0; i--)
				{
					img = (RawImage)portraits[i].GetComponent<RawImage>();
					target = pcList[i].GetComponent<CharacterTemplate>();
					img.texture = (Texture)target.portrait;
				}					
			}	
			Debug.Log(pcList.Length);
		}
	}


	void UpdatePCList()
	{
		pcList = GameObject.FindGameObjectsWithTag("PlayableCharacter");		
	}

	void UpdatePlayerHealth()
	{

	}

}
