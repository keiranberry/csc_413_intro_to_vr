using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandGrab : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty leftHandGrab;

    [SerializeField]
    private GameObject handModel;

    [SerializeField]
    private Material grabbedColliderMaterial;

    [SerializeField]
    private Material nonGrabbedColliderMaterial;

    public static bool IsGrabbingGrip = false;

    private bool isInGripZone = false;
    private Collider currentGripCollider;

    private bool checkForVr = false;

    private void Start()
    {
        leftHandGrab.action.performed += OnLeftHandGrab;
        leftHandGrab.action.canceled += OnLeftHandGrabCancel;
    }

    private void Update()
    {
        if(UnityEngine.XR.XRSettings.isDeviceActive && !checkForVr)
        {
            handModel.transform.localEulerAngles = new Vector3(-180, -75, 0);
            handModel.transform.localPosition = new Vector3(-0.4f, 0.01f, 0.08f);
            checkForVr = true;
        }
    }

    /// <summary>
    /// If left hand comes withing range of a left hand grip on 2-handed weapons,
    /// mark that the grip zone can be grabbed with a boolean and change color visually.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LeftHandGrip")
        {
            isInGripZone = true;
            currentGripCollider = other;
        }
    }

    /// <summary>
    /// If left hand leaves range of left hand grip on 2-handed weapons, 
    /// unmark object as able to be grabbed, and change color back to default.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other == currentGripCollider)
        {
            currentGripCollider.gameObject.GetComponent<MeshRenderer>().SetMaterials(new List<Material>(new Material[] { nonGrabbedColliderMaterial }));
            isInGripZone = false;
            currentGripCollider = null;
            IsGrabbingGrip = false;            
        }
    }

    /// <summary>
    /// When left hand grip object is grabbed, set grabbing value to true and change color to specified material.
    /// </summary>
    /// <param name="context"></param>
    public void OnLeftHandGrab(InputAction.CallbackContext context)
    {
        if (isInGripZone)
        {
            IsGrabbingGrip = true;
            if (grabbedColliderMaterial != null) {
                currentGripCollider.gameObject.GetComponent<MeshRenderer>().SetMaterials(new List<Material>(new Material[] {grabbedColliderMaterial}));
            }
        }
    }

    /// <summary>
    /// When left hand grip object is released, set grabbing value to false, and change color back to default material.
    /// </summary>
    /// <param name="context"></param>
    public void OnLeftHandGrabCancel(InputAction.CallbackContext context)
    {
        IsGrabbingGrip = false;
        currentGripCollider.gameObject.GetComponent<MeshRenderer>().SetMaterials(new List<Material>(new Material[] { nonGrabbedColliderMaterial }));
    }
}

