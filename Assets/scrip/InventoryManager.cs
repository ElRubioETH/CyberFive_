using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Su dung singleton de dua item vao ruong

    public static InventoryManager Instance { get; private set; }
    public List<Item> items = new List<Item>(); 
    private void Awake()
    {
        if (Instance != null||Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }
    public void Add(Item item)
    {
        items.Add(item);
    }
}
