using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabEffectHover : GrabEffect
{
  private List<Material> materials;

  [SerializeField]
  private Color colorLeft = new Color(0.25f, 0.25f, 0.75f);  //black will have no effect, white will be white.

  [SerializeField]
  private Color colorRight = new Color(0.75f, 0.25f, 0.25f);

  //Gets all the materials from each renderer
  private void Awake()
  {
    materials = new List<Material>();
    Renderer[] rendererlist = GetComponents<Renderer>();
    foreach (Renderer renderer in rendererlist)
    {
      materials.AddRange(new List<Material>(renderer.materials));
    }
  }

  public override bool OnGrab(Grab controller)
  {
    OnRemove(controller);
    return false;
  }

  public override void OnHover(Grab controller)
  {
    //sanity check that the hand can grab something still
    if (controller.InHand != null)
    return;

    if (controller.GetNearestGrabbable() == this.gameObject)
    {
      Color color = colorRight;
      if (controller.WhichHand == Grab.Hand.LEFT)
      {
        color = colorLeft;
      }

      foreach (Material material in materials)
      {
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);
      }
    }
    else
    {
      OnRemove(controller); 
    }
  }

  public override void OnRemove(Grab controller)
  {
    //sanity check
    foreach (Material material in materials)
    {
      material.DisableKeyword("_EMISSION");
    }
  }


}
