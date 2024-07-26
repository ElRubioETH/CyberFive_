using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    public GameObject[] alreadyBoughtPanels;
    public TMP_Text goldDecreaseText;
    public TMP_Text notEnoughMoneyText;
    // Panels to display when each weapon is already bought





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
                    buttonContainer = shopPanel.transform.Find("ButtonContainer")?.gameObject;


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
        if (buttonContainer != null)
        {
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
                playerController.UpdateGoldText();
                // Hide the purchased weapon button in the shop panel
                string buttonName = "WeaponButton" + (weaponIndex + 1);
                Transform buttonTransform = shopPanel.transform.Find("ButtonContainer")?.Find(buttonName);
                if (buttonTransform != null)
                {
                    buttonTransform.gameObject.SetActive(false);
                }

                if (weaponIndex >= 0 && weaponIndex < alreadyBoughtPanels.Length && alreadyBoughtPanels[weaponIndex] != null)
                {
                    alreadyBoughtPanels[weaponIndex].SetActive(true);
                }

                Debug.Log($"Weapon bought: {weaponNames[weaponIndex]}");
                StartCoroutine(ShowGoldDecreaseText(weaponCost));
            }
            else
            {
                Debug.Log("Not enough gold to buy this weapon.");
                StartCoroutine(ShowNotEnoughMoneyText());
            }
        }
        else
        {
            Debug.LogError($"Weapon index {weaponIndex} is out of bounds.");
        }
    }

    private IEnumerator ShowGoldDecreaseText(int amount)
    {
        goldDecreaseText.text = $"-{amount} Gold";
        goldDecreaseText.gameObject.SetActive(true);
        goldDecreaseText.GetComponent<Animator>().Play("GoldDecreaseAnimation");
        yield return new WaitForSeconds(1); // Display for 1 second
        goldDecreaseText.gameObject.SetActive(false);
    }

    private IEnumerator ShowNotEnoughMoneyText()
    {
        notEnoughMoneyText.text = "Not Enough Money";
        notEnoughMoneyText.gameObject.SetActive(true);
        notEnoughMoneyText.GetComponent<Animator>().Play("NotEnoughMoneyAnimation");
        yield return new WaitForSeconds(1); // Display for 1 second
        notEnoughMoneyText.gameObject.SetActive(false);
    }




    private void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}