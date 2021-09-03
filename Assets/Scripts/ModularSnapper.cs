using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularSnapper : MonoBehaviour
{
    public GameObject Fence;
    public GameObject Pillar;

    public GameObject[] Pillars;
    void Start()
    {
        Pillars = GameObject.FindGameObjectsWithTag("Pillar");
        
        GameObject fence = Instantiate(Fence, gameObject.transform.position, Quaternion.identity);
        fence.transform.LookAt(GetClosestEnemy(Pillars));
    }

    
    void Update()
    {
        

    }

    Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }
}
