using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;
using UnityEngine.TerrainUtils;
using UnityEngine.UI;

public class Grab : MonoBehaviour
{
    private PlayerInput mapper;

    [SerializeField]
    private InputActionProperty grab;
    [SerializeField]
    private InputActionProperty release;
    [SerializeField]
    private InputActionProperty attack;
    [SerializeField]
    private InputActionProperty aim;
    [SerializeField]
    private InputActionProperty select;

    [SerializeField]
    private GameObject handModel;

    // make select line for when empty handed and want to deal with menus
    private LineRenderer selectLine;
    private Vector3[] points = new Vector3[2];
    private Vector3 origin;
    private Vector3 direction;

    protected List<GrabEffect> grabObjects = new List<GrabEffect>();

    public GameObject InHand { set; get; } = null;

    //for setting color of buttons
    private GameObject lastOver;

    private bool checkForVr = false;

    private void Start()
    {
        grab.action.performed += OnGrab;
        release.action.performed += OnRelease;
        attack.action.performed += OnAttack;
        attack.action.canceled += OnEndAttack;
        aim.action.performed += OnAim;
        aim.action.canceled += OnAimCancel;
        select.action.performed += OnSelectPerformed;

        selectLine = GetComponent<LineRenderer>();
        selectLine.enabled = false;
    }

    private void Update()
    {
        if(UnityEngine.XR.XRSettings.isDeviceActive && !checkForVr)
        {
            handModel.transform.localEulerAngles = new Vector3(-180, -75, 0);
            handModel.transform.localPosition = new Vector3(-0.4f, 0.01f, 0.08f);
            checkForVr = true;
        }
        if (selectLine.enabled)
        {
            origin = transform.position;

            if (UnityEngine.XR.XRSettings.isDeviceActive)
                direction = -transform.up;
            else
            {
                direction = transform.parent.transform.forward;
            }

            points[0] = origin;
            points[1] = origin + direction * 50f;

            selectLine.SetPositions(points);

            RaycastHit hit;
            if(Physics.Raycast(origin, direction, out hit))
            {
                GameObject target = hit.collider.gameObject;
                if (target.CompareTag("Button"))
                {
                    target.GetComponent<Image>().color = Color.cyan;
                    lastOver = target;
                }
                else
                {
                    if (lastOver != null)
                    {
                        lastOver.GetComponent<Image>().color = Color.white;
                        lastOver = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Helper function to check if object being hovered can be grabbed
    /// </summary>
    /// <param name="item"></param>
    public void RegisterGrab(GrabEffect item)
    {
        if (!grabObjects.Contains(item))
        {
            grabObjects.Add(item);
        }
    }

    /// <summary>
    /// Resets hover if user hand moves out of range.
    /// </summary>
    /// <param name="item"></param>
    public void UnregisterGrab(GrabEffect item)
    {
        grabObjects.Remove(item);
    }

    /// <summary>
    /// Grabs whatever the user is hovering over
    /// </summary>
    /// <param name="context"></param>
    public void OnGrab(InputAction.CallbackContext context)
    {
        if (InHand == null)
        {
            GrabEffect[] list = GetGrabCallbackSet();
            foreach (GrabEffect item in list)
            {
                if(item != null && item.enabled)
                    item.OnGrab(this);
            }
        }
    }
    

    /// <summary>
    /// Releases whatever object the user is holding
    /// </summary>
    /// <param name="context"></param>
    public void OnRelease(InputAction.CallbackContext context)
    {
        GrabEffect[] list = GetReleaseCallbackSet();
        foreach (GrabEffect item in list)
        {
            item.OnRelease(this);
        }
    }

    /// <summary>
    /// Passthrough function to call OnAttack on whatever object the user is holding.
    /// If no object in hand, a selection laser will fire from the users right hand.
    /// </summary>
    /// <param name="callback"></param>
    public void OnAttack(InputAction.CallbackContext callback)
    {
        if(InHand != null)
        {
            InHand.SendMessage("OnAttack",
                GetComponent<Collider>(),
                SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            selectLine.enabled = true;
        }
    }

    /// <summary>
    /// Stops selection laser when user lets off of the right trigger.
    /// </summary>
    /// <param name="callback"></param>
    public void OnEndAttack(InputAction.CallbackContext callback)
    {
        if(selectLine.enabled == true)
            selectLine.enabled = false;
    }

    /// <summary>
    /// Passthrough function to cancel aiming laser on 2-handed weapons.
    /// </summary>
    /// <param name="callback"></param>
    public void OnAimCancel(InputAction.CallbackContext callback)
    {
        if (InHand != null)
        {
            InHand.SendMessage("OnAimCancel",
                GetComponent<Collider>(),
                SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Passthrough function to start aim laser on 2-handed weapons.
    /// </summary>
    /// <param name="callback"></param>
    public void OnAim(InputAction.CallbackContext callback)
    {
        if (InHand != null)
        {
            InHand.SendMessage("OnAim",
                GetComponent<Collider>(),
                SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Allows selection of buttons with select laser when user does not have an object in their hand.
    /// </summary>
    /// <param name="callback"></param>
    public void OnSelectPerformed(InputAction.CallbackContext callback)
    {
        if (selectLine.enabled)
        {
            RaycastHit hit;
            if(Physics.Raycast(origin, direction, out hit))
            {
                GameObject target = hit.collider.gameObject;
                if (target.CompareTag("Button"))
                {
                    target.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
    
    /// <summary>
    /// Helper function to get all objects that are in range to be held by the user.
    /// </summary>
    /// <returns></returns>
    protected virtual GrabEffect[] GetGrabCallbackSet()
    {
        GameObject closest = null;
        foreach (GrabEffect obj in grabObjects)
        {
            if (closest == null || 
                (this.transform.position - closest.transform.position).magnitude >
                (this.transform.position - obj.transform.position).magnitude)
            {
                closest = obj.gameObject;
            }
        }

        if(closest == null)
            return new GrabEffect[0];
        else 
            return closest.GetComponents<GrabEffect>();
    }

    /// <summary>
    /// Helper function to get all obejcts that can be released by the user
    /// </summary>
    /// <returns></returns>
    protected virtual GrabEffect[] GetReleaseCallbackSet()
    {
        if(InHand == null)
            return new GrabEffect[0];
        else
            return InHand.GetComponents<GrabEffect>();
    }

    /// <summary>
    /// Debugging function to draw Gizmos in the scene editor
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.25f);
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
