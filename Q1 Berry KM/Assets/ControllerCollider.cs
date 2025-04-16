using UnityEngine;

public class ControllerCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject cube;
    private float moveDistance = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit by the controller has "Move" in its name
        if (collision.gameObject.name.Contains("Move"))
        {
            // Move the cube to the right by 1 meter
            cube.transform.Translate(Vector3.right * moveDistance);
        }
    }
}
