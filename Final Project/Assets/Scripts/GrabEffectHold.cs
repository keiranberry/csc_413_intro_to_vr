public class GrabEffectHold : GrabEffect
{
    public override void OnGrab(Grab controller)
    {
        //Debug.Log("Weapon is grabbed");
        //transform.parent = null;

        //transform.parent = controller.transform;
        //transform.localPosition = new Vector3(0, 0, 0.1f);
        //transform.localRotation = Quaternion.Euler(25, 25, 15);

        //controller.InHand = this.gameObject;

        Inventory.Instance.AddItem(gameObject);
        gameObject.SetActive(false);
    }

    public override void OnRelease(Grab controller)
    {
        transform.parent = null;

        controller.InHand = null;
    }
}
