using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;

/// <summary>
/// Class that defines a shop item object.
/// Used to associate prices and names with 
/// specific prefabs.
/// </summary>
[System.Serializable]
public class ShopItem
{
    public string itemName;

    public int price;

    public GameObject prefab;

    [HideInInspector] 
    public bool hasBeenPurchased = false;
}

/// <summary>
/// Class that controls the behavior of 
/// the shop menu
/// </summary>
public class ShopMenu : MonoBehaviour
{
    public List<ShopItem> shopItems;

    public Transform buttonParent;
    public GameObject buttonPrefab;
    
    public Transform spawnPoint;

    public RectTransform menu;

    /// <summary>
    /// Listener function that populates the shop menu when
    /// it gets enabled. It also sets an auto selected item to
    /// increase ux for desktop and gamepad users.
    /// </summary>
    private void OnEnable()
    {
        PopulateShop();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonParent.GetChild(buttonParent.childCount / 2).gameObject);
    }

    /// <summary>
    /// This function dynamically creates the shop menu ui
    /// based on the items set in the inspector. It creates
    /// a button for each item so the user can select what they
    /// want to buy.
    /// </summary>
    private void PopulateShop()
    {
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        foreach (ShopItem item in shopItems)
        {
            GameObject button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{item.itemName}\n{item.price} Points";
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuyItem(item);
            });

            if (item.hasBeenPurchased)
            {
                button.GetComponent<Button>().interactable = false;
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Purchased";
            }
        }

        AdjustMenuHeight();
    }

    /// <summary>
    /// Function that allows the user to buy a new item.
    /// The user must have enough points in the bank to
    /// buy the item and once the item is bought, the 
    /// menu will be updated so the item can not be
    /// bought again
    /// </summary>
    public void BuyItem(ShopItem item)
    {
        if (GameManager.Instance.bank >= item.price)
        {
            GameManager.Instance.bank -= item.price;
            Debug.Log($"Bought {item.itemName} for {item.price} points!");

            // Instantiate the item in the world
            if (item.prefab != null)
            {
                Vector3 weaponSpawnPoint = spawnPoint.position + (spawnPoint.forward * 0.7f);
                GameObject newItem = Instantiate(item.prefab, weaponSpawnPoint, Quaternion.identity);
                Animations newItemAnimationScript = newItem.GetComponent<Animations>();
                if(newItemAnimationScript != null)
                {
                    newItem.transform.localScale = Vector3.zero;
                    newItemAnimationScript.OnPurchase();
                }
            }
            else
            {
                Debug.LogWarning($"No prefab assigned for {item.itemName}!");
            }

            item.hasBeenPurchased = true;
            PopulateShop();
        }
        else
        {
            Debug.Log("Not enough points to buy this item.");
        }
    }

    /// <summary>
    /// Helper function to dynamically size the menu
    /// </summary>
    public void AdjustMenuHeight()
    {
        int rowCount = Mathf.CeilToInt(buttonParent.childCount / (float)6);
        float rowHeight = 0.5f;
        float padding = 0.5f;
        SetInventoryHeight(rowCount * rowHeight + padding);
    }

    /// <summary>
    /// Helper function to dynamically size the menu
    /// </summary>
    public void SetInventoryHeight(float newHeight)
    {
        Vector2 size = menu.sizeDelta;
        size.y = newHeight;
        menu.sizeDelta = size;
    }
}

