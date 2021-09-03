using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroyer : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "MapComponent")
        {
            ReTransform();
        }
    }

    public void ReTransform()
    {
        gameObject.transform.position = randPos();
        gameObject.transform.rotation = randRot();
    }

    Vector3 randPos()
    {
        RandomObjectsSpawn ROS = FindObjectOfType<MapBuilder>().ROS;
        Vector3 pos = new Vector3(Random.Range(ROS.beginX, ROS.endX), ROS.offsetY, Random.Range(ROS.beginZ, ROS.endZ));
        return pos;
    }
    Quaternion randRot()
    {
        float Axis = Random.Range(0, 360);
        Quaternion rot = Quaternion.Euler(0, Axis, 0);
        return rot;
    }
}
