using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenadge : MonoBehaviour
{
    public float delay;
    public float damage;
    public GameObject effect;
    void Start()
    {
        Invoke("Exp", delay);
    }

    
    public void Exp()
    {
        GameObject eff = Instantiate(effect, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
