using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Su dung singleton de dua item vao ruong

    public static InventoryManager Instance { get; private set; }
    public List<Item> items = new List<Item>();
    public Transform itemHolder;
    public GameObject itemHolderPrefab;
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
    public void DisplayInventory()
    {
        foreach(Item item in items)
        {
            GameObject obj = Instantiate(itemHolderPrefab, itemHolder);
            var itemName = obj.transform.Find("name").GetComponent<TextMeshProUGUI>();
            var itemImage = obj.transform.Find("anh").GetComponent<Image>();
            
            itemName.text = item.name;
            itemImage.sprite = item.image;
        }
    }
}
