using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SpatialTracking;
using UnityEngine.XR;
using static Grab;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.ParticleSystem;


public class Grab : MonoBehaviour
{
    #region Editor Variables------------------------
    [SerializeField]
    private float maxDistance = 0.5f;

    [SerializeField]
    private InputActionProperty grabAction;

    [SerializeField]
    private InputActionProperty releaseAction;
    #endregion ------------------------

    public enum Hand { NEITHER, LEFT, RIGHT }

    protected List<GrabEffect> grabObjects = new List<GrabEffect>();

    [ReadOnly]
    [SerializeField]
    private Hand whichHand;

    [field: ReadOnly]
    [field: SerializeField]
    public GameObject InHand { set; get; } = null;
    [field: ReadOnly]
    [field: SerializeField]
    public UnityEngine.XR.InputDevice SourceDevice { get; set; }
    public Hand WhichHand { get { return whichHand; } }

    /// <summary>
    /// Answers if the object can be grabbed in the current context
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True is the object should allow a grab if choosen</returns>
    public virtual bool CanGrab(GameObject obj)
    {
    //Disallow grab is hand is full
    if (InHand == null)
    {
        //get ray from hand to object
        Vector3 lineTo = obj.transform.position - transform.position;
        Ray r = new Ray(transform.position, lineTo);
        RaycastHit hit;
        int layerMask = 1 << 2; //skip the ignore raycast layer XXXXXXXXXXst controller to ignore raycast
        Physics.Raycast(r, out hit, lineTo.magnitude, ~layerMask);

        //if no hit, nothing in between, but also ignore self-collisions
        if (hit.collider == null || hit.collider.gameObject == obj.gameObject)
        {
        return true;
        }
    }
    return false;
    }

    /// <summary>
    /// Get the closet legal grabbable item
    /// </summary>
    /// <returns>Returns the nearest object with a GrabEffect, 
    /// or null if there are none</returns>
    public GameObject GetNearestGrabbable()
    {

    //only works because the list should be sorted each update
    if (grabObjects.Count > 0 && CanGrab(grabObjects[0].gameObject))
    {
        return grabObjects[0].gameObject;

    }

    return null;
    }

    /// <summary>
    /// Grab action occurred.
    /// Get list of objects that need to be notified in preference order.
    /// </summary>
    public void OnGrab(InputAction.CallbackContext context)
    {
    SetController(context);

    //sanity check, do not grab if hands are full
    if (InHand == null)
    {
        //check for all the objects that are to be notified, by default, this is just one
        GrabEffect[] list = GetGrabCallbackSet();
        Array.Sort(list, (firstObj, secondObj) =>
        {
        return firstObj.Priority - secondObj.Priority;
        });

        foreach (GrabEffect go in list)
        {
        //run effect only if still good
        if (go != null && go.enabled)
        {
            if (go.OnGrab(this))
            {
            break;
            }
        }
        }
    }
    }

    /// <summary>
    /// Release action occurred. Notify objects who want to know and are in the active list.
    /// It does NOT make the hand open, in cases where releasing should be disallowed
    /// </summary>
    public void OnRelease(InputAction.CallbackContext context)
    {
    //safety check. If a release occur with nothing in the hand,
    //make sure everything is released
    if (InHand == null)
    {
        ResetHand();
    }

    //check for all the objects that are to be notified, but default, this is just one
    GrabEffect[] list = GetReleaseCallbackSet();
    Array.Sort(list, (firstObj, secondObj) =>
    {
        return firstObj.Priority - secondObj.Priority;
    });

    foreach (GrabEffect go in list)
    {
        if (go.OnRelease(this))
        {
        break;
        }
    }


    }

    public void RegisterGrab(GrabEffect item)
    {
    //disallow duplicates
    if (!grabObjects.Contains(item))
    {
        grabObjects.Add(item);
        item.OnAdd(this);
    }
    }

    /// <summary>
    /// Resets the hand to a non-grabbed state, by called reset() on all
    /// the effects
    /// </summary>
    public void ResetHand()
    {
    foreach (GrabEffect go in grabObjects)
    {
        go.Reset(this);
    }
    }

    public void UnregisterGrab(GrabEffect item)
    {
    if (grabObjects.Remove(item))
    {
        item.OnRemove(this);
    }

    }

    /// <summary>
    /// Run hover update as needed
    /// </summary>
    public void Update()
    {
    CleanUp();
    grabObjects.Sort((firstObj, secondObj) =>
    {
        return DistanceOrder(firstObj, secondObj);
    });

    foreach (GrabEffect g in grabObjects)
    {
        if (g.gameObject != InHand)
        g.OnHover(this);
    }
    }

