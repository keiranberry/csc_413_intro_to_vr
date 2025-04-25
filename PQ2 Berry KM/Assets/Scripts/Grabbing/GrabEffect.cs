using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the grab scripts that grabbing should notify
/// </summary>
public abstract class GrabEffect : MonoBehaviour
{
  [field: SerializeField]
  public int Priority { get; set; } = 0;

  public void OnTriggerEnter(Collider other)
  {
    Grab g = other.GetComponent<Grab>();
    if (g != null)
    {
      //found an object that triggers grab, register
      g.RegisterGrab(this);

    }
  }

  public void OnTriggerExit(Collider other)
  {
    Grab g = other.GetComponent<Grab>();
    if (g != null)
    {
      //left area that allows grab, deregister
      g.UnregisterGrab(this);

    }
  }

  public virtual bool OnGrab(Grab controller) { return false; }

  public virtual bool OnRelease(Grab controller) { return false; }

  public virtual void OnHover(Grab controller) { }
  public virtual void OnRemove(Grab controller) { }
  public virtual void OnAdd(Grab controller) { }

  public virtual void Reset(Grab controller) { }

}

