using UnityEngine;
using UnityEngine.XR;

public class MoveXRrigOnAButtonPress : MonoBehaviour
{
    [SerializeField]
    public GameObject xrRig;  // Reference to the XR rig (parent of the camera and controllers)
    public Vector3 targetPosition = new Vector3(0, 10, 0);  // Target position for the feet or base

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
        // Check for "A" button press on either the left or right controller
        bool isAButtonPressedLeft = false;
        bool isAButtonPressedRight = false;

        if (leftController.isValid)
        {
            leftController.TryGetFeatureValue(CommonUsages.primaryButton, out isAButtonPressedLeft);
        }

        if (rightController.isValid)
        {
            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out isAButtonPressedRight);
        }

        bool isAKeyPressed = Input.GetKeyDown(KeyCode.A);

        // If the "A" button is pressed on either controller or the keyboard, move the XR rig
        if (isAButtonPressedLeft || isAButtonPressedRight || isAKeyPressed)
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
