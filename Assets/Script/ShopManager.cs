using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel; // Reference to the shop panel UI
    public GameObject player; // Reference to the player GameObject
    public string[] weaponNames; // Names of the weapons for display purposes
    public Sprite[] weaponSprites; // Sprites for the weapons

    private PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        if (shopPanel != null)
        {
            for (int i = 0; i < weaponNames.Length; i++)
            {
                int index = i; // Capture the loop variable
                string buttonName = "WeaponButton" + (i + 1);
                AssignButtonAction(shopPanel, buttonName, () => BuyWeapon(index));
            }

            AssignButtonAction(shopPanel, "CloseButton", CloseShop); // Ensure this line is included to assign the CloseButton action
        }
        else
        {
            Debug.LogError("Shop Panel is not assigned in the inspector.");
        }
    }

    private void AssignButtonAction(GameObject panel, string buttonName, UnityEngine.Events.UnityAction action)
    {
        Transform buttonTransform = panel.transform.Find(buttonName);
        if (buttonTransform != null)
        {
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
            else
            {
                Debug.LogError($"Button component not found on {buttonName}");
            }
        }
        else
        {
            Debug.LogError($"{buttonName} not found in {panel.name}");
        }
    }

    private void BuyWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponSprites.Length)
        {
            playerController.ChangeWeapon(weaponIndex);
            Debug.Log($"Weapon bought: {weaponNames[weaponIndex]}");
        }
        else
        {
            Debug.LogError($"Weapon index {weaponIndex} is out of bounds.");
        }
    }

    private void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
