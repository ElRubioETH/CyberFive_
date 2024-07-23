using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventorymenu;
    private bool menuActived;
    public Itemslot[] itemslot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory")&&menuActived)
        {
            Time.timeScale = 1;
            inventorymenu.SetActive(false);
            menuActived = false;
        }
        else if (Input.GetButtonDown("Inventory")&&!menuActived)
        {
            Time .timeScale = 0;
            inventorymenu.SetActive(true);
            menuActived = true;
        }
    }
    public void Additem(string itemname, int quantity, Sprite itemsprite) 
    {
        for (int i = 0; i < itemslot.Length; i++)
        {
            if (itemslot[i].isfull == false)
            {
                itemslot[i].Additem(itemname, quantity, itemsprite);
                return;
            }
        }
    }
}
