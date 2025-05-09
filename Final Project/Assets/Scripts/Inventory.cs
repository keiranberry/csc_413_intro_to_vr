using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the inventory of the player.
/// It uses the singleton pattern to keep a static
/// instance of itself accessible from anywhere.
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<GameObject> items = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Function to add an item to the inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(GameObject item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
        }
    }

    /// <summary>
    /// Function to get all items in inventory
    /// </summary>
    public List<GameObject> GetAllItems() => items;
}
