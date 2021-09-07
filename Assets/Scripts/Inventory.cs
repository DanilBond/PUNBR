using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item currentItem;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            AddItem(other.GetComponent<PickableItem>().item);
            Destroy(other.gameObject);
        }
    }
    public void AddItem(Item i)
    {
        currentItem = i;
    }
}
