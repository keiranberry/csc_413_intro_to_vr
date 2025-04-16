using UnityEngine;

public class ApparatusVR : DoubleGrab
{
    [SerializeField]
    private float minRaise = 0;
    [SerializeField]
    private float maxRaise = 0.3f;

    private GameObject handle;
    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        //movement will be relative to the handle
        handle = transform.Find("Handle").gameObject;
        startPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (first != null && second != null)
        {
            //converts the hand's world position to the apparatus's (not bar's) local space
            Vector3 localHandA = handle.transform.parent.InverseTransformPoint(first.Item2.transform.position);
            Vector3 localHandB = handle.transform.parent.InverseTransformPoint(second.Item2.transform.position);

            Vector3 average = (localHandA + localHandB) / 2;

            float y = transform.localPosition.y + average.y;
            if(y < startPos.y + minRaise)
            {
                y = startPos.y + minRaise;
            }
            if(y > startPos.y + maxRaise)
            {
                y = startPos.y + maxRaise;
            }
            transform.localPosition = new Vector3(startPos.x, y, startPos.z);

            //basic rotation
            Vector3 direction = localHandA - localHandB;
            
            //flip direction if gripping in reverse
            if(localHandA.x < localHandB.x)
            {
                direction = localHandB - localHandA;
            }

            float theta = -Mathf.Atan2(direction.z, direction.x);
            transform.Rotate(new Vector3(0, theta * Mathf.Rad2Deg, 0));
        }
    }
}
