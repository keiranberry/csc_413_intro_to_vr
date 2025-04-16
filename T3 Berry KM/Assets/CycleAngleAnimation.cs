using UnityEngine;

public class CycleAngleAnimation : MonoBehaviour
{
    [SerializeField]
    private float rotationPerSecond = 1f;
    private Vector3 rotAxis;
    private Quaternion spin;
    private Vector3 pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // find the vector by subtracting the location of the moon from the
        // location of the earth (which is the parent)
        pos = gameObject.transform.position - gameObject.transform.parent.position;

        // make the vector length one
        Vector3 normal = pos.normalized;

        // the cross product of this vector and the Moon's forward direction will give us a
        // vector that is at a right angle to both vectors
        // this is what we want to rotate around
        rotAxis = Vector3.Cross(normal, gameObject.transform.forward);

        // Make a plane that slices through the parent’s left and right axis
        Plane plane = new Plane(gameObject.transform.parent.right, gameObject.transform.parent.position);

        // if the point is not on the right side of the parent, correct the rotation axis by flipping it
        if (plane.GetSide(gameObject.transform.position))
            rotAxis = rotAxis * -1;

        // convert that to a quarternion rotating on that axis, with 0 theta rotation
        spin = Quaternion.AngleAxis(0.0f, rotAxis);
    }

    // Update is called once per frame
    void Update()
    {
        // find rotation update for this frame
        float timeToDegreePerSecond = 360 * rotationPerSecond;
        float rotationAngleUpdate = Time.deltaTime * timeToDegreePerSecond;
        
        // quaternion that rotates around rotAxis by rotationAngleUpdate degrees
        Quaternion rotThisFrame = Quaternion.AngleAxis(rotationAngleUpdate, rotAxis);
        
        // add in the rotation on the moon itself
        spin *= rotThisFrame;
        
        // rotate the moon around the earth
        Vector3 currentPos = spin * pos;    
        gameObject.transform.localPosition = currentPos;    
        gameObject.transform.localRotation = spin;
    }
}
