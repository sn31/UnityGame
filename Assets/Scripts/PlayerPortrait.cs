using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour 
{
	private GameObject[] pcList;

	public List<GameObject> portraits = new List<GameObject>();

	private RawImage img;
	private CharacterTemplate target;
     
	void Start () 
	{

		foreach(GameObject portrait in GameObject.FindGameObjectsWithTag("Portrait")) 
		{

				portraits.Add(portrait);
		}			

		if (pcList == null)
		{
			UpdatePCList();
		}
		if (pcList.Length > 0)
		{
			for(int i = 3; i >= 0; i--)
			{
				Debug.Log("pc" + pcList.Length + " textures " + portraits.Count);
				img = (RawImage)portraits[i].GetComponent<RawImage>();
				target = pcList[i].GetComponent<CharacterTemplate>();
				img.texture = (Texture)target.portrait;
			}					
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

	void UpdatePlayerHealth()
	{

	}

}
