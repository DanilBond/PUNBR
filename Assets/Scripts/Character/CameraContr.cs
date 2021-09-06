using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContr : MonoBehaviour
{
    public GameObject camPivot;
    public float speed;

    void Start()
    {
        
    }

   
    void FixedUpdate()
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, camPivot.transform.position, speed);
        gameObject.transform.rotation = camPivot.transform.rotation;
    }
}
