using UnityEngine;

public class PointerLine : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float lineLength = 3f;    

    void Start()
    {
        // Set the width of the line
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;  // Line has two points: start and end
        }
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);  // Start of the line is the pointer's position
            lineRenderer.SetPosition(1, transform.position + transform.forward * lineLength);  // End of the line is 3 meters forward from the pointer
        }
    }
}
