using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    //lam bien mat item
    public Item item;
    void pickup()
    {
        //des
        Destroy(this.gameObject);
        //add inventory
        InventoryManager.Instance.Add(item);
        /*InventoryManager.Instance.DisplayInventory();*/
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            pickup();
            
        }
    }
}
