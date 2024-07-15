using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "item", menuName = "chest/item")]

public class Item : ScriptableObject
{
    public int id;
    public string name;
    public int value;
    public Sprite image;
}
