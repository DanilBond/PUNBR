using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameScene : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform[] SpawnPoints;

    void Start()
    {
        int rand = Random.Range(0, SpawnPoints.Length);
        PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoints[rand].position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
