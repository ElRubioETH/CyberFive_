using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject player;
    public string[] weaponNames;
    public Sprite[] weaponSprites;
    public int weaponCost = 1000; // Cost of each weapon
    public GameObject weaponDetailPanel;
    public TMP_Text weaponDetailText;
    public int[] weaponDamages;
    public float[] weaponFireRates;
    public GameObject[] weaponDetailPanels;
    private GameObject buttonContainer;
    public TMP_Text goldChangeText; // Add this line at the top
    public Animator goldChangeAnimator; // Add this line at the top
    public float goldChangeDisplayTime = 1.0f; // Add this line at the top



    private PlayerController playerController;

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        if (shopPanel != null)
        {
            GameObject buttonContainer = shopPanel.transform.Find("ButtonContainer")?.gameObject;
            if (buttonContainer != null)
            {
                for (int i = 0; i < weaponNames.Length; i++)
                {
                    int index = i;
                    string buttonName = "WeaponButton" + (i + 1);
                    AssignButtonAction(buttonContainer, buttonName, () => BuyWeapon(index));

                    // Add these lines to handle hover events
                    Button button = buttonContainer.transform.Find(buttonName).GetComponent<Button>();
                    EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
                    EventTrigger.Entry entryEnter = new EventTrigger.Entry();
                    entryEnter.eventID = EventTriggerType.PointerEnter;
                    entryEnter.callback.AddListener((eventData) => ShowWeaponDetails(index));
                    trigger.triggers.Add(entryEnter);

                    EventTrigger.Entry entryExit = new EventTrigger.Entry();
                    entryExit.eventID = EventTriggerType.PointerExit;
                    entryExit.callback.AddListener((eventData) => HideWeaponDetails());
                    trigger.triggers.Add(entryExit);
                    buttonContainer = shopPanel.transform.Find("ButtonContainer")?.gameObject;


                }
            }
            else
            {
                Debug.LogError("ButtonContainer is not found in the Shop Panel.");
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

    private void ShowWeaponDetails(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponDetailPanels.Length)
        {
            for (int i = 0; i < weaponDetailPanels.Length; i++)
            {
                weaponDetailPanels[i].SetActive(i == weaponIndex);
            }

            TMP_Text detailText = weaponDetailPanels[weaponIndex].GetComponentInChildren<TMP_Text>();
            detailText.text = $"Name: {weaponNames[weaponIndex]}\nDamage: {weaponDamages[weaponIndex]}\nFire Rate: {weaponFireRates[weaponIndex]}";

            RectTransform buttonRect = weaponDetailPanels[weaponIndex].GetComponent<RectTransform>();
            buttonRect.position = buttonContainer.transform.Find("WeaponButton" + (weaponIndex + 1)).position;
        }
    }



    private void HideWeaponDetails()
    {
        foreach (GameObject panel in weaponDetailPanels)
        {
            panel.SetActive(false);
        }
    }
    private IEnumerator ShowGoldChange(int goldAmount)
    {
        goldChangeText.text = $"-{goldAmount} Bought";
        goldChangeText.gameObject.SetActive(true);
        goldChangeAnimator.Play("GoldChangeTextAnimation"); // Trigger the animation

        yield return new WaitForSeconds(goldChangeDisplayTime);

        goldChangeText.gameObject.SetActive(false);
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
                Transform buttonTransform = buttonContainer.transform.Find(buttonName); // Modify this line to use buttonContainer
                if (buttonTransform != null)
                {
                    buttonTransform.gameObject.SetActive(false);
                }

                // Update gold text in the PlayerController
                playerController.UpdateGoldText();

                // Show gold change message
                StartCoroutine(ShowGoldChange(weaponCost)); // Add this line

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