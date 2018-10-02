using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPortrait : MonoBehaviour 
{
	private GameObject[] pcList;


    private GameObject Portrait1;
	private GameObject Portrait2;
	private GameObject Portrait3;
	private GameObject Portrait4;	
	public Texture text1;
	public Texture text2;
	public Texture text3;
	public Texture text4; 	 
	public List<GameObject> portraits = new List<GameObject>();

    private RawImage img;
	private CharacterTemplate target;
     
    void Start () {

         foreach(GameObject portrait in GameObject.FindGameObjectsWithTag("Portrait")) {
 
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
				img.texture = (Texture)target.Portrait;
			}
			// img = (RawImage)portraits[0].GetComponent<RawImage>();
			// img.texture = (Texture)text1;
			// img = (RawImage)portraits[1].GetComponent<RawImage>();
			// img.texture = (Texture)text2;
			// img = (RawImage)portraits[2].GetComponent<RawImage>();
			// img.texture = (Texture)text3;
			// img = (RawImage)portraits[3].GetComponent<RawImage>();
			// img.texture = (Texture)text4;						
		}	

	}


 
 

	

	void Update () 
	{
		UpdatePlayerHealth();
	}


	void UpdatePCList()
	{
		pcList = GameObject.FindGameObjectsWithTag("PlayableCharacter");
		Portrait1 = pcList[0];
		Portrait2 = pcList[1];
		Portrait3 = pcList[2];
		Portrait4 = pcList[3];
		
	}

	void UpdatePlayerHealth()
	{

	}

}
