using UnityEngine;


/// <summary>
/// Class for the grab scripts that grabbing should notify
/// </summary>
public abstract class GrabEffect : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Grab g = other.GetComponent<Grab>();
        if (g != null )
        {
            //found an object that triggers grab, register
            Debug.Log(g.name + " is in range");
            g.RegisterGrab(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Grab g = other.GetComponent<Grab>();
        if (g != null)
        {
            // left area that allows grab, deregister
            Debug.Log(g.name + " is NOT in range");
            g.UnregisterGrab(this);
        }
    }

    public abstract void OnGrab(Grab controller);
    public abstract void OnRelease(Grab controller);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
