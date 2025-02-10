using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1) get the main camera for efficiency
        cam = Camera.main;
    }

    // Update is called once per frame
    private void OnMouseDrag()
    {
        // 2) get mouse location on screen
        Vector3 curMouse = Input.mousePosition;
        
        // 3) determine how far away on the z-axis the object is from the camera
        float distanceToCam = transform.position.z - cam.transform.position.z;
        
        // convert that screen point to a location in the world
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(curMouse.x, curMouse.y, distanceToCam));
        
        // 4) place this object to be under the mouse
        transform.position = new Vector3(point.x, transform.position.y, transform.position.z);

        // 5) prevent moving outside of bounds
        if (this.transform.position.x > 3.5)
            transform.position = new Vector3((float)3.5, transform.position.y, transform.position.z);
        else if (this.transform.position.x < -3.5)
            transform.position = new Vector3((float)-3.5, transform.position.y, transform.position.z);
    }
}
