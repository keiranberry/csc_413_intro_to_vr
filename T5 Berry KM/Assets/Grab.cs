using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grab : MonoBehaviour
{
    private PlayerInput mapper; 

    [SerializeField] 
    private string grabActionName = "Grab"; 
    
    [SerializeField] 
    private string releaseActionName = "Release";

    protected List<GrabEffect> grabObjects = new List<GrabEffect>();

    public GameObject InHand { set; get; } = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get actions
        PlayerInput[] temp = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);
        
        if (temp.Length == 0)    
        {      
            Debug.LogWarning("No player input component found. " +"Grab will not work until added to the scene.");
            
            //safety catch, if no player input there should be no actions
            return;    
        }    
        
        mapper = temp[0];
        
        //find actions from the input asset with the provided name
        InputAction grab = mapper.actions[grabActionName];
        InputAction release = mapper.actions[releaseActionName];

        //sanity check, if the action does not exist, do not save it
        if (grab != null)
            grab.started += OnGrab;
        else
            Debug.LogWarning(grabActionName + " action not found on "
                + name + ". Grab will not work. Is there a typo or is the action missing?");
        if (release != null)
            release.performed += OnRelease;
        else
            Debug.LogWarning(releaseActionName + " action not found on "
                + name + ". Release will not worl. Is there a typo or is the action missing?");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //grab action
    public void OnGrab(InputAction.CallbackContext context)
    {
        CleanUp();

        //sanity check, do not grab if hands are full
        if(InHand == null)
        {
            //check for all the objects that are to be notified, but default, this is just one
            GrabEffect[] list = GetGrabCallbackSet();
            foreach (GrabEffect go in list)
            {
                if (go != null && go.enabled)
                {
                    go.OnGrab(this);
                }
            }
        }

        CleanUp();
    }

    protected virtual GrabEffect[] GetGrabCallbackSet()
    {
        GameObject closest = null;
        foreach (GrabEffect obj in grabObjects)
        {
            //get ray from hand to object
            Vector3 lineTo = obj.transform.position - transform.position;
            Ray r = new Ray(transform.position, lineTo);
            RaycastHit hit;
            Physics.Raycast(r, out hit, lineTo.magnitude, ~2);

            //if no hit, nothing there, but also ignore collision with other objects
            if (hit.collider == null || hit.collider.gameObject != obj.gameObject)
            {
                //always use the first one, and then update if closer later
                if (closest == null || (this.transform.position - closest.transform.position).magnitude >
                    (this.transform.position - obj.transform.position).magnitude)
                {
                    closest = obj.gameObject;
                }
            }
        }

        if (closest == null)
            return new GrabEffect[0];
        else
            return closest.GetComponents<GrabEffect>();
    }

    // release action
    public void OnRelease(InputAction.CallbackContext context)
    {
        CleanUp();

        GrabEffect[] list = GetReleaseCallbackSet();
        foreach (GrabEffect go in list)
        {
            go.OnRelease(this);
        }

        CleanUp();
    }

    protected virtual GrabEffect[] GetReleaseCallbackSet()
    {
        if (InHand == null)
            return new GrabEffect[0];
        else
            return InHand.GetComponents<GrabEffect>();
    }

    /// <summary>
    /// Draw the Debugging Gizmos
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f); //translucent white
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }

    public void RegisterGrab(GrabEffect item)
    {
        //disallow duplicates
        if(!grabObjects.Contains(item))
            grabObjects.Add(item);
    }

    public void UnregisterGrab(GrabEffect item) 
    { 
        grabObjects.Remove(item);
    }

    protected virtual void CleanUp()
    {
        List<GrabEffect> toRemove = new List<GrabEffect>();

        //find any grabbable game object that is no longer active or has been destroyed
        foreach (GrabEffect obj in grabObjects)
        {
            if(obj == null || !obj.gameObject.activeSelf)
            {
                toRemove.Add(obj);
            }
        }

        foreach (GrabEffect obj in toRemove)
        {
            grabObjects.Remove(obj);

            if (obj == InHand)
                InHand = null;
        }
    }
}
