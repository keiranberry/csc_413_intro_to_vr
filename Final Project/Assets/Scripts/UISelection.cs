using UnityEngine;
using UnityEngine.InputSystem;

public class UISelection : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty mainInventoryToggle;

    [SerializeField]
    private InputActionProperty uiInventoryToggle;

    [SerializeField] 
    private InputActionProperty cancel;

    [SerializeField]
    private GameObject inventoryMenu;
    [SerializeField]
    private GameObject shopMenu;

    void Start()
    {
        mainInventoryToggle.action.performed += ToggleInventory;
        uiInventoryToggle.action.performed += ToggleInventory;
        cancel.action.performed += Cancel;
    }

    private void ToggleInventory(InputAction.CallbackContext ctx)
    {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf);

        var playerInput = GetComponent<PlayerInput>();
        if (inventoryMenu.activeSelf)
            playerInput.SwitchCurrentActionMap("UI");
        else if(!inventoryMenu.activeSelf)
            playerInput.SwitchCurrentActionMap("Main");
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        inventoryMenu.SetActive(false);
        shopMenu.SetActive(false);

        var playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Main");
    }
}
