using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [Header("Spawner General Settings")]
    //The Radius of the circle that the models are spawns around
    public int Radius;

    [Header("Player Squad Spawns")]
    //Models to be spawned
    public GameObject[] ToSpawnPlayer;
    //Locations for a group of models to be spawned at, use an empty gameobject for the location
    public GameObject SpawnLocationPlayer;

    [Header("Enemy Squad Spawns")]
    //Rough number of enemies to spawn in a pack +/- the AverageEnemyRange
    public float AverageEnemies;
    //Percentage of fluctuation of the spawn average in a pack
    public float AverageEnemyRange;
    //Models to be spawned
    public GameObject[] ToSpawnEnemy;
    //Locations for a group of models to be spawned at, use an empty gameobject for the location
    public GameObject[] SpawnLocationEnemy;

    [Header("Boss Spawn")]
    //Models to be spawned
    public GameObject[] ToSpawnBoss;
    //Locations for a group of models to be spawned at, use an empty gameobject for the location
    public GameObject[] SpawnLocationBoss;

    private GameObject _spawning;
    private float _offset;

    //Spawn the prefab players in the prefab locations. Loads 1 of each from the Array of Players.
    private void SpawnPlayers()
    {
        //At the SpawnLocation, spawns 1 of each from the array of players in a circle equaldistance apart.
        Vector3 location = SpawnLocationPlayer.transform.position;
        for (int i = 0; i < ToSpawnPlayer.Length; i++)
        {
            _offset = Mathf.PI * 2.0f / ToSpawnPlayer.Length + Random.Range(-0.02f, 0.02f);
            _spawning = Instantiate(ToSpawnPlayer[i], new Vector3(location.x + Radius * Mathf.Cos(_offset * i), location.y, location.z + Radius * Mathf.Sin(_offset * i)), Quaternion.Euler(0, 0, 0)) as GameObject;
        }
    }

    //Spawn the prefab enemies in the prefab locations. Loads Randomly from the Array of Enemies.
    private void SpawnEnemies()
    {
        foreach (GameObject spawnerObject in SpawnLocationEnemy)
        {
            //foreach element in enemy location array, take that location and get an amount of models (determined by avg enemies and range) and spawn them around the center of a circle, offset by the amount of models being loaded
            Vector3 location = spawnerObject.transform.position;
            int _spawnCount = (int)Mathf.Round(AverageEnemies + Random.Range(-AverageEnemyRange, AverageEnemyRange));
            for (int i = 0; i < _spawnCount; i++)
			{
                int _randomModel = (int)Mathf.Floor(Random.Range(0,ToSpawnEnemy.Length));
                _offset = Mathf.PI * 2.0f / _spawnCount + Random.Range(-0.02f, 0.02f);
                _spawning = Instantiate(ToSpawnEnemy[_randomModel], new Vector3(location.x + Radius*Mathf.Cos(_offset * i), location.y, location.z + Radius*Mathf.Sin(_offset * i)), Quaternion.Euler(0, 0, 0)) as GameObject;
			}          
        }
    }

    //Spawn the prefab bosses in the prefab locations
    private void SpawnBoss()
    {
        foreach (GameObject spawnerObject in SpawnLocationBoss)
        {
            Vector3 location = spawnerObject.transform.position;
            for (int i = 0; i < ToSpawnBoss.Length; i++)
            {
                _offset = Mathf.PI * 2.0f / ToSpawnEnemy.Length + Random.Range(-0.02f, 0.02f);
                _spawning = Instantiate(ToSpawnBoss[i], new Vector3(location.x + Radius * Mathf.Cos(_offset * i), location.y, location.z + Radius * Mathf.Sin(_offset * i)), Quaternion.Euler(0, 0, 0)) as GameObject;
            }
        }
    }


    // Use this for initialization
    void Awake () {
        SpawnEnemies();
        SpawnPlayers();

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space"))
		{
            SpawnEnemies();
			print("space key was pressed");
		}
	}
}
