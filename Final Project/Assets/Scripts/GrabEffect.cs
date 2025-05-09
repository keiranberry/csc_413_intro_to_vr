using UnityEngine;

public abstract class GrabEffect : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Grab g = other.GetComponent<Grab>();
        if (g != null)
        {
            g.RegisterGrab(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Grab g = other.GetComponent<Grab>();
        if (g != null)
        {
            g.UnregisterGrab(this);
        }
    }

    public abstract void OnGrab(Grab controller);
    public abstract void OnRelease(Grab controller);
}
