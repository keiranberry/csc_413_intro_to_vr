using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    public Transform rightController;
    public Transform leftController;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    [SerializeField]
    public GameObject leftHand;
    private Vector3 previousLeftHandPosition;
    public Grab controller;

    public RectTransform menu;

    /// <summary>
    /// Listener function that populates the inventory menu when
    /// it gets enabled. It also sets an auto selected item to
    /// increase ux for desktop and gamepad users.
    /// </summary>
    private void OnEnable()
    {
        PopulateMenu();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonParent.GetChild(buttonParent.childCount / 2).gameObject);
    }

    /// <summary>
    /// This function dynamically creates the inventory menu ui
    /// based on the items in the player's inventory. It creates
    /// a button for each item so the user can select what they
    /// currently want to use.
    /// </summary>
    private void PopulateMenu()
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        GameObject button = Instantiate(buttonPrefab, buttonParent);
        button.GetComponentInChildren<TextMeshProUGUI>().text = "None";

        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            UnequipItem();
        });

        var items = Inventory.Instance.GetAllItems();
        foreach (GameObject item in items)
        {
            button = Instantiate(buttonPrefab, buttonParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = Format(item.gameObject.name);

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                EquipItem(item);
            });
        }

        AdjustMenuHeight();
    }

    /// <summary>
    /// This function allows the user to equip an item. It
    /// is called when one of the inventory menu buttons is
    /// pressed. The item that the user selects is then 
    /// placed into the users hand.
    /// </summary>
    /// <param name="item"></param>
    private void EquipItem(GameObject item)
    {
        //Empty the hand before equip
        UnequipItem();

        if (item != null)
        {
            item.transform.parent = rightController;
            item.transform.localPosition = new Vector3(0, 0, 0.1f);
            if (item.gameObject.name.Contains("Pistol"))
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive)
                {
                    item.transform.localRotation = Quaternion.Euler(75, 0, 0);
                    item.transform.localPosition = new Vector3(-0.01f, -0.08f, 0.05f);
                }
                else
                {
                    item.transform.localRotation = Quaternion.Euler(25, 25, 15);
                    item.transform.localPosition = new Vector3(0, 0, 0.05f);
                }
            }
            else if (item.gameObject.name.Contains("Rifle"))
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive)
                {
                    item.transform.localRotation = Quaternion.Euler(0, 90, 75);
                    item.transform.localPosition = new Vector3(0, -0.15f, 0.1f);
                }
                else
                {
                    item.transform.localRotation = Quaternion.Euler(-10, 110, 30);
                    item.transform.localPosition = new Vector3(0.05f, -0.02f, 0.13f);
                }
            }
            else if (item.gameObject.name.Contains("Sniper"))
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive)
                {
                    item.transform.localRotation = Quaternion.Euler(0, 90, 75);
                    item.transform.localPosition = new Vector3(0, -0.3f, 0.2f);
                }
                else
                {
                    item.transform.localRotation = Quaternion.Euler(-13, 114, 25);
                }
            }
            else if (item.gameObject.name.Contains("SMG"))
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive)
                {
                    item.transform.localRotation = Quaternion.Euler(0, 90, 70);
                    item.transform.localPosition = new Vector3(0, -0.04f, 0.05f);
                }
                else
                {
                    item.transform.localRotation = Quaternion.Euler(-12, 110, 25);
                    item.transform.localPosition = new Vector3(0, 0, 0.02f);
                }
            }
            //Make object grow from nothing when equiping
            Animations animationScript = item.GetComponent<Animations>();
            if (animationScript != null)
            {
                item.transform.localScale = new Vector3(0, 0, 0);
                animationScript.StartGrowing();
            }

            item.SetActive(true);
            controller.InHand = item;

            Transform leftGrip = item.GetComponentsInChildren<Transform>(true)
                         .FirstOrDefault(t => t.name == "LeftHandGrip");
            if (leftGrip != null)
            {
                if (UnityEngine.XR.XRSettings.isDeviceActive)
                {
                    //set constraints to "tie" left hand to two-handed guns for two handed aiming and interactions
                    var constraint = leftController.gameObject.GetComponent<PositionConstraint>();
                    if (constraint == null)
                        constraint = leftController.gameObject.AddComponent<PositionConstraint>();

                    constraint.constraintActive = false;
                    constraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
                    constraint.locked = true;
                    ConstraintSource source = new ConstraintSource();
                    source.sourceTransform = leftGrip;
                    source.weight = 1;
                    constraint.SetSources(new List<ConstraintSource> { source });
                    constraint.constraintActive = true;
                    //leftGrip.position = leftHand.transform.position;

                }
                else 
                {
                    //if not in vr mode, move the left hand to the left grip of the gun
                    //shows the user they are using two handed interactions even if not in vr mode
                    previousLeftHandPosition = leftHand.transform.localPosition;
                    leftHand.transform.position = leftGrip.position;
                }
            }
        }
    }

    /// <summary>
    /// This function allows the user to unequip an item. It
    /// is called when the none button is selected on the 
    /// menu or everytime a new item is selected. The item 
    /// that was previously in the user's hand is then removed.
    /// </summary>
    private void UnequipItem()
    {
        if (controller.InHand)
        {
            GameObject heldItem = controller.InHand;
            controller.InHand = null;

            // Shrink object to nothing when unequipped
            Animations animationScript = heldItem.GetComponent<Animations>();
            if (animationScript != null)
            {
                animationScript.StartShrinking();
            }

            //heldItem.transform.parent = null;
            //heldItem.SetActive(false);
            if (previousLeftHandPosition != null)
            {
                //if we moved the left hand (non vr modes), put it back where it was
                leftHand.transform.localPosition = previousLeftHandPosition;
            }

            //destroy constraint on left controller if it exists
            var constraint = leftController.GetComponent<PositionConstraint>();
            if (constraint != null)
            {
                Destroy(constraint);
            }
        }
    }

    /// <summary>
    /// This is a helper function that will reformat the
    /// rawName to something more user friendly.
    /// </summary>
    /// <param name="rawName"></param>
    /// <param name="maxLength"></param>
    public string Format(string rawName, int maxLength = 16)
    {
        if (string.IsNullOrEmpty(rawName)) return "Unnamed";

        // Remove common unwanted parts
        string cleaned = rawName.Replace("(Clone)", "")
                                .Replace("_", " ")
                                .Trim();

        // Capitalize each word
        string[] words = cleaned.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length == 0) continue;
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }

        string result = string.Join(" ", words);

        // Truncate if too long
        if (result.Length > maxLength)
            result = result.Substring(0, maxLength).TrimEnd() + "...";

        return result;
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