    /// <summary>
    /// Helper function to remove elements from the list if they have someone 
    /// been disabled or removed since the last grab/release event
    /// </summary>
    protected virtual void CleanUp()
    {
    List<GrabEffect> toRemove = new List<GrabEffect>();

    //find any grabbable game object that is no longer active or has been destroyed
    foreach (GrabEffect obj in grabObjects)
    {
        if (obj == null || !obj.gameObject.activeSelf || Vector3.Distance(obj.gameObject.transform.position, transform.position) > maxDistance)
        {
        toRemove.Add(obj);
        }
    }

    //remove found items from the list
    foreach (GrabEffect obj in toRemove)
    {
        if (grabObjects.Remove(obj))
        {
        obj.OnRemove(this);
        }

        if (obj == InHand)
        {
        obj.Reset(this);
        InHand = null;
        }
    }
    }

    /// <summary>
    /// Gets a list of objects to be notified of a grab callbacks. 
    /// The default just uses ALL grab effects of the closest object
    /// </summary>
    /// <returns>The set of effects to call</returns>
    protected virtual GrabEffect[] GetGrabCallbackSet()
    {
    GameObject closest = GetNearestGrabbable();

    if (closest == null)
        return new GrabEffect[0];
    else
        return closest.GetComponents<GrabEffect>();
    }

    /// <summary>
    /// Gets a list of objects to be notified of a release callbacks. 
    /// The default just uses ALL grab effects of what is in the hand
    /// </summary>
    /// <returns></returns>
    protected virtual GrabEffect[] GetReleaseCallbackSet()
    {
    if (InHand == null)
        return new GrabEffect[0];
    else
        return InHand.GetComponents<GrabEffect>();
    }

    /// <summary>
    /// Draw the Debugging Gizmoe
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    if (whichHand == Hand.LEFT)
    {
        Gizmos.color = new Color(0, 0, 1, 0.25f);
    }
    else if (whichHand == Hand.RIGHT)
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
    }
    else
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f);
    }

    Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }

    /// <summary>
    /// Helper function to determine the hand
    /// </summary>
    private void DetermineHand()
    {
    whichHand = Hand.NEITHER;

    //if not XR, try other ways
    if (whichHand == Hand.NEITHER)
    {
        TrackedPoseDriver driver = GetComponent<TrackedPoseDriver>();
        if (tag.ToLower().Contains("left")
        || name.ToLower().Contains("left")
        || (driver != null && driver.poseSource == TrackedPoseDriver.TrackedPose.LeftPose))
        {
        whichHand = Hand.LEFT;
        }
        else if (tag.ToLower().Contains("right")
        || name.ToLower().Contains("right")
        || (driver != null && driver.poseSource == TrackedPoseDriver.TrackedPose.RightPose))
        {
        whichHand = Hand.RIGHT;
        }
    }
    }

    /// <summary>
    /// Custom sort for objected based on distance
    /// </summary>
    /// <param name="firstObj"></param>
    /// <param name="secondObj"></param>
    /// <returns></returns>
    private int DistanceOrder(GrabEffect firstObj, GrabEffect secondObj)
    {
    float distanceFirst = (transform.position - firstObj.transform.position).magnitude;
    float distanceSecond = (transform.position - secondObj.transform.position).magnitude;
    if (distanceFirst > distanceSecond) { return 1; }
    else if (distanceFirst < distanceSecond) { return -1; }
    else { return 0; }
    }

    /// <summary>
    /// Helper function to pull the VR controller from the context
    /// </summary>
    /// <param name="context"></param>
    private void SetController(InputAction.CallbackContext context)
    {

    //get left\right controller that caused the action
    UnityEngine.InputSystem.InputDevice device = context.control.device;
    List<UnityEngine.XR.InputDevice> inputDevices = new List<UnityEngine.XR.InputDevice>();

    if (device.usages.Contains(UnityEngine.InputSystem.CommonUsages.LeftHand))
    {
        InputDevices.GetDevicesWithCharacteristics(
        InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left, inputDevices);
    }
    else if (device.usages.Contains(UnityEngine.InputSystem.CommonUsages.RightHand))
    {
        InputDevices.GetDevicesWithCharacteristics(
        InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right, inputDevices);
    }
    SourceDevice = inputDevices.FirstOrDefault();
    }

    // Start is called before the first frame update
    void Start()
    {
    DetermineHand();
    //sanity check, if the action does not exists, do not save it
    if (grabAction != null)
        grabAction.action.started += OnGrab;
    else
        Debug.LogWarning("Grab action not found on "
                + name + ". Grab will not work. Is there an action missing?");
    if (releaseAction != null)
        releaseAction.action.performed += OnRelease;
    else
        Debug.LogWarning("Release action not found on "
                        + name + ". Release will not work. Is there an action missing?");

    }
}
