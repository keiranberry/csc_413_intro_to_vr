using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    [SerializeField] 
    private bool useMass = false; 
    
    [SerializeField] 
    private bool holdForce = false; 
    
    [SerializeField] 
    private KeyCode onKey = KeyCode.None; 
    
    [SerializeField] 
    private float force = 700f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(onKey))
        {
            FireCannon();
        }
    }

    ///<summary>
    /// Gets the force mode depending on whether use is to be used or not,
    /// and if the force is instant or not
    ///</summary>
    ///<returns>The associated force mode</returns>
    private ForceMode GetForceMode()  
    {
        if (!useMass)    
        {
            if (!holdForce)
                return ForceMode.VelocityChange;
            else return ForceMode.Acceleration;
        }
        else 
        { 
            if (!holdForce) return ForceMode.Impulse; 
            else return ForceMode.Force; 
        }  
    }

    ///<summary>
    /// Fires the cannon based on the current force, and mode
    ///</summary>
    private void FireCannon()  
    {
        // load a new ball
        GameObject temp = Resources.Load<GameObject>("Cannon Ball");
        
        // height is cannon height plus have the ball size
        float height = gameObject.transform.lossyScale.y + temp.transform.localScale.x / 2;
        
        // place the ball
        Vector3 up = gameObject.transform.up;        
        Vector3 startingPos = height * up + gameObject.transform.position;       
        GameObject ball = Instantiate(temp, startingPos, Quaternion.identity);
        
        // apply force
        Rigidbody rb = ball.GetComponent<Rigidbody>();        
        Vector3 force = this.force * up;
        rb.AddForce(force, GetForceMode());
    }

    protected void OnTriggerEnter(Collider other) 
    { 
        if (other.gameObject.name.Contains("Controller")) 
        { 
            FireCannon(); 
        } 
    }
}
