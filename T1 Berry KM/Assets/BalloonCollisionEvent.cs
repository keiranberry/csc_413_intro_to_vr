using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class BalloonCollisionEvent : MonoBehaviour
{
    private GameEvents game;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.game = GameObject.Find("Game").GetComponent<GameEvents>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Stick"))
        {
            gameObject.SetActive(false); // remove object

            // find all devices that are held in the hand, and are on the "right" side
            List<InputDevice> inputDevices = new List<InputDevice>();            
            InputDevices.GetDevicesWithCharacteristics(               
                InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right, inputDevices);
            
            // grab the first
            InputDevice targetDevice = inputDevices.FirstOrDefault();

            // short, strong vibration
            targetDevice.SendHapticImpulse(0, 1, 0.01f);

            this.game.AddPoints(1);
        }
    }

}
