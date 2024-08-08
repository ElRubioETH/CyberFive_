using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    //Detection Point
    public Transform detectionPoint;
    //Detection Radius
    private const float detectionRadius = 0.01f;
    //Detection Layer
    public LayerMask detectionLayer;
    //Cached Trigger Object
    public GameObject detectedObject;
    [Header("Examine Fields")]
    //Examine Window object
    public GameObject examineWindow;
    public Image examineImage;
    public TextMeshProUGUI examineText;
    public bool isExamning;


    // Update is called once per frame
    void Update()
    {
        if (DetectObject())
        {
            if(InteractInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }
    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    bool DetectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position,detectionRadius,detectionLayer);
        if(obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position,detectionRadius);
    }
    public void ExamineItem(Item item)
    {
        if(isExamning)
        {
            Debug.Log("Close");
            //Hide the Examine Window
            examineWindow.SetActive(false);
            //Disable the boolean
            isExamning = false;
        }
        else
        {
            Debug.Log("Examine");
            //Show the item's in the middle
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;
            //Write the discription text underneath the image
            examineText.text = item.discriptionText;
            //Display an Examine Window
            examineWindow.SetActive(true);
            //Enable the boolean
            isExamning = true;
        }
    }
}
