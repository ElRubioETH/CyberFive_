using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{
    public enum InteractionType { NONE,PickUp,Examine}
    public InteractionType type;
    [Header("Examine")]
    public string discriptionText;
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
        switch (type)
        {
            case InteractionType.PickUp:
                //Add the object to the PickUpItem list
                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);
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

