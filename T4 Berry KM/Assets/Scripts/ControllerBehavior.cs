using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ControllerBehavior : MonoBehaviour
{
    private List<InputDevice> inputDevices = new List<InputDevice>();

    private bool first = false; //flag for the first time we see a device

    private bool isPressed;
    private bool isGripped;

    private Arc[] arcScripts;
    private Grab[] grabScripts;

    [SerializeField]
    private InputDeviceCharacteristics hand = InputDeviceCharacteristics.Right;

    // Start is called before the first frame update
    void Start()
    {
        arcScripts = FindObjectsByType<Arc>(FindObjectsSortMode.None);
        grabScripts = FindObjectsByType<Grab>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (!first)
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand, inputDevices);
        }

        foreach (InputDevice device in inputDevices)
        {
            if (inputDevices.Count == 1 || (device.characteristics & hand) > 0)
            {
                //find features for this device
                List<InputFeatureUsage> supportedFeatrures = new List<InputFeatureUsage>();
                device.TryGetFeatureUsages(supportedFeatrures);

                //for each feature, identify the type of it (bool, float, or Vector2), and print its state

                bool state;
                device.TryGetFeatureValue(CommonUsages.triggerButton, out state);

                if (state)  //is the button down on this frame? 
                {
                    if (!isPressed) //was the button up on the last frame?
                    {
                        foreach (Arc a in arcScripts)
                        {
                            a.BeginPlace();
                        }
                    }
                }
                else ////is the button up on this frame? 
                {
                    if (isPressed)  //is the button down on last frame?
                    {
                        foreach (Arc a in arcScripts)
                        {
                            a.EndPlace();
                        }
                    }

                }
                isPressed = state; //update its state

                device.TryGetFeatureValue(CommonUsages.gripButton, out state);

                if (state) //was the button down on this frame?
                {
                    if (!isGripped)  //is the button up on last frame?
                    {
                        foreach (Grab g in grabScripts)
                        {
                            g.GrabItem(device);
                        }
                    }
                }
                else //was the button up on this frame?
                {

                    if (isGripped)  //is the button up on this frame?
                    {
                        foreach (Grab g in grabScripts)
                        {
                            g.DropItem(device);
                        }
                    }
                }
                isGripped = state; //update its state 
            }
        }
    }
}