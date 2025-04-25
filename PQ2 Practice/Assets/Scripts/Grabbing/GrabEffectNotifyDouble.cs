using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabEffectNotifyDouble : GrabEffect
{
  public DoubleGrab DoubleGrab { get; set; }

  //overrite default priority to run after non-hand editing,
  //but before single grab
  private GrabEffectNotifyDouble()
  {
    Priority = 1;
  }

  public override bool OnGrab(Grab controller)
  {
    return DoubleGrab.OnGrab(gameObject, controller);

  }


  public override bool OnRelease(Grab controller)
  {
    return DoubleGrab.OnRelease(gameObject, controller);

  }

}
