using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static Grab;

public class GrabEffectCollect : GrabEffect
{

    public override bool OnGrab(Grab controller)
    {
        DestroyImmediate(gameObject);
        return false;
    }

    public override bool OnRelease(Grab controller)
    {
        // no release needed
        Debug.LogWarning("Collected object was not removed from current play");
        return false;
  }
}
