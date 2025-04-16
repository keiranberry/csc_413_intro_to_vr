using System;
using Unity.Properties;
using UnityEngine;

public abstract class DoubleGrab : MonoBehaviour
{
    // Step (1) the user must choose the grab points to activate ------------
    [SerializeField]
    private GameObject grabPoint1;

    [SerializeField]
    private GameObject grabPoint2;
    
    // monitors what hand has what object
    protected Tuple<GameObject, Grab> first;

    protected Tuple<GameObject, Grab> second;

    // Step (2) add the notifier scripts
    public virtual void Start()
    {
        //if nothing is set, assume this object is to be double grabbed
        if(grabPoint1 == null && grabPoint2 == null)
        {
            grabPoint1 = gameObject;
            grabPoint2 = gameObject;
        }

        //if grip objects do not have the detect grab script add it
        GrabEffectNotifyDouble gripForwarder1 = grabPoint1.GetComponent<GrabEffectNotifyDouble>();
        if (gripForwarder1 == null)
        {
            gripForwarder1 = grabPoint1.AddComponent<GrabEffectNotifyDouble>();
        }

        //register this as the callback if one part of the grab happens
        gripForwarder1.DoubleGrab = this;

        //if the same grab point, only need one forwarder
        if(grabPoint1 != grabPoint2)
        {
            GrabEffectNotifyDouble gripForwarder2 = grabPoint2.GetComponent<GrabEffectNotifyDouble>();
            if(gripForwarder2 == null)
            {
                gripForwarder2 = grabPoint2.AddComponent<GrabEffectNotifyDouble>();
            }

            gripForwarder2.DoubleGrab = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Step (3) Watch for grab/release notification on grip points

    public bool OnGrab(GameObject obj, Grab controller)
    {
        Debug.Log("In on Grab Object: " + obj.name + " with " + controller.name);

        // step (1) not a monitored object
        if(!(obj == grabPoint1 || obj == grabPoint2))
            return false;

        // step (2a) no item selected, add it
        if(first == null)
        {
            first = new Tuple<GameObject, Grab>(obj, controller);
            return SingleGrabEvent(first.Item1, first.Item2);
        }
        else if ((first != null && grabPoint1 == grabPoint2) || obj != first.Item1)
        {
            //step (2b) sanity check, this should be a different controller
            if(first.Item2 != controller && second == null)
            {
                second = new Tuple<GameObject, Grab>(obj, controller);

                // step (3) both hands have something, trigger!
                return DoubleGrabEvent(first.Item1, first.Item2, second.Item1, second.Item2);
            }
        }

        return false;
    }

    public bool OnRelease(GameObject obj, Grab controller)
    {
        Debug.Log("In on Release Object: " + obj.name + " with " + controller.name);

        bool consumedEvent = false;

        //sanity check, if there is no grab, do nothing
        if(first == null)
        {
            return false;
        }
        //Step (A) last release
        else if (obj == first.Item1 && controller == first.Item2 && second == null)
        {
            consumedEvent = LastReleaseEvent(obj, controller);

            //release hand
            first = null;
        }
        //step (B) second grab is still active, move it to first
        else if (obj == first.Item1 && controller == first.Item2 && second != null)
        {
            // went from both hands having something to one, trigger!
            consumedEvent = SingleReleaseEvent(obj, controller);

            //release hand
            first = second;
            second = null;
        }
        //step (C) first grab is still active, remove second
        else
        {
            consumedEvent = SingleReleaseEvent(obj, controller);

            second = null;
        }

        return consumedEvent;
    }

    // Step (4) Double grab occirred or ended. Notify whoever needs to know
    protected virtual bool SingleGrabEvent(GameObject inHand, Grab hand)
    {
        hand.InHand = inHand;

        return true;
    }

    protected virtual bool DoubleGrabEvent(GameObject inHand1, Grab hand1,
        GameObject inHand2, Grab hand2)
    {
        hand1.InHand = inHand1;
        hand2.InHand = inHand2;

        return true;
    }

    protected virtual bool SingleReleaseEvent(GameObject inHand, Grab hand)
    {
        hand.InHand = null;
        hand.ResetHand();
        return true; 
    }

    protected virtual bool LastReleaseEvent(GameObject obj, Grab hand)
    {
        hand.InHand = null;
        hand.ResetHand();
        return true;
    }
}
