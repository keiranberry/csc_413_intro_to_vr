using UnityEngine;
using UnityEngine.EventSystems;

public class SliderBehavior : DoubleGrab
{
    [SerializeField]
    private ApparatusLaser laser;

    //the bar controls
    private float minExtent = 0;
    private float maxExtent = 0;

    [SerializeField]
    private float minLaserWidth = 0.01f;

    [SerializeField]
    private float maxLaserWidth = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        //determine range of slider
        minExtent = -transform.lossyScale.x / 2;
        maxExtent = transform.lossyScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(first != null && second != null)
        {
            MoveHandle(first.Item1, first.Item2);
            MoveHandle(second.Item1, second.Item2);

            float distance = Vector3.Distance(first.Item1.transform.position, second.Item1.transform.position);
            float maxRange = maxExtent - minExtent;
            float percent = distance / maxRange;
            float amount = minLaserWidth + percent * (maxLaserWidth - minLaserWidth);
            laser.SetWidth(amount);
        }
    }

    private void MoveHandle(GameObject grip, Grab hand)
    {
        //converts the hand's world position to the bar's local space
        Vector3 handInLocalSpace = transform.InverseTransformPoint(hand.transform.position);

        //only care about x direction
        float x = handInLocalSpace.x;
        if (x < minExtent)
        {
            x = minExtent;
        }
        if (x > maxExtent)
        {
            x = maxExtent;
        }
        grip.transform.localPosition = new Vector3 (x, 0, 0);
    }
}
