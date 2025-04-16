using UnityEngine;

public class ColorChangePinball: PinBallBehavior
{
    public override void OnCollisionEnter(Collision other)
    {
        // 1) have the parent do its stuff
        base.OnCollisionEnter(other);

        // 2) get a random color
        Material temp = (Material)Resources.Load("Materials/Red");
        int val = (int)(Random.value * 3);
        switch(val)
        {
            case 0:
                //already done
                break;
            case 1:
                temp = (Material)Resources.Load("Materials/Blue");
                break;
            case 2:
                temp = (Material)Resources.Load("Materials/Yellow");
                break;
        }

        // and set it 
        color.material = temp;
    }
}
