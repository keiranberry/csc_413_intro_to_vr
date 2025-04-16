using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
  [SerializeField]
  private InputActionProperty action;

    [SerializeField]
    private InputActionProperty action2;

  private Color[] colors = new Color[] {
     Color.white,
     Color.red, 
     Color.blue, 
     Color.green,
     Color.black };

  private int i = 0;

  private void Start()
  {
    action.action.performed += OnColorChange;
    action2.action.performed += On2DColorChange;
  }

  public void OnColorChange(InputAction.CallbackContext context)
  {
    int dir = Mathf.RoundToInt(context.ReadValue<float>());
    i = (i + dir + colors.Length) % colors.Length;
    Renderer r = this.GetComponent<Renderer>();
    Material m = r.material;
    m.color = colors[i];
   
  }

    public void On2DColorChange(InputAction.CallbackContext context)
    {
        Vector2 raw = context.ReadValue<Vector2>();
        int dir = Mathf.RoundToInt(raw.x);

        i = (i + dir + colors.Length) % colors.Length;

        Renderer r = this.GetComponent<Renderer>();
        Material m = r.material;
        m.color = colors[i];
    }
}
