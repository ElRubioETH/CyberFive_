using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{
    public enum InteractionType { NONE,PickUp,Examine}
    public enum ItemType { Static, Consumables }
    [Header("Attibutes")]
    public InteractionType interactType;
    public ItemType type;
    [Header("Examine")]
    public string discriptionText;
    [Header("Custom Event")]
    public UnityEvent consumeEvent;
    public Sprite image;
    //Collider Trigger 
    //Interaction Type
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }
    public void Interact()
    {
        switch (interactType)
        {
            case InteractionType.PickUp:
                //Add the object to the PickUpItem list
                FindObjectOfType<InventorySystem>().PickUp(gameObject);
                //Disable
                gameObject.SetActive(false);
                Debug.Log("Pick Up");
                break;
            case InteractionType.Examine:
                //Call the Examine item in the interaction system
                FindObjectOfType<InteractionSystem>().ExamineItem(this); 
                Debug.Log("Examine");
                break;
            default:
                Debug.Log("Null item");
                    break;
        }
    }
}

