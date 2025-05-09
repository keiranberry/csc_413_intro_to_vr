using UnityEngine;

public class BouncyBallScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 startPosition;
    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y <= -3)
        {
            rb.linearVelocity = Vector3.zero;
            transform.position = startPosition;
        }
    }
}
