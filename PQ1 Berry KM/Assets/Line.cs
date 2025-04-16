using UnityEngine;

public class Line : MonoBehaviour
{
    private LineRenderer line;

    private Vector3[] points = new Vector3[2];

    private float laserLength = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.startWidth = .05f;
        line.endWidth = .05f;
    }

    // Update is called once per frame
    void Update()
    {
        points[0] = gameObject.transform.position;
        points[1] = gameObject.transform.position + gameObject.transform.forward * laserLength;
        line.SetPositions(points);

        Vector3 direction = points[1] - points[0];
        float distance = direction.magnitude;
        RaycastHit hit;
        Physics.Raycast(points[0], direction, out hit, distance);

        Collider hitCollider = hit.collider;

        GameObject hitObject = null;

        if(hitCollider != null)
        {
            hitObject = hitCollider.gameObject;

            points[1] = hit.point;
            line.SetPositions(points);
        }
    }
}
