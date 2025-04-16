using UnityEngine;

public class FullManipulation : DoubleGrab
{
    [SerializeField]
    private float minScale = 0.5f;
    [SerializeField]
    private float maxScale = 2;

    //now watch for scale instead of coordinate system
    private float initScale;
    private Vector3 origScale = Vector3.one;

    [field: SerializeField]
    protected Transform ParentOnRelease { set; get; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        if(GetComponent<GrabEffectHold>() == null)
        {
            GrabEffectHold hold = gameObject.AddComponent<GrabEffectHold>();
            hold.ApplyPhysicsOnRelease = ParentOnRelease;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(first != null && second != null)
        {
            //point halfway between
            Vector3 p = (first.Item2.transform.position + second.Item2.transform.position) / 2;

            Vector3 v = second.Item2.transform.position - first.Item2.transform.position;

            if(first.Item2.WhichHand == Grab.Hand.LEFT)
            {
                v = first.Item2.transform.position - second.Item2.transform.position;
            }

            //percentage change from base, with limits
            float newScale = (v.magnitude / initScale);
            if(newScale < minScale)
            {
                newScale = minScale;
            }
            if(newScale > maxScale)
            {
                newScale = maxScale;
            }

            v.Normalize();
            Quaternion q = Quaternion.LookRotation(v);

            float a = GetRotationDown(first.Item2.transform.forward, first.Item2.transform.position, v);
            float b = GetRotationDown(second.Item2.transform.forward, second.Item2.transform.position, v);
            float theta = (a + b) / 2;
            Quaternion roll = Quaternion.AngleAxis(theta, v);

            transform.transform.position = p;
            transform.localScale = newScale * origScale;
            transform.rotation = roll * q;
        }
    }

    protected override bool DoubleGrabEvent(GameObject inHand1, Grab hand1, GameObject inHand2, Grab hand2)
    {
        origScale = transform.localScale;
        initScale = transform.localScale.x; // now watch for scale instead of coordinate system
        GetComponent<Rigidbody>().isKinematic = true;

        return false;
    }

    protected override bool SingleGrabEvent(GameObject inHand, Grab hand)
    {
        return false;
    }

    protected override bool LastReleaseEvent(GameObject obj, Grab hand)
    {
        return false;
    }

    private float GetRotationDown(Vector3 direction, Vector3 pointOnPlane, Vector3 planeForward)
    {
        //angle from up to direction
        float rotFromUp = Vector3.Angle(Vector3.up, direction);

        //make a vertical plane facing look direction
        Vector3 planeNormal = Vector3.Cross(Vector3.up, planeForward);
        Plane plane = new Plane(planeNormal, pointOnPlane);

        //which
        bool left = plane.GetSide(pointOnPlane + direction);

        if (left)
        {
            return -rotFromUp;
        }
        else
        {
            return rotFromUp;
        }
    }
}
