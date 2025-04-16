using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    void Update()
    {
        // Raycast parameters
        Vector3 origin = Vector3.zero;  // Start position (0,0,0)
        Vector3 direction = new Vector3(1, 1, 1).normalized;  // Direction (1,1,1), normalized to make it a unit vector

        Ray ray = new Ray(origin, direction);  // Create the ray

        // Visualize the ray in the Scene view (it will only appear in the Scene, not in Game view)
        Debug.DrawRay(origin, direction * 100f, Color.blue);  // Make the ray longer (100 units) for better visibility

        // Cast the ray
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // If the ray hits something, output the name of the hit object
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);
        }
        else
        {
            // If no hit, output that the ray didn't hit anything
            Debug.Log("Ray did not hit anything");
        }
    }
}
