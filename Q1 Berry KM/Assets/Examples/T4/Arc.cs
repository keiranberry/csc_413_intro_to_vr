using UnityEngine;

public class Arc : MonoBehaviour
{
    // trajectory settings------------------
    [SerializeField]
    private int initCount = 30;

    [SerializeField]
    private float force = 10;

    [SerializeField]
    private float mass = 1;

    // how close are the points, make smaller for smoother curves
    [SerializeField]
    private float stepSize = 0.1f;

    // line settings------------------------------
    [SerializeField]
    private float lineSize = 0.1f;

    private GameObject item;

    [SerializeField]
    private float epsilon = 0.01f;

    private LineRenderer line;

    private Vector3[] points;

    [SerializeField] 
    private GameObject toPlace; 
    
    [SerializeField] 
    private bool turnOffCollisionWhenPlacing = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = initCount;
        line.startWidth = lineSize;
        line.endWidth = lineSize;

        points = new Vector3[initCount];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
            BeginPlace(); 
        
        if (Input.GetKeyUp(KeyCode.Space)) 
            EndPlace();

        if (item != null)
        {
            UpdateLine();
            PlaceObject();
        }
    }

    private void UpdateLine()
    {
        // find starting point based on the object is
        Vector3 currentPoint = gameObject.transform.position;

        // calculate initial a and v based on force
        Vector3 a = Physics.gravity; 
        Vector3 v = gameObject.transform.forward * (force / mass);
        
        // do Euler steps
        for (int i = 0; i < initCount; i++)        
        {            
            points[i] = currentPoint;            
            v = v + a * stepSize;            
            currentPoint = currentPoint + v * stepSize;        
        }
        
        // give the line render these positions this frame
        line.SetPositions(points);
    }

    private void PlaceObject()
    {
        // where is the bottom of the item
        Bounds bounds = item.GetComponent<Renderer>().bounds;
        float offset = bounds.extents.y + epsilon;
        
        // turn off item in case of no hit, and to avoid colliding with it
        item.SetActive(false);
        
        // check each segment for a collision
        for (int i = 1; i < initCount - 1; i++)    
        {
            // figure out ray, and check
            Ray r = new Ray(points[i - 1], points[i] - points[i - 1]);
            float distance = (points[i] - points[i - 1]).magnitude;      
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, distance))      
            {
                // turn object back on,
                // and place with the bottom of the object at the hit point
                item.SetActive(true);        
                item.transform.position = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
                
                // one hit is enough, stop the loop
                break;      
            }    
        }  
    }

    public void BeginPlace()
    {
        item = Instantiate(toPlace);     // make a copy

        // show the line, and turn off physics on the copy
        line.enabled = true;
        if (turnOffCollisionWhenPlacing) 
        { 
            Collider body = item.GetComponent<Collider>(); 
            body.enabled = false; 
        }
    }
    public void EndPlace() 
    { 
        Collider body = item.GetComponent<Collider>(); 
        body.enabled = true; 
        item = null; 
        line.enabled = false; 
    }
}
