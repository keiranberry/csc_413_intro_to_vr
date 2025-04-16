using UnityEngine;

public class GrabEffectHold : GrabEffect
{
    [SerializeField]
    protected bool hideHand = false;

    protected void HideHand(bool hide) 
    {
        
        //hide mesh and turnoff collision if desired
        if (hideHand)    
        {
            foreach (Transform child in transform)      
            {       
                child.gameObject.SetActive(hide);      
            }    
        }  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnGrab(Grab controller)
    {
        HideHand(false);

        //break old parenting and positioning
        transform.parent = null;

        //parent to controller, and place directly in hand
        transform.parent = controller.transform;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;

        //tell the grabber that their hand now has something
        controller.InHand = this.gameObject;
    }

    public override void OnRelease(Grab controller)
    {
        //release from grabber
        transform.parent = null;

        //tell the grabber they are now able to grab another item
        controller.InHand = null;

        HideHand(false);
    }
}
