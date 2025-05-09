using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Supports basic mouse rotation of the camera
/// </summary>
public class CameraMove : MonoBehaviour
{
  #region Editor Variables------------------------

  [SerializeField]
  private float hoizontalArc = 100;

  [SerializeField]
  private InputActionProperty moveAction;

  [SerializeField]
  private float speed = 0.2f;

  [SerializeField]
  private float verticalArc = 30;

  #endregion---------------------------------


  private float thetaX = 0;
  private float thetaY = 0;

    /// <summary>
    /// Callback for mouse movement the rotations the camera left/right and up/down within limits
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        float currentSpeed = speed;

        // Adjust sensitivity if input came from a gamepad
        if (context.control.device is Gamepad)
        {
            currentSpeed *= 5f; 
        }

        thetaY += delta.x * currentSpeed;
        thetaX -= delta.y * currentSpeed;

        thetaY = Mathf.Clamp(thetaY, -hoizontalArc, hoizontalArc);
        thetaX = Mathf.Clamp(thetaX, -verticalArc, verticalArc);

        transform.localRotation = Quaternion.Euler(thetaX, thetaY, 0);
    }


    private void Start()
  {
    if (moveAction != null)
    {
      moveAction.action.performed += OnMove;
    }
    else
    {
      Debug.LogWarning("Camera move action not found on "
               + name + ". Camera movement will not work. Is there an action missing?");
    }
  }
}
