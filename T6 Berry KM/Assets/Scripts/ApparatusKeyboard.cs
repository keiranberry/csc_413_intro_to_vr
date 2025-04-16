using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows movement of the apparatus based on keyboard behaviors for testing
/// </summary>
public class ApparatusKeyboard : MonoBehaviour
{
  #region Editor Variables------------------------

  [SerializeField]
  private float maxRaise = 1.3f;

  [SerializeField]
  private float maxWidth = 0.1f;

  [SerializeField]
  private float minRaise = 1;

  [SerializeField]
  private float minWidth = 0.01f;

  [SerializeField]
  private float raiseSpeed = 0.01f;

  [SerializeField]
  private float widthChangeSpeed = 0.01f;

  [SerializeField]
  private InputActionProperty colorChangeAction;

  [SerializeField]
  private InputActionProperty moveAction;

  [SerializeField]
  private InputActionProperty scaleAction;
  #endregion---------------------------------

  private float curWidth = 0.05f;
  private ApparatusLaser laser;

  private void Start()
  {
    laser = GetComponentInChildren<ApparatusLaser>();

    //color change check
    if (colorChangeAction != null)
    {
      colorChangeAction.action.performed += OnColorChange;
    }
    else
    {
      Debug.LogWarning("Camera move action not found on "
               + name + ". Camera movement will not work. Is there an action missing?");
    }

    //move check
    if (moveAction != null)
    {
      moveAction.action.performed += OnMove;
    }
    else
    {
      Debug.LogWarning("Laser move action not found on "
               + name + ". Laser movement will not work. Is there an action missing?");
    }

    //scale check
    if (scaleAction != null)
    {
      scaleAction.action.performed += OnScale;
    }
    else
    {
      Debug.LogWarning("Laser scale action not found on "
               + name + ". Laser scaling will not work. Is there an action missing?");
    }
  }

  /// <summary>
  /// Change the color of the laser
  /// </summary>
  /// <param name="value">The key pressed</param>
  void OnColorChange(InputAction.CallbackContext value)
  {
    //this is a hack. I scaled the input, so that the keys give me different values.
    float v = value.ReadValue<float>();
    int which = (int)v;

    switch (which)
    {
      case 1:
        laser.SetColor(Color.blue);
        break;
      case 2:
        laser.SetColor(Color.red);
        break;
      case 3:
        laser.SetColor(Color.green);
        break;
      case 4:
        laser.SetColor(Color.white);
        break;
    }

  }

  void OnMove(InputAction.CallbackContext value)
  {
    Vector2 amount = value.ReadValue<Vector2>();

    float y = transform.position.y + amount.y * raiseSpeed;
    if (y < minRaise)
      y = minRaise;
    if (y > maxRaise)
      y = maxRaise;
    transform.position = new Vector3(transform.position.x, y, transform.position.z);
    transform.Rotate(transform.up, amount.x);

  }

  void OnScale(InputAction.CallbackContext value)
  {

    float amount = value.ReadValue<float>();
    curWidth = curWidth + amount * widthChangeSpeed;

    if (curWidth < minWidth)
      curWidth = minWidth;
    if (curWidth > maxWidth)
      curWidth = maxWidth;
    laser.SetWidth(curWidth);
  }
}
