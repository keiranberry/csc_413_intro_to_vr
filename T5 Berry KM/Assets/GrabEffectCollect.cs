using UnityEngine;

public class GrabEffectCollect : GrabEffect
{
    private DartInventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory = FindAnyObjectByType<DartInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnGrab(Grab controller)
    {
        gameObject.SetActive(false);
        inventory.AddDart(gameObject);
    }

    public override void OnRelease(Grab controller)
    {
        throw new System.NotImplementedException();
    }
}
