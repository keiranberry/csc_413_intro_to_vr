using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRButtons : MonoBehaviour
{
    private List<InputDevice> inputDevices = new List<InputDevice>();
    private bool isPressedTrigger;
    [SerializeField]
    private GameObject sphere;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() //cant test with vr not working but writing the code in hopes it works anyway
    {
        bool state;
        foreach(InputDevice device in inputDevices)
        {
            device.TryGetFeatureValue(CommonUsages.triggerButton, out state);

            if((device.characteristics & InputDeviceCharacteristics.Right) > 0)
            {
                if (!state)
                {
                    if(isPressedTrigger)
                    {
                        sphere.transform.position = new Vector3(0, 3, 1);
                    }
                }

                isPressedTrigger = state;
            }
        }
    }
}
