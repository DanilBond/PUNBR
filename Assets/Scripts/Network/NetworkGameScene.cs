using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameScene : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform[] SpawnPoints;

    public GameObject[] items;
    void Start()
    {
        int rand = Random.Range(0, SpawnPoints.Length);
        PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoints[rand].position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-20, 20), 1.5f, Random.Range(-20, 20));
                PhotonNetwork.InstantiateRoomObject("Items/" + items[Random.Range(0, items.Length)].name, pos, Quaternion.identity);
            }
        }
    }

    
    void Update()
    {
        
    }
}
