using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Grab : MonoBehaviour
{
    private GameObject closestObject;
    private GameObject hand;
    private Transform originalParent;
    private GameObject objectInHand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hand = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            Debug.Log("Closest object is: " + other.gameObject.name);
            closestObject = other.gameObject;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (closestObject != null && other != closestObject)
        {
            Debug.Log("No object in reach");
            closestObject = null;
        }
    }

    public void GrabItem(InputDevice device)
    {
        bool isLeft = device.characteristics.HasFlag(InputDeviceCharacteristics.Left);
        
        // wrong hand stop
        if (isLeft && !name.Contains("Left") || !isLeft && !name.Contains("Right"))
            return;
        
        if (closestObject != null && objectInHand == null)    
        {
            // swap hand to grabbed object
            hand.gameObject.SetActive(false);
            
            // move object to be in hand
            objectInHand = closestObject;
            
            // parent to the hand, but save the original to place it back
            originalParent = objectInHand.transform.parent;      
            objectInHand.transform.parent = this.transform;      
            objectInHand.transform.position = transform.position;
            
            // turn off physics
            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();      
            rb.isKinematic = true;    
        }  
    }

    public void DropItem(InputDevice device)
    {
        bool isLeft = device.characteristics.HasFlag(InputDeviceCharacteristics.Left);
        
        // wrong hand stop
        if (isLeft && !name.Contains("Left") || !isLeft && !name.Contains("Right"))
            return;
        
        if (objectInHand)    
        {      
            // swap hand to original object
            hand.gameObject.SetActive(true);
            
            // place object back into original parent
            objectInHand.transform.parent = originalParent;
            
            // turn on physics
            Rigidbody rb = objectInHand.GetComponent<Rigidbody>();      
            rb.isKinematic = false;

            Vector3 state; 
            device.TryGetFeatureValue(CommonUsages.deviceVelocity, out state); 
            rb.linearVelocity = state;

            //move out of range of the hand
            transform.position = objectInHand.transform.position + 0.1f * state;
            
            //estimate force by velocity over time
            float force = state.magnitude / Time.fixedDeltaTime;      
            Vector3 applyForce = force * state.normalized;
            rb.AddForce(applyForce, ForceMode.Force);

            device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out state);
            rb.angularVelocity = state;
        }    
        objectInHand = null;  
    }
}
