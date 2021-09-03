using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ObjectPooling : MonoBehaviour
{
    public GameObject[] objectsToPool;
    public int poolBuffer;

    public List<GameObject> Objects;
    
    void Start()
    {
        for (int i = 0; i < poolBuffer; i++)
        {
            
            for (int j = 0; j < objectsToPool.Length; j++)
            {
                GameObject Object = Instantiate(objectsToPool[j], Vector3.zero, Quaternion.identity);
                Object.name = objectsToPool[j].name;
                Object.transform.parent = gameObject.transform;
                Object.gameObject.SetActive(false);
                Objects.Add(Object);
                
            }
        }        
    }

    public GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        foreach (GameObject i in Objects)
        {
            if (i.gameObject.name == name)
            {
                i.SetActive(true);
                i.transform.position = position;
                i.transform.rotation = rotation;
                Objects.Remove(i);
                return i;
            }
        }
        return null;
    }

    public void SetObject(GameObject obj)
    {

        Objects.Add(obj);
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = gameObject.transform;
        if (obj.GetComponent<Rigidbody>())
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void SetObject(GameObject obj, float time)
    {
        StartCoroutine(waitPool(time,obj));
    }

    IEnumerator waitPool(float t, GameObject obj)
    {
        yield return new WaitForSeconds(t);

        Objects.Add(obj);
        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = gameObject.transform;
        if (obj.GetComponent<Rigidbody>())
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
