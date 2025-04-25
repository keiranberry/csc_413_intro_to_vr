using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GrabEffectHold : GrabEffect
{
  [field: SerializeField]
  public bool ApplyPhysicsOnRelease { get; set; } = true;


  [SerializeField]
  protected Vector3 rotationOffset;
  [SerializeField]
  protected Vector3 positionOffset;

  [SerializeField]
  protected bool hideHand = false;

  protected Transform ParentOnRelease { set; get; }

  protected List<Grab> holdingControllers = new List<Grab>();


  //overrite default priority of 100
  protected GrabEffectHold()
  {
    Priority = 2;
  }

  public void Start()
  {
    //save original parent
    ParentOnRelease = transform.parent;
  }

  public delegate void ReleaseEvent(GameObject obj);
  protected ReleaseEvent releaseScript;
  public void SetReleaseFunction(ReleaseEvent obj)
  {
    releaseScript = obj;
  }

  public override bool OnGrab(Grab controller)
  {
    //sanity check, if this controller already has this object stop
    if (holdingControllers.Contains(controller) || controller.InHand != null)
      return false;

    if (hideHand)
      controller.HideHand(true);

    //remember which hand has this
    holdingControllers.Add(controller);

    //parent to controller, and place directly in hand
    transform.parent = controller.transform;
    transform.localPosition = positionOffset;
    transform.localRotation = Quaternion.Euler(rotationOffset);

    //tell the grabber that their hand now has something
    controller.InHand = this.gameObject;

    //turn off physics
    GetComponent<Rigidbody>().isKinematic = true;

    return true;
  }


  public override bool OnRelease(Grab controller)
  {
    //sanity check that the object needs to be this one to continue
    if (!holdingControllers.Contains(controller) || controller.InHand != gameObject)
      return false;
   
    Reset(controller);

    //let the release script take over if needed
    if (releaseScript != null)
    {
      releaseScript(gameObject);
    }
    //apply phyics if desired, and not already handled
    else if (ApplyPhysicsOnRelease && holdingControllers.Count == 0)
    {
      GetComponent<Rigidbody>().isKinematic = !ApplyPhysicsOnRelease;
      ApplyPhysics(controller);
    }

    return true;
  }

  protected void ApplyPhysics(Grab hand)
  {
    //give an average of physics of both hands
    Vector3 ave = Vector3.zero;
    Vector3 aveA = Vector3.zero;

    //get values from first hand
    hand.SourceDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out ave);
    hand.SourceDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out aveA);

    //transfer force...if this is not supported, ave and aveA will be 0, and the object will just drop
    Rigidbody body = GetComponent<Rigidbody>();
    body.linearVelocity = ave;
    body.angularVelocity = aveA;

    //get approximate vertical size
    Bounds bounds = GetComponent<Renderer>().bounds;
    float height = bounds.extents.y/2;

    //move out of range of the hand. If speed is to low just drop it
    if (ave.magnitude < 0.1f)
      transform.position = hand.transform.position + height * Vector3.down;
    else
      transform.position = hand.transform.position + height * ave;

    //estimate force by velocity over time
    float force = ave.magnitude / Time.fixedDeltaTime;
    Vector3 applyForce = force * ave.normalized;
    body.AddForce(applyForce, ForceMode.Force);
  }

  public override void Reset(Grab controller)
  {
    //sanity check that the object needs to be this one to continue
    if (!holdingControllers.Contains(controller))
      return;

    holdingControllers.Remove(controller);

    //if not the final release, reparent to some controller
    //and stop
    if (holdingControllers.Count != 0)
    {
      holdingControllers.Sort(DistanceOrder);
      transform.parent = holdingControllers[0].transform;
      transform.localPosition = positionOffset;
      transform.localRotation = Quaternion.Euler(rotationOffset);
    }
    else
    {
      //release from grabber
      transform.parent = ParentOnRelease;
    }

    //tell the grabber they are now able to grab another item
    controller.InHand = null;

    if (hideHand)
      controller.HideHand(false);
  }

  private int DistanceOrder(Grab firstObj, Grab secondObj)
  {
    float distanceFirst = (transform.position - firstObj.transform.position).magnitude;
    float distanceSecond = (transform.position - secondObj.transform.position).magnitude;
    if (distanceFirst > distanceSecond) { return 1; }
    else if (distanceFirst < distanceSecond) { return -1; }
    else { return 0; }
  }

}
