using UnityEngine;

public class GrabEffectToggleColor : GrabEffect
{
    private Renderer objRenderer;

    private Color colorA = Color.red;
    private Color colorB = Color.blue;

    private bool isColorA = true;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogWarning("No Renderer found on " + gameObject.name);
        }
    }

    public override bool OnGrab(Grab controller)
    {
        Debug.Log("GrabEffectToggleColor.OnGrab called on " + gameObject.name);
        if (objRenderer != null)
        {
            ToggleColor();
        }

        return false; // Return false so this effect doesn't take control of the grab
    }

    private void ToggleColor()
    {
        isColorA = !isColorA;
        objRenderer.material.color = isColorA ? colorA : colorB;
    }
}
