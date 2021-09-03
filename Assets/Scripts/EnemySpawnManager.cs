using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public int CurrentWave;
    public int[] Waves;
    public float delayMin;
    public float delayMax;
    public float WaveDelay;
    public float distance;
    public Vector3 Rotate;

    public Transform Parent;
    public Transform Child;

    public GameObject EnemyPrefab;

    public GameObject NextWaveObj;

    void Start()
    {
        StartCoroutine(Spawn());
    }

   
    void Update()
    {
        
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            float delay = Random.Range(delayMin, delayMax);
            yield return new WaitForSeconds(delay);
          
                Rotate = new Vector3(0, Random.Range(0, 360), 0);
                Parent.transform.rotation = Quaternion.Euler(Rotate);
                GameObject Enemy = Instantiate(EnemyPrefab, Child.position, Quaternion.identity);
                Debug.Log("Enemy Spawned");
                if(Waves[CurrentWave] <= 0)
                {
                    CurrentWave++;
                Debug.Log("Next wave");
                NextWaveObj.SetActive(true);
                yield return new WaitForSeconds(3f);
                NextWaveObj.SetActive(false);
                yield return new WaitForSeconds(WaveDelay);
                }
                Waves[CurrentWave] -= 1;
            
            
        }
    }
}
