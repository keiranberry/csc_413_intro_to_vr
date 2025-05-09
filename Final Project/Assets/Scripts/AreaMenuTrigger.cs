using UnityEngine;
using UnityEngine.InputSystem;

public class AreaMenuTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject shopMenu;

    public PlayerInput playerInput;

    /// <summary>
    /// Checks for player entering the trigger area, 
    /// when the player enters the area the menu
    /// is set to active and allows for user interaction.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            shopMenu.SetActive(true);
            playerInput.actions.FindActionMap("UI").Enable();
        }
    }

    /// <summary>
    /// Checks for the player exiting the trigger area,
    /// the menu is then removed.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            shopMenu.SetActive(false);
            playerInput.actions.FindActionMap("UI").Disable();
        }
    }
}
