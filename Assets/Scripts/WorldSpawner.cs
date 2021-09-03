using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject[] TilesLevel1;
    public GameObject[] TilesLevel2;
    public GameObject[] TilesLevel3;

    public GameObject Bot_Prefab;
    public GameObject[] temp;
    //public List<GameObject> spawnPoses;
    public int botsToSpawn;
    void Start()
    {
        temp = GameObject.FindGameObjectsWithTag("SpawnPos");
        //foreach (var item in temp)
        //{
        //    spawnPoses.Add(item);
        //}
        Spawn();
        //SpawnBots();
    }

   
    void Update()
    {
        
    }

    public void Spawn()
    {
        GameObject tile1 = Instantiate(TilesLevel1[Random.Range(0, TilesLevel1.Length)], SpawnPoints[0].transform.position, Quaternion.identity);
        GameObject tile2 = Instantiate(TilesLevel2[Random.Range(0, TilesLevel2.Length)], SpawnPoints[1].transform.position, Quaternion.identity);
        
        
    }
    void SpawnBots()
    {
        for (int i = 0; i < temp.Length; i++)
        {
            GameObject Bot = Instantiate(Bot_Prefab, temp[i].transform.position, Quaternion.identity);
           
        }

    }
}
