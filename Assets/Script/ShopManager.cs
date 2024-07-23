using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject player;
    public string[] weaponNames;
    public Sprite[] weaponSprites;
    public int weaponCost = 1000; // Cost of each weapon

    private PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        if (shopPanel != null)
        {
            for (int i = 0; i < weaponNames.Length; i++)
            {
                int index = i;
                string buttonName = "WeaponButton" + (i + 1);
                AssignButtonAction(shopPanel, buttonName, () => BuyWeapon(index));
            }

            AssignButtonAction(shopPanel, "CloseButton", CloseShop);
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
                Debug.Log($"Assigned action to {buttonName}");
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

    public void BuyWeapon(int weaponIndex)
    {
        Debug.Log($"BuyWeapon called with index {weaponIndex}");
        if (weaponIndex >= 0 && weaponIndex < weaponSprites.Length)
        {
            Debug.Log($"Player gold: {playerController.gold}, Weapon cost: {weaponCost}");
            if (playerController.gold >= weaponCost)
            {
                playerController.gold -= weaponCost;
                playerController.ChangeWeapon(weaponIndex);
                playerController.ShowWeaponButton(weaponIndex); // Display the purchased weapon button in inventory

                // Hide the purchased weapon button in the shop panel
                string buttonName = "WeaponButton" + (weaponIndex + 1);
                Transform buttonTransform = shopPanel.transform.Find(buttonName);
                if (buttonTransform != null)
                {
                    buttonTransform.gameObject.SetActive(false);
                }

                // Update gold text in the PlayerController
                playerController.UpdateGoldText();

                Debug.Log($"Weapon bought: {weaponNames[weaponIndex]}");
            }
            else
            {
                Debug.Log("Not enough gold to buy this weapon.");
            }
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
