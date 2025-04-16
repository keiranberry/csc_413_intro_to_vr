using UnityEngine;
using UnityEngine.XR;

public class MoveXRrigOnPadClick : MonoBehaviour
{
    [SerializeField]
    public GameObject xrRig;  // Reference to the XR rig (parent of the camera and controllers)
    public Vector3 targetPosition = new Vector3(5, 0, 5);  // Target position on the 2D axis pad click

    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        // Get references to the input devices (controllers)
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    void Update()
    {
        // Check for "pad click" (secondary button or thumbstick button) press on either the left or right controller
        bool isPadClickedLeft = false;
        bool isPadClickedRight = false;

        if (leftController.isValid)
        {
            leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out isPadClickedLeft);  // Check for pad click (or secondary button)
        }

        if (rightController.isValid)
        {
            rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out isPadClickedRight);  // Check for pad click (or secondary button)
        }

        // If the pad is clicked on either controller, move the XR rig
        if (isPadClickedLeft || isPadClickedRight)
        {
            MoveXRrig();
        }
    }

    void MoveXRrig()
    {
        // Move the XR rig to the target position
        if (xrRig != null)
        {
            xrRig.transform.position = targetPosition;
        }
    }
}
