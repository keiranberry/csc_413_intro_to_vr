using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer line;

    //store the laser positions and laser length
    private Vector3[] points = new Vector3[2];  

    [SerializeField]  
    private float laserLength = 30f;

    // 1a) what was detected last
    private GameObject lastObjectFound = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // update the laser pointer position
        points[0] = gameObject.transform.position;    
        points[1] = gameObject.transform.position + gameObject.transform.forward * laserLength;    
        line.SetPositions(points);
        
        // shoot out a ray aligned to the laser
        Vector3 direction = points[1] - points[0];
        float distance = direction.magnitude;    
        RaycastHit hit;    Physics.Raycast(points[0], direction, out hit, distance);
        
        // what was hit?
        Collider hitCollider = hit.collider;

        // initially assume nothing, but if something was hit,
        // look at its game object AND stop the ray
        GameObject hitObject = null;
        
        if (hitCollider != null)    
        {      
            hitObject = hitCollider.gameObject;
            
            // stop the ray
            points[1] = hit.point;      
            line.SetPositions(points);    
        }
        
        // 2) object change!
        if (lastObjectFound != hitObject)    
        {
            // 3) notify enter and exit
            if (lastObjectFound != null)       
            {         
                lastObjectFound.SendMessage("OnTriggerExit",            
                    GetComponent<SphereCollider>(),            
                    SendMessageOptions.DontRequireReceiver);       
            }
            
            if (hitObject != null)       
            {         
                hitObject.SendMessage("OnTriggerEnter",            
                    GetComponent<SphereCollider>(),            
                    SendMessageOptions.DontRequireReceiver);       
            }
        }

        // 1b update last found object
        lastObjectFound = hitObject;
    }
}
