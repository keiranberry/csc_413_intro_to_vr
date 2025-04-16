using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class T1GameEvents : MonoBehaviour
{
    private List<InputDevice> inputDevices = new List<InputDevice>(); 
    private bool first = false; // flag the first time we see a device
    private bool isPressedTrigger;

    public TMP_Text score;
    private int points;

    [SerializeField]
    private SpawnArea spawnArea;
    [SerializeField]
    private SpawnArea itemTwoSpawnArea;
    [SerializeField]
    private SpawnArea itemThreeSpawnArea;
    [SerializeField]
    private SpawnArea itemFourSpawnArea;
    [SerializeField]
    private Teleportation teleportation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + points;

        if (!first)
        {
            // collect controller list
            InputDeviceCharacteristics desiredCharacteristics = InputDeviceCharacteristics.HeldInHand;
            InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, inputDevices);

            // have a controller set now, do not pull device list again
            if (inputDevices.Count > 0)
            {
                first = true;
            }
        }

        foreach (InputDevice device in inputDevices)
        {
            // find features for this device
            List<InputFeatureUsage> supportedFeatures = new List<InputFeatureUsage>();
            device.TryGetFeatureUsages(supportedFeatures);

            bool state;
            device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out state);
            if (state) // is the button down on this frame?
            {
                Debug.Log("Reset button is pressed.");
                Reset();
            }

            device.TryGetFeatureValue(CommonUsages.triggerButton, out state);
            
            // if only one controller is active, use it, otherwise only look at right hand
            // characteristics are a bit masked. If this is not zero, it is a right controller
            if (inputDevices.Count == 1 ||          
                (device.characteristics & InputDeviceCharacteristics.Right) > 0)      
            {
                // trigger teleport onDown--------
                // button is down
                if (!state)
                {
                    // but was up last frame
                    if (isPressedTrigger)
                    {
                        teleportation.ShowLine(false);
                        teleportation.Teleport();
                    }
                }
                // button is up
                else
                {
                    // only care about the teleport button's "up"
                    if (!isPressedTrigger)
                    {
                        teleportation.ShowLine(true);
                    }
                }

                //save state for next frame
                isPressedTrigger = state;
            }
        }
    }

    public void Reset()
    {
        spawnArea.Reset();
        itemTwoSpawnArea.Reset();
        itemThreeSpawnArea.Reset();
        itemFourSpawnArea.Reset();
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }
}
