using UnityEngine;

public class GrabEffectA : GrabEffect
{
    private new Renderer renderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer= GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool OnGrab(Grab controller)
    {
        if(renderer != null)
        {
            renderer.material.color = Color.red;
        }
        return false;
    }
}
