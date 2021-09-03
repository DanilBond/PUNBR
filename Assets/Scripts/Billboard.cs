using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cam;
    void Start()
    {

    }

    
    void Update()
    {
        gameObject.transform.LookAt(cam.transform.position);
    }
}
