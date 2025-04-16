using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Teleportation : MonoBehaviour
{
    [SerializeField]
    public LineRenderer line;

    [SerializeField]
    private GameObject stick;

    [SerializeField] 
    private GameObject head;
    [SerializeField]
    private GameObject feet;

    private Vector3[] points = new Vector3[2];

    // hit information for this frame
    private RaycastHit hit;

    private Color noHitColor = Color.magenta;
    private Color hitColor = Color.green;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowLine(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        // point at hand
        points[0] = gameObject.transform.position; 
        
        // point 4 units (meters) forward from hand
        points[1] = gameObject.transform.position + gameObject.transform.forward * 4;

        // give the line render this positions this frame
        line.SetPositions(points);

        //change line color if teleportation is legal
        if (HaveCollision())
        {
            line.startColor = hitColor; 
            line.endColor = hitColor;
        }
        else
        {
            line.startColor = noHitColor;
            line.endColor = noHitColor;
        }
    }

    public void Teleport()
    { 
        if (hit.collider != null)    
        {
            // 1) should teleport?
            if (hit.collider.gameObject.name.Equals("Plane"))      
            {
                // 2) offset between play area and head location
                Vector3 difference = feet.transform.position - head.transform.position;
                
                // 3) ignore changes in y right now, to keep the head at the same height!
                difference.y = 0;
                
                // 4) final position
                feet.transform.position = hit.point + difference;      
            }    
        }
    }

    public bool HaveCollision()
    {
        // shoot out a ray that aligns to the current points
        Vector3 origin = points[0];
        Vector3 direction = points[1] - origin;
        float distance = direction.magnitude;

        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            if (hit.collider.gameObject.name.Equals("Plane"))
            {
                return true;
            }
        }

        return false;
    }

    public void ShowLine(bool on) 
    { 
        line.enabled = on; 
        stick.SetActive(!on); 
    }
}
