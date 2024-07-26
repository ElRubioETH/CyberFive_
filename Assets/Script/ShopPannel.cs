using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public GameObject closeButton; // Reference to the close button
    public Vector2 closeButtonPosition; // Position for the close button

    private RectTransform closeButtonRectTransform;

    void Start()
    {
        if (closeButton != null)
        {
            closeButtonRectTransform = closeButton.GetComponent<RectTransform>();
            if (closeButtonRectTransform != null)
            {
                // Set the close button position
                closeButtonRectTransform.anchoredPosition = closeButtonPosition;
                Debug.Log("Close button position set to: " + closeButtonPosition);
            }
            else
            {
                Debug.LogError("RectTransform component not found on the close button.");
            }
        }
        else
        {
            Debug.LogError("Close button is not assigned in the inspector.");
        }
    }

    public void SetCloseButtonPosition(Vector2 newPosition)
    {
        if (closeButtonRectTransform != null)
        {
            closeButtonRectTransform.anchoredPosition = newPosition;
            Debug.Log("Close button position updated to: " + newPosition);
        }
        else
        {
            Debug.LogError("RectTransform component not found on the close button.");
        }
    }
}
