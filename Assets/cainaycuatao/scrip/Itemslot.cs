using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Itemslot : MonoBehaviour
{
    //item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isfull;


    //item slot
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;



    public void Additem(string itemname, int quantity, Sprite itemsprite)
    {
        this.itemName = itemname;
        this.quantity = quantity;
        this.itemSprite = itemsprite;
        isfull = true;
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemsprite;
    }
}
